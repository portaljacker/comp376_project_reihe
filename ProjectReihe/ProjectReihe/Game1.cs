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
        SpriteFont menuFont;
        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;

        List<Skills.Skill> chain;
        int skillCount = 0;
        bool showMenu = false;

        float battleTimer = 0.0f;
        int battleStep = 0;

        Character cadwyn;
        Character bossSlime;

        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic 

            menu = new Menu(Menu.MenuType.Battle);
            chain = new List<Skills.Skill>();
            cadwyn = new Character(Character.CharacterType.Cadwyn, Content.Load<Texture2D>("cadwynsheet"), new Vector2(graphics.PreferredBackBufferWidth - (385/2 * 1.5f + 25), graphics.PreferredBackBufferHeight - (327/2 * 1.5f + 25)), 384, 327);
            bossSlime = new Character(Character.CharacterType.BossSlime, Content.Load<Texture2D>("slimesheet"), new Vector2(25 + 384/2 * 1.5f, 25 + 327/2 * 1.5f), 384, 327);

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
            menuFont = Content.Load<SpriteFont>("MenuFont");
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
            battleTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            if (showMenu)
            {
                switch (battleStep)
                {
                    case 0:
                        if (battleTimer >= 500.0f)
                        {
                            cadwyn.CurrrentFrame++;
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 1:
                        if (battleTimer >= 750.0f)
                        {
                            cadwyn.CurrrentFrame = 0;
                            bossSlime.Position -= new Vector2(25, 0);
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 2:
                        if (battleTimer >= 750.0f)
                        {
                            bossSlime.Position += new Vector2(25, 0);
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 3:
                        if (battleTimer >= 1000.0f)
                        {
                            bossSlime.Position += new Vector2(25, 0);
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 4:
                        if (battleTimer >= 500.0f)
                        {
                            bossSlime.Position -= new Vector2(25, 0);
                            cadwyn.Position += new Vector2(25, 0);
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 5:
                        if (battleTimer >= 500.0f)
                        {
                            cadwyn.Position -= new Vector2(25, 0);
                            battleTimer = 0;
                            battleStep = 0;
                            showMenu = false;
                        }
                        break;
                }
               
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
                        else if (menu.CurrentMenuType == Menu.MenuType.Fight)
                        {
                            if (skillCount < 3)
                            {
                                switch (menu.Iterator)
                                {
                                    case 0:
                                        chain.Add(Skills.Skill.Attack);
                                        menu.InfoText += "Attack ";
                                        skillCount++;
                                        if (skillCount == 3)
                                            showMenu = true;
                                        break;
                                    case 1:
                                        chain.Add(Skills.Skill.Fire);
                                        menu.InfoText += "Fire ";
                                        skillCount++;
                                        if (skillCount == 3)
                                        {
                                            showMenu = true;
                                            chain = new List<Skills.Skill>();
                                            menu.InfoText = String.Empty;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else if (KeyPressed(Keys.Back))
                    {
                        if (menu.CurrentMenuType == Menu.MenuType.Fight)
                        {
                            menu = new Menu(Menu.MenuType.Battle);
                            skillCount = 0;
                            chain = new List<Skills.Skill>();
                            menu.InfoText = string.Empty;
                        }
                    }
                    if (skillCount == 3)
                    {
                        List<Skills.Skill> slimeChain = new List<Skills.Skill>();
                        slimeChain.Add(Skills.Skill.Attack);
                        slimeChain.Add(Skills.Skill.Fire);
                        slimeChain.Add(Skills.Skill.Attack);
                        Console.WriteLine(string.Format("Boss Slime HP before: {0:0}/{1:0}", bossSlime.HP, bossSlime.MaxHP));
                        cadwyn.attack(bossSlime, chain);
                        Console.WriteLine(string.Format("Boss Slime HP after: {0:0}/{1:0}", bossSlime.HP, bossSlime.MaxHP));
                        Console.WriteLine(string.Format("Cadwyn HP before: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP));
                        bossSlime.attack(cadwyn, slimeChain);
                        Console.WriteLine(string.Format("Cadwyn HP before: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP));
                        //showMenu = false;
                        skillCount = 0;
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

            if (!showMenu)
            {
                menu.DrawMenu(spriteBatch, graphics.PreferredBackBufferWidth, menuFont);
            }
            spriteBatch.DrawString(menuFont, menu.InfoText, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.White);

            cadwyn.Draw(spriteBatch, 1.5f);
            bossSlime.Draw(spriteBatch, 1.5f);

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
