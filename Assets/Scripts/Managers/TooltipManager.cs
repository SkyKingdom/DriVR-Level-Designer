using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
    public class TooltipManager : MonoBehaviour
    {
        [SerializeField] private float tooltipOffset = 16f;  
        [SerializeField] private RectTransform tooltipTransform;
        [SerializeField] private TextMeshProUGUI tooltipText;
        
        public void DisplayTooltip(string text, bool inRightHalf, bool inTopHalf)
        {
            tooltipTransform.gameObject.SetActive(true);
            tooltipText.text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipTransform);
            tooltipTransform.position = GetOffsetPosition(inRightHalf, inTopHalf);
        }
        
        public void DisplayTooltip(string text, Vector3 position)
        {
            tooltipTransform.gameObject.SetActive(true);
            tooltipText.text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipTransform);
            tooltipTransform.position = position;
        }
        
        public void RemoveTooltip()
        {
            tooltipTransform.gameObject.SetActive(false);
        }

        public float GetTooltipWidth()
        {
            return tooltipText.preferredWidth / 2;
        }
        
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