using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Logger = Utilities.Logger;

namespace Actions
{
    /// <summary>
    /// Keeps track of actions, executes them and allows undoing them.
    /// </summary>
    public class ActionRecorder : StaticInstance<ActionRecorder>
    {
        private readonly Stack<ActionBase> _actions = new();
        [SerializeField] private Logger logger;

        // Records an action and executes it.
        public void Record(ActionBase action)
        {
            _actions.Push(action);
            action.Execute();
        }

        // Undoes last action executed.
        public void Undo()
        {
            if (_actions.Count == 0)
            {
                logger.Log("No actions to undo", this);
                return;
            }

            var action = _actions.Pop();
            action.Undo();
        }
    }
}