using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [CreateAssetMenu(fileName = "PrefabData", menuName = "Saving/PrefabData", order = 0)]
    public class PrefabData : ScriptableObject
    {
        [SerializeField] private PrefabDataDictionary prefabsDictionary;
        
        public IDictionary<int, GameObject> PrefabsDictionary
        {
            get { return prefabsDictionary; }
            set { prefabsDictionary.CopyFrom(value); }
        }
    }
}