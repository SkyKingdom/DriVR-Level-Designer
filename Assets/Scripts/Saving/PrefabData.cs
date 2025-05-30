﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [CreateAssetMenu(fileName = "PrefabData", menuName = "Saving/PrefabData", order = 0)]
    public class PrefabData : ScriptableObject
    {
        [SerializeField] private PrefabDataDictionary prefabsDictionary;
        
        public IDictionary<string, GameObject> PrefabsDictionary
        {
            get { return prefabsDictionary; }
            set { prefabsDictionary.CopyFrom(value); }
        }

        public GameObject GetPrefab(string prefabName)
        {
            return prefabsDictionary[prefabName];
        }
    }
}