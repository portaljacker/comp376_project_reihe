using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectReihe
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            TitleScreen,
            Battle,
            GameOver
        }
        GameState currentGameState = GameState.Battle;

        Menu menu;
        SpriteFont arial;
        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;

        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            menu = new Menu(Menu.MenuType.Battle);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Put the name of the font
            _spr_font = Content.Load<SpriteFont>("FPS");
            arial = Content.Load<SpriteFont>("Arial");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            // Update
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }
 
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            switch(currentGameState)
            {
                case GameState.Battle:
                    if (KeyPressed(Keys.Down))
                        menu.Iterator += 1;
                    else if (KeyPressed(Keys.Up))
                        menu.Iterator -= 1;
                    else if (KeyPressed(Keys.Space))
                    {
                        if (menu.CurrentMenuType == Menu.MenuType.Battle && menu.Iterator == 0)
                            menu = new Menu(Menu.MenuType.Fight);
                    }
                    else if (KeyPressed(Keys.Back))
                    {
                        if (menu.CurrentMenuType == Menu.MenuType.Fight)
                            menu = new Menu(Menu.MenuType.Battle);
                    }
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // Only update total frames when drawing
            _total_frames++;
            menu.DrawMenu(spriteBatch, graphics.PreferredBackBufferWidth, arial);

            spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps), Vector2.Zero, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #region Keyboard Region

        public void Flush()
        {
            lastKeyboardState = keyboardState;
        }

        public bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
            lastKeyboardState.IsKeyDown(key);
        }
        public bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
            lastKeyboardState.IsKeyUp(key);
        }
        public bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        #endregion
    }
}
