using System.Collections.Generic;
using Utilities;

namespace Actions
{
        public class ActionRecorder : StaticInstance<ActionRecorder>
        {
                private Stack<ActionBase> _actions = new();
                public void Record(ActionBase action)
                {
                        _actions.Push(action);
                        action.Execute();
                }

                public void Undo()
                {
                        var action = _actions.Pop();
                        action.Undo();
                }
        }
}