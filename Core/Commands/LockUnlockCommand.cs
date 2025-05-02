using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class LockUnlockCommand(Instruction instruction, bool isLock) : Command(instruction)
    {
        public override void Execute()
        {
            if (isLock)
            {
                GameController.IsLocked = true;
            }
            else
            {
                GameController.IsLocked = false;
            }
        }
    }
}
