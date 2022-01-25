using nsMovingShape;
using System;

namespace nsEventManager
{
    public class EventManager
    {
        public static event Action<SctMovingShape> OnShapeSpawn;

        public static void InvokeOnShapeSpawn(SctMovingShape sctMovingShape)
        {
            OnShapeSpawn?.Invoke(sctMovingShape);
        }
    }
}
