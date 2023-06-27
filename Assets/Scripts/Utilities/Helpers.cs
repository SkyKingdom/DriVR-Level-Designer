using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    public static class Helpers
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

        // Get a WaitForSeconds object for a given time
        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;
            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }

        private static PointerEventData _EventDataCurrentPosition;
        private static List<RaycastResult> _Results;
    }
}