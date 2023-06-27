using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace User_Interface
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {      
        private const float HoverTime = 2f; // Time before tooltip is displayed
        
        [SerializeField, TextArea(3, 6)] private string textToDisplay; // Text to display in tooltip
        
        [Header("Positioning"), SerializeField] private bool displayAtMousePosition; // If true, tooltip will display at mouse position
        
        [SerializeField] private Vector3 tooltipPosition; // Position to display tooltip at if displayAtMousePosition is false
        
        // Stores screen width and height
        private int _screenWidth;
        private int _screenHeight;
        
        // Stores time since mouse entered trigger
        private float _timer;
        
        // Stores whether mouse is hovering over trigger
        private bool _isHovering;
        private void Start()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }
        
        private void Update()
        {
            if (!_isHovering) return; // If mouse is not hovering over trigger, do nothing
            
            _timer += Time.deltaTime; // Increment timer
            
            // If mouse has been hovering over trigger for HoverTime, display tooltip
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
        
        // Start timer when mouse enters trigger
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
        }

        // Reset timer when mouse exits trigger
        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
            DesignerManager.Instance.DesignerUIManager.TooltipManager.RemoveTooltip();
        }

        // Get mouse position relative to screen
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