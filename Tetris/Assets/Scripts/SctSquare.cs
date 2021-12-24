using System;
using UnityEngine;

namespace nsSquare
{
    public class SctSquare : MonoBehaviour
    {
        public event Action MyOnDestroy;

        private void OnDestroy()
        {
            MyOnDestroy?.Invoke();
        }
    }
}
