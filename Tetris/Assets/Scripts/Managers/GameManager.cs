using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RotationDirection
{
    CW,
    CCW
}

public class GameManager : MonoBehaviour
{
    private nsGameBoard.SctGameBoard m_sctGameBoard;
    private nsShapeSpawner.SctShapeSpawner m_sctShapeSpawner;
    private nsMovingShape.SctMovingShape m_movingShape;
    private nsSoundManager.SctSoundManager m_soundManager;
    private nsScoreManager.SctScoreManager m_scoreManager;
    private nsShapeHolder.SctShapeHolder m_shapeHolder;

    [SerializeField] private RotationDirection m_rotationDirection;
    [SerializeField] private nsImageTogglerRotate.SctImageTogglerRotate m_imageTogglerRotate;

    [Space]

    [SerializeField] private GameObject m_panelGameOver;
    [SerializeField] private GameObject m_panelGamePaused;

    [Space]

    //Minimum amount of time between procs for each key while holding
    [SerializeField] private float m_moveLeftCooldown;
    [SerializeField] private float m_moveRightCooldown;
    [SerializeField] private float m_moveDownCooldown;
    [SerializeField] private float m_rotateCooldown;

    //Time interval used to drop the current moving shape by 1 tile automatically
    [SerializeField] private float m_shapeDropInterval;
    private float m_shapeDropCooldownAtStart;

    //Absolute timestamp when each key can proc
    private float m_timeOfNextMoveLeft;
    private float m_timeOfNextMoveRight;
    private float m_timeOfNextMoveDown;
    private float m_timeOfNextRotate;

    //Absolute timestamp when the current moving shape has to be dropped by 1 tile automatically
    private float m_timeOfNextShapeDrop;

    //Prevents any held key from working on the next spawned shape
    private bool m_isAllowedToHold;

    //Do we need a comment on that one? Srsly?
    private bool m_isGameOver;
    private bool m_isGamePaused;

    public event Action OnGameOver;
    public event Action OnShapeDrop;
    public event Action OnShapeMoveError;
    public event Action OnShapeMoveSuccess;
    public event Action<int, bool> OnRowClear;
    public event Action<bool> OnPauseToggled;

    private void Awake()
    {
        m_sctGameBoard = FindObjectOfType<nsGameBoard.SctGameBoard>();
        m_sctShapeSpawner = FindObjectOfType<nsShapeSpawner.SctShapeSpawner>();
        m_soundManager = FindObjectOfType<nsSoundManager.SctSoundManager>();
        m_scoreManager = FindObjectOfType<nsScoreManager.SctScoreManager>();
        m_shapeHolder = FindObjectOfType<nsShapeHolder.SctShapeHolder>();

        //Any key is allowed to proc as soon as the game has started
        m_timeOfNextMoveLeft = Time.time;
        m_timeOfNextMoveRight = Time.time;
        m_timeOfNextMoveDown = Time.time;
        m_timeOfNextRotate = Time.time;

        m_isAllowedToHold = false;
        m_isGameOver = false;

        OnGameOver += HandleGameOver;
        m_shapeDropCooldownAtStart = m_shapeDropInterval;
    }

    private void Start()
    {
        if (m_sctShapeSpawner != null)
        {
            //Shapes spawn relative to the Transform of the Spawner, so just in case, round it with Vectorf
            m_sctShapeSpawner.transform.position = nsVectorf.Vectorf.RoundToFloat(m_sctShapeSpawner.transform.position);
            if (m_movingShape == null) m_movingShape = m_sctShapeSpawner.GetNextShape();
            //Assuming a new shape has been created, it shouldn't start falling down immediately
            m_timeOfNextShapeDrop = Time.time + m_shapeDropInterval;
        }
        IsAnyRequiredObjectNull(true);
        m_imageTogglerRotate.SetImage(m_rotationDirection);
    }

    private void Update()
    {
        if (IsAnyRequiredObjectNull(false) || m_isGameOver || m_isGamePaused) return;
        if (Input.anyKey) HandleInput();
        if (Time.time > m_timeOfNextShapeDrop)
        {
            m_timeOfNextShapeDrop = Time.time + m_shapeDropInterval;
            HandleShapeDrop();
        }
    }

    private bool IsAnyRequiredObjectNull(bool isDebugLogNeeded)
    {
        bool result = false;
        if (m_sctGameBoard == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! SctGameBoard not found!");
        }
        if (m_sctShapeSpawner == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! SctShapeSpawner not found!");
        }
        if (m_movingShape == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! MovingShape not found!");
        }
        if (m_panelGameOver == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! m_panelGameOver is unassigned!");
        }
        if (m_panelGamePaused == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! m_panelGamePaused is unassigned!");
        }
        if (m_soundManager == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! SoundManager not found!");
        }
        if (m_scoreManager == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! ScoreManager not found!");
        }
        if (m_shapeHolder == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! ShapeHolder not found!");
        }
        return result;
    }

    private void HandleInput()
    {
        //If held down, the key procs only if enough time has passed and if it's still applied to the same shape
        //Otherwise you have let go of the key and press it again, this also allows for faster movements and rotations
        //m_isAllowedToHold mainly helps the "down" key, because if you quickly drop one shape, you don't wanna drop the next one too
        if ((Input.GetButton("MoveLeft") && (Time.time > m_timeOfNextMoveLeft) && m_isAllowedToHold) || Input.GetButtonDown("MoveLeft"))
        {
            m_timeOfNextMoveLeft = Time.time + m_moveLeftCooldown;
            //Once any key has been pressed for the current shape, it allows to hold any other key for that shape
            m_isAllowedToHold = true;
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
        else if ((Input.GetButton("MoveRight") && (Time.time > m_timeOfNextMoveRight) && m_isAllowedToHold) || Input.GetButtonDown("MoveRight"))
        {
            m_timeOfNextMoveRight = Time.time + m_moveRightCooldown;
            m_isAllowedToHold = true;
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
            m_movingShape.Rotate(m_rotationDirection);
            if (m_sctGameBoard.IsPositionValid(m_movingShape))
            {
                OnShapeMoveSuccess?.Invoke();
            }
            else
            {
                OnShapeMoveError?.Invoke();
                m_movingShape.RotateOppositeDirection(m_rotationDirection);
            }
        }
    }

    private void HandleShapeDrop()
    {
        //Try moving it down
        m_movingShape.MoveDown();
        if (!m_sctGameBoard.IsPositionValid(m_movingShape))
        {
            //If the new position is against the rules of tetris, just move it back to where it was
            m_movingShape.MoveUp();
            //Also this means that the bottom of the shape has hit something, landed, so in the grid it goes and maybe completes some rows
            m_sctGameBoard.StoreShapeInGrid(m_movingShape);
            int rowsCleared = m_sctGameBoard.ClearAllCompleteRows();
            if (rowsCleared > 0)
            {
                bool hasLeveledUp = m_scoreManager.AddScore(rowsCleared);
                OnRowClear?.Invoke(rowsCleared, hasLeveledUp);
                if (hasLeveledUp) m_shapeDropInterval = Mathf.Clamp(m_shapeDropCooldownAtStart - 0.05f * (m_scoreManager.Level - 1), 0.05f, 1f);
            }
            else
            {
                OnShapeDrop?.Invoke();
            }
            //If the shape landed above the visible grid, it's a game over
            m_isGameOver = m_sctGameBoard.IsShapeInHeaderSpace();
            if (m_isGameOver)
            {
                OnGameOver?.Invoke();
            }
            else
            {
                //Otherwise we're gonna need a new shape, which is also not allowed to drop immediately, hence the cooldown
                m_movingShape = m_sctShapeSpawner.GetNextShape();
                m_timeOfNextShapeDrop = Time.time + m_shapeDropInterval;
                //If the player is holding down any buttons, the new shape shouldn't be affected
                m_isAllowedToHold = false;
            }
        }
    }

    private void HandleGameOver()
    {
        m_panelGameOver.SetActive(true);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleRotationDirection()
    {
        switch (m_rotationDirection)
        {
            case RotationDirection.CW:
                m_rotationDirection = RotationDirection.CCW;
                break;
            case RotationDirection.CCW:
                m_rotationDirection = RotationDirection.CW;
                break;
        }
        m_imageTogglerRotate.SetImage(m_rotationDirection);
    }

    public void TogglePause()
    {
        if (m_isGameOver) return;
        if (!m_panelGamePaused) return;
        m_isGamePaused = !m_isGamePaused;
        m_panelGamePaused.SetActive(m_isGamePaused);
        OnPauseToggled?.Invoke(m_isGamePaused);
        Time.timeScale = m_isGamePaused ? 0 : 1;
    }

    public void HandleShapeHolding()
    {
        m_movingShape = m_shapeHolder.HoldShape(m_movingShape.ToSctShapeProperties());
        if (m_movingShape == null)
        {
            m_movingShape = m_sctShapeSpawner.GetNextShape();
            m_timeOfNextShapeDrop = Time.time + m_shapeDropInterval;
            //If the player is holding down any buttons, the new shape shouldn't be affected
            m_isAllowedToHold = false;
        }
    }
}
