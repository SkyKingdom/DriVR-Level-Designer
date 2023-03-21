using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class UiNavigation : MonoBehaviour
{
    [SerializeField] private int activeMode;
    [SerializeField] private float panelSize = 920f;
    [SerializeField] private Color activeBtnColor;
    [SerializeField] private Color inactiveBtnColor;
    [SerializeField] private Toggle mapToggle;
    
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
                modeButtons[i].GetComponent<Image>().color = activeBtnColor;
            }
            else
            {
                modeButtons[i].GetComponent<Image>().color = inactiveBtnColor;
            }
        }
        mapToggle.isOn = LevelGeneratorManager.Instance.MapEnabled;
    }

    
    #region Button Tabs
    public void SwitchToActives()
    {
        SetActiveButton(1);
    }
    
    public void SwitchToInteractables()
    {
        SetActiveButton(2);
    }
    
    public void SwitchToProps()
    {
        SetActiveButton(3);
    }
    #endregion
    
    //Map Modes
    public void SwitchToMode(int target)
    {
        var move = target - activeMode;
        
        foreach (var p in modePanels)
        {
            var rect = p.GetComponent<RectTransform>();
            MoveObj(p, new Vector2(0, rect.anchoredPosition.y + (panelSize * move)), 0.3f);
        }
        
        modeButtons[activeMode].GetComponent<Image>().color = inactiveBtnColor; 
        modeButtons[target].GetComponent<Image>().color = activeBtnColor; 
        activeMode = target;
        

    }

    private void SetOpacity(GameObject element, float opacity, float time)
    {
        element.GetComponent<Image>().DOFade(opacity, time);
    }

    private void SetColor(GameObject element, Color color)
    {
        element.GetComponent<Image>().color = color;
    }

    //1 = actives, 2 = interactables, 3 = decorations
    private void SetActiveButton(int active)
    {
        switch (active)
        {
            case 1:
                SetColor(activesBtn, activeBtnColor);
                SetColor(interactableBtn, inactiveBtnColor);
                SetColor(propsBtn, inactiveBtnColor);
                
                activesPnl.SetActive(true);
                interactablePnl.SetActive(false);
                propsPnl.SetActive(false);
                break;
            case 2:
                SetColor(activesBtn, inactiveBtnColor);
                SetColor(interactableBtn, activeBtnColor);
                SetColor(propsBtn, inactiveBtnColor);
                
                activesPnl.SetActive(false);
                interactablePnl.SetActive(true);
                propsPnl.SetActive(false);
                break;
            case 3:
                SetColor(activesBtn, inactiveBtnColor);
                SetColor(interactableBtn, inactiveBtnColor);
                SetColor(propsBtn, activeBtnColor);
                
                activesPnl.SetActive(false);
                interactablePnl.SetActive(false);
                propsPnl.SetActive(true);
                break;
        }
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
