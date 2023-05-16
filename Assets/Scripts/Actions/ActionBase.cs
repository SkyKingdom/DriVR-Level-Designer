using System.Runtime.InteropServices;

namespace Actions
{
    public abstract class ActionBase
    {
        public abstract void Execute();

        public abstract void Undo();

    }
}