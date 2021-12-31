using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nsGameBoard
{
    public class SctGameBoard : MonoBehaviour
    {
        [SerializeField] private Transform m_emptySquaresParent;
        [SerializeField] private Transform m_emptySquare;
        [SerializeField] private int m_header;
        [SerializeField] private int m_height;
        [SerializeField] private int m_width;
        [SerializeField] private float m_delayBeforeRowsDie;

        //Stores shapes that have stopped moving
        private Transform[,] m_grid;
        private nsParticleRow.SctParticleRow[] m_sctParticleRows;
        public event Action OnRowClear;

        private void Awake()
        {
            m_grid = new Transform[m_width, m_height];
            m_sctParticleRows = GetComponentsInChildren<nsParticleRow.SctParticleRow>();
        }

        private void Start()
        {
            //Instantiates the gameboard if the prefab is set
            DrawEmptySquares();
        }

        private void DrawEmptySquares()
        {
            if (m_emptySquare == null)
            {
                Debug.Log("ERROR! m_emptySquare is unassigned!");
                return;
            }
            for (int y = 0; y < m_height - m_header; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Transform newEmptySquare = Instantiate(m_emptySquare, new Vector3(x, y, 0), Quaternion.identity);
                    newEmptySquare.name = "Square at " + x.ToString() + "," + y.ToString();
                    newEmptySquare.parent = m_emptySquaresParent;
                }
            }
        }

        public bool IsPositionValid(nsGhostShape.SctGhostShape shape)
        {
            //Check every square of the given shape
            foreach (Transform child in shape.transform)
            {
                Vector3Int childPosition = nsVectorf.Vectorf.RoundToInt(child.position);
                int x = childPosition.x;
                int y = childPosition.y;

                //The position of the shape is invalid in 2 cases:
                //1. If a square is not within the grid
                bool isWithinGrid = (x >= 0) && (x < m_width) && (y >= 0) && (y < m_height);
                if (!isWithinGrid) return false;

                //2. If a square is at a position occupied by previously landed shapes
                //It's within the grid though (because of the previous 2 lines)
                //So it's safe to check like this, can't cause a null-ref exception
                bool hasShapeStored = m_grid[x, y] != null;
                if (hasShapeStored) return false;
            }
            return true;
        }

        public void StoreShapeInGrid(nsMovingShape.SctMovingShape shape)
        {
            if (shape == null) return;
            //Children squares are going to live with their grampa
            Transform newParent = shape.transform.parent;
            List<Transform> children = new List<Transform>();
            foreach (Transform child in shape.transform)
            {
                children.Add(child);
            }
            //Loops through every square of the shape, puts in the grid
            foreach (Transform child in children)
            {
                Vector3Int childPosition = nsVectorf.Vectorf.RoundToInt(child.position);
                int x = childPosition.x;
                int y = childPosition.y;
                m_grid[x, y] = child;
                child.parent = newParent;
            }
            Destroy(shape.gameObject);
        }

        private bool IsRowCompleteAt(int y)
        {
            for (int x = 0; x < m_width; x++)
            {
                if (m_grid[x, y] == null) return false;
            }
            return true;
        }

        private void ClearRowAt(int y)
        {
            for (int x = 0; x < m_width; x++)
            {
                //Destroys a row of squares in the grid at a given Y
                if (m_grid[x, y] != null) Destroy(m_grid[x, y].gameObject);
                m_grid[x, y] = null;
            }
        }

        private void ShiftRowDownAt(int y)
        {
            for (int x = 0; x < m_width; x++)
            {
                if (m_grid[x, y] != null)
                {
                    //For every non-null square in a given row move it down 1 tile, in the world space and in the grid
                    m_grid[x, y].Translate(Vector3.down, Space.World);
                    m_grid[x, y - 1] = m_grid[x, y];
                    m_grid[x, y] = null;
                }
            }
        }

        private void ShiftRowsDownFrom(int y)
        {
            for (int i = y; i < m_height; i++)
            {
                ShiftRowDownAt(i);
            }
        }

        public int ClearAllCompleteRows()
        {
            List<int> rowsToClear = new List<int>();
            for (int y = 0; y < m_height; y++)
            {
                if (IsRowCompleteAt(y))
                {
                    TriggerFxRowClear(rowsToClear.Count, y);
                    rowsToClear.Add(y);
                }
            }
            if (rowsToClear.Count > 0) StartCoroutine(ClearAllCompleteRowsRoutine(rowsToClear));
            return rowsToClear.Count;
        }

        private void TriggerFxRowClear(int i, int y)
        {
            if (m_sctParticleRows == null) return;
            m_sctParticleRows[i].transform.position = new Vector3(0, y, m_sctParticleRows[i].transform.position.z);
            m_sctParticleRows[i].Play();
        }

        private IEnumerator ClearAllCompleteRowsRoutine(List<int> rowsToClear)
        {
            yield return new WaitForSeconds(m_delayBeforeRowsDie);
            int rowsCount = rowsToClear.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                int rowIndex = rowsToClear[i] - i;
                ClearRowAt(rowIndex);
                ShiftRowsDownFrom(rowIndex + 1);
            }
            OnRowClear?.Invoke();
        }

        public bool IsShapeInHeaderSpace()
        {
            //Check every (X, Y) above the visible grid for a non-null entry
            for (int y = m_height - m_header; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    if (m_grid[x, y] != null) return true;
                }
            }
            return false;
        }
    }
}
