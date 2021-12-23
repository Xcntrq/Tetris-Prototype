using UnityEngine;
using UnityEngine.UI;

namespace nsScoreManager
{
    public class SctScoreManager : MonoBehaviour
    {
        [SerializeField] private int m_linesPerLevel;

        [SerializeField] private Text m_textScore;
        [SerializeField] private Text m_textLines;
        [SerializeField] private Text m_textLevel;

        private int m_score;
        private int m_lines;
        private int m_level;

        public int Level
        {
            get { return m_level; }
        }

        private void Awake()
        {
            m_score = 0;
            m_level = 1;
            m_lines = m_linesPerLevel;
            UpdateUIValues();
        }

        public bool AddScore(int rowsCleared)
        {
            bool hasLeveledUp = false;
            switch (rowsCleared)
            {
                case 1:
                    m_score += 40 * m_level;
                    break;
                case 2:
                    m_score += 100 * m_level;
                    break;
                case 3:
                    m_score += 300 * m_level;
                    break;
                case 4:
                    m_score += 1200 * m_level;
                    break;
            }
            m_lines -= rowsCleared;
            if (m_lines <= 0)
            {
                LevelUp();
                hasLeveledUp = true;
            }
            UpdateUIValues();
            return hasLeveledUp;
        }

        private void UpdateUIValues()
        {
            if (m_textScore) m_textScore.text = m_score.ToString("D5");
            if (m_textLines) m_textLines.text = m_lines.ToString();
            if (m_textLevel) m_textLevel.text = m_level.ToString();
        }

        public void LevelUp()
        {
            m_level++;
            m_lines = m_level * m_linesPerLevel;
        }
    }
}
