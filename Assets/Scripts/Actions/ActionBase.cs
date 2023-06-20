using System.Runtime.InteropServices;

namespace Actions
{
    /// <summary>
    /// Abstract class for commands.
    /// </summary>
    public abstract class ActionBase
    {
        public abstract void Execute();

        public abstract void Undo();

    }
}