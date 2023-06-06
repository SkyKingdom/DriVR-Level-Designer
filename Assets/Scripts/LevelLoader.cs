using System.Collections;
using Objects;
using Saving;
using UnityEngine;
using Utilities;

public class LevelLoader : MonoBehaviour
{
    private PrefabData _prefabData;
    
    private SaveData _saveData;
    
    /// <summary>
    /// Loads level from save data
    /// </summary>
    public IEnumerator LoadLevel(SaveData saveData, PrefabData prefabData)
    {
        _prefabData = prefabData;
        _saveData = saveData;
        yield return GenerateLevelObjects();
    }
    
    /// <summary>
    /// Load level coroutine
    /// </summary>
    private IEnumerator GenerateLevelObjects()
    {
        yield return GeneratePlayableObjects(_saveData.playableObjects.ToArray());
        yield return GenerateInteractableObjects(_saveData.interactableObjects.ToArray());
        yield return GenerateDecorativeObjects(_saveData.decorativeObjects.ToArray());
        yield return GenerateRoad(_saveData.roadPoints);
        yield return SetupMap();
        
        yield return Helpers.GetWait(2f);

        if (_saveData.cameraPosition != Vector3.zero)
            DesignerManager.Instance.SceneCameraTransform.position = _saveData.cameraPosition;
    }

    /// <summary>
    /// Generate playable objects
    /// </summary>
    private IEnumerator GeneratePlayableObjects(PlayableObjectData[] playableObjects)
    {
        if (playableObjects.Length == 0) yield break;

        foreach (var objData in playableObjects)
        {
            ObjectBase objectBase = ObjectFactory.InstantiateObjectBase(objData, _prefabData.GetPrefab(objData.prefabName));

            if (ObjectDataUtility.SetupPath(objectBase.Path, objData))
            {
                SpawnObjectPath(objectBase, objData.pathPoints);
            }
            
            ObjectDataUtility.SetupPlayable(objectBase.Playable, objData);
            yield return null;
        }
    }

    /// <summary>
    /// Generate interactable objects
    /// </summary>
    private IEnumerator GenerateInteractableObjects(InteractableObjectData[] interactableObjects)
    {
        if (interactableObjects.Length == 0) yield break;

        foreach (var objData in interactableObjects)
        {
            ObjectBase objectBase = ObjectFactory.InstantiateObjectBase(objData, _prefabData.GetPrefab(objData.prefabName));

            if (ObjectDataUtility.SetupPath(objectBase.Path, objData))
            {
                SpawnObjectPath(objectBase, objData.pathPoints);
            }
            
            ObjectDataUtility.SetupInteractable(objectBase.Interactable, objData);
            yield return null;
        }
    }

    /// <summary>
    /// Generate decorative objects
    /// </summary>
    private IEnumerator GenerateDecorativeObjects(DecorativeObjectData[] decorativeObjects)
    {
        if (decorativeObjects.Length == 0) yield break;

        foreach (var objData in decorativeObjects)
        {
            ObjectFactory.InstantiateObjectBase(objData, _prefabData.GetPrefab(objData.prefabName));
        }
    }

    /// <summary>
    /// Generates road mesh
    /// </summary>
    private IEnumerator GenerateRoad(Vector3[] roadPoints)
    {
        foreach (var point in roadPoints)
        {
            RoadTool.Instance.AddPoint(point);
            yield return null;
        }
    }

    /// <summary>
    /// Loads saved map coordinates
    /// </summary>
    private IEnumerator SetupMap()
    {
        if (!_saveData.mapEnabled) yield break;
        
        DesignerManager.Instance.LoadMap(_saveData.cameraZoom, _saveData.mapLocationX, _saveData.mapLocationY);
        yield return null;
    }

    #region Helpers

    /// <summary>
    /// Spawn path points for object
    /// </summary>
    private void SpawnObjectPath(ObjectBase objBase, Vector3[] pathPoints)
    {
        foreach (var point in pathPoints)
        {
            var nodeContainer = ObjectFactory.InstantiateNodeContainer(_prefabData.GetPrefab("PathPoint"), point);
            var node = new Node(nodeContainer, objBase, point);
            objBase.Path.AddPathPoint(node, false);
        }
    }

    #endregion
}