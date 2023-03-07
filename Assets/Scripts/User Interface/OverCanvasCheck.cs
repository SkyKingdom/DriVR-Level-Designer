using UnityEngine;
using UnityEngine.EventSystems;

namespace User_Interface
{
    public class OverCanvasCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool isOverCanvas;
        public bool IsOverCanvas => isOverCanvas;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isOverCanvas = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOverCanvas = false;
        }
    }
}
