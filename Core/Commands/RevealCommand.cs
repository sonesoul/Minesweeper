using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class RevealCommand(Instruction instruction) : Command(instruction)
    {
        public override void Execute()
        {
            var pos = GameController.Field.TargetCell;
            GameController.Field.Cells[pos.X, pos.Y].Reveal();
        }
    }
}