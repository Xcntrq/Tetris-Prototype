using UnityEngine;

namespace nsShape
{
    public class SctShape : MonoBehaviour
    {
        public void MoveUp()
        {
            transform.Translate(Vector3.up, Space.World);
        }

        public void MoveDown()
        {
            transform.Translate(Vector3.down, Space.World);
        }

        public Color Color
        {
            set
            {
                SpriteRenderer[] childSprites = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer childSprite in childSprites)
                {
                    childSprite.color = value;
                }
            }
        }
    }
}
