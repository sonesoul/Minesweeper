using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper.Core.Resources
{
    public static class Fonts
    {
        public static SpriteFont Pico8 { get; private set; }

        [Load]
        private static void Load() => Pico8 = Asset.Load<SpriteFont>(@"Fonts\Pico-8 Mono");
    }
}
