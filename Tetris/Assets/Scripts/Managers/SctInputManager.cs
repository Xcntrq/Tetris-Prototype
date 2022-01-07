using System;
using UnityEngine;

namespace nsInputManager
{
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

        //Prevents any held key from working on the next spawned shape
        private bool m_isAllowedToHold;

        public event Action OnShapeMoveError;
        public event Action OnShapeMoveSuccess;

        public bool IsAllowedToHold { set { m_isAllowedToHold = value; } }

        private void Awake()
        {
            m_sctGameBoard = FindObjectOfType<nsGameBoard.SctGameBoard>();
            m_gameManager = FindObjectOfType<GameManager>();
            m_isAllowedToHold = false;
        }

        public void HandleInput(nsMovingShape.SctMovingShape movingShape)
        {
            //If held down, the key procs only if enough time has passed and if it's still applied to the same shape
            //Otherwise you have let go of the key and press it again, this also allows for faster movements and rotations
            //m_isAllowedToHold mainly helps the "down" key, because if you quickly drop one shape, you don't wanna drop the next one too
            if ((Input.GetButton("MoveLeft") && (Time.time > m_timeOfNextMoveLeft) && m_isAllowedToHold) || Input.GetButtonDown("MoveLeft"))
            {
                m_timeOfNextMoveLeft = Time.time + m_moveSidewaysCooldown;
                //Once any key has been pressed for the current shape, it allows to hold any other key for that shape
                m_isAllowedToHold = true;
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
            else if ((Input.GetButton("MoveRight") && (Time.time > m_timeOfNextMoveRight) && m_isAllowedToHold) || Input.GetButtonDown("MoveRight"))
            {
                m_timeOfNextMoveRight = Time.time + m_moveSidewaysCooldown;
                m_isAllowedToHold = true;
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
            if ((Input.GetButton("MoveDown") && (Time.time > m_timeOfNextMoveDown) && m_isAllowedToHold) || Input.GetButtonDown("MoveDown"))
            {
                m_timeOfNextMoveDown = Time.time + m_moveDownCooldown;
                m_isAllowedToHold = true;
                m_gameManager.HandleShapeDrop();
            }
            if ((Input.GetButton("Rotate") && (Time.time > m_timeOfNextRotate) && m_isAllowedToHold) || Input.GetButtonDown("Rotate"))
            {
                m_timeOfNextRotate = Time.time + m_rotateCooldown;
                m_isAllowedToHold = true;
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
        }
    }
}
