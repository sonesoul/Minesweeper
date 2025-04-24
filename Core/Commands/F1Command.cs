using Minesweeper.Core;
using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class F1Command(Instruction instruction) : Command(instruction)
    {
        public override void Execute() => Main.Instance.Exit();
    }
}
