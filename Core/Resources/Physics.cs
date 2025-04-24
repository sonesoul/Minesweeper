using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D;

namespace Minesweeper.Core.Resources
{
    public static class Physics
    {
        public static World World { get; private set; }
        public static int UpdateCount { get; private set; } = 1;

        static Physics()
        {
            Settings.AllowSleep = false;
            Settings.ContinuousPhysics = true;

            Time.FixedDelta = 1f / 60;

            World = new(new Vector2(0, 2 / Time.FixedDelta));
        }

        public static void Update()
        {
            for (int i = 0; i < UpdateCount; i++)
            {
                World.Step(Time.FixedDelta);
            }
        }
    }
}
