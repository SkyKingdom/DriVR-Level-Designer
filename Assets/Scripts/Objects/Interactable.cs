using Objects.Interfaces;
using UnityEngine;

namespace Objects
{
    public class Interactable : MonoBehaviour, IObjectComponent
    {
        // The object that owns this component.
        public ObjectBase Owner { get; private set; }
        
        // The answer to the interaction.
        public bool Answer { get; private set; }
        
        // Whether the object is always interactable.
        public bool AlwaysInteractable { get; private set; }
        
        // The time at which the interaction starts and ends.
        public float InteractionStartTime { get; private set; }
        public float InteractionEndTime { get; private set; }
        
        // Initialize the component.
        public void Initialize(ObjectBase objectBase)
        {
            Owner = objectBase;
        }
        
        // Set the interaction time.
        public void SetInteractionTime(float interactionStartTime, float interactionEndTime)
        {
            InteractionStartTime = interactionStartTime;
            InteractionEndTime = interactionEndTime;
        }

        // Set whether the object is always interactable.
        public void SetAlwaysInteractable(bool alwaysInteractable, bool answer)
        {
            AlwaysInteractable = alwaysInteractable;
            Answer = answer;
        }
    }
}