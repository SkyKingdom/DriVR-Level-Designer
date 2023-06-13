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
        public  List<Node> PathPoints { get; private set; }
        
        public bool AnimateOnStart { get; private set; }
        public float AnimationStartTime { get; private set; }
        public float Speed { get; private set; }

        [SerializeField] private PathCreator bezierPath;
        private RoadMeshCreator meshCreator;

        public bool Highlighted { get; set; }
        
        
        public void Initialize(ObjectBase objectBase)
        {
            Owner = objectBase;
            PathPoints = new List<Node>();
        }

        public void SetPath(PathCreator pathCreator)
        {
            bezierPath = pathCreator;
            meshCreator = bezierPath.GetComponent<RoadMeshCreator>();
        }

        public void Spawn(bool select = true)
        {
            NodeContainer cont = Instantiate(PathManager.Instance.pathPointPrefab, Owner.GetPosition(),
                Quaternion.Euler(Owner.GetRotation()));
            Node n = new Node(cont, Owner, Owner.GetPosition());
            cont.Node = n;
            AddPathPoint(n, select);
        }

        public void AddPathPoint(Node node, bool select = true)
        {
            PathPoints.Add(node);
            UpdatePath();
        }

        public void RemovePathPoint(Node node)
        {
            PathPoints.Remove(node);
            node.Dispose();
            UpdatePath();
        }
        
        public int RemovePathTemporarily(Node node)
        {
            var i = PathPoints.IndexOf(node);
            PathPoints.Remove(node);
            UpdatePath();
            return i;
        }
        
        public void AddPathPoint(Node node, int index)
        {
            PathPoints.Insert(index, node);
            UpdatePath();
        }

        public void RepositionPathPoint(Node node) => UpdatePath();


        public void HandleObjectReposition(Vector3 position) => PathPoints[0].SetPosition(position);
        
        public void HighlightPath(NodeContainer initiator)
        {
            Highlighted = true;
            foreach (var p in PathPoints)
            {
                if (p.Container == initiator)
                    continue;
                p.Container.Highlight();
            }
        }
        
        public void UnhighlightPath(NodeContainer initiator)
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