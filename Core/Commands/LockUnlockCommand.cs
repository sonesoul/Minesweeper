using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class LockUnlockCommand(Instruction instruction, bool isLock) : Command(instruction)
    {
        public override void Execute()
        {
            if (isLock)
            {
                Game1.IsLocked = true;
            }
            else
            {
                Game1.IsLocked = false;
            }
        }
    }
}
