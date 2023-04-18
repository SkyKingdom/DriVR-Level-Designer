using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [Serializable]
    public class SaveData
    {
        public string levelName;
        public string levelDescription;
        public List<DecorativeObjectData> decorativeObjects = new();
        public List<InteractableObjectData> interactableObjects = new();
        public List<PlayableObjectData> playableObjects = new();
        public bool mapEnabled;
        public float cameraZoom;
        public double mapLocationX;
        public double mapLocationY;
        public Vector3 cameraPosition;
    }

    [Serializable]
    public class DecorativeObjectData
    {
        public string objectName;
        public string prefabName;
        public Vector3 position;
        public Vector3 rotation;
    }
    
    [Serializable]
    public class InteractableObjectData : DecorativeObjectData
    {
        public float interactionStartTime;
        public float interactionEndTime;
        public bool isCorrect;

        public Vector3[] pathPoints;
        public float speed;
        public float animationStartTime;
    }
    
    [Serializable]
    public class PlayableObjectData : DecorativeObjectData
    {
        public float switchTime;
        public Vector3[] pathPoints;
        public float speed;
        public float animationStartTime;
    }
}