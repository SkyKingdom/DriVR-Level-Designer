using UnityEngine;

namespace Objects
{
    /// <summary>
    /// Playable object class.<br/> Inherits from <see cref="ObjectBase"/>.<br/> Requires <see cref="Path"/> and <see cref="Playable"/> components.
    /// </summary>
    [RequireComponent(typeof(Path), typeof(Playable))]
    class PlayableObject : ObjectBase
    {
    }
}