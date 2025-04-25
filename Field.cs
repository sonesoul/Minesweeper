using PixelBox.Drawing;
using System;

namespace Minesweeper
{
    public class Field
    {
        public static Color RevealedColor { get; } = new(160, 160, 160);
        public static Color UnrevealedColor { get; } = new(190, 190, 190);
        public static Color HoverColor { get; } = new(210, 210, 210);
        public static Color GridColor { get; } = new(130, 130, 130);

        public Cell[,] Cells { get; set; }

        public Point Size { get; }
        public Point CellSize { get; }
        

        public event Action MineRevealed;
        
        private bool isMinesSpawned = false;
        private int mineCount = 0;

        public Point TargetCell { get; set; } = new Point(0);

        public Color[] Colors { get; } =
        {
            RevealedColor * 0.95f,
            Color.Blue,
            Color.Green,
            Color.Red,
            Color.DarkBlue,
            Color.DarkRed, 
            Color.DarkCyan,
            Color.Black, 
            Color.LightGray,
        };

        public Field(Point size, Point cellSize, int mineCount)
        {
            Size = size;
            CellSize = cellSize;
            Cells = new Cell[size.X, size.Y];
            this.mineCount = mineCount;

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    Cells[x, y] = new(false);
                }
            }
        }

        public void Draw(DrawContext context)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    Cell cell = Cells[x, y];
                    Color color = UnrevealedColor;

                    if (cell.IsRevealed)
                        color = RevealedColor;
                    else if (x == TargetCell.X && y == TargetCell.Y)
                        color = HoverColor;
                   
                    context.Rectangle(new Rectangle(CellSize * new Point(x, y), CellSize), color);
                    context.Rectangle(new Rectangle(CellSize * new Point(x, y), CellSize), GridColor, 1);
                    
                    if (cell.IsRevealed)
                    {
                        if (cell.IsMine)
                        {
                            context.String(
                                $"*",
                                Fonts.Pico8,
                                CellSize.ToVector2() * new Vector2(x, y),
                                Color.Red,
                                new Vector2(1f),
                                new Vector2(-3, -2));
                        }
                        else
                        {
                            context.String(
                                $"{cell.AdjacentMines}", 
                                Fonts.Pico8, 
                                CellSize.ToVector2() * new Vector2(x, y), 
                                Colors[cell.AdjacentMines], 
                                new Vector2(1f), 
                                new Vector2(-3, -2));
                        }
                    }
                    else if (cell.HasFlag)
                    {
                        context.String(
                            $"P",
                            Fonts.Pico8,
                            CellSize.ToVector2() * new Vector2(x, y),
                            Color.Red,
                            new Vector2(1f),
                            new Vector2(-3, -2));
                    }
                }
            }
        }


        public void Reveal()
        {
            if (!isMinesSpawned)
            {
                SpawnMines(TargetCell.ToVector2());
            }

            Reveal(TargetCell.X, TargetCell.Y);
        }
        public void ToggleFlag()
        {
            int x = TargetCell.X;
            int y = TargetCell.Y;

            if (IsInBounds(x, y))
            {
                var cell = Cells[x, y];
                Cells[x, y].HasFlag = !cell.HasFlag;
            }
        }

        public void RevealMines()
        {
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    if (Cells[x, y].IsMine)
                        Cells[x, y].IsRevealed = true;
                }
            }
        }

        public void SpawnMines(Vector2 safeZone)
        {
            Random random = new();

            do
            {
                int x = random.Next(Cells.GetLength(0));
                int y = random.Next(Cells.GetLength(1));

                if (new Vector2(x, y).DistanceTo(safeZone) < 3)
                    continue;

                if (Cells[x, y].IsMine)
                    continue;

                Cells[x, y] = new(true);
                mineCount--;

            } while (mineCount > 0);

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    if (Cells[x, y].IsMine)
                        continue;

                    Cells[x, y].AdjacentMines = GetAdjacentMines(new Point(x, y));
                }
            }

            isMinesSpawned = true;
        }

        private int GetAdjacentMines(Point pos)
        {
            int count = 0;

            int minX = Math.Max(pos.X - 1, 0);
            int maxX = Math.Min(pos.X + 1, Size.X - 1);

            int minY = Math.Max(pos.Y - 1, 0);
            int maxY = Math.Min(pos.Y + 1, Size.Y - 1);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (x == pos.X && y == pos.Y)
                        continue;

                    if (Cells[x, y].IsMine)
                        count++;
                }
            }

            return count;
        }
        private void Reveal(int x, int y)
        {
            if (!IsInBounds(x, y))
                return;

            var cell = Cells[x, y];

            if (cell.IsMine)
            {
                MineRevealed?.Invoke();
                return;
            }

            if (cell.IsRevealed)
                return;

            cell.IsRevealed = true;

            if (cell.AdjacentMines > 0)
                return;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    Reveal(nx, ny);
                }
            }

        }

        private bool IsInBounds(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= Size.X || y >= Size.Y);
        }
    }

    public class Cell(bool isMine)
    {
        public bool IsMine { get; } = isMine;
        public bool IsRevealed { get; set; } = false;
        public bool HasFlag { get; set; } = false;
        public int AdjacentMines { get; set; } = 0;
    }
}