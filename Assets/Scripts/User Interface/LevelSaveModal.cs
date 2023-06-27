using System.Collections;
using TMPro;
using UnityEngine;
using Utilities;
using DG.Tweening;

namespace User_Interface
{
    /// <summary>
    /// Struct containing level name and description
    /// </summary>
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
    
    /// <summary>
    /// Save modal class for saving level data
    /// </summary>
    public class LevelSaveModal : MonoBehaviour
    {
        public static bool Success { get; private set; }
        public static LevelInfo Result { get; private set; }
        [SerializeField] private TMP_InputField levelNameInputField;
        [SerializeField] private TMP_InputField levelDescriptionInputField;
        [SerializeField] private GameObject popUp;
        
        // Shows the modal
        public void Show()
        {
            gameObject.SetActive(true);
            popUp.transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.2f)
                .OnComplete(() => popUp.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f));
        }

        /// <summary>
        /// If the modal operation was successful
        /// </summary>
        private void SetSuccess(bool success)
        {
            Success = success;
        }
        
        // Hides the modal
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        // Cancel the modal operation
        public void Cancel()
        {
            SetSuccess(false);
            Hide();
        }
        
        /// <summary>
        /// Confirm the modal operation<br/>Checks if level name and description are not empty
        /// </summary>
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

        // Wait for the modal to be closed
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