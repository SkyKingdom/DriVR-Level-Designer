using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class InspectorPanel : MonoBehaviour
{
    [SerializeField] private int activeMode;
    [SerializeField] private float panelSize = 920f;
    [SerializeField] private Color activeButtonColor;
    [SerializeField] private Color inactiveButtonColor;
    
    
    [SerializeField] private Toggle mapToggle;
    
    [Header("Tab Buttons")]
    public GameObject activesBtn;
    public GameObject interactableBtn;
    public GameObject propsBtn;
    [SerializeField] private Image switchModeBlanket;

    [Header("Map Mode Buttons")]
    public List<Button> modeButtons;

    [Header("Panels")]
    public GameObject activesPnl;
    public GameObject interactablePnl;
    public GameObject propsPnl;
    public List<GameObject> modePanels;
    [SerializeField] private GameObject roadModePanel;

    private void Start()
    {
        DesignerManager.Instance.OnModeChange += HandleModeChange;
        DesignerManager.Instance.OnEditTypeChange += HandleEditModeChange;
        
        for (int i = 0; i < modeButtons.Count; i++)
        {
            if (activeMode == i)
            {
                modeButtons[i].GetComponent<Image>().color = activeButtonColor;
            }
            else
            {
                modeButtons[i].GetComponent<Image>().color = inactiveButtonColor;
            }
        }
    }

    private void HandleEditModeChange(EditMode arg1, EditMode arg2)
    {
        roadModePanel.SetActive(arg2 == EditMode.Road);
    }

    private void HandleModeChange(Mode oldValue, Mode value)
    {
        if (value != Mode.FirstPerson)
            OpenModePanel((int)value);
    }


    #region Button Tabs
    public void SwitchToActives()
    {
        switchModeBlanket.gameObject.SetActive(true);
        switchModeBlanket.DOFade(1, 0.1f).OnComplete(() => SetActiveButton(1));
    }
    
    public void SwitchToInteractables()
    {
        switchModeBlanket.gameObject.SetActive(true);
        switchModeBlanket.DOFade(1, 0.1f).OnComplete(() => SetActiveButton(2));

    }
    
    public void SwitchToProps()
    {
        switchModeBlanket.gameObject.SetActive(true);
        switchModeBlanket.DOFade(1, 0.1f).OnComplete(() => SetActiveButton(3));

    }
    #endregion
    
    //Map Modes
    private void OpenModePanel(int target)
    {
        var move = target - activeMode;
        
        foreach (var p in modePanels)
        {
            var rect = p.GetComponent<RectTransform>();
            MoveObj(p, new Vector2(0, rect.anchoredPosition.y + (panelSize * move)), 0.3f);
        }
        
        modeButtons[activeMode].GetComponent<Image>().color = inactiveButtonColor; 
        modeButtons[target].GetComponent<Image>().color = activeButtonColor; 
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
                SetColor(activesBtn, activeButtonColor);
                SetColor(interactableBtn, inactiveButtonColor);
                SetColor(propsBtn, inactiveButtonColor);
                
                activesPnl.SetActive(true);
                interactablePnl.SetActive(false);
                propsPnl.SetActive(false);
                switchModeBlanket.DOFade(0, 0.1f).OnComplete(() => switchModeBlanket.gameObject.SetActive(false));
                break;
            case 2:
                SetColor(activesBtn, inactiveButtonColor);
                SetColor(interactableBtn, activeButtonColor);
                SetColor(propsBtn, inactiveButtonColor);
                
                activesPnl.SetActive(false);
                interactablePnl.SetActive(true);
                propsPnl.SetActive(false);
                switchModeBlanket.DOFade(0, 0.1f).OnComplete(() => switchModeBlanket.gameObject.SetActive(false));
                break;
            case 3:
                SetColor(activesBtn, inactiveButtonColor);
                SetColor(interactableBtn, inactiveButtonColor);
                SetColor(propsBtn, activeButtonColor);
                
                activesPnl.SetActive(false);
                interactablePnl.SetActive(false);
                propsPnl.SetActive(true);
                switchModeBlanket.DOFade(0, 0.1f).OnComplete(() => switchModeBlanket.gameObject.SetActive(false));
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
                if (!DesignerManager.Instance.MapManager.IsMapEnabled && i == (int)Mode.Map)
                    continue;

                modeButtons[i].interactable = true;
            }
        };
    }
}
