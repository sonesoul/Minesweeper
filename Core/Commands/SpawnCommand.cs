using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class SpawnCommand(Instruction instruction) : Command(instruction)
    {
        public override void Execute()
        {
            GameController.Field.SpawnMines(new Vector2(-99));
        }
    }
}