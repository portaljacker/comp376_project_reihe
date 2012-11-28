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
        GameState currentGameState = GameState.TitleScreen;

        Menu menu;
        SpriteFont menuFont;
        SpriteFont startFont;
        SpriteFont endFont1;
        SpriteFont endFont2;
        SpriteFont endFont3;
        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;
        GamePadState playerOneState;
        GamePadState lastPlayerOneState;

        List<Skills.Skill> chain;
        int skillCount = 0;
        bool showMenu = false;

        float battleTimer = 0.0f;
        int battleStep = 0;

        bool gameOver = false;
        bool win;

        Character cadwyn;
        Character bossSlime;
        Sprite logo;
        Sprite battle;
        Song battleTheme;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            logo = new Sprite(Content.Load<Texture2D>("logo"), new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), 1074, 900);
            battle = new Sprite(Content.Load<Texture2D>("BattleBackground"), new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), 1280, 720);
            battleTheme = Content.Load<Song>("Ghostpocalypse - 7 Master");
            cadwyn = new Character(Character.CharacterType.Cadwyn, Content.Load<Texture2D>("cadwynsheet"), new Vector2(graphics.PreferredBackBufferWidth - (385 / 2 * 1.5f + 25), graphics.PreferredBackBufferHeight - (327 / 2 * 1.5f + 25)), 384, 327);
            bossSlime = new Character(Character.CharacterType.BossSlime, Content.Load<Texture2D>("slimesheet"), new Vector2(25 + 384 / 2 * 1.5f, 25 + 327 / 2 * 1.5f), 384, 327);
            // Put the name of the font
            _spr_font = Content.Load<SpriteFont>("FPS");
            startFont = Content.Load<SpriteFont>("StartFont");
            menuFont = Content.Load<SpriteFont>("MenuFont");
            endFont1 = Content.Load<SpriteFont>("EndFont1");
            endFont2 = Content.Load<SpriteFont>("EndFont2");
            endFont3 = Content.Load<SpriteFont>("EndFont3");
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
            lastPlayerOneState = playerOneState;
            playerOneState = GamePad.GetState(PlayerIndex.One);
            
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

            if (showMenu && !gameOver)
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
                            menu = new Menu(Menu.MenuType.Battle);
                            skillCount = 0;
                            chain = new List<Skills.Skill>();
                            menu.InfoText = string.Empty;
                            if (cadwyn.HP == 0)
                            {
                                gameOver = true;
                                showMenu = true;
                                break;
                            }
                            else if (bossSlime.HP == 0)
                            {
                                gameOver = true;
                                showMenu = true;
                            }
                        }
                        break;
                }
            }

            // Allows the game to exit
            if (playerOneState.Buttons.Back == ButtonState.Pressed || KeyPressed(Keys.Escape))
                this.Exit();

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    if (KeyPressed(Keys.Space) || ButtonPressed(Buttons.A) || ButtonPressed(Buttons.Start))
                    {
                        currentGameState = GameState.Battle;
                        MediaPlayer.Play(battleTheme);
                        MediaPlayer.IsRepeating = true;
                    }
                    break;

                case GameState.Battle:
                    if (!showMenu)
                    {
                        if (KeyPressed(Keys.Down) || ButtonPressed(Buttons.DPadDown))
                            menu.Iterator += 1;
                        else if (KeyPressed(Keys.Up) || ButtonPressed(Buttons.DPadUp))
                            menu.Iterator -= 1;
                        else if (KeyPressed(Keys.Space) || ButtonPressed(Buttons.A))
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
                                            if (skillCount == 3)
                                                showMenu = true;
                                            skillCount++;
                                            break;
                                    }
                                }
                            }
                        }
                        else if (KeyPressed(Keys.Back) || ButtonPressed(Buttons.A))
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
                            Console.WriteLine(string.Format("Cadwyn HP after: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP));
                            showMenu = true;
                            if (bossSlime.HP == 0)
                            {
                                win = true;
                                cadwyn.HP = cadwyn.PreviousHealth;
                            }
                            else if (cadwyn.HP == 0)
                                win = false;
                        }
                    }

                    if (gameOver && battleTimer >= 1000)
                    {
                        MediaPlayer.Stop();
                        currentGameState = GameState.GameOver;
                    }

                    break;  //end Battle
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
            GraphicsDevice.Clear(Color.DarkRed);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // Only update total frames when drawing
            _total_frames++;

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    logo.Draw(spriteBatch, 1.2f);
                    DrawShadowedText(startFont, "Press Space to begin!", new Vector2(graphics.PreferredBackBufferWidth / 2 - startFont.MeasureString("Press Space to begin!").X / 2,
                        graphics.PreferredBackBufferHeight - 100), Color.LightGray);
                    break;

                case GameState.Battle:
                    battle.Draw(spriteBatch, 1);
                    if (!showMenu)
                    {
                        menu.DrawMenu(spriteBatch, graphics.PreferredBackBufferWidth, menuFont);
                    }
                    spriteBatch.DrawString(menuFont, menu.InfoText, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.White);

                    DrawShadowedText(menuFont, string.Format("Cadwyn HP: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP),
                        new Vector2(0, 576 - 2 * menuFont.MeasureString(string.Format("Cadwyn HP: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP)).Y), Color.White);
                    DrawShadowedText(menuFont, string.Format("Boss Slime HP: {0:0}/{1:0}", bossSlime.HP, bossSlime.MaxHP),
                        new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Boss Slime HP: {0:0}/{1:0}", bossSlime.HP, bossSlime.MaxHP)).Y), Color.White);

                    cadwyn.Draw(spriteBatch, 1.5f);
                    bossSlime.Draw(spriteBatch, 1.5f);
                    break;

                case GameState.GameOver:
                    if (win)
                        DrawShadowedText(endFont1, "Game Over. You win!", new Vector2(graphics.PreferredBackBufferWidth / 2 - endFont1.MeasureString("Game Over. You win!").X / 2,
                            graphics.PreferredBackBufferHeight / 2 - endFont1.MeasureString("Game Over. You win!").Y), Color.LightGray);
                    else
                        DrawShadowedText(endFont1, "Game Over. You lose!", new Vector2(graphics.PreferredBackBufferWidth / 2 - endFont1.MeasureString("Game Over. You lose!").X / 2,
                            graphics.PreferredBackBufferHeight / 2 - endFont1.MeasureString("Game Over. You lose!").Y), Color.LightGray);
                    DrawShadowedText(endFont2, "Thank you for playing!", new Vector2(graphics.PreferredBackBufferWidth / 2 - endFont2.MeasureString("Thank you for playing!").X / 2,
                        graphics.PreferredBackBufferHeight / 2 - endFont2.MeasureString("Thank you for playing!").Y + 50), Color.LightGray);
                    DrawShadowedText(endFont3, "Made by Team 11, Battle Theme: \"Ghostpocalypse - 7 Master\" by Kevin MacLeod",
                        new Vector2(graphics.PreferredBackBufferWidth - endFont3.MeasureString("Made by Team 11, Battle Theme: \"Ghostpocalypse - 7 Master\" by Kevin MacLeod").X,
                        graphics.PreferredBackBufferHeight - endFont3.MeasureString("Made by Team 11, Battle Theme: \"Ghostpocalypse - 7 Master\" by Kevin MacLeod").Y), Color.LightGray);
                    break;
            }

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

        #region Game Pad Region

        public bool ButtonReleased(Buttons button)
        {
            return playerOneState.IsButtonUp(button) &&
                lastPlayerOneState.IsButtonDown(button);
        }

        public bool ButtonPressed(Buttons button)
        {
            return playerOneState.IsButtonDown(button) &&
                lastPlayerOneState.IsButtonUp(button);
        }

        public bool ButtonDown(Buttons button)
        {
            return playerOneState.IsButtonDown(button);
        }

        #endregion

        private void DrawShadowedText(SpriteFont textFont, string textString, Vector2 textPosition, Color textColor)
        {
            Vector2 textShadow = new Vector2(textPosition.X + 1f, textPosition.Y + 1f);

            spriteBatch.DrawString(textFont, textString, textShadow, Color.DarkGray);
            spriteBatch.DrawString(textFont, textString, textShadow + new Vector2(0.5f, 0.5f), Color.Black);
            spriteBatch.DrawString(textFont, textString, textPosition, textColor);
        }
    }
}
