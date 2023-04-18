using System.Collections;
using TMPro;
using UnityEngine;
using Utilities;
using DG.Tweening;

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
        [SerializeField] private GameObject popUp;


        protected override void Awake()
        {
            base.Awake();
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            popUp.transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.2f)
                .OnComplete(() => popUp.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f));
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