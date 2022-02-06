using System;
using UnityEngine;
using nsTouchScreenManager;
using nsMovingShape;
using nsGameStateManager;

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
        private SctMovingShape m_movingShape;

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

        private void OnEnable()
        {
            SctTouchScreenManager.OnSwipe += HandleSwipe;
            SctTouchScreenManager.OnSwipeEnd += HandleSwipeEnd;
            m_gameManager.OnShapeSpawn += RegisterMovingShape;
        }

        private void OnDisable()
        {
            SctTouchScreenManager.OnSwipe -= HandleSwipe;
            SctTouchScreenManager.OnSwipeEnd -= HandleSwipeEnd;
            m_gameManager.OnShapeSpawn -= RegisterMovingShape;
        }

        private void RegisterMovingShape(SctMovingShape movingShape)
        {
            m_movingShape = movingShape;
        }

        private void Update()
        {
            bool isGameOver = GameStateManager.Instance.GameState == GameState.Over;
            bool isGamePaused = GameStateManager.Instance.GameState == GameState.Paused;
            if (isGameOver || isGamePaused) return;

            if (m_movingShape == null) return;

            //If held down, the key procs only if enough time has passed and if it's still applied to the same shape
            //Otherwise you have let go of the key and press it again, this also allows for faster movements and rotations
            //HasReceivedInput mainly helps the "down" key, because if you quickly drop one shape, you don't wanna drop the next one too
            if (
                (Input.GetButton("MoveLeft") && (Time.time > m_timeOfNextMoveLeft) && m_movingShape.HasReceivedInput) || Input.GetButtonDown("MoveLeft")
                ||
                ((m_swipeDirection == SwipeDirection.left) && (Time.time > m_timeOfNextMoveLeft) && m_movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.left)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextMoveLeft = Time.time + m_moveSidewaysCooldown;
                //Once any key has been pressed for the current shape, it allows to hold any other key for that shape
                //Try moving it left
                m_movingShape.MoveLeft();
                //If the new position is against the rules of tetris, just move it back to where it was
                if (m_sctGameBoard.IsPositionValid(m_movingShape))
                {
                    OnShapeMoveSuccess?.Invoke();
                }
                else
                {
                    OnShapeMoveError?.Invoke();
                    m_movingShape.MoveRight();
                }
            }
            else if (
                (Input.GetButton("MoveRight") && (Time.time > m_timeOfNextMoveRight) && m_movingShape.HasReceivedInput) || Input.GetButtonDown("MoveRight")
                ||
                ((m_swipeDirection == SwipeDirection.right) && (Time.time > m_timeOfNextMoveRight) && m_movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.right)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextMoveRight = Time.time + m_moveSidewaysCooldown;
                m_movingShape.MoveRight();
                if (m_sctGameBoard.IsPositionValid(m_movingShape))
                {
                    OnShapeMoveSuccess?.Invoke();
                }
                else
                {
                    OnShapeMoveError?.Invoke();
                    m_movingShape.MoveLeft();
                }
            }
            if (
                (Input.GetButton("MoveDown") && (Time.time > m_timeOfNextMoveDown) && m_movingShape.HasReceivedInput) || Input.GetButtonDown("MoveDown")
                ||
                ((m_swipeDirection == SwipeDirection.down) && (Time.time > m_timeOfNextMoveDown) && m_movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.down)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextMoveDown = Time.time + m_moveDownCooldown;
                m_gameManager.HandleShapeDrop(true);
            }
            if (
                (Input.GetButton("Rotate") && (Time.time > m_timeOfNextRotate) && m_movingShape.HasReceivedInput) || Input.GetButtonDown("Rotate")
                ||
                ((m_swipeDirection == SwipeDirection.up) && (Time.time > m_timeOfNextRotate) && m_movingShape.HasReceivedInput) || (m_swipeEndDirection == SwipeDirection.up)
                )
            {
                m_swipeDirection = SwipeDirection.none;
                m_swipeEndDirection = SwipeDirection.none;

                m_timeOfNextRotate = Time.time + m_rotateCooldown;
                RotationDirection rotationDirection = m_gameManager.GetRotationDirection;
                m_movingShape.Rotate(rotationDirection);
                if (m_sctGameBoard.IsPositionValid(m_movingShape))
                {
                    OnShapeMoveSuccess?.Invoke();
                }
                else
                {
                    OnShapeMoveError?.Invoke();
                    m_movingShape.RotateOppositeDirection(rotationDirection);
                }
            }
            if (Input.GetButtonDown("Hold"))
            {
                m_gameManager.HandleShapeHolding();
            }
        }

        private void HandleSwipe(Vector2 swipeVector)
        {
            m_swipeDirection = GetSwipeDirection(swipeVector);
            Debug.Log($"swiping {m_swipeDirection}");
        }

        private void HandleSwipeEnd(Vector2 swipeVector)
        {
            m_swipeEndDirection = GetSwipeDirection(swipeVector);
            Debug.Log($"swipe ended {m_swipeEndDirection}");
        }

        private SwipeDirection GetSwipeDirection(Vector2 swipeVector)
        {
            SwipeDirection swipeDirection; // = SwipeDirection.none;
            if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
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
