using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Objects;
using SimpleFileBrowser;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using User_Interface;
using Utilities;
using File = System.IO.File;

namespace Saving
{
    public class LevelDataManager : PersistentSingleton<LevelDataManager>
    {
        public PrefabData prefabData;
        private List<ObjectBase> _registeredObjects = new();
        private List<ObjectBase> _deletedObjects = new();
        private SaveData _saveData;
        private ScreenshotManager _screenshotManager;

        private void Start()
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("JSON Levels", ".json"));
            FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
            FileBrowser.SetDefaultFilter(".json");
            _screenshotManager = GetComponent<ScreenshotManager>();
        }

        public void SaveLevel()
        {
            _saveData = new SaveData();
            VerifyObjects();
            DeleteObjects();
            SaveObjects();
            if (!SaveRoad())
            {
                Debug.Log("Could not save road. There must be at least two road points.");
            }
            SaveCamera();
            if (VerifyPlayOnStart(_saveData))
            { 
                StartCoroutine(SaveFile());
            }
            else
            {
                Debug.Log("Could not save level. There must be just one object with PlayOnStart set to true.");
            }
        }

        private void SaveCamera()
        {
            if (!LevelGeneratorManager.Instance.MapEnabled) return;
            
            _saveData.mapEnabled = true;
            var cameraData = LevelGeneratorManager.Instance.MapMode.GetMapData();
            _saveData.cameraZoom = cameraData.Zoom;
            _saveData.mapLocationX = cameraData.CenterX;
            _saveData.mapLocationY = cameraData.CenterY;
            _saveData.cameraPosition = LevelGeneratorManager.Instance.SceneCameraTransform.position;
        }

        private bool SaveRoad()
        {
            _saveData.roadPoints = RoadTool.Instance.RoadPoints;
            
            if (_saveData.roadPoints.Length < 2)
            {
                Debug.Log("Could not save level. There must be at least two road points.");
                return false;
            }

            return true;
        }

        private void DeleteObjects()
        {
            foreach (var obj in _deletedObjects)
            {
                _registeredObjects.Remove(obj);
            }
        }

        public void LoadLevel(SaveData data)
        {
            StartCoroutine(LoadLevelAsync(data));
        }
        
        IEnumerator LoadLevelAsync(SaveData data)
        {
            
            var asyncLoad = SceneManager.LoadSceneAsync(0);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            asyncLoad = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            yield return GeneratePlayableObjects(data);
            yield return GenerateInteractableObjects(data);
            yield return GenerateDecorativeObjects(data);
            yield return GenerateRoad(data);
            yield return HandleMap(data);
            
            yield return Helpers.GetWait(2f);
            Debug.Log("Done loading level.");
            if (data.cameraPosition != Vector3.zero)
                LevelGeneratorManager.Instance.SceneCameraTransform.position = data.cameraPosition;
            SceneManager.UnloadSceneAsync(2);
        }

        private IEnumerator GenerateRoad(SaveData data)
        {
            foreach (var point in data.roadPoints)
            {
                RoadTool.Instance.AddPoint(point);
                yield return null;
            }
        }

        private IEnumerator HandleMap(SaveData data)
        {
            if (!data.mapEnabled) yield break;
            LevelGeneratorManager.Instance.LoadMap(data.cameraZoom, data.mapLocationX, data.mapLocationY);
            LevelGeneratorManager.Instance.OnMapEnabledValueChange(true);
            yield return null;
        }

        private IEnumerator GeneratePlayableObjects(SaveData data)
        {
            if (data.playableObjects.Count == 0)
                yield break;
            foreach (var obj in data.playableObjects)
            {
                var prefab = prefabData.PrefabsDictionary[obj.prefabName];
                var instance = Instantiate(prefab, obj.position, Quaternion.Euler(obj.rotation));
                var objectBase = instance.GetComponent<ObjectBase>();
                objectBase.Initialize(obj.objectName, prefab.name, false);
                if (obj.switchTime >= 0)
                {
                    objectBase.Playable.SetViewValues(obj.switchTime);
                }
                else
                {
                    objectBase.Playable.SetPlayOnStart(obj.switchTime < 0);
                }
                objectBase.Path.SetSpeed(obj.speed);
                if (obj.animationStartTime > 0)
                {
                    objectBase.Path.SetAnimationStartTime(obj.animationStartTime);
                }
                else
                {
                    objectBase.Path.SetAnimateOnStart(obj.animationStartTime <= 0);
                }

                if (obj.pathPoints.Length <= 1) continue;
                for (int i = 1; i < obj.pathPoints.Length; i++)
                {
                    var pos = obj.pathPoints[i];
                    var nodeObj = Instantiate(prefabData.PrefabsDictionary["PathPoint"], pos,
                        Quaternion.identity);
                    var node = new Node(nodeObj, objectBase, pos);
                    nodeObj.GetComponent<NodeContainer>().node = node;
                    objectBase.Path.AddPathPoint(node);
                }
            }
            yield return null;
        }
        
        private IEnumerator GenerateInteractableObjects(SaveData data)
        {
            if (data.interactableObjects.Count == 0)
                yield break;
            foreach (var obj in data.interactableObjects)
            {
                var prefab = prefabData.PrefabsDictionary[obj.prefabName];
                var instance = Instantiate(prefab, obj.position, Quaternion.Euler(obj.rotation));
                var objectBase = instance.GetComponent<ObjectBase>();
                objectBase.Initialize(obj.objectName, prefab.name, false);
                if (obj.interactionStartTime < 0 && obj.interactionEndTime < 0)
                {
                    objectBase.Interactable.SetAlwaysInteractable(true, obj.isCorrect);
                }
                objectBase.Interactable.SetInteractionValues(obj.isCorrect, obj.interactionStartTime, obj.interactionEndTime);
                objectBase.Path.SetSpeed(obj.speed);
                if (obj.animationStartTime > 0)
                {
                    objectBase.Path.SetAnimationStartTime(obj.animationStartTime);
                }
                else
                {
                    objectBase.Path.SetAnimateOnStart(obj.animationStartTime <= 0);
                }
                if (obj.pathPoints.Length <= 1) continue;
                for (int i = 1; i < obj.pathPoints.Length; i++)
                {
                    var pos = obj.pathPoints[i];
                    var nodeObj = Instantiate(prefabData.PrefabsDictionary["PathPoint"], pos,
                        Quaternion.identity);
                    var node = new Node(nodeObj, objectBase, pos);
                    nodeObj.GetComponent<NodeContainer>().node = node;
                    objectBase.Path.AddPathPoint(node);
                }
            }
            yield return null;
        }

        private IEnumerator GenerateDecorativeObjects(SaveData data)
        {
            if (data.decorativeObjects.Count == 0)
                yield break;
            foreach (var obj in data.decorativeObjects)
            {
               var prefab = prefabData.PrefabsDictionary[obj.prefabName];
                var instance = Instantiate(prefab, obj.position, Quaternion.Euler(obj.rotation));
                var objectBase = instance.GetComponent<ObjectBase>();
                objectBase.Initialize(obj.objectName, prefab.name, false);
            }
            yield return null;
        }
    
        public void RegisterObject(ObjectBase obj)
        {
            _registeredObjects.Add(obj);
        }

        public void DeregisterObject(ObjectBase obj)
        {
            _registeredObjects.Remove(obj);
        }
        
        private void VerifyObjects()
        {
            foreach (var obj in _registeredObjects.Where(obj => obj.IsDeleted))
            {
                _deletedObjects.Add(obj);
            }
        }

        IEnumerator SaveFile()
        {
            yield return LevelInfoModal.Instance.WaitForLevelInfo();

            if (LevelInfoModal.Success)
            {
                _saveData.levelName = LevelInfoModal.Result.Name;
                _saveData.levelDescription = LevelInfoModal.Result.Description;
            }
            else
            {
                yield break;
            }
            
            yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, null, "JSON Level",
                "Save");
            if (FileBrowser.Success)
            {
                string json = JsonUtility.ToJson(_saveData);
                File.WriteAllText(FileBrowser.Result[0], json);
                _screenshotManager.TakeScreenShot(_saveData.levelName ,FileBrowserHelpers.GetDirectoryName(FileBrowser.Result[0]));
            }
        }
        
        private void SaveObjects()
        {
            foreach (var obj in _registeredObjects)
            {
                if (obj.Interactable)
                {
                    var interactable = new InteractableObjectData();
                    interactable.prefabName = obj.PrefabName;
                    interactable.objectName = obj.ObjectName;
                    var transform1 = obj.transform;
                    interactable.position = transform1.position;
                    interactable.rotation = transform1.rotation.eulerAngles;
                    if (obj.Interactable.AlwaysInteractable)
                    {
                        interactable.interactionStartTime = -1;
                        interactable.interactionEndTime = -1;
                    }
                    else
                    {
                        interactable.interactionStartTime = obj.Interactable.InteractionStartTime;
                        interactable.interactionEndTime = obj.Interactable.InteractionEndTime;
                    }
                    interactable.isCorrect = obj.Interactable.Answer;
                    interactable.pathPoints = GetObjectPath(obj.Path);
                    interactable.speed = obj.Path.Speed;
                    interactable.animationStartTime = obj.Path.AnimationStartTime;
                    _saveData.interactableObjects.Add(interactable);
                    continue;
                }

                if (obj.Playable)
                {
                    var playable = new PlayableObjectData();
                    playable.prefabName = obj.PrefabName;
                    playable.objectName = obj.ObjectName;
                    var transform1 = obj.transform;
                    playable.position = transform1.position;
                    playable.rotation = transform1.rotation.eulerAngles;
                    playable.switchTime = obj.Playable.PlayOnStart ? -1 : obj.Playable.SwitchViewTime;
                    playable.pathPoints = GetObjectPath(obj.Path);
                    playable.speed = obj.Path.Speed;
                    playable.animationStartTime = obj.Path.AnimationStartTime;
                    _saveData.playableObjects.Add(playable);
                    continue;
                }
                
                var decorative = new DecorativeObjectData();
                decorative.prefabName = obj.PrefabName;
                decorative.objectName = obj.ObjectName;
                var objTransform = obj.transform;
                decorative.position = objTransform.position;
                decorative.rotation = objTransform.rotation.eulerAngles;
                _saveData.decorativeObjects.Add(decorative);
            }
        }

        private bool VerifyPlayOnStart(SaveData data)
        {
            // Check for at least one playable object
            if (data.playableObjects.Count == 0)
                return false;
            
            // Check for only one playable object with play on start
            int i = data.playableObjects.Count(obj => obj.switchTime < 0);
            return i == 1;
        }

        private Vector3[] GetObjectPath(Path path)
        {
            var pathPoints = new List<Vector3>();
            foreach (var point in path.PathPoints)
            {
                pathPoints.Add(point.Position);
            }

            return pathPoints.ToArray();
        }
        
        public async Task Cleanup()
        {
            _registeredObjects.Clear();
            _deletedObjects.Clear();
            _saveData = new SaveData();
            await Task.CompletedTask;
        }
    }
    
}
