using System.Collections;
using TMPro;
using UnityEngine;
using Utilities;

namespace User_Interface
{
    public struct LevelInfo
    {
        public string Name { get; }
        public string Description { get; }

        public LevelInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
    public class LevelInfoModal : StaticInstance<LevelInfoModal>
    {
        public static bool Success { get; private set; }
        public static LevelInfo Result { get; private set; }
        [SerializeField] private TMP_InputField levelNameInputField;
        [SerializeField] private TMP_InputField levelDescriptionInputField;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void SetSuccess(bool success)
        {
            Success = success;
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            SetSuccess(false);
            Hide();
        }
        
        public void Confirm()
        {
            if (levelNameInputField.text == "" || levelDescriptionInputField.text == "")
            {
                Debug.Log("Level name or description is empty");
                SetSuccess(false);
                Hide();
                return;
            }
            SetSuccess(true);
            Result = new LevelInfo(levelNameInputField.text, levelDescriptionInputField.text);
            Hide();
        }

        public IEnumerator WaitForLevelInfo()
        {
            Show();
            while (gameObject.activeSelf)
            {
                yield return null;
            }
            
        }
    }
}