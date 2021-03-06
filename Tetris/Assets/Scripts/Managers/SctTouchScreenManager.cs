using System;
using UnityEngine;

namespace nsTouchScreenManager
{
    public class SctTouchScreenManager : MonoBehaviour
    {
        [SerializeField] private int m_swipeTriggeringDistance;

        private Vector2 m_touchVector;

        public static event Action<Vector2> OnSwipe;
        public static event Action<Vector2> OnSwipeEnd;

        private void Update()
        {
            if (Input.touchCount == 0) return;
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    m_touchVector = Vector2.zero;
                    Debug.Log("TouchPhase.Began");
                    break;
                case TouchPhase.Moved:
                    Debug.Log("TouchPhase.Moved");
                    m_touchVector += touch.deltaPosition;
                    if (m_touchVector.magnitude > m_swipeTriggeringDistance) OnSwipe?.Invoke(m_touchVector);
                    break;
                case TouchPhase.Stationary:
                    Debug.Log("TouchPhase.Stationary");
                    m_touchVector += touch.deltaPosition;
                    if (m_touchVector.magnitude > m_swipeTriggeringDistance) OnSwipe?.Invoke(m_touchVector);
                    break;
                case TouchPhase.Ended:
                    Debug.Log("TouchPhase.Ended");
                    OnSwipeEnd?.Invoke(m_touchVector);
                    break;
            }
        }
    }
}
