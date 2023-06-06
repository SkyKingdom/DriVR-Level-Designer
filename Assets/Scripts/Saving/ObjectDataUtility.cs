using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Saving
{
    public static class ObjectDataUtility
    {
        /// <summary>
        /// Returns path point as a Vector3 array
        /// </summary>
        public static Vector3[] GetObjectPath(Path path)
        {
            var pathPoints = new List<Vector3>();
            foreach (var point in path.PathPoints)
            {
                pathPoints.Add(point.Position);
            }

            return pathPoints.ToArray();
        }

        /// <summary>
        /// Sets up playable component values
        /// </summary>
        public static void SetupPlayable(Playable playable, PlayableObjectData playableData)
        {
            playable.SetSwitchTime(playableData.switchTime);
            playable.SetPlayOnStart(playableData.switchTime <= 0);
        }

        /// <summary>
        /// Sets up path component values
        /// </summary>
        public static bool SetupPath(Path path, PathObjectData pathData)
        {
            path.SetSpeed(pathData.speed);
            path.SetAnimationStartTime(pathData.animationStartTime);
            path.SetAnimateOnStart(pathData.animationStartTime <= 0);

            return pathData.pathPoints.Length > 1;
        }

        /// <summary>
        /// Sets up interactable component values
        /// </summary>
        public static void SetupInteractable(Interactable interactable, InteractableObjectData interactableData)
        {
            var isAlwaysInteractable = interactableData.interactionStartTime <= 0 && interactableData.interactionEndTime <= 0;
            interactable.SetAlwaysInteractable(isAlwaysInteractable, interactableData.isCorrect);
            interactable.SetInteractionTime(interactableData.interactionStartTime, interactableData.interactionEndTime);
        }
    }
}