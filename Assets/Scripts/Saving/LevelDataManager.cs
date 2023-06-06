using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Objects;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.SceneManagement;
using User_Interface;
using Utilities;
using File = System.IO.File;

namespace Saving
{
    public class LevelDataManager : PersistentSingleton<LevelDataManager>
    {
        #region Dependencies

        // Reference to the screenshot manager
        private ScreenshotManager _screenshotManager;
        
        // Reference to the prefab data scriptable object
        public PrefabData prefabData;

        #endregion
        
        // Objects registered to be saved
        private readonly List<ObjectBase> _registeredObjects = new();
        
        // Objects that are tagged for deletion
        private readonly List<ObjectBase> _deletedObjects = new();
        
        // Level save data
        private SaveData _saveData;


        private void Start()
        {
            _screenshotManager = GetComponent<ScreenshotManager>();
            
            // Set up file browser
            FileBrowser.SetFilters(true, new FileBrowser.Filter("JSON Levels", ".json"));
            FileBrowser.AddQuickLink( "Users", "C:\\Users" );
            FileBrowser.SetDefaultFilter(".json");
        }

        #region Saving

        // Save the level to a json file
        public void SaveLevel()
        {
            _saveData = new SaveData(); // Create a new save data object
            
            VerifyObjects();
            DeleteObjects();
            SaveObjects();
            SaveCamera();
            
            if (!SaveRoad())
            {
                Debug.Log("Could not save road. There must be at least two road points.");
            }
            
            if (VerifyPlayOnStart(_saveData))
            { 
                StartCoroutine(SaveFile());
            }
            else
            {
                Debug.Log("Could not save level. There must be just one object with PlayOnStart set to true.");
            }
        }
        
        /// <summary>
        /// Checks if road is valid and saves it to the save data
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// Generates data for all registered objects and adds them to the save data
        /// </summary>
        private void SaveObjects()
        {
            foreach (var obj in _registeredObjects)
            {
                if (obj.Interactable)
                {
                    _saveData.interactableObjects.Add(ObjectFactory.CreateInteractable(obj));
                    continue;
                }

                if (obj.Playable)
                {
                    _saveData.playableObjects.Add(ObjectFactory.CreatePlayable(obj));
                    continue;
                }
                
                _saveData.decorativeObjects.Add(ObjectFactory.CreateDecorative(obj));
            }
        }
        
        /// <summary>
        /// Saves map data if map is enabled and camera position
        /// </summary>
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
        
        /// <summary>
        /// Verifies that there is only one playable object with play on start
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool VerifyPlayOnStart(SaveData data)
        {
            // Check for at least one playable object
            if (data.playableObjects.Count == 0)
                return false;
            
            // Check for only one playable object with play on start
            int i = data.playableObjects.Count(obj => obj.switchTime < 0);
            return i == 1;
        }
        
        /// <summary>
        /// Saves json file and screenshot of the level
        /// </summary>
        /// <returns></returns>
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
            
            yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, null, "JSON Level");
            if (FileBrowser.Success)
            {
                string json = JsonUtility.ToJson(_saveData);
                File.WriteAllText(FileBrowser.Result[0], json);
                _screenshotManager.TakeScreenShot(_saveData.levelName ,FileBrowserHelpers.GetDirectoryName(FileBrowser.Result[0]));
            }
        }

        #endregion

        #region Object Management

        /// <summary>
        /// Remove objects tagged for deletion from the registered objects list
        /// </summary>
        private void DeleteObjects()
        {
            foreach (var obj in _deletedObjects)
            {
                _registeredObjects.Remove(obj);
            }
        }
        
        /// <summary>
        /// Loops through all registered objects and sort out objects tagged for deletion.
        /// </summary>
        private void VerifyObjects()
        {
            foreach (var obj in _registeredObjects.Where(obj => obj.IsDeleted))
            {
                _deletedObjects.Add(obj);
            }
        }
        
        
        /// <summary>
        /// Registers object to the list of objects to be saved
        /// </summary>
        public void RegisterObject(ObjectBase obj)
        {
            _registeredObjects.Add(obj);
        }

        /// <summary>
        /// De-registers object from the list of objects to be saved<br/>
        /// Used when an object is deleted
        /// </summary>
        public void DeregisterObject(ObjectBase obj)
        {
            _registeredObjects.Remove(obj);
        }

        #endregion

        #region Loading

        /// <summary>
        /// Starts load level coroutine<br/>
        /// Button click event.
        /// </summary>
        /// <param name="data">Selected level data</param>
        public void LoadLevel(SaveData data)
        {
            StartCoroutine(LoadLevelAsync(data));
        }
        
        /// <summary>
        /// Loads level designer scene and generates level from save data
        /// </summary>
        IEnumerator LoadLevelAsync(SaveData data)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(0);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            var levelLoader = FindObjectOfType<LevelLoader>();
            
            asyncLoad = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            yield return levelLoader.LoadLevel(data, prefabData);
            
            SceneManager.UnloadSceneAsync(2);
        }

        #endregion

        /// <summary>
        /// Cleans up data when exiting the level designer
        /// </summary>
        public async Task Cleanup()
        {
            _registeredObjects.Clear();
            _deletedObjects.Clear();
            _saveData = new SaveData();
            await Task.CompletedTask;
        }
    }
    
}
