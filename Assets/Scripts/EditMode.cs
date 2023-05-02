using UnityEngine;
using UnityEngine.UI;

public class EditMode : ModeBase
{
    private SpawnManager _manager;
    private GameObject _blanket;
    public EditMode(SpawnManager manager, GameObject blanket)
    {
        _manager = manager;
        _blanket = blanket;
    }
    
    public override void OnEnter()
    {
        _manager.enabled = true;
        _blanket.SetActive(false);
        SpawnManager.Instance.HandleEditTypeChange(EditType.Object);
    }
    
    public override void OnExit()
    {
        _manager.enabled = false;
        _blanket.SetActive(true);
    }
}