using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class UiNavigation : MonoBehaviour
{
    [SerializeField] private int activeMode;
    [SerializeField] private float panelSize = 920f;
    public Color activeButtonBackgroundColor;
    public Color inactiveButtonBackgroundColor;
    public Toggle mapToggle;
    
    [Header("Tab Buttons")]
    public GameObject eventsBtn;
    public GameObject activesBtn;
    public GameObject interactableBtn;
    public GameObject propsBtn;
    public GameObject eventPrefab;
    
    [Header("Map Mode Buttons")]
    public List<Button> modeButtons;

    [Header("Panels")] 
    public GameObject eventsPnl;
    public GameObject activesPnl;
    public GameObject interactablePnl;
    public GameObject propsPnl;
    public Transform eventContainer;
    public List<GameObject> modePanels;

    private void Start()
    {
        for (int i = 0; i < modeButtons.Count; i++)
        {
            if (activeMode == i)
            {
                modeButtons[i].GetComponent<Image>().color = activeButtonBackgroundColor;
            }
            else
            {
                modeButtons[i].GetComponent<Image>().color = inactiveButtonBackgroundColor;
            }
        }
        mapToggle.isOn = LevelGeneratorManager.Instance.MapEnabled;
    }

    public void SwitchToActives()
    {
        SetOpacity(propsBtn, 0.3f, 0.2f);
        SetOpacity(interactableBtn, 0.3f, 0.2f);
        SetOpacity(activesBtn, 1f, 0.2f);
        
        activesPnl.SetActive(true);
        propsPnl.SetActive(false);
        interactablePnl.SetActive(false);
    }
    
    public void SwitchToInteractables()
    {
        SetOpacity(propsBtn, 0.3f, 0.2f);
        SetOpacity(interactableBtn, 1f, 0.2f);
        SetOpacity(activesBtn, 0.3f, 0.2f);
        
        activesPnl.SetActive(false);
        propsPnl.SetActive(false);
        interactablePnl.SetActive(true);
    }
    
    public void SwitchToProps()
    {
        SetOpacity(activesBtn, 0.3f, 0.2f);
        SetOpacity(propsBtn, 1f, 0.2f);
        SetOpacity(interactablePnl, 0.3f, 0.2f);
        
        activesPnl.SetActive(false);
        interactablePnl.SetActive(false);
        propsPnl.SetActive(true);
    }
    
    //Map Modes
    
    public void SwitchToMode(int target)
    {
        var move = target - activeMode;
        
        foreach (var p in modePanels)
        {
            var rect = p.GetComponent<RectTransform>();
            MoveObj(p, new Vector2(0, rect.anchoredPosition.y + (panelSize * move)), 0.3f);
        }
        
        modeButtons[activeMode].GetComponent<Image>().color = inactiveButtonBackgroundColor; 
        modeButtons[target].GetComponent<Image>().color = activeButtonBackgroundColor; 
        activeMode = target;
        

    }

    private void SetOpacity(GameObject element, float opacity, float time)
    {
        element.GetComponent<Image>().DOFade(opacity, time);
    }

    private void MoveObj(GameObject element, Vector2 destination, float time)
    {
        foreach (var b in modeButtons)
        {
            b.interactable = false;
        }

        element.GetComponent<RectTransform>().DOAnchorPos(destination, time).onComplete += () =>
        {
            
                for (int i = 0; i < modeButtons.Count; i++)
                {
                    if (!LevelGeneratorManager.Instance.MapEnabled && i == (int)Mode.Map)
                        continue;

                    modeButtons[i].interactable = true;
                }
        };
    }
    
    //Instantiate event button prefab
    public void InstantiateEventBtn()
    {
        Instantiate(eventPrefab, eventContainer);
    }
}
