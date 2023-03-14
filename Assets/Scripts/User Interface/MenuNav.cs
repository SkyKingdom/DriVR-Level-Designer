using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuNav : MonoBehaviour
{
    public Animator hammer;

    private bool _animRunning = false;

    private void Start()
    {
        InvokeRepeating(nameof(HandleAnimationTrigger), 3f, 5f);
    }

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
}
