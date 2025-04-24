using Microsoft.Xna.Framework.Graphics;
using PixelBox.Drawing;
using PixelBox.InputHandling;

namespace Minesweeper.Core.Resources
{
    public static class Render
    {
        public static Camera Camera { get; set; }
        public static Drawer Drawer { get; set; }

        public static Vector2 Resolution { get; } = new(161);
        public static Vector2 WindowResolution { get; } = new(640);

        public static Vector2 MousePosition => Drawer.ScreenToWorldPoint(Input.MousePosition, Camera);

        [Init(-1)]
        private static void Init()
        {
            RenderSource source = new(Main.Instance.SpriteBatch, Main.Instance.GraphicsManager);
            RenderOptions options = new()
            {
                BlendState = BlendState.NonPremultiplied,
                SamplerState = SamplerState.PointClamp,
            };

            Drawer drawer = new(source, WindowResolution);
            drawer.Options = options;
            drawer.Canvas.Options = options;
            drawer.UseStretching = false;

            Camera camera = drawer.MainCamera;
            camera.Size = Resolution;
            camera.Options = options;

            Drawer = drawer;
            Camera = camera;
        }
    }
}
