using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    public static class Helpers
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;
            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        private static PointerEventData _EventDataCurrentPosition;
        private static List<RaycastResult> _Results;

        public static bool IsOverUi(Vector2 position)
        {
            _EventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = position };
            _Results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_EventDataCurrentPosition, _Results);
            return _Results.Count > 0;
        }
        
        public static void DestroyChildren(this Transform t)
        {
            foreach (Transform child in t)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}