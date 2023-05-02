using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace User_Interface
{
    public class OverViewportCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool isOverViewport;
        public bool IsOverViewport => isOverViewport;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isOverViewport = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOverViewport = false;
        }
    }
}
