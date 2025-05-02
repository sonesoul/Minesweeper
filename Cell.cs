using PixelBox.Drawing;

namespace Minesweeper
{
    public enum CellState
    {
        None, 
        MineInfo, 
        Flag,
        Mine
    }
    
    public class Cell(bool isMine)
    {
        public static Color RevealedColor { get; } = new(160, 160, 160);
        public static Color UnrevealedColor { get; } = new(190, 190, 190);
        public static Color HoverColor { get; } = new(210, 210, 210);
        public static Color[] Colors { get; } =
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

        public bool IsMine { get; } = isMine;
        public bool IsRevealed { get; private set; } = false;
        public int AdjacentMines { get; set; } = 0;
        public CellState State => _cellState;


        private DrawOptions infoOptions = new() 
        {
            origin = new(-2, -2),
            scale = Vector2.One,
            color = Color.Red
        };
        private string text = string.Empty;
        private CellState _cellState = CellState.None;

        public void Draw(DrawContext context, in Rectangle rect, Vector2 position, bool highlight)
        {
            Color color = UnrevealedColor;

            if (IsRevealed)
            {
                color = RevealedColor;
            }
            else if (highlight)
            {
                color = HoverColor;
            }
                
            context.Rectangle(rect, color);
            infoOptions.position = position;
            context.String(text, Fonts.Pico8, infoOptions);
        }

        public void Reveal()
        {
            IsRevealed = true;
            SetState(IsMine ? CellState.Mine : CellState.MineInfo);
        }
        public void ToggleFlag()
        {
            if (IsRevealed)
                return;

            SetState(State == CellState.None ? CellState.Flag : CellState.None);
        }
        public void ToggleMineVisible()
        {
            if (IsRevealed || !IsMine)
            {
                return;
            }
            
            SetState(State == CellState.Mine ? CellState.None : CellState.Mine);
        }

        public void UpdateOptions()
        {
            switch (State)
            {
                case CellState.MineInfo:
                    text = $"{AdjacentMines}";
                    infoOptions.color = Colors[AdjacentMines];
                    break;

                case CellState.Flag:
                    text = "P";
                    break;

                case CellState.Mine:
                    text = "*";
                    break;

                default:
                    text = string.Empty;
                    return;
            }
        }

        private void SetState(CellState state)
        {
            _cellState = state;
            UpdateOptions();
        }
    }
}