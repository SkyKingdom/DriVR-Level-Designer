using System;
using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;
using Utilities;
using File = System.IO.File;

namespace Saving
{
    public class LevelDataManager : StaticInstance<LevelDataManager>
    {
        private List<ObjectBase> _registeredObjects = new();
        private List<ObjectBase> _deletedObjects = new();
        private SaveData _saveData;
        private string _filePath;

        protected override void Awake()
        {
            base.Awake();
            _filePath = Application.persistentDataPath + "/level.json";
        }

        public void SaveLevel()
        {
            _saveData = new SaveData();
            VerifyObjects();
            DeleteObjects();
            SaveObjects();
            SaveFile();
        }

        private void DeleteObjects()
        {
            foreach (var obj in _deletedObjects)
            {
                _registeredObjects.Remove(obj);
            }
        }

        public void LoadLevel()
        {
            
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

        private void SaveFile()
        {
            string json = JsonUtility.ToJson(_saveData);
            File.WriteAllText(_filePath, json);
        }
        
        private void SaveObjects()
        {
            foreach (var obj in _registeredObjects)
            {
                if (obj.Interactable)
                {
                    var interactable = new InteractableObjectData();
                    interactable.prefabName = obj.PrefabName;
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
                    _saveData.interactableObjects.Add(interactable);
                    continue;
                }

                if (obj.Playable)
                {
                    var playable = new PlayableObjectData();
                    playable.prefabName = obj.PrefabName;
                    var transform1 = obj.transform;
                    playable.position = transform1.position;
                    playable.rotation = transform1.rotation.eulerAngles;
                    playable.switchTime = obj.Playable.PlayOnStart ? -1 : obj.Playable.SwitchViewTime;
                    playable.pathPoints = GetObjectPath(obj.Path);
                    playable.speed = obj.Path.Speed;
                    _saveData.playableObjects.Add(playable);
                    continue;
                }
                
                var decorative = new DecorativeObjectData();
                decorative.prefabName = obj.PrefabName;
                var objTransform = obj.transform;
                decorative.position = objTransform.position;
                decorative.rotation = objTransform.rotation.eulerAngles;
                _saveData.decorativeObjects.Add(decorative);
            }
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
