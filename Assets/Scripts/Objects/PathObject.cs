using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(LineRenderer))]
    public class PathObject : ObjectBase
    {
        private Stack<Node> _pathPoints = new();
        public Stack<Node> PathPoints => _pathPoints;
        
        public bool AnimateOnStart { get; protected set; }

        public float Speed { get; protected set; }
        
        public float AnimationStartTime { get; protected set; }
        
        private LineRenderer _lineRenderer;

        public override void OnSpawn()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.startWidth = 0.2f;
            _lineRenderer.endWidth = 0.2f;
            _lineRenderer.endColor = Color.yellow;
            _lineRenderer.startColor = Color.yellow;
            _lineRenderer.material = PathManager.Instance.Selected;
            _lineRenderer.positionCount = 0;
            _lineRenderer.numCapVertices = 10;
            var pathObject = Instantiate(PathManager.Instance.pathPointPrefab, transform.position, Quaternion.identity);
            Node node = new Node(pathObject, transform.position);
            AddPathPoint(node);
            Debug.Log("PathObject spawned");
        }

        public override void OnReposition()
        {
            var points = new Stack<Node>(_pathPoints).ToArray();
            points[0].SetPosition(transform.position);
            _pathPoints.Clear();
            for (int i = 0; i < points.Length; i++)
            {
                _pathPoints.Push(points[i]);
                _lineRenderer.SetPosition(i, points[i].Position);
            }
        }

        public override void Select()
        {
            base.Select();
            if (_lineRenderer == null)
                return;
            _lineRenderer.startWidth = 0.2f;
            _lineRenderer.endWidth = 0.2f;
            _lineRenderer.material = PathManager.Instance.Selected;
        }
        
        public override void Deselect()
        {
            base.Deselect();
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
            _lineRenderer.endColor = Color.white;
            _lineRenderer.startColor = Color.white;
            _lineRenderer.material = PathManager.Instance.Deselected;
        }

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
            _lineRenderer.positionCount = _pathPoints.Count;
            _lineRenderer.SetPosition(_pathPoints.Count - 1, point.Position);
        }
    }
}