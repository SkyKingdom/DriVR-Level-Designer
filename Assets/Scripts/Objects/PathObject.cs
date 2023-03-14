using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    class PathObject : ObjectBase
    {
        private Stack<Vector3> _pathPoints;

        public Vector3[] Path => _pathPoints.ToArray();
    
        public GameObject pathPointPrefab;
    
        public bool AnimateOnStart { get; protected set; }

        public float Speed { get; protected set; }
        
        public float AnimationStartTime { get; protected set; }
        
        public void SetAnimationValues(float speed, float animationStartTime)
        {
            Speed = speed;
            AnimationStartTime = animationStartTime;
        }
        
        public void SetAnimationValues(float speed, bool animateOnStart)
        {
            Speed = speed;
            AnimateOnStart = animateOnStart;
        }
        
    }
}