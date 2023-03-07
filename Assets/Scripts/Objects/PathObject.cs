using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    class PathObject : ObjectBase
    {
        private Stack<Vector3> _pathPoints;

        public Vector3[] Path => _pathPoints.ToArray();
    
        public GameObject pathPointPrefab;
    
        public bool animateOnStart;

        public float animationSpeed;
    
        public float animationStartTime;

        public void AddPoint(Vector3 point)
        {
            _pathPoints.Push(point);
        }
    
        public void DrawPath()
        {
            foreach (var point in _pathPoints)
            {
                Instantiate(pathPointPrefab, point, Quaternion.identity);
            }
        }
    }
}