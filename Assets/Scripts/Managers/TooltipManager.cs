using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Managers
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private float tooltipOffset = 16f;  // Extra offset to prevent tooltip from overlapping mouse
        
        // Tooltip references
        [Header("Tooltip References"), SerializeField] private RectTransform tooltipTransform;
        [SerializeField] private TextMeshProUGUI tooltipText;
        
        // Display tooltip at mouse position
        public void DisplayTooltip(string text, bool inRightHalf, bool inTopHalf)
        {
            tooltipTransform.gameObject.SetActive(true);
            tooltipText.text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipTransform);
            tooltipTransform.position = GetOffsetPosition(inRightHalf, inTopHalf);
        }
        
        // Display tooltip at given position
        public void DisplayTooltip(string text, Vector3 position)
        {
            tooltipTransform.gameObject.SetActive(true);
            tooltipText.text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipTransform);
            tooltipTransform.position = position;
        }
        
        // Hide tooltip
        public void RemoveTooltip()
        { 
            tooltipTransform.gameObject.SetActive(false);
        }

        // Get width of tooltip
        public float GetTooltipWidth()
        {
            return tooltipText.preferredWidth / 2;
        }
        
        // Get offset position for tooltip relative to mouse position
        private Vector3 GetOffsetPosition(bool inRightHalf, bool inTopHalf)
        {
            var position = Mouse.current.position.ReadValue();
            var offset = Vector2.zero;
            if (inRightHalf)
            {
                offset.x = -(GetTooltipWidth()/2 + tooltipOffset);
            }
            else
            {
                offset.x = GetTooltipWidth()/2 + tooltipOffset;
            }

            if (inTopHalf)
            {
                offset.y = -tooltipText.preferredHeight/2;
            }
            else
            {
                offset.y = tooltipText.preferredHeight/2;
            }

            return position + offset;
        }
    }
}