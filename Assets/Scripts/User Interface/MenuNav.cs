using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace User_Interface
{
    public class MenuNav : MonoBehaviour
    {
        public Animator hammer;

        [Header("Tutorial")] 
        public TextMeshProUGUI tutorialText;
        public List<GameObject> tutorialMasks;
        public GameObject tutorialPanel;

        private int _tutorialCounter = 0;

        private bool _animRunning = false;

        private void Start()
        {
            InvokeRepeating(nameof(HandleAnimationTrigger), 3f, 5f);
        }

        public void NewLevelButton()
        {
            SceneManager.LoadScene(0);
        }
    
        //animations
        private void HandleAnimationTrigger()
        {
            if (_animRunning)
            {
                return;
            }

            var delay = Random.Range(3, 6);
            StartCoroutine(TriggerAnimDelay(delay));
        }

        private IEnumerator TriggerAnimDelay(int delayTime)
        {
            _animRunning = true;
        
            yield return new WaitForSeconds(delayTime);
            hammer.SetTrigger("Hammertime");

            _animRunning = false;
        }

        #region Tutorial

        public void GuideButtonPress()
        {
            tutorialPanel.SetActive(true);
        
            tutorialPanel.transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.2f)
                .OnComplete(() => tutorialPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f));
        }

        public void CloseButtonPress()
        {
            tutorialPanel.SetActive(false);
            tutorialPanel.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    
        public void NextButtonPress()
        {
            if (_tutorialCounter > tutorialMasks.Count-1)
            {
                _tutorialCounter = 0;
            }
        
            foreach (var mask in tutorialMasks)
            {
                mask.SetActive(false);
            }
        
            tutorialMasks[_tutorialCounter].SetActive(true);
            SetTutorialText(_tutorialCounter);
            _tutorialCounter++;
        }

        private void SetTutorialText(int counter)
        {
            switch (counter)
            {
                case 0:
                    tutorialText.SetText("toggle the map to accurately adjust object positions");
                    break;
                case 1:
                    tutorialText.SetText("switch between map modes");
                    break;
                case 2:
                    tutorialText.SetText("switch between various object types to place in your level");
                    break;
                case 3:
                    tutorialText.SetText("adjust object settings based on the type of object selected");
                    break;
                case 4:
                    tutorialText.SetText("make sure to save the changes to your object before saving the level");
                    break;
                case 5:
                    tutorialText.SetText("once your level is done, you may choose to save it to your computer or publish it");
                    break;
            }
        }
        
        #endregion
    
    }
}
