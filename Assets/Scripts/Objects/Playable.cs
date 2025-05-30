﻿using Objects.Interfaces;
using UnityEngine;

namespace Objects
{
    /// <summary>
    /// Playable object component.<br/>
    /// </summary>
    public class Playable : MonoBehaviour, IObjectComponent
    {
        public ObjectBase Owner { get; private set; }
        public float SwitchTime { get; private set; }
        public bool PlayOnStart { get; private set;}
        
        public void Initialize(ObjectBase objectBase)
        {
            Owner = objectBase;
        }
        
        public void SetSwitchTime(float switchTime)
        {
            SwitchTime = switchTime;
        }

        public void SetPlayOnStart(bool playOnStart)
        {
            PlayOnStart = playOnStart;
        }
        
    }
}