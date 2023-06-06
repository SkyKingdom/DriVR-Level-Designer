using Objects;
using UnityEngine;

namespace Saving
{
    public static class ObjectFactory
    {
        /// <summary>
        /// Create interactable object data from object base
        /// </summary>
        public static InteractableObjectData CreateInteractable(ObjectBase obj)
        {
            var objTransform = obj.transform;
            var objInteractable = obj.Interactable;
            
            InteractableObjectData interactableData = new InteractableObjectData();
            
            // Setup object data
            interactableData.prefabName = obj.PrefabName;
            interactableData.objectName = obj.ObjectName;
            interactableData.position = objTransform.position;
            interactableData.rotation = objTransform.rotation.eulerAngles;

            // Setup interaction data
            interactableData.interactionStartTime = objInteractable.AlwaysInteractable ? -1 : objInteractable.InteractionStartTime;
            interactableData.interactionEndTime = objInteractable.AlwaysInteractable ? -1 : objInteractable.InteractionEndTime;
            interactableData.isCorrect = objInteractable.Answer;
            
            // Setup path data
            interactableData.pathPoints = ObjectDataUtility.GetObjectPath(obj.Path);
            interactableData.speed = obj.Path.Speed;
            interactableData.animationStartTime = obj.Path.AnimationStartTime;

            return interactableData;
        }

        /// <summary>
        /// Create playable object data from object base
        /// </summary>
        public static PlayableObjectData CreatePlayable(ObjectBase obj)
        {
            var objTransform = obj.transform;
            var objPlayable = obj.Playable;
            
            var playableData = new PlayableObjectData();
            
            // Setup object data
            playableData.prefabName = obj.PrefabName;
            playableData.objectName = obj.ObjectName;
            playableData.position = objTransform.position;
            playableData.rotation = objTransform.rotation.eulerAngles;
            
            // Setup path data
            playableData.pathPoints = ObjectDataUtility.GetObjectPath(obj.Path);
            playableData.speed = obj.Path.Speed;
            playableData.animationStartTime = obj.Path.AnimationStartTime;
            
            // Setup playable data
            playableData.switchTime = objPlayable.PlayOnStart ? -1 : objPlayable.SwitchTime;
            
            return playableData;
        }

        /// <summary>
        /// Create decorative object data from object base
        /// </summary>
        public static DecorativeObjectData CreateDecorative(ObjectBase obj)
        {
            var objTransform = obj.transform;
            
            var decorativeData = new DecorativeObjectData();

            // Setup object data
            decorativeData.prefabName = obj.PrefabName;
            decorativeData.objectName = obj.ObjectName;
            decorativeData.position = objTransform.position;
            decorativeData.rotation = objTransform.rotation.eulerAngles;
            
            return decorativeData;
        }

        /// <summary>
        /// Instantiate object and returns object base
        /// </summary>
        public static ObjectBase InstantiateObjectBase(DecorativeObjectData objData, GameObject prefab)
        {
            var obj = Object.Instantiate(prefab, objData.position, Quaternion.Euler(objData.rotation));
            var objBase = obj.GetComponent<ObjectBase>();
            objBase.Initialize(objData.objectName, objData.prefabName, false);

            return objBase;
        }
        
        /// <summary>
        /// Instantiates path point and returns container
        /// </summary>
        public static NodeContainer InstantiateNodeContainer(GameObject prefab, Vector3 pos)
        {
            var container = Object.Instantiate(prefab, pos, Quaternion.identity).GetComponent<NodeContainer>();
            return container;
        }
    }
}