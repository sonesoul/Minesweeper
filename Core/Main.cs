global using Microsoft.Xna.Framework;
global using KernelTerminal;
global using PixelBox;
global using PixelBox.Extensions;
global using Minesweeper.Core.Resources;
global using Minesweeper.Core.Attributes;

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelBox.InputHandling;
using System.Collections;

namespace Minesweeper.Core
{
    public class Main : Game
    {
        public static Main Instance { get; private set; }

        public GraphicsDeviceManager GraphicsManager { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
           
        public Main()
        {
            Instance = this;
            
            GraphicsManager = new(this)
            {
                PreferredBackBufferWidth = (int)Render.WindowResolution.X,
                PreferredBackBufferHeight = (int)Render.WindowResolution.Y,
                SynchronizeWithVerticalRetrace = true,
            };

            Window.AllowAltF4 = false;
            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 60);
            InactiveSleepTime = TimeSpan.Zero;

            Activated += (o, e) =>
            {
                static IEnumerator Unlock()
                {
                    yield return null;
                    GameController.IsLocked = false;
                }

                StepTask.Run(Unlock);
            };
            Deactivated += (o, e) => GameController.IsLocked = true;

            Content.RootDirectory = "Content";
            Asset.Content = Content;
        }

        protected override void Initialize()
        {
            SpriteBatch = new(GraphicsDevice);

            InitAttribute.Invoke();

            base.Initialize(); //calls LoadContent
        }
        protected override void LoadContent()
        {
            LoadAttribute.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            Input.Update(Keyboard.GetState(), Mouse.GetState());
            StepTask.Manager.Update();
        }
        protected override void Draw(GameTime gameTime)
        {
            Render.Drawer.Draw();
        }
    }
}
