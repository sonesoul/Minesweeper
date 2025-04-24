using Minesweeper.Core;

try
{
    using var game = new Main();
    game.Run();
}
catch (System.Exception e)
{
    DialogBox.ShowException(e);
}
