﻿using KernelTerminal.Execution;

namespace Minesweeper.Core.Commands
{
    public class RestartCommand(Instruction instruction) : Command(instruction)
    {
        public override void Execute() => GameController.CreateField();
    }
}