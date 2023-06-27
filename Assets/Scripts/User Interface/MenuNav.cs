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
        public GameObject tutorialPanel;

        private int _tutorialCounter = 0;

        private bool _animRunning;

        private void Start()
        {
            // Start hammer animations
            InvokeRepeating(nameof(HandleAnimationTrigger), 3f, 5f);
        }

        // Start new level
        public void NewLevelButton()
        {
            SceneManager.LoadScene(1);
        }
    
        // Main menu hammer animations
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

        // Tutorial
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

    }
}
