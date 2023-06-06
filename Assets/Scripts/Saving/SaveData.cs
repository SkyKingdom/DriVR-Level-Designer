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
        public Vector3[] roadPoints;
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
    public class PathObjectData: DecorativeObjectData
    {
        public Vector3[] pathPoints;
        public float speed;
        public float animationStartTime;
    }
    
    [Serializable]
    public class InteractableObjectData : PathObjectData
    {
        public float interactionStartTime;
        public float interactionEndTime;
        public bool isCorrect;
    }
    
    [Serializable]
    public class PlayableObjectData : PathObjectData
    {
        public float switchTime;
    }
}