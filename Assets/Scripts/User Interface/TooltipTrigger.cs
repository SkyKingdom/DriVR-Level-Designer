using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace User_Interface
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {      
        private const float HoverTime = 2f;
        
        [SerializeField, TextArea(3, 6)] private string textToDisplay;
        [Header("Positioning"), SerializeField] private bool displayAtMousePosition;
        [SerializeField] private Vector3 tooltipPosition;
        
        private int _screenWidth;
        private int _screenHeight;
        private float _timer;
        private bool _isHovering;
        private void Start()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }
        
        private void Update()
        {
            if (!_isHovering) return;
            
            _timer += Time.deltaTime;
            
            if (_timer >= HoverTime)
            {
                if (displayAtMousePosition)
                {
                    var mouseCoords = GetMousePosition();
                    DesignerManager.Instance.DesignerUIManager.TooltipManager.DisplayTooltip(textToDisplay, mouseCoords[0], mouseCoords[1]);
                }
                else 
                {
                    DesignerManager.Instance.DesignerUIManager.TooltipManager.DisplayTooltip(textToDisplay, tooltipPosition);
                }
                _isHovering = false;
                _timer = 0f;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
            DesignerManager.Instance.DesignerUIManager.TooltipManager.RemoveTooltip();
        }

        private bool[] GetMousePosition()
        {
            bool[] mouseCoords = new bool[2];
            var mousePos = Mouse.current.position.ReadValue();
            
            mouseCoords[0] = mousePos.x > _screenWidth / 2;
            mouseCoords[1] = mousePos.y > _screenHeight / 2;
           
            return mouseCoords;
        }
    }
}