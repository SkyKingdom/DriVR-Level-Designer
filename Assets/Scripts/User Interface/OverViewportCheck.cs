using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace User_Interface
{
    /// <summary>
    /// Class for checking if the mouse is over the viewport
    /// </summary>
    public class OverViewportCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static bool IsOverViewport;
        public void OnPointerEnter(PointerEventData eventData)
        {
            IsOverViewport = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsOverViewport = true;
        }
    }
}
