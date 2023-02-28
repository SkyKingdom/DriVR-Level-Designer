using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class UiNavigation : MonoBehaviour
{
    [Header("Tab Buttons")]
    public GameObject eventsBtn;
    public GameObject settingsBtn;
    public GameObject activesBtn;
    public GameObject propsBtn;
    public GameObject eventPrefab;
    
    [Header("Map Mode Buttons")]
    public GameObject exploreViewBtn;
    public GameObject editViewBtn;
    public GameObject mapViewBtn;

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

        MoveObj(settingsPnl, new Vector2(460 , 0), 0.3f);
        MoveObj(eventsPnl, new Vector2(0 , 0), 0.3f);
    }
    
    public void SwitchToSettings()
    {
        SetOpacity(eventsBtn, 0.8f, 0.2f);
        SetOpacity(settingsBtn, 1f, 0.2f);

        MoveObj(settingsPnl, new Vector2(0 , 0), 0.3f);
        MoveObj(eventsPnl, new Vector2(-460 , 0), 0.3f);
    }
    
    public void SwitchToActives()
    {
        SetOpacity(propsBtn, 0.8f, 0.2f);
        SetOpacity(activesBtn, 1f, 0.2f);
        
        activesPnl.SetActive(true);
        propsPnl.SetActive(false);
    }
    
    public void SwitchToProps()
    {
        SetOpacity(activesBtn, 0.8f, 0.2f);
        SetOpacity(propsBtn, 1f, 0.2f);
        
        activesPnl.SetActive(false);
        propsPnl.SetActive(true);
    }
    
    //Map Modes
    public void SwitchToExploreMode()
    {
        SetOpacity(exploreViewBtn, 1f, 0.2f);
        SetOpacity(editViewBtn, 0.7f, 0.2f);
        SetOpacity(mapViewBtn, 0.7f, 0.2f);
    }
    
    public void SwitchToEditMode()
    {
        SetOpacity(exploreViewBtn, 0.7f, 0.2f);
        SetOpacity(editViewBtn, 1f, 0.2f);
        SetOpacity(mapViewBtn, 0.7f, 0.2f);
    }
    
    public void SwitchToMapMode()
    {
        SetOpacity(exploreViewBtn, 0.7f, 0.2f);
        SetOpacity(editViewBtn, 0.7f, 0.2f);
        SetOpacity(mapViewBtn, 1f, 0.2f);
    }

    private void SetOpacity(GameObject element, float opacity, float time)
    {
        element.GetComponent<Image>().DOFade(opacity, time);
    }

    private void MoveObj(GameObject element, Vector2 destination ,float time)
    {
        element.GetComponent<RectTransform>().DOAnchorPos(destination, time);
    }
    
    //Instantiate event button prefab
    public void InstantiateEventBtn()
    {
        Instantiate(eventPrefab, eventContainer);
    }
}
