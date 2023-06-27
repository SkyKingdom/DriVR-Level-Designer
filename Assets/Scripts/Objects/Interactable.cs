using Objects.Interfaces;
using UnityEngine;

namespace Objects
{
    public class Interactable : MonoBehaviour, IObjectComponent
    {
        public ObjectBase Owner { get; private set; }
        public bool Answer { get; private set; }
        public bool AlwaysInteractable { get; private set; }
        public float InteractionStartTime { get; private set; }
        public float InteractionEndTime { get; private set; }
        
        public void Initialize(ObjectBase objectBase)
        {
            Owner = objectBase;
        }
        public void SetInteractionTime(float interactionStartTime, float interactionEndTime)
        {
            InteractionStartTime = interactionStartTime;
            InteractionEndTime = interactionEndTime;
        }

        public void SetAlwaysInteractable(bool alwaysInteractable, bool answer)
        {
            AlwaysInteractable = alwaysInteractable;
            Answer = answer;
        }
    }
}