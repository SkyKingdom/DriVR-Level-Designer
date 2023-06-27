using System.Collections;
using System.IO;
using Saving;
using UnityEngine;
using SimpleFileBrowser;

public class LoadManager : MonoBehaviour
{
    private void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("JSON Levels", ".json"));
        FileBrowser.AddQuickLink( "Users", "C:\\Users" );
        FileBrowser.SetDefaultFilter(".json");
    }
    
    // Handles pressing the load button
    public void LoadLevel() => StartCoroutine(ShowLoadDialogCoroutine());
    
    // Coroutine for load level dialog
    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Load Level", "Load" );
 
        if( FileBrowser.Success )
        {
            var json = File.ReadAllText(FileBrowser.Result[0]);
            Debug.Log(json);

            var data = JsonUtility.FromJson<SaveData>(json);
            
            LevelDataManager.Instance.LoadLevel(data);
        }
    }
}