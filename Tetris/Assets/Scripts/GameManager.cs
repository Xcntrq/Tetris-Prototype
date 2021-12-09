using UnityEngine;

public class GameManager : MonoBehaviour
{
    private nsGameBoard.SctGameBoard m_sctGameBoard;
    private nsShapeSpawner.SctShapeSpawner m_sctShapeSpawner;
    private nsShape.SctShape m_movingShape;

    [SerializeField] private GameObject m_panelGameOver;

    [SerializeField] private float m_moveLeftCooldown;
    [SerializeField] private float m_moveRightCooldown;
    [SerializeField] private float m_moveDownCooldown;
    [SerializeField] private float m_rotateCooldown;
    [SerializeField] private float m_shapeDropCooldown;
    private float m_timeOfNextMoveLeft;
    private float m_timeOfNextMoveRight;
    private float m_timeOfNextMoveDown;
    private float m_timeOfNextRotate;
    private float m_timeOfNextShapeDrop;

    private bool m_isAllowedToHold;

    private bool m_isGameOver;

    private void Awake()
    {
        m_sctGameBoard = FindObjectOfType<nsGameBoard.SctGameBoard>();
        m_sctShapeSpawner = FindObjectOfType<nsShapeSpawner.SctShapeSpawner>();

        m_timeOfNextMoveLeft = Time.time;
        m_timeOfNextMoveRight = Time.time;
        m_timeOfNextMoveDown = Time.time;
        m_timeOfNextRotate = Time.time;
        m_timeOfNextShapeDrop = Time.time + m_shapeDropCooldown;

        m_isAllowedToHold = false;
        m_isGameOver = false;
    }

    private void Start()
    {
        if (m_panelGameOver == null)
        {
            Debug.Log("ERROR! m_panelGameOver is unassigned!");
        }

        if (m_sctGameBoard == null)
        {
            Debug.Log("ERROR! SctGameBoard not found!");
        }

        if (m_sctShapeSpawner != null)
        {
            m_sctShapeSpawner.transform.position = nsVectorf.Vectorf.RoundToFloat(m_sctShapeSpawner.transform.position);
            if (m_movingShape == null)
            {
                m_movingShape = m_sctShapeSpawner.SpawnRandomShape();
            }
        }
        else
        {
            Debug.Log("ERROR! SctShapeSpawner not found!");
        }
    }

    private void Update()
    {
        if (m_sctGameBoard == null || m_sctShapeSpawner == null || m_movingShape == null || m_isGameOver || m_panelGameOver == null) return;
        if (Input.anyKey) HandleInput();
        if (Time.time > m_timeOfNextShapeDrop)
        {
            m_timeOfNextShapeDrop = Time.time + m_shapeDropCooldown;
            HandleShapeDrop();
        }
    }

    private void HandleInput()
    {
        if ((Input.GetButton("MoveLeft") && (Time.time > m_timeOfNextMoveLeft) && m_isAllowedToHold) || Input.GetButtonDown("MoveLeft"))
        {
            m_timeOfNextMoveLeft = Time.time + m_moveLeftCooldown;
            m_isAllowedToHold = true;
            m_movingShape.MoveLeft();
            if (!m_sctGameBoard.IsPositionValid(m_movingShape)) m_movingShape.MoveRight();
        }
        else if ((Input.GetButton("MoveRight") && (Time.time > m_timeOfNextMoveRight) && m_isAllowedToHold) || Input.GetButtonDown("MoveRight"))
        {
            m_timeOfNextMoveRight = Time.time + m_moveRightCooldown;
            m_isAllowedToHold = true;
            m_movingShape.MoveRight();
            if (!m_sctGameBoard.IsPositionValid(m_movingShape)) m_movingShape.MoveLeft();
        }
        if ((Input.GetButton("MoveDown") && (Time.time > m_timeOfNextMoveDown) && m_isAllowedToHold) || Input.GetButtonDown("MoveDown"))
        {
            m_timeOfNextMoveDown = Time.time + m_moveDownCooldown;
            m_isAllowedToHold = true;
            HandleShapeDrop();
        }
        if ((Input.GetButton("Rotate") && (Time.time > m_timeOfNextRotate) && m_isAllowedToHold) || Input.GetButtonDown("Rotate"))
        {
            m_timeOfNextRotate = Time.time + m_rotateCooldown;
            m_isAllowedToHold = true;
            m_movingShape.RotateCW();
            if (!m_sctGameBoard.IsPositionValid(m_movingShape)) m_movingShape.RotateCCW();
        }
    }

    private void HandleShapeDrop()
    {
        m_movingShape.MoveDown();
        if (!m_sctGameBoard.IsPositionValid(m_movingShape))
        {
            m_movingShape.MoveUp();
            m_sctGameBoard.StoreShapeInGrid(m_movingShape);
            m_sctGameBoard.ClearAllCompleteRows();
            m_isGameOver = m_sctGameBoard.IsShapeInHeaderSpace();
            if (!m_isGameOver)
            {
                m_movingShape = m_sctShapeSpawner.SpawnRandomShape();
                m_timeOfNextShapeDrop = Time.time + m_shapeDropCooldown;
                m_isAllowedToHold = false;
            }
            else
            {
                m_panelGameOver.SetActive(true);
            }
        }
    }
}
