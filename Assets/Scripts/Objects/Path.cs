using System.Collections.Generic;
using Objects.Interfaces;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

namespace Objects
{
    /// <summary>
    /// Path component class.
    /// </summary>
    public class Path : MonoBehaviour, IObjectComponent
    {
        public ObjectBase Owner { get; private set; } // The object that owns this component.
        public  List<PathPoint> PathPoints { get; private set; } // The points that make up the path.
        
        public bool AnimateOnStart { get; private set; } // Whether the path should animate on start.
        public float AnimationStartTime { get; private set; } // The time at which the animation starts.
        public float Speed { get; private set; } // The speed of the animation.

        [SerializeField] private PathCreator bezierPath; // The bezier path.
        private RoadMeshCreator meshCreator; // The mesh creator.

        public bool Highlighted { get; private set; } // Whether the path is highlighted.
        
        public void Initialize(ObjectBase objectBase)
        {
            Owner = objectBase;
            PathPoints = new List<PathPoint>();
        }

        /// <summary>
        /// Sets the reference to the path and path mesh creator.
        /// </summary>
        /// <param name="pathCreator"></param>
        public void SetPath(PathCreator pathCreator)
        {
            bezierPath = pathCreator;
            meshCreator = bezierPath.GetComponent<RoadMeshCreator>();
        }

        /// <summary>
        /// Spawns a new path point.
        /// </summary>
        /// <param name="select">Whether the path should be selected on spawn</param>
        public void Spawn(bool select = true)
        {
            PathPointContainer cont = Instantiate(PathManager.Instance.pathPointPrefab, Owner.GetPosition(),
                Quaternion.Euler(Owner.GetRotation()));
            PathPoint n = new PathPoint(cont, Owner, Owner.GetPosition());
            cont.PathPoint = n;
            AddPathPoint(n, select);
        }

        /// <summary>
        /// Adds path point to the path point list and updates the path visuals.
        /// </summary>
        /// <param name="pathPoint"></param>
        /// <param name="select"></param>
        public void AddPathPoint(PathPoint pathPoint, bool select = true)
        {
            PathPoints.Add(pathPoint);
            UpdatePath();
        }
        
                
        /// <summary>
        /// Adds path point to the path point list at the specified index and updates the path visuals.
        /// </summary>
        /// <param name="pathPoint"></param>
        /// <param name="index"></param>
        public void AddPathPoint(PathPoint pathPoint, int index)
        {
            PathPoints.Insert(index, pathPoint);
            UpdatePath();
        }

        /// <summary>
        /// Removes path point from the path point list and updates the path visuals.
        /// </summary>
        public void RemovePathPoint(PathPoint pathPoint)
        {
            PathPoints.Remove(pathPoint);
            pathPoint.Container.Delete();
            pathPoint.Dispose();
            UpdatePath();
        }
        
        /// <summary>
        /// Removes path point from the path point list temporarily and updates the path visuals.<br/>Allows for undoing the deletion.
        /// </summary>
        /// <param name="pathPoint"></param>
        /// <returns></returns>
        public int RemovePathTemporarily(PathPoint pathPoint)
        {
            var i = PathPoints.IndexOf(pathPoint);
            pathPoint.Container.Delete();
            PathPoints.Remove(pathPoint);
            UpdatePath();
            return i;
        }
        
        public void RepositionPathPoint(PathPoint pathPoint) => UpdatePath();
        
        public void HandleObjectReposition(Vector3 position) => PathPoints[0].SetPosition(position);
        
        public void HighlightPath(PathPointContainer initiator)
        {
            Highlighted = true;
            foreach (var p in PathPoints)
            {
                if (p.Container == initiator)
                    continue;
                p.Container.Highlight();
            }
        }
        
        public void UnhighlightPath(PathPointContainer initiator)
        {
            Highlighted = false;
            foreach (var p in PathPoints)
            {
                if (p.Container == initiator)
                    continue;
                p.Container.Unhighlight();
            }
        }
        
        public void UpdatePath()
        {
            if (PathPoints.Count < 2)
            {
                bezierPath.gameObject.SetActive(false);
                return;
            }
            
            bezierPath.gameObject.SetActive(true);
            bezierPath.bezierPath = new BezierPath(PathPoints.ConvertAll(p => p.Position), false, PathSpace.xyz);
            TriggerPathUpdate();
        }

        /// <summary>
        /// Force updates the path visuals.
        /// </summary>
        private void TriggerPathUpdate()
        {
            bezierPath.TriggerPathUpdate();
            meshCreator.TriggerUpdate();
        }

        /// <summary>
        /// Deletes entire path.
        /// </summary>
        public void DeletePath()
        {
            foreach (var p in PathPoints)
            {
                Destroy(p.Container.gameObject);
            }

            PathPoints.Clear();
        }
        
        public void HidePath()
        {
            foreach (var p in PathPoints)
            {
                p.Container.gameObject.SetActive(false);
            }
            bezierPath.gameObject.SetActive(false);
        }
        
        public void ShowPath()
        {
            foreach (var p in PathPoints)
            {
                p.Container.gameObject.SetActive(true);
            }
            bezierPath.gameObject.SetActive(true);
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void SetAnimationStartTime(float animationStartTime)
        {
            AnimationStartTime = animationStartTime;
        }

        public void SetAnimateOnStart(bool animateOnStart)
        {
            AnimateOnStart = animateOnStart;
        }
    }
}