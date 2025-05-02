using PixelBox.Drawing;
using System;

namespace Minesweeper
{
    public class Field
    {
        public static Color GridColor { get; } = new(130, 130, 130);

        public Cell[,] Cells { get; set; }
        public Point TargetCell { get; set; } = new Point(0);
        public Point Size { get; }
        public Point CellSize { get; }

        public event Action MineRevealed;
        
        private bool isMinesSpawned = false;
        private int mineCount = 0;

        public Field(Point size, Point cellSize, int mineCount)
        {
            Size = size;
            CellSize = cellSize;
            Cells = new Cell[size.X, size.Y];
            this.mineCount = mineCount;

            ForEachCell((x, y) => Cells[x, y] = new(false));
        }

        public void Draw(DrawContext context)
        {
            ForEachCell((x, y) =>
            {
                Point position = CellSize * new Point(x, y);
                bool highlight = x == TargetCell.X && y == TargetCell.Y;

                Cells[x, y].Draw(
                    context, 
                    new Rectangle(position, CellSize), 
                    position.ToVector2(),
                    highlight);
            });

            for (int x = 0; x < Size.X; x++)
            {
                Vector2 end = (CellSize * new Point(x, Size.Y)).ToVector2();
                context.Line(end.WhereY(0), end, GridColor);
            }
            for (int y = 0; y < Size.Y; y++)
            {
                Vector2 end = (CellSize * new Point(Size.X, y)).ToVector2();
                context.Line(end.WhereX(0), end, GridColor);
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

            ForEachCell((x, y) =>
            {
                if (Cells[x, y].IsMine)
                    return;

                Cells[x, y].AdjacentMines = GetAdjacentMines(new Point(x, y));
            });
            isMinesSpawned = true;
        }
        public Cell GetTargetCell()
        {
            int x = TargetCell.X;
            int y = TargetCell.Y;

            if (IsInBounds(x, y))
                return Cells[x, y];

            return null;
        }

        public void ForEachCell(Action<int, int> action)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    action(x, y);
                }
            }
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

            cell.Reveal();

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
}