using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Objects;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private string _filePath;
        
        private void Start()
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("JSON Levels", ".json"));
            FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
            FileBrowser.SetDefaultFilter(".json");
        }

        public void SaveLevel()
        {
            _saveData = new SaveData();
            VerifyObjects();
            DeleteObjects();
            SaveObjects();
            if (VerifyPlayOnStart(_saveData))
            { 
                StartCoroutine(SaveFile());
            }
            else
            {
                Debug.Log("Could not save level. There must be just one object with PlayOnStart set to true.");
            }
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
            var asyncLoad = SceneManager.LoadSceneAsync(2);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            asyncLoad = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelGenerator"));
            yield return LoadObjects(data);

            SceneManager.UnloadSceneAsync(2);
        }

        IEnumerator LoadObjects(SaveData data)
        {
            Debug.Log("Loading objects...");
            foreach (var obj in data.decorativeObjects)
            {
                var prefab = prefabData.PrefabsDictionary[obj.prefabName];
                var instance = Instantiate(prefab, obj.position, Quaternion.Euler(obj.rotation));
                var objectBase = instance.GetComponent<ObjectBase>();
                objectBase.Initialize(obj.objectName, prefab.name);
                yield return null;
            }
            yield return null;
            foreach (var obj in data.interactableObjects)
            {
                var prefab = prefabData.PrefabsDictionary[obj.prefabName];
                var instance = Instantiate(prefab, obj.position, Quaternion.Euler(obj.rotation));
                var objectBase = instance.GetComponent<ObjectBase>();
                objectBase.Initialize(obj.objectName, prefab.name);
                if (obj.interactionStartTime < 0 && obj.interactionEndTime < 0)
                {
                    objectBase.Interactable.SetAlwaysInteractable(true, obj.isCorrect);
                }
                objectBase.Interactable.SetInteractionValues(obj.isCorrect, obj.interactionStartTime, obj.interactionEndTime);
                objectBase.Path.SetSpeed(obj.speed);
                if (obj.animationStartTime >= 0)
                {
                    objectBase.Path.SetAnimationStartTime(obj.animationStartTime);
                }
                else
                {
                    objectBase.Path.SetAnimateOnStart(obj.animationStartTime < 0);
                }
                // Generate Path Points
                yield return null; 
            }
            yield return null;
            foreach (var obj in data.playableObjects)
            {
                var prefab = prefabData.PrefabsDictionary[obj.prefabName];
                var instance = Instantiate(prefab, obj.position, Quaternion.Euler(obj.rotation));
                var objectBase = instance.GetComponent<ObjectBase>();
                objectBase.Initialize(obj.objectName, prefab.name);
                if (obj.switchTime >= 0)
                {
                    objectBase.Playable.SetViewValues(obj.switchTime);
                }
                else
                {
                    objectBase.Playable.SetPlayOnStart(obj.switchTime < 0);
                }
                objectBase.Path.SetSpeed(obj.speed);
                if (obj.animationStartTime >= 0)
                {
                    objectBase.Path.SetAnimationStartTime(obj.animationStartTime);
                }
                else
                {
                    objectBase.Path.SetAnimateOnStart(obj.animationStartTime < 0);
                }
                yield return null; 
            }
            Debug.Log("Loaded Level");
            yield return null; 
        }
    
        public void RegisterObject(ObjectBase obj)
        {
            _registeredObjects.Add(obj);
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
            yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, null, "JSON Level",
                "Save");
            if (FileBrowser.Success)
            {
                string json = JsonUtility.ToJson(_saveData);
                File.WriteAllText(FileBrowser.Result[0], json);
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
                    interactable.objectName = obj.name;
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
                    playable.objectName = obj.name;
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
                decorative.objectName = obj.name;
                var objTransform = obj.transform;
                decorative.position = objTransform.position;
                decorative.rotation = objTransform.rotation.eulerAngles;
                _saveData.decorativeObjects.Add(decorative);
            }
        }

        private bool VerifyPlayOnStart(SaveData data)
        {
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
    }
}
