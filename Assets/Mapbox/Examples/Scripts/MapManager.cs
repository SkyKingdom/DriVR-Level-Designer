using System;
using Utilities;

namespace Mapbox.Examples
{
	using Geocoding;
	using UnityEngine.UI;
	using Mapbox.Unity.Map;
	using UnityEngine;
	using System.Collections;

	public class MapManager : MonoBehaviour
	{
		[Header("Dependencies")]
		[SerializeField] private GameObject plane;
		[SerializeField] private CameraController cameraController;
		
		[field: Header("Camera"), SerializeField] public Camera SceneCamera { get; private set; }
		private Vector3 _cameraStartPos;
		
		[field:Header("Map")]
		[field: SerializeField] public AbstractMap Map { get; private set; }
		public bool IsMapEnabled => Map.gameObject.activeSelf;
		[SerializeField] private ForwardGeocodeUserInput forwardGeocoder;
		
		[Header("Settings")]
		[SerializeField] private bool mapEnabledOnStart;
		[SerializeField] private Slider zoomSlider;

		private HeroBuildingSelectionUserInput[] _heroBuildingSelectionUserInput;
		private Coroutine _reloadRoutine;
		private const float WaitInSeconds = .3f;
		
		public event Action<bool> OnMapStatusChange; 

		private void Awake()
		{
			_cameraStartPos = SceneCamera.transform.position;
			if(Map == null)
			{
				Debug.LogError("Error: No Abstract Map component found in scene.");
				return;
			}
			if (zoomSlider != null)
			{
				Map.OnUpdated += () => { zoomSlider.value = Map.Zoom; };
				zoomSlider.onValueChanged.AddListener(Reload);
			}
			if(forwardGeocoder != null)
			{
				forwardGeocoder.OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
			}
			_heroBuildingSelectionUserInput = GetComponentsInChildren<HeroBuildingSelectionUserInput>();
			if(_heroBuildingSelectionUserInput != null)
			{
				for (int i = 0; i < _heroBuildingSelectionUserInput.Length; i++)
				{
					_heroBuildingSelectionUserInput[i].OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
				}
			}
		}

		private void Start()
		{
			ToggleMap(mapEnabledOnStart); // Sets the map to be enabled or disabled on start
		}

		private void OnEnable()
		{
			DesignerManager.Instance.OnModeChange += OnModeChange;
		}
		
		private void OnDisable()
		{
			DesignerManager.Instance.OnModeChange -= OnModeChange;
		}

		
		// Handles mode changes
		private void OnModeChange(Mode oldValue, Mode value)
		{
			if (value == Mode.Map)
			{
				// Enables camera controller and updates the map
				cameraController.enabled = true;
				Map.UpdateMap();
				Map.ForceUpdateColliders();
			}
			else if (oldValue == Mode.Map)
			{
				// Disables camera controller
				cameraController.enabled = false;
			}
		}

		// Updates map zoom when slider value changes even when the game is not running
		private void OnValidate()
		{
			if (Map == null) return;
			if (zoomSlider != null)
				zoomSlider.value = Map.Zoom;
		}

		// Handles search
		private void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response)
		{
			if (null != response.Features && response.Features.Count > 0)
			{
				int zoom = Map.AbsoluteZoom;
				Map.UpdateMap(response.Features[0].Center, zoom);
			}
		}

		private void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response, bool resetCamera)
		{
			if (response == null)
			{
				return;
			}
			if (resetCamera)
			{
				SceneCamera.transform.position = _cameraStartPos;
			}
			ForwardGeocoder_OnGeocoderResponse(response);
		}

		private void Reload(float value)
		{
			if (_reloadRoutine != null)
			{
				StopCoroutine(_reloadRoutine);
				_reloadRoutine = null;
			}
			_reloadRoutine = StartCoroutine(ReloadAfterDelay((int)value));
		}

		private IEnumerator ReloadAfterDelay(int zoom)
		{
			yield return Helpers.GetWait(WaitInSeconds);
			SceneCamera.transform.position = _cameraStartPos;
			Map.UpdateMap(Map.CenterLatitudeLongitude, zoom);
			_reloadRoutine = null;
		}

		// Toggles map on and off
		public void ToggleMap(bool value)
		{
			Map.gameObject.SetActive(value);
			Map.Terrain.EnableCollider(value);
			plane.SetActive(!value);
			
			OnMapStatusChange?.Invoke(value);
			
			if (value) return;

			if (DesignerManager.Instance.CurrentMode == Mode.Map)
				DesignerManager.Instance.SetMode((int)Mode.Edit);
		}
		
		// Returns map data, position and zoom, used for saving
		public MapData GetMapData()
		{
			var mapData = new MapData
			{
				Zoom = Map.Zoom,
				CenterX = Map.CenterLatitudeLongitude.x,
				CenterY = Map.CenterLatitudeLongitude.y
			};

			return mapData;
		}
	}
	
	// Map data struct
	public struct MapData
	{
		public float Zoom;
		public double CenterX;
		public double CenterY;
	}
}