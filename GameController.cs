using PixelBox.Drawing;
using PixelBox.InputHandling;

namespace Minesweeper
{
    public static class GameController
    {
        public static Field Field { get; private set; }
        public static Point FieldSize { get; set; } = new(20);
        public static int MineCount { get; set; } = 99;
        public static bool IsLocked { get; set; } = false;


        [Init]
        private static void Init()
        {
            CreateField();

            Input.Bind(Key.MouseLeft, KeyPhase.Press, () =>
            {
                if (IsLocked) 
                    return;

                Field.Reveal();
            });

            Input.Bind(Key.MouseRight, KeyPhase.Press, () =>
            {
                if (IsLocked)
                    return;

                Field.GetTargetCell().ToggleFlag();              
            });

            Input.Bind(Key.R, KeyPhase.Press, CreateField);

            Render.Camera.Register(Draw);
        }

        private static void Draw(DrawContext context)
        {
            var normalizedPos = Render.MousePosition / Render.Camera.Size;

            Field.TargetCell = (FieldSize.ToVector2() * normalizedPos).Floored().ToPoint();

            Field.Draw(context);
        }

        private static void OnMineRevealed()
        {
            Field.ForEachCell((x, y) => Field.Cells[x, y].ToggleMineVisible());
            IsLocked = true;

            Field.MineRevealed -= OnMineRevealed;
        }
        public static void CreateField()
        {
            Vector2 fieldSize = FieldSize.ToVector2();

            Render.Camera.Size = fieldSize * 8;

            Field = new Field(FieldSize, (Render.Camera.Size / fieldSize).ToPoint(), MineCount);
            Field.MineRevealed += OnMineRevealed;
            IsLocked = false;
        }
    }
}