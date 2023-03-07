using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Logger = Utilities.Logger;

namespace Actions
{
        public class ActionRecorder : StaticInstance<ActionRecorder>
        {
                private Stack<ActionBase> _actions = new();
                [SerializeField] private Logger logger;
                public void Record(ActionBase action)
                {
                        _actions.Push(action);
                        action.Execute();
                }

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