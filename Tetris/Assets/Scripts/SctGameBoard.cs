using UnityEngine;

namespace nsGameBoard
{
    public class SctGameBoard : MonoBehaviour
    {
        [SerializeField] private Transform m_emptySquare;
        [SerializeField] private int m_boardHeader;
        [SerializeField] private int m_boardHeight;
        [SerializeField] private int m_boardWidth;

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
            for (int y = 0; y < m_boardHeight - m_boardHeader; y++)
            {
                for (int x = 0; x < m_boardWidth; x++)
                {
                    Transform newEmptySquare = Instantiate(m_emptySquare, new Vector3(x, y, 0), Quaternion.identity);
                    newEmptySquare.name = "Square at " + x.ToString() + "," + y.ToString();
                    newEmptySquare.parent = transform;
                }
            }
        }
    }
}
