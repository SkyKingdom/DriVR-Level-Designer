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
		
		[Header("Camera"), SerializeField] private Camera sceneCamera;
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
			_cameraStartPos = sceneCamera.transform.position;
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
			ToggleMap(mapEnabledOnStart);
		}

		private void OnValidate()
		{
			if (Map == null) return;
			if (zoomSlider != null)
				zoomSlider.value = Map.Zoom;
		}

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
				sceneCamera.transform.position = _cameraStartPos;
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
			sceneCamera.transform.position = _cameraStartPos;
			Map.UpdateMap(Map.CenterLatitudeLongitude, zoom);
			_reloadRoutine = null;
		}

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
	public struct MapData
	{
		public float Zoom;
		public double CenterX;
		public double CenterY;
	}
}