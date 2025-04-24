using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class FieldSizeCommand(Instruction instruction) : Command(instruction)
    {
        public override void Execute()
        {
            Game1.FieldSize = new(int.Parse(RawString));
        }
    }
}