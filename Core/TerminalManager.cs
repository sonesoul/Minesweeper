using Minesweeper.Core.Commands;
using KernelTerminal.Execution;
using PixelBox.InputHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    public static class TerminalManager
    {
        [Init]
        private static void Init()
        {
            Terminal.HideFromTaskbar = false;
            Terminal.HideButtons = false;
            Terminal.Opened = OnOpened;

            string exitCommand = "f1";

            Input.Bind(Key.F1, KeyPhase.Press, () => Executor.Create(exitCommand).Execute());
            Input.Bind(Key.OemTilde, KeyPhase.Press, Terminal.Toggle);

            Executor.RegisterCommand(exitCommand, i => new F1Command(i));
            Executor.RegisterFactory(new CommandFactory()
            {
                ["field"] = i => new FieldSizeCommand(i),
                ["lock"] = i => new LockUnlockCommand(i, true),
                ["unlock"] = i => new LockUnlockCommand(i, false),
                ["mines"] = i => new MineCountCommand(i),
                ["restart"] = i => new RestartCommand(i),
                ["reveal"] = i => new RevealCommand(i),
                ["spawn"] = i => new SpawnCommand(i),
            });
        }

        private static void HandleInput()
        {
            while (true)
            {
                try
                {
                    Executor.Create(Console.ReadLine())?.Execute();
                }
                catch (IOException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Terminal.WriteLine(ex.Message, ConsoleColor.Red).Wait();
                }
            }
        }

        private static void OnOpened()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var sb = new StringBuilder();
            var rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            List<ConsoleColor> colors =
                Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>()
                .Where(c => c != ConsoleColor.Black)
                .ToList();

            int colorIndex = rnd.Next(colors.Count);

            Terminal.WriteLine($"| KernelTerminal [{sb}]\n", colors[colorIndex]);
            Console.SetWindowSize(60, 15);

            Task.Run(HandleInput);
        }
    }
}
