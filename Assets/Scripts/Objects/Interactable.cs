using Objects.Interfaces;
using UnityEngine;

namespace Objects
{
    public class Interactable : MonoBehaviour, IInteractable, IObjectComponent
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
        public void SetInteractionValues(bool answer, float interactionStartTime, float interactionEndTime)
        {
            Answer = answer;
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