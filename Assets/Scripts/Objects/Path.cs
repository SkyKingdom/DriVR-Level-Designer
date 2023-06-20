using System.Collections.Generic;
using Objects.Interfaces;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

namespace Objects
{
    public class Path : MonoBehaviour, IPath, IObjectComponent
    {
        public ObjectBase Owner { get; private set; }
        public  List<PathPoint> PathPoints { get; private set; }
        
        public bool AnimateOnStart { get; private set; }
        public float AnimationStartTime { get; private set; }
        public float Speed { get; private set; }

        [SerializeField] private PathCreator bezierPath;
        private RoadMeshCreator meshCreator;

        public bool Highlighted { get; set; }
        
        
        public void Initialize(ObjectBase objectBase)
        {
            Owner = objectBase;
            PathPoints = new List<PathPoint>();
        }

        public void SetPath(PathCreator pathCreator)
        {
            bezierPath = pathCreator;
            meshCreator = bezierPath.GetComponent<RoadMeshCreator>();
        }

        public void Spawn(bool select = true)
        {
            PathPointContainer cont = Instantiate(PathManager.Instance.pathPointPrefab, Owner.GetPosition(),
                Quaternion.Euler(Owner.GetRotation()));
            PathPoint n = new PathPoint(cont, Owner, Owner.GetPosition());
            cont.PathPoint = n;
            AddPathPoint(n, select);
        }

        public void AddPathPoint(PathPoint pathPoint, bool select = true)
        {
            PathPoints.Add(pathPoint);
            UpdatePath();
        }

        public void RemovePathPoint(PathPoint pathPoint)
        {
            PathPoints.Remove(pathPoint);
            pathPoint.Dispose();
            UpdatePath();
        }
        
        public int RemovePathTemporarily(PathPoint pathPoint)
        {
            var i = PathPoints.IndexOf(pathPoint);
            pathPoint.Container.Delete();
            PathPoints.Remove(pathPoint);
            UpdatePath();
            return i;
        }
        
        public void AddPathPoint(PathPoint pathPoint, int index)
        {
            PathPoints.Insert(index, pathPoint);
            UpdatePath();
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

        private void TriggerPathUpdate()
        {
            bezierPath.TriggerPathUpdate();
            meshCreator.TriggerUpdate();
        }

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