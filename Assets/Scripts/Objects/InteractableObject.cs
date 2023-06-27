using UnityEngine;

namespace Objects
{
    /// <summary>
    /// Interactable object class.<br/> Inherits from <see cref="ObjectBase"/>.<br/> Requires <see cref="Path"/> and <see cref="Interactable"/> components.
    /// </summary>
    [RequireComponent(typeof(Path), typeof(Interactable))]
    class InteractableObject : ObjectBase
    {
    }
}