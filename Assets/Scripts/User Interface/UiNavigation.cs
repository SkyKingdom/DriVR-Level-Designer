using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class UiNavigation : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject eventsBtn;
    public GameObject settingsBtn;
    public GameObject activesBtn;
    public GameObject propsBtn;
    public GameObject eventPrefab;

    [Header("Panels")] 
    public GameObject eventsPnl;
    public GameObject settingsPnl;
    public GameObject activesPnl;
    public GameObject propsPnl;
    public Transform eventContainer;
    
    public void SwitchToEvents()
    {
        SetOpacity(settingsBtn, 0.3f, 0.2f);
        SetOpacity(eventsBtn, 1f, 0.2f);
        
        eventsPnl.SetActive(true);
        settingsPnl.SetActive(false);
    }
    
    public void SwitchToSettings()
    {
        SetOpacity(eventsBtn, 0.3f, 0.2f);
        SetOpacity(settingsBtn, 1f, 0.2f);
        
        eventsPnl.SetActive(false);
        settingsPnl.SetActive(true);
    }
    
    public void SwitchToActives()
    {
        SetOpacity(propsBtn, 0.3f, 0.2f);
        SetOpacity(activesBtn, 1f, 0.2f);
        
        activesPnl.SetActive(true);
        propsPnl.SetActive(false);
    }
    
    public void SwitchToProps()
    {
        SetOpacity(activesBtn, 0.3f, 0.2f);
        SetOpacity(propsBtn, 1f, 0.2f);
        
        activesPnl.SetActive(false);
        propsPnl.SetActive(true);
    }

    private void SetOpacity(GameObject element, float opacity, float time)
    {
        element.GetComponent<Image>().DOFade(opacity, time);
    }
    
    //Instantiate event button prefab
    public void InstantiateEventBtn()
    {
        Instantiate(eventPrefab, eventContainer);
    }
}
