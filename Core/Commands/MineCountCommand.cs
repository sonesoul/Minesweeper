using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class MineCountCommand(Instruction instruction) : Command(instruction)
    {
        public override void Execute()
        {
            Game1.MineCount = int.Parse(RawString);
        }
    }
}