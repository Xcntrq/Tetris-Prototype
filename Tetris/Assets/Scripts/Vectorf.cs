using UnityEngine;

namespace nsVectorf
{
    public static class Vectorf
    {
        public static Vector3 RoundToFloat(Vector3 vector)
        {
            float x = Mathf.Round(vector.x);
            float y = Mathf.Round(vector.y);
            float z = Mathf.Round(vector.z);
            return new Vector3(x, y, z);
        }

        public static Vector3Int RoundToInt(Vector3 vector)
        {
            int x = (int)Mathf.Round(vector.x);
            int y = (int)Mathf.Round(vector.y);
            int z = (int)Mathf.Round(vector.z);
            return new Vector3Int(x, y, z);
        }
    }
}
