namespace Minesweeper
{
    public class Cell(bool isMine)
    {
        public bool IsMine { get; } = isMine;
        public bool IsRevealed { get; set; } = false;
        public bool HasFlag { get; set; } = false;
        public int AdjacentMines { get; set; } = 0;
    }
}
