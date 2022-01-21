using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private nsInputManager.SctInputManager m_inputManager;

    [SerializeField] private RotationDirection m_rotationDirection;
    [SerializeField] private nsImageTogglerRotate.SctImageTogglerRotate m_imageTogglerRotate;

    [Space]

    [SerializeField] private GameObject m_panelGameOver;
    [SerializeField] private GameObject m_panelGamePaused;

    [Space]

    //Time interval used to drop the current moving shape by 1 tile automatically
    [SerializeField] private float m_shapeDropInterval;
    private float m_shapeDropCooldownAtStart;

    [SerializeField] private Text m_textStart;

    //Absolute timestamp when the current moving shape has to be dropped by 1 tile automatically
    private float m_timeOfNextShapeDrop;

    //Do we need a comment on that? Srsly?
    private bool m_isGameOver;
    private bool m_isGamePaused;
    private bool m_isMovingShapeNeeded;

    public event Action OnGameOver;
    public event Action OnShapeHold;
    public event Action<int, bool> OnShapeDrop;
    public event Action<bool> OnPauseToggled;

    public RotationDirection GetRotationDirection { get { return m_rotationDirection; } }

    private void Awake()
    {
        m_sctGameBoard = FindObjectOfType<nsGameBoard.SctGameBoard>();
        m_sctShapeSpawner = FindObjectOfType<nsShapeSpawner.SctShapeSpawner>();
        m_soundManager = FindObjectOfType<nsSoundManager.SctSoundManager>();
        m_scoreManager = FindObjectOfType<nsScoreManager.SctScoreManager>();
        m_shapeHolder = FindObjectOfType<nsShapeHolder.SctShapeHolder>();
        m_inputManager = FindObjectOfType<nsInputManager.SctInputManager>();

        m_isGameOver = false;

        OnGameOver += HandleGameOver;
        m_shapeDropCooldownAtStart = m_shapeDropInterval;

        m_sctGameBoard.OnRowClear += SctGameBoard_OnRowClear;
    }

    private void Start()
    {
        if (m_sctShapeSpawner != null)
        {
            //Shapes spawn relative to the Transform of the Spawner, so just in case, round it with Vectorf
            m_sctShapeSpawner.transform.position = nsVectorf.Vectorf.RoundToFloat(m_sctShapeSpawner.transform.position);
        }
        IsAnyRequiredObjectNull(true);
        m_imageTogglerRotate.SetImage(m_rotationDirection);
        m_isMovingShapeNeeded = true;
    }

    private void Update()
    {
        if (IsAnyRequiredObjectNull(false) || m_isGameOver || m_isGamePaused) return;
        if (m_isMovingShapeNeeded == true) SpawnNewMovingShape();
        if (m_movingShape == null) return;
        if (Input.anyKey) m_inputManager.HandleInput(m_movingShape);
        if (Time.time > m_timeOfNextShapeDrop)
        {
            m_timeOfNextShapeDrop = Time.time + m_shapeDropInterval;
            HandleShapeDrop(false);
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
        if (m_inputManager == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! InputManager not found!");
        }
        if (m_shapeHolder == null)
        {
            result = true;
            if (isDebugLogNeeded) Debug.Log("ERROR! ShapeHolder not found!");
        }
        return result;
    }

    public void HandleShapeDrop(bool isShapeReceivingInput)
    {
        //Try moving it down
        m_movingShape.MoveDown(isShapeReceivingInput);
        if (!m_sctGameBoard.IsPositionValid(m_movingShape))
        {
            //If the new position is against the rules of tetris, just move it back to where it was
            m_movingShape.MoveUp();
            //Also this means that the bottom of the shape has hit something, landed, so in the grid it goes and maybe completes some rows
            m_sctGameBoard.StoreShapeInGrid(m_movingShape);
            int rowsCleared = m_sctGameBoard.ClearAllCompleteRows();
            bool hasLeveledUp = m_scoreManager.AddScore(rowsCleared);
            if (hasLeveledUp) m_shapeDropInterval = Mathf.Clamp(m_shapeDropCooldownAtStart - 0.05f * (m_scoreManager.Level - 1), 0.05f, 1f);
            OnShapeDrop?.Invoke(rowsCleared, hasLeveledUp);
        }
    }

    private void HandleGameOver()
    {
        if (m_movingShape != null) Destroy(m_movingShape.gameObject);
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
        m_textStart.gameObject.SetActive(!m_isGamePaused);
        m_panelGamePaused.SetActive(m_isGamePaused);
        OnPauseToggled?.Invoke(m_isGamePaused);
        Time.timeScale = m_isGamePaused ? 0 : 1;
    }

    public void HandleShapeHolding()
    {
        m_movingShape = m_shapeHolder.HoldShape(m_movingShape);
        OnShapeHold?.Invoke();
    }

    public void SctGameBoard_OnRowClear()
    {
        //If the shape landed above the visible grid, it's a game over
        m_isGameOver = m_sctGameBoard.IsShapeInHeaderSpace();
        if (m_isGameOver)
        {
            OnGameOver?.Invoke();
        }
        else
        {
            m_isMovingShapeNeeded = true;
        }
    }

    private void SpawnNewMovingShape()
    {
        if (m_movingShape == null)
        {
            m_movingShape = m_sctShapeSpawner.GetNextShape();
            if (m_movingShape == null)
            {
                Debug.Log("Couldn't spawn a new moving shape for some reason!");
            }
            else
            {
                //Assuming a new shape has been created, it shouldn't start falling down immediately
                m_timeOfNextShapeDrop = Time.time + m_shapeDropInterval;
                m_isMovingShapeNeeded = false;
            }
        }
        else
        {
            Debug.Log("Can't spawn new moving shape! There's already a moving shape present!");
        }
    }
}
