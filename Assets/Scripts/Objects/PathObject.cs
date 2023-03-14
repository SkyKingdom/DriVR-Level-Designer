using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class PathObject : ObjectBase
    {
        private Stack<Node> _pathPoints = new();
        public Stack<Node> PathPoints => _pathPoints;
        
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
        
        public void AddPathPoint(Node point)
        {
            _pathPoints.Push(point);
        }
    }
}