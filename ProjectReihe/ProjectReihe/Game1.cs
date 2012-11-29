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
            Map,
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

        bool[] showPrevHP = { false, false };

        bool gameOver = false;
        bool win;

        Character cadwyn;
        Character bossSlime;
        Character enemy;
        static Random rand;
        Sprite logo;
        Sprite battle;
        Sprite bubble;
        Song battleTheme;

        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        float _frame_timer = 0.0f;
        int _fps = 0;


        Sprite cad;
        Texture2D cads;
        Texture2D tiles;

        float timer = 0f;
        float idle_timer = 0f;
        float interval = 100f;
        Map m;
        Vector2 trans;
        Vector2 cadPos;
        int scale;
        int stepCounter;

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
            bubble = new Sprite(Content.Load<Texture2D>("Bubble"), new Vector2(graphics.PreferredBackBufferWidth - 175, 200), 350, 350);
            battleTheme = Content.Load<Song>("Ghostpocalypse - 7 Master");
            bossSlime = new Character(Character.CharacterType.BossSlime, Content.Load<Texture2D>("slimesheet3"), new Vector2(25 + 384 / 2 * 1.5f, 25 + 327 / 2 * 1.5f), 384, 327);
            // Put the name of the font
            _spr_font = Content.Load<SpriteFont>("FPS");
            startFont = Content.Load<SpriteFont>("StartFont");
            menuFont = Content.Load<SpriteFont>("MenuFont");
            endFont1 = Content.Load<SpriteFont>("EndFont1");
            endFont2 = Content.Load<SpriteFont>("EndFont2");
            endFont3 = Content.Load<SpriteFont>("EndFont3");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();


            cadPos = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            cads = Content.Load<Texture2D>("cadsmallsheet");
            cad = new Sprite(cads, cadPos, 25, 35);
            tiles = Content.Load<Texture2D>("tileset");
            m = new Map(tiles, scale);
            trans = new Vector2(0, 0); //No translation at the start.
            scale = 1;

            stepCounter = 0;

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
                            if (chain[0] == Skills.Skill.Attack)
                            {
                                if (cadwyn.HP > cadwyn.MaxHP / 4)
                                {
                                    cadwyn.CurrentRow = 0;
                                }

                                else
                                {
                                    cadwyn.CurrentRow = 2;
                                }
                                _frame_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (_frame_timer > 50)
                                {
                                    cadwyn.CurrentFrame++;
                                    _frame_timer = 0;
                                }
                                if (cadwyn.CurrentFrame == 5)
                                {
                                    cadwyn.CurrentFrame = 0;
                                    battleTimer = 0;
                                    battleStep++;
                                }
                            }

                            else
                            {
                                if (cadwyn.HP > cadwyn.MaxHP / 4)
                                {
                                    cadwyn.CurrentRow = 1;
                                }

                                else
                                {
                                    cadwyn.CurrentRow = 3;
                                }
                                _frame_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (_frame_timer > 50)
                                {
                                    cadwyn.CurrentFrame++;
                                    _frame_timer = 0;
                                }
                                if (cadwyn.CurrentFrame == 3)
                                {
                                    cadwyn.CurrentFrame = 0;
                                    battleTimer = 0;
                                    battleStep++;
                                }
                            }

                        }
                        break;
                    case 1:
                        if (battleTimer >= 500.0f)
                        {
                            if (chain[1] == Skills.Skill.Attack)
                            {
                                if (cadwyn.HP > cadwyn.MaxHP / 4)
                                {
                                    cadwyn.CurrentRow = 0;
                                }

                                else
                                {
                                    cadwyn.CurrentRow = 2;
                                }
                                _frame_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (_frame_timer > 50)
                                {
                                    cadwyn.CurrentFrame++;
                                    _frame_timer = 0;
                                }
                                if (cadwyn.CurrentFrame == 5)
                                {
                                    cadwyn.CurrentFrame = 0;
                                    battleTimer = 0;
                                    battleStep++;
                                }
                            }

                            else
                            {
                                if (cadwyn.HP > cadwyn.MaxHP / 4)
                                {
                                    cadwyn.CurrentRow = 1;
                                }

                                else
                                {
                                    cadwyn.CurrentRow = 3;
                                }
                                _frame_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (_frame_timer > 50)
                                {
                                    cadwyn.CurrentFrame++;
                                    _frame_timer = 0;
                                }
                                if (cadwyn.CurrentFrame == 3)
                                {
                                    cadwyn.CurrentFrame = 0;
                                    battleTimer = 0;
                                    battleStep++;
                                }
                            }

                        }
                        break;
                    case 2:
                        if (battleTimer >= 500.0f)
                        {
                            if (chain[2] == Skills.Skill.Attack)
                            {
                                if (cadwyn.HP > cadwyn.MaxHP / 4)
                                {
                                    cadwyn.CurrentRow = 0;
                                }

                                else
                                {
                                    cadwyn.CurrentRow = 2;
                                }
                                _frame_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (_frame_timer > 50)
                                {
                                    cadwyn.CurrentFrame++;
                                    _frame_timer = 0;
                                }
                                if (cadwyn.CurrentFrame == 5)
                                {
                                    cadwyn.CurrentFrame = 0;
                                    battleTimer = 0;
                                    battleStep++;
                                }
                            }

                            else
                            {
                                if (cadwyn.HP > cadwyn.MaxHP / 4)
                                {
                                    cadwyn.CurrentRow = 1;
                                }

                                else
                                {
                                    cadwyn.CurrentRow = 3;
                                }
                                _frame_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (_frame_timer > 50)
                                {
                                    cadwyn.CurrentFrame++;
                                    _frame_timer = 0;
                                }
                                if (cadwyn.CurrentFrame == 3)
                                {
                                    cadwyn.CurrentFrame = 0;

                                    battleTimer = 0;
                                    battleStep++;
                                }
                            }

                        }
                        break;
                    case 3:
                        if (battleTimer >= 750.0f)
                        {
                            cadwyn.CurrentFrame = 0;
                            enemy.Position -= new Vector2(25, 0);
                            showPrevHP[0] = false;
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 4:
                        if (battleTimer >= 750.0f)
                        {
                            enemy.Position += new Vector2(25, 0);
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 5:
                        if (battleTimer >= 1000.0f)
                        {

                            _frame_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                            if (_frame_timer > 100)
                            {
                                enemy.CurrentFrame++;
                                _frame_timer = 0;
                                enemy.Position += new Vector2(25, 0);
                            }
                            if (enemy.CurrentFrame == 3)
                            {
                                enemy.CurrentFrame = 0;
                                enemy.Position += new Vector2(-50, 0);
                                cadwyn.Position += new Vector2(25, 0);
                                showPrevHP[1] = false;
                                battleTimer = 0;
                                battleStep++;
                            }
                        }
                        break;
                    case 6:
                        if (battleTimer >= 500.0f)
                        {
                            enemy.CurrentFrame = 0;
                            enemy.Position -= new Vector2(25, 0);
                            cadwyn.Position -= new Vector2(25, 0);
                            battleTimer = 0;
                            battleStep++;
                        }
                        break;
                    case 7:
                        if (battleTimer >= 500.0f)
                        {
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
                            else if (enemy.HP == 0)
                            {
                                gameOver = true;
                                showMenu = true;
                            }
                        }
                        if (win)
                            if (enemy.CharType == Character.CharacterType.BossSlime)
                                currentGameState = GameState.GameOver;
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
                        currentGameState = GameState.Map;
                        MediaPlayer.Play(battleTheme);
                        MediaPlayer.IsRepeating = true;
                    }
                    break;

                case GameState.Map:
                    bool is_idle = true;

                    KeyboardState ks = Keyboard.GetState();
                    Keys[] keys = ks.GetPressedKeys();

                    foreach (Keys key in keys)
                    {
                        switch (key)
                        {
                            case Keys.Up:
                                cad.CurrentRow = 5;
                                is_idle = false;
                                trans += new Vector2(0, 4);

                                break;

                            case Keys.Down:
                                cad.CurrentRow = 4;
                                is_idle = false;
                                trans += new Vector2(0, -4);

                                break;

                            case Keys.Left:
                                cad.CurrentRow = 7;
                                is_idle = false;
                                trans += new Vector2(4, 0);

                                break;

                            case Keys.Right:
                                cad.CurrentRow = 6;
                                is_idle = false;
                                trans += new Vector2(-4, 0);

                                break;
                        }
                    }

                    if (ButtonDown(Buttons.DPadDown))
                    {
                        cad.CurrentRow = 4;
                        is_idle = false;
                        trans += new Vector2(0, -4);
                    }

                    else if (ButtonDown(Buttons.DPadUp))
                    {
                        cad.CurrentRow = 5;
                        is_idle = false;
                        trans += new Vector2(0, 4);
                    }

                    else if (ButtonDown(Buttons.DPadLeft))
                    {
                        cad.CurrentRow = 7;
                        is_idle = false;
                        trans += new Vector2(4, 0);
                    }

                    else if (ButtonDown(Buttons.DPadRight))
                    {
                        cad.CurrentRow = 6;
                        is_idle = false;
                        trans += new Vector2(-4, 0);
                    }

                    if (-400 < trans.X && trans.X < -350 && 1450 > trans.Y && trans.Y > 1350)
                    {
                        currentGameState = GameState.Battle;
                        enemy = bossSlime;
                        win = false;
                        showMenu = false;
                        gameOver = false;
                        cadwyn = new Character(Character.CharacterType.Cadwyn, Content.Load<Texture2D>("cadwynsheet3"), new Vector2(graphics.PreferredBackBufferWidth - (385 / 2 * 1.5f + 25), graphics.PreferredBackBufferHeight - (327 / 2 * 1.5f + 25)), 384, 327);            
                    }

                    if (is_idle)
                    {

                        if (cad.CurrentRow == 4)
                            cad.CurrentRow = 0;

                        if (cad.CurrentRow == 5)
                            cad.CurrentRow = 1;

                        if (cad.CurrentRow == 6)
                            cad.CurrentRow = 2;

                        if (cad.CurrentRow == 7)
                            cad.CurrentRow = 3;

                        cad.CurrentFrame = 0;

                        idle_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        if (idle_timer > 3000)  //Wait for a specific time
                        {

                            cad.CurrentFrame++;

                        }

                        if (idle_timer > 3100)  //Wait for a specific time
                        {

                            cad.CurrentFrame++;

                        }

                        if (idle_timer > 3200)  //Wait for a specific time
                        {

                            cad.CurrentFrame++;

                        }

                        if (idle_timer > 3300)  //Wait for a specific time
                        {

                            cad.CurrentFrame = 0;
                            idle_timer = 0f;

                        }
                    }


                    else   //Character is moving 
                    {
                        idle_timer = 0f;
                        timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        if (timer > interval)
                        {
                            //Show the next frame
                            cad.CurrentFrame++;
                            //Reset the timer
                            timer = 0f;
                        }

                        if (cad.CurrentFrame == 3)  //Depending on the sprites per row of the spritesheet 
                        {
                            cad.CurrentFrame = 0;
                        }


                        stepCounter++;
                    }

                    cad.SourceRect = new Rectangle(cad.Width * cad.CurrentFrame, cad.Height * cad.CurrentRow, cad.Width, cad.Height);

                    if (stepCounter > 200)
                    {
                        stepCounter = 0;
                        currentGameState = GameState.Battle;
                        LoadBattle();
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
                                            skillCount++;
                                            if (skillCount == 3)
                                            {
                                                showMenu = true;
                                                menu.InfoText += "Attack";
                                            }
                                            else
                                                menu.InfoText += "Attack-->";
                                            break;
                                        case 1:
                                            chain.Add(Skills.Skill.Fire);
                                            skillCount++;
                                            if (skillCount == 3)
                                            {
                                                showMenu = true;
                                                menu.InfoText += "Fire";
                                            }
                                            else
                                                menu.InfoText += "Fire-->";
                                            break;
                                        case 2:
                                            chain.Add(Skills.Skill.Ice);
                                            skillCount++;
                                            if (skillCount == 3)
                                            {
                                                showMenu = true;
                                                menu.InfoText += "Ice";
                                            }
                                            else
                                                menu.InfoText += "Ice-->";
                                            break;
                                        case 3:
                                            chain.Add(Skills.Skill.Bolt);
                                            skillCount++;
                                            if (skillCount == 3)
                                            {
                                                showMenu = true;
                                                menu.InfoText += "Bolt";
                                            }
                                            else
                                                menu.InfoText += "Bolt-->";
                                            break;
                                    }
                                }
                            }
                        }
                        else if (KeyPressed(Keys.Back) || ButtonPressed(Buttons.B))
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
                            Console.WriteLine(string.Format("Boss Slime HP before: {0:0}/{1:0}", enemy.HP, enemy.MaxHP));
                            cadwyn.attack(enemy, chain);
                            Console.WriteLine(string.Format("Boss Slime HP after: {0:0}/{1:0}", enemy.HP, enemy.MaxHP));
                            Console.WriteLine(string.Format("Cadwyn HP before: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP));
                            enemy.attack(cadwyn, slimeChain);
                            Console.WriteLine(string.Format("Cadwyn HP after: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP));
                            showMenu = true;
                            showPrevHP[0] = true;
                            showPrevHP[1] = true;
                            if (enemy.HP == 0)
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
                        currentGameState = GameState.Map;
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
                    DrawShadowedText(startFont, "Start Game", new Vector2(graphics.PreferredBackBufferWidth / 2 - startFont.MeasureString("Start Game").X / 2,
                        graphics.PreferredBackBufferHeight - 100), Color.LightGray);
                    break;

                case GameState.Map:
                    m.Draw(spriteBatch, scale, trans, cadPos);
                    cad.Draw(spriteBatch, scale);
                    break;

                case GameState.Battle:
                    battle.Draw(spriteBatch, 1);
                    if (!showMenu)
                    {
                        bubble.Draw(spriteBatch, 1f);
                        menu.DrawMenu(spriteBatch, graphics.PreferredBackBufferWidth, menuFont);
                    }
                    spriteBatch.DrawString(menuFont, menu.InfoText, new Vector2(graphics.PreferredBackBufferWidth / 2 - 50, graphics.PreferredBackBufferHeight / 2 + 15), Color.White);

                    if (showPrevHP[1] == true)
                    {
                        DrawShadowedText(menuFont, string.Format("Cadwyn HP: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP),
                            new Vector2(0, 576 - 2 * menuFont.MeasureString(string.Format("Cadwyn HP: {0:0}/{1:0}", cadwyn.PreviousHealth, cadwyn.MaxHP)).Y), Color.White);
                    }
                    else
                    {
                        DrawShadowedText(menuFont, string.Format("Cadwyn HP: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP),
                            new Vector2(0, 576 - 2 * menuFont.MeasureString(string.Format("Cadwyn HP: {0:0}/{1:0}", cadwyn.HP, cadwyn.MaxHP)).Y), Color.White);
                    }

                    switch (enemy.CharType)
                    {
                        case Character.CharacterType.BossSlime:
                            if (showPrevHP[0] == true)
                            {
                                DrawShadowedText(menuFont, string.Format("Boss Slime HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Boss Slime HP: {0:0}/{1:0}", enemy.PreviousHealth, enemy.MaxHP)).Y), Color.White);
                            }
                            else
                            {
                                DrawShadowedText(menuFont, string.Format("Boss Slime HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Boss Slime HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP)).Y), Color.White);
                            }
                            break;

                        case Character.CharacterType.Slime:
                            if (showPrevHP[0] == true)
                            {
                                DrawShadowedText(menuFont, string.Format("Slime HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Slime HP: {0:0}/{1:0}", enemy.PreviousHealth, enemy.MaxHP)).Y), Color.White);
                            }
                            else
                            {
                                DrawShadowedText(menuFont, string.Format("Slime HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Slime HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP)).Y), Color.White);
                            }
                            break;

                        case Character.CharacterType.LabRat:
                            if (showPrevHP[0] == true)
                            {
                                DrawShadowedText(menuFont, string.Format("Lab Rat HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Lab Rat HP: {0:0}/{1:0}", enemy.PreviousHealth, enemy.MaxHP)).Y), Color.White);
                            }
                            else
                            {
                                DrawShadowedText(menuFont, string.Format("Lab Rat HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Lab Rat HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP)).Y), Color.White);
                            }
                            break;

                        case Character.CharacterType.ZombieJanitor:
                            if (showPrevHP[0] == true)
                            {
                                DrawShadowedText(menuFont, string.Format("Zombie Janitor HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Zombie Janitor HP: {0:0}/{1:0}", enemy.PreviousHealth, enemy.MaxHP)).Y), Color.White);
                            }
                            else
                            {
                                DrawShadowedText(menuFont, string.Format("Zombie Janitor HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP),
                                    new Vector2(0, 576 - 1 * menuFont.MeasureString(string.Format("Zombie Janitor HP: {0:0}/{1:0}", enemy.HP, enemy.MaxHP)).Y), Color.White);
                            }
                            break;
                    }

                    cadwyn.Draw(spriteBatch, 1.5f);
                    enemy.Draw(spriteBatch, 1.5f);
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

        private void LoadBattle()
        {
            win = false;
            showMenu = false;
            gameOver = false;
            switch (rand.Next(3))
            {
                case 0:
                    enemy = new Character(Character.CharacterType.Slime, Content.Load<Texture2D>("minislime2"), new Vector2(25 + 384 / 2 * 1.5f, 35 + 327 / 2 * 1.5f), 384, 327);
                    enemy.HP = enemy.MaxHP;
                    break;

                case 1:
                    enemy = new Character(Character.CharacterType.ZombieJanitor, Content.Load<Texture2D>("zombiesheet"), new Vector2(25 + 384 / 2 * 1.5f, 25 + 327 / 2 * 1.5f), 384, 327);
                    enemy.HP = enemy.MaxHP;
                    break;

                case 2:
                    enemy = new Character(Character.CharacterType.LabRat, Content.Load<Texture2D>("ratSheet"), new Vector2(25 + 384 / 2 * 1.5f, 35 + 327 / 2 * 1.5f), 384, 327);
                    enemy.HP = enemy.MaxHP;
                    break;
            }
            cadwyn = new Character(Character.CharacterType.Cadwyn, Content.Load<Texture2D>("cadwynsheet3"), new Vector2(graphics.PreferredBackBufferWidth - (385 / 2 * 1.5f + 25), graphics.PreferredBackBufferHeight - (327 / 2 * 1.5f + 25)), 384, 327);            
        }

        private void DrawShadowedText(SpriteFont textFont, string textString, Vector2 textPosition, Color textColor)
        {
            Vector2 textShadow = new Vector2(textPosition.X + 1f, textPosition.Y + 1f);

            spriteBatch.DrawString(textFont, textString, textShadow, Color.DarkGray);
            spriteBatch.DrawString(textFont, textString, textShadow + new Vector2(0.5f, 0.5f), Color.Black);
            spriteBatch.DrawString(textFont, textString, textPosition, textColor);
        }
    }
}
