using UnityEditor;
using User_Interface;

namespace Editor
{
    [CustomEditor(typeof(TooltipTrigger))]
    public class TooltipTriggerEditor : UnityEditor.Editor
    {
        SerializedProperty textToDisplay;
        SerializedProperty displayAtMousePosition;
        SerializedProperty tooltipPosition;
    
        void OnEnable()
        {
            textToDisplay = serializedObject.FindProperty("textToDisplay");
            displayAtMousePosition = serializedObject.FindProperty("displayAtMousePosition");
            tooltipPosition = serializedObject.FindProperty("tooltipPosition");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(textToDisplay);

            EditorGUILayout.PropertyField(displayAtMousePosition);
            if(!displayAtMousePosition.boolValue)
            {
                EditorGUILayout.PropertyField(tooltipPosition);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}