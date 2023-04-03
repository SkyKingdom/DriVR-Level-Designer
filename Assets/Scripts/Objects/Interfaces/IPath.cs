using System.Collections.Generic;
using UnityEngine;

namespace Objects.Interfaces
{
    public interface IPath
    {
        public  List<Node> PathPoints { get; }
        public LineRenderer LineRenderer { get; }
        public bool AnimateOnStart { get; }
        public float AnimationStartTime { get; }
        public float Speed { get; }


        public void Spawn(bool select = true);
        public void AddPathPoint(Node node);
        public void RemovePathPoint(Node node);
        public void RepositionPathPoint(Node node);
        
        public void HandleObjectReposition(Vector3 position);
        
        public void Select();
        public void Deselect();
        public void UpdatePath();
        public void DeletePath();

        public void HidePath();

        public void ShowPath();
        
        public void SetSpeed(float speed);
        public void SetAnimationStartTime(float animationStartTime);
        public void SetAnimateOnStart(bool animateOnStart);
    }
}