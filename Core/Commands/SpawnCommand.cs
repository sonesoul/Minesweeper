using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class SpawnCommand(Instruction instruction) : Command(instruction)
    {
        public override void Execute()
        {
            Game1.Field.SpawnMines(new Vector2(-99));
        }
    }
}