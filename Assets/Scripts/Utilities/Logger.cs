using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities
{
    [AddComponentMenu("RehabRevolution/Helpers/Logger")]
    public class Logger : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private bool showLogs;

        [SerializeField] private string prefix;

        [SerializeField] private Color prefixColor;

        private string _hexColor;

        private void OnValidate()
        {
            _hexColor = "#"+ColorUtility.ToHtmlStringRGB(prefixColor);
        }

        public void Log(object message, Object sender)
        {
            if (!showLogs) return;
        
            Debug.Log($"<color={_hexColor}>{prefix}: {message}</color>", sender);
        }

        [ContextMenu("Test")]
        public void Test()
        {
            Log("Hello World", this);
        }
    }
}
