using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class RevealCommand(Instruction instruction) : Command(instruction)
    {
        public override void Execute()
        {
            var pos = Game1.Field.TargetCell;
            Game1.Field.Cells[pos.X, pos.Y].IsRevealed = true;
        }
    }
}