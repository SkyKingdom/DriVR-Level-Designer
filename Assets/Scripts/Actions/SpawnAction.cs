using Objects;
using UnityEngine;

namespace Actions
{
    class SpawnAction : ActionBase
    {
        private ObjectManager _manager;
        private ObjectBase _spawnedObject;
        private Vector3 _position;

        public SpawnAction(ObjectManager manager, Vector3 position)
        {
            _manager = manager;
            _position = position;
        }
        public override void Execute()
        {
            _spawnedObject = _manager.Spawn(_position);
        }

        public override void Undo()
        {
            _spawnedObject.gameObject.Destroy();
        }
    }
}