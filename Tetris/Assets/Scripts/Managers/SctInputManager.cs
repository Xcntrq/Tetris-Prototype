using System;
using UnityEngine;
using nsTouchScreenManager;

namespace nsInputManager
{
    enum SwipeDirection
    {
        none,
        left,
        right,
        up,
        down
    }

    public class SctInputManager : MonoBehaviour
    {
        //Minimum amount of time between procs for each key while holding
        [SerializeField] private float m_moveSidewaysCooldown;
        [SerializeField] private float m_moveDownCooldown;
        [SerializeField] private float m_rotateCooldown;

        private nsGameBoard.SctGameBoard m_sctGameBoard;
        private GameManager m_gameManager;

        //Absolute timestamp when each key can proc
        private float m_timeOfNextMoveLeft;
        private float m_timeOfNextMoveRight;
        private float m_timeOfNextMoveDown;
        private float m_timeOfNextRotate;

        private SwipeDirection m_swipeDirection;
        private SwipeDirection m_swipeEndDirection;

        public event Action OnShapeMoveError;
        public event Action OnShapeMoveSuccess;

        private void Awake()
        {
            m_sctGameBoard = FindObjectOfType<nsGameBoard.SctGameBoard>();
            m_gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            m_swipeDirection = SwipeDirection.none;
            m_swipeEndDirection = SwipeDirection.none;
        }

        public void HandleInput(nsMovingShape.SctMovingShape movingShape)
        {
            //If held down, the key procs only if enough time has passed and if it's still applied to the same shape
            //Otherwise you have let go of the key and press it again, this also allows for faster movements and rotations
            //HasReceivedInput mainly helps the "down" key, because if you quickly drop one shape, you don't wanna drop the next one too
            if (
                (Input.GetButton("MoveLeft") && (Time.time > m_timeOfNextMoveLeft) && movingShape.HasReceivedInput) || Input.GetButtonDown("MoveLeft")
                ||
                ((m_swipeDirection == SwipeDirection.left) && (Time.time > m_timeOfNextMoveLeft) && movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.left)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextMoveLeft = Time.time + m_moveSidewaysCooldown;
                //Once any key has been pressed for the current shape, it allows to hold any other key for that shape
                //Try moving it left
                movingShape.MoveLeft();
                //If the new position is against the rules of tetris, just move it back to where it was
                if (m_sctGameBoard.IsPositionValid(movingShape))
                {
                    OnShapeMoveSuccess?.Invoke();
                }
                else
                {
                    OnShapeMoveError?.Invoke();
                    movingShape.MoveRight();
                }
            }
            else if (
                (Input.GetButton("MoveRight") && (Time.time > m_timeOfNextMoveRight) && movingShape.HasReceivedInput) || Input.GetButtonDown("MoveRight")
                ||
                ((m_swipeDirection == SwipeDirection.right) && (Time.time > m_timeOfNextMoveRight) && movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.right)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextMoveRight = Time.time + m_moveSidewaysCooldown;
                movingShape.MoveRight();
                if (m_sctGameBoard.IsPositionValid(movingShape))
                {
                    OnShapeMoveSuccess?.Invoke();
                }
                else
                {
                    OnShapeMoveError?.Invoke();
                    movingShape.MoveLeft();
                }
            }
            if (
                (Input.GetButton("MoveDown") && (Time.time > m_timeOfNextMoveDown) && movingShape.HasReceivedInput) || Input.GetButtonDown("MoveDown")
                ||
                ((m_swipeDirection == SwipeDirection.down) && (Time.time > m_timeOfNextMoveDown) && movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.down)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextMoveDown = Time.time + m_moveDownCooldown;
                m_gameManager.HandleShapeDrop(true);
            }
            if (
                (Input.GetButton("Rotate") && (Time.time > m_timeOfNextRotate) && movingShape.HasReceivedInput) || Input.GetButtonDown("Rotate")
                ||
                ((m_swipeDirection == SwipeDirection.up) && (Time.time > m_timeOfNextRotate) && movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.up)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextRotate = Time.time + m_rotateCooldown;
                RotationDirection rotationDirection = m_gameManager.GetRotationDirection;
                movingShape.Rotate(rotationDirection);
                if (m_sctGameBoard.IsPositionValid(movingShape))
                {
                    OnShapeMoveSuccess?.Invoke();
                }
                else
                {
                    OnShapeMoveError?.Invoke();
                    movingShape.RotateOppositeDirection(rotationDirection);
                }
            }
            if (Input.GetButtonDown("Hold"))
            {
                m_gameManager.HandleShapeHolding();
            }
        }

        private void OnEnable()
        {
            SctTouchScreenManager.OnSwipe += HandleSwipe;
            SctTouchScreenManager.OnSwipeEnd += HandleSwipeEnd;
        }

        private void OnDisable()
        {
            SctTouchScreenManager.OnSwipe -= HandleSwipe;
            SctTouchScreenManager.OnSwipeEnd -= HandleSwipeEnd;
        }

        private void HandleSwipe(Vector2 swipeVector)
        {
            m_swipeDirection = GetSwipeDirection(swipeVector);
        }

        private void HandleSwipeEnd(Vector2 swipeVector)
        {
            m_swipeEndDirection = GetSwipeDirection(swipeVector);
        }

        private SwipeDirection GetSwipeDirection(Vector2 swipeVector)
        {
            SwipeDirection swipeDirection; // = SwipeDirection.none;
            if (Mathf.Abs(swipeVector.x) >= Mathf.Abs(swipeVector.y))
            {
                swipeDirection = (swipeVector.x >= 0) ? SwipeDirection.right : SwipeDirection.left;
            }
            else
            {
                swipeDirection = (swipeVector.y >= 0) ? SwipeDirection.up : SwipeDirection.down;
            }
            return swipeDirection;
        }
    }
}
