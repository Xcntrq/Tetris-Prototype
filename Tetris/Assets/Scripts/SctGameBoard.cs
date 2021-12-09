using UnityEngine;

namespace nsGameBoard
{
    public class SctGameBoard : MonoBehaviour
    {
        [SerializeField] private Transform m_emptySquare;
        [SerializeField] private int m_header;
        [SerializeField] private int m_height;
        [SerializeField] private int m_width;

        private Transform[,] m_grid;

        private void Awake()
        {
            m_grid = new Transform[m_width, m_height];
        }

        private void Start()
        {
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
                    newEmptySquare.parent = transform;
                }
            }
        }

        public bool IsPositionValid(nsShape.SctShape shape)
        {
            foreach (Transform child in shape.transform)
            {
                Vector3Int childPosition = nsVectorf.Vectorf.RoundToInt(child.position);
                int x = childPosition.x;
                int y = childPosition.y;

                bool isWithinBoard = (x >= 0) && (x < m_width) && (y >= 0) && (y < m_height);
                if (!isWithinBoard) return false;

                //It's within board so it's safe to check here, can't cause a null-ref exception
                bool hasShapeStored = m_grid[x, y] != null;
                if (hasShapeStored) return false;
            }
            return true;
        }

        public void StoreShapeInGrid(nsShape.SctShape shape)
        {
            if (shape == null) return;

            foreach (Transform child in shape.transform)
            {
                Vector3Int childPosition = nsVectorf.Vectorf.RoundToInt(child.position);
                int x = childPosition.x;
                int y = childPosition.y;
                m_grid[x, y] = child;
            }
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

        public void ClearAllCompleteRows()
        {
            for (int y = 0; y < m_height; y++)
            {
                if (IsRowCompleteAt(y))
                {
                    ClearRowAt(y);
                    ShiftRowsDownFrom(y + 1);
                    y--;
                }
            }
        }

        public bool IsShapeInHeaderSpace()
        {
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
