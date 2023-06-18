using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static string dungeonInfoText = "The name of the game is simple. You must find and collect the right key among the keys before you. #However, be careful with your choice, as it may result in a negative outcome";

        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        Texture2D[] playerSprites;
        //public static Player player;
        Cutscene1 cutscene1;
        Texture2D heartText;
        Texture2D gameFinishedText;
        Rectangle gameFinishedRect;
        List<SpriteFont> fonts;
        Rectangle textRect;

        Dungeon dungeon;

        public Texture2D blankButtonText;

        public DeathCutScene deathCutScene;

        public static int screenW;
        public static int screenH;

        public Texture2D textTexture;

        public static int gameTimer = 60 * 5;
        public static int timer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            graphics.ApplyChanges();
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
            //player = new Player(3);
            playerSprites = new Texture2D[4];
            gameFinishedRect = new Rectangle(0, 0, 800, 800);
            fonts = new List<SpriteFont>();
            textRect = new Rectangle(0, 600, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 600);

            fonts.Add(this.Content.Load<SpriteFont>("Fonts/textFont"));

            cutscene1 = new Cutscene1(spriteBatch);
            IsMouseVisible = true;

            screenW = graphics.PreferredBackBufferWidth;
            screenH = graphics.PreferredBackBufferHeight;

            timer = 0;

            //FloorManager.initializeVars();


            base.Initialize();

        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            screenW = GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenH = GraphicsDevice.PresentationParameters.BackBufferHeight;
            graphics.ApplyChanges();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //Directory.SetCurrentDirectory(GlobalVars.Content.RootDirectory);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            HUD.setSpriteBatch(spriteBatch);

            blankButtonText = this.Content.Load<Texture2D>("Sprites/Buttons/button");
            GlobalVars.loadVars(spriteBatch, blankButtonText, fonts[0], Content);
            if (GlobalVars.gState == GlobalVars.GAMESTATE.CUTSCENE1 && GlobalVars.retTimer != 0)
            {
                GlobalVars.gState = GlobalVars.GAMESTATE.DUNGEON;
            }
            textTexture = this.Content.Load<Texture2D>("Sprites/TextSprites/textBox");
            HUD.initializeTextVars(fonts[0], textRect, textTexture);


            heartText = this.Content.Load<Texture2D>("DungeonSprites/heart");

            gameFinishedText = this.Content.Load<Texture2D>("DungeonSprites/gameComplete");

            cutscene1.loadContent();

            FloorManager.loadContent();

            deathCutScene = new DeathCutScene(Content);
            //deathCutScene.loadContent(Content);
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
            // Allows the game to exit
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            KeyboardState kbr = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kbr.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            
            HUD.update();
            Sprite.checkRemoveSprites();

            FloorManager.update(gameTime);

            if (GlobalVars.player.percentage <= 0)
            {
                GlobalVars.gState = GlobalVars.GAMESTATE.DEATHCUTSCENE;
            }
            if (GlobalVars.gState > GlobalVars.GAMESTATE.CUTSCENE1 && GlobalVars.gState < GlobalVars.GAMESTATE.DEATHCUTSCENE)
            {
                if (!HUD.writingText)
                {
                    timer++;
                    if (timer % 60 == 0)
                        gameTimer--;

                    if (gameTimer == 0) GlobalVars.gState = GlobalVars.GAMESTATE.DEATHCUTSCENE;
                }
            }
            if (GlobalVars.gState == GlobalVars.GAMESTATE.CUTSCENE1)
            {
                cutscene1.update(gameTime);
            }
            else
            {
                cutscene1.disposeMusic();
            }
            if (GlobalVars.gState == GlobalVars.GAMESTATE.DEATHCUTSCENE || GlobalVars.gState == GlobalVars.GAMESTATE.CUTSCENE1)
            {
                Window.AllowUserResizing = false;
                graphics.PreferredBackBufferWidth = 800;
                graphics.PreferredBackBufferHeight = 800;
                graphics.ApplyChanges();
            }
            else
            {
                this.Window.AllowUserResizing = true;
            }

            if (GlobalVars.gState == GlobalVars.GAMESTATE.WINGAMESCENE)
            {
                GlobalVars.player.update(new List<Tile2>(), gameTime);
                GlobalVars.camera.follow(GlobalVars.player.playerRect);

            }


            if (GlobalVars.gState == GlobalVars.GAMESTATE.DEATHCUTSCENE)
            {
                KeyboardState kb = Keyboard.GetState();

                if (kb.IsKeyDown(InputMaps.keyboardInputs[InputMaps.INTERACT]))
                {
                    Resetter.reset();
                    base.Initialize();
                    base.LoadContent();

                  
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            if (GlobalVars.gState > 0 && GlobalVars.gState < GlobalVars.GAMESTATE.DEATHCUTSCENE || GlobalVars.gState == GlobalVars.GAMESTATE.WINGAMESCENE)
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, transformMatrix: GlobalVars.camera.transform);
            else
                spriteBatch.Begin();

            if (GlobalVars.gState == GlobalVars.GAMESTATE.CUTSCENE1)
                cutscene1.draw(spriteBatch, GraphicsDevice);

            
            if (GlobalVars.gState > GlobalVars.GAMESTATE.CUTSCENE1 && GlobalVars.gState < GlobalVars.GAMESTATE.DEATHCUTSCENE)
            {
                FloorManager.draw();
                for (int i = 0; i < GlobalVars.player.particles.Count(); i++)
                {
                    GlobalVars.player.particles[i].draw();
                }
                drawTimer();
            }
            if (GlobalVars.gState == GlobalVars.GAMESTATE.WINGAMESCENE)
            {
                GlobalVars.player.draw();
                string str = "PRESS E TO RESTART FROM BEGINNING";
                Vector2 textPos = new Vector2(400 - GlobalVars.spriteFont.MeasureString(str).X / 2, 30);
                GlobalVars.spriteBatch.DrawString(GlobalVars.spriteFont, str, textPos, Color.White);

            }
            
            
            if (GlobalVars.gState == GlobalVars.GAMESTATE.DEATHCUTSCENE)
            {
                deathCutScene.play(GraphicsDevice);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void drawTimer()
        {
            double startingNum = Math.Floor(gameTimer / 60.0);
            double endingNum = ((gameTimer / 100) - startingNum) * 100;
            string clock;
            if (gameTimer % 60 > 9)
            {
                clock = (gameTimer / 60).ToString() + ":" + (gameTimer % 60).ToString();
            }
            //displays the current time remaining
            else
            {
                clock = (gameTimer / 60).ToString() + ":0" + (gameTimer % 60).ToString();
            }

            Rectangle clockRect = new Rectangle(GlobalVars.camera.transformX, GlobalVars.camera.transformY, 95, 50);
            Vector2 clockTextPos = new Vector2(GlobalVars.camera.transformX + 25, GlobalVars.camera.transformY + 14);

            GlobalVars.spriteBatch.Draw(HUD.textBackgroundText, clockRect, Color.White);
            GlobalVars.spriteBatch.DrawString(GlobalVars.spriteFont, clock, clockTextPos, Color.White);
        }
        public static void reset()
        {
            dungeonInfoText = "The name of the game is simple. You must find and collect the right key among the keys before you. #However, be careful with your choice, as it may result in a negative outcome";
            gameTimer = 60 * 5;
            timer = 0;
        }
    }

}
