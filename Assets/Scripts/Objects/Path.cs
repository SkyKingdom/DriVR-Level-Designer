using System;
using System.Collections.Generic;
using Objects.Interfaces;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(LineRenderer))]
    public class Path : MonoBehaviour, IPath, IObjectComponent
    {
        public ObjectBase Owner { get; private set; }
        public  List<Node> PathPoints { get; private set; }
        public LineRenderer LineRenderer { get; private set; }
        public bool AnimateOnStart { get; private set; }
        public float AnimationStartTime { get; private set; }
        public float Speed { get; private set; }
        
        public void Initialize(ObjectBase objectBase)
        {
            Owner = objectBase;
            PathPoints = new List<Node>();
            LineRenderer = GetComponent<LineRenderer>();
            LineRenderer.positionCount = 0;
            LineRenderer.numCapVertices = 10;
        }

        public void Spawn(bool select = true)
        {
            NodeContainer cont = Instantiate(PathManager.Instance.pathPointPrefab, Owner.GetPosition(),
                Quaternion.Euler(Owner.GetRotation()));
            Node n = new Node(cont.gameObject, Owner, Owner.GetPosition());
            cont.node = n;
            AddPathPoint(n);
            if (select)
                Select();
            else
                Deselect();
        }

        public void AddPathPoint(Node node)
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

        public void RepositionPathPoint(Node node)
        {
            int id = PathPoints.IndexOf(node);
            LineRenderer.SetPosition(id, node.Position);
        }

        public void HandleObjectReposition(Vector3 position) => PathPoints[0].SetPosition(position);

        public void Select()
        {
            LineRenderer.startWidth = 0.2f;
            LineRenderer.endWidth = 0.2f;
            LineRenderer.endColor = Color.yellow;
            LineRenderer.startColor = Color.yellow;
            LineRenderer.material = PathManager.Instance.Selected;
        }

        public void Deselect()
        {
            LineRenderer.startWidth = 0.1f;
            LineRenderer.endWidth = 0.1f;
            LineRenderer.endColor = Color.white;
            LineRenderer.startColor = Color.white;
            LineRenderer.material = PathManager.Instance.Deselected;
        }

        public void UpdatePath()
        {
            LineRenderer.positionCount = PathPoints.Count;
            for (int i = 0; i < PathPoints.Count; i++)
            {
                LineRenderer.SetPosition(i, PathPoints[i].Position);
            }
        }

        public void DeletePath()
        {
            foreach (var p in PathPoints)
            {
                Destroy(p.GameObject);
            }

            PathPoints.Clear();
        }
        
        public void HidePath()
        {
            foreach (var p in PathPoints)
            {
                p.GameObject.SetActive(false);
            }
            LineRenderer.enabled = false;
        }

        public void ShowPath()
        {
            foreach (var p in PathPoints)
            {
                p.GameObject.SetActive(true);
            }
            LineRenderer.enabled = true;
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