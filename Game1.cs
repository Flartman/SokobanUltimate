using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using SokobanUltimate.Controller;
using SokobanUltimate.Models;
using System.Collections.Generic;

namespace SokobanUltimate
{
    public class Game1 : Game
    {
        public enum Element
        {
            Empty,
            Ground,
            Wall,
            Player,
            Box,
            EndPoint,
        }
        private readonly int spriteSize = 64;
        private readonly Dictionary<Element, Texture2D> elementToTexture;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameModel _model;

        private Texture2D playerTexture;
        private Texture2D boxTexture;
        private Texture2D endPointTexture;
        private Texture2D groundTexture;
        private Texture2D wallTexture;

        private readonly int width = 7;
        private readonly int height = 7;
        private KeyboardState _oldKeyboardState;
        private IGameController controller;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.IsFullScreen = true;
            elementToTexture = new Dictionary<Element, Texture2D>();
            _oldKeyboardState = Keyboard.GetState();
            _graphics.PreferredBackBufferWidth = width * spriteSize;
            _graphics.PreferredBackBufferHeight = height * spriteSize;
            LevelLibrary.GenerateAll();
            StartGame();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
           
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here

            playerTexture = Content.Load<Texture2D>("Character4");
            wallTexture = Content.Load<Texture2D>("Wall_Gray");
            boxTexture = Content.Load<Texture2D>("Crate_Brown");
            endPointTexture = Content.Load<Texture2D>("EndPoint_Brown");
            groundTexture =  Content.Load<Texture2D>("Ground_Grass");
        }

        public void StartGame()
        {
            var groundMapInText = LevelLibrary.Level1Ground;
            var collisionMapInText = LevelLibrary.Level1Collision;
            var level = new Level("Name", "Author", groundMapInText, collisionMapInText, width, height);
            _model = new GameModel(level);
            controller = new KeyboardGameController(_model, this);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                StartGame();
            }

            controller.GetInput();

            base.Update(gameTime);
        }



        private Texture2D GetTexture(Element element)
        {
            return element switch
            {
                Element.Player => playerTexture,
                Element.Wall => wallTexture,
                Element.Box => boxTexture,
                Element.EndPoint => endPointTexture,
                Element.Ground => groundTexture,
                _ => null,
            };
        }

        private void DrawMap()
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack);
            for (var y = 0; y < _model.Map.Height; y++)
                for (var x = 0; x < _model.Map.Width; x++)
                {
                    var position = new Vector2(x * spriteSize, y * spriteSize);
                    var texture = GetTexture(_model.Map.GroundLayer[x, y]); 
                    if(texture != null)
                    {
                        if (texture != groundTexture)
                        {
                            _spriteBatch.Draw(
                            groundTexture,
                            position,
                            null,
                            Color.White,
                            0f,
                            new Vector2(0, 0),
                            Vector2.One,
                            SpriteEffects.None,
                            0f);
                        }

                        _spriteBatch.Draw(
                        texture,
                        position,
                        null,
                        Color.White,
                        0f,
                        new Vector2(0, 0),
                        Vector2.One,
                        SpriteEffects.None,
                        0.5f);
                    }
                    
                    texture = GetTexture(_model.Map.CollisionLayer[x, y]);
                    if (texture != null)
                    {
                        _spriteBatch.Draw(
                        texture,
                        position,
                        null,
                        Color.White,
                        0f,
                        new Vector2(0, 0),
                        Vector2.One,
                        SpriteEffects.None,
                        1f);
                    }
            }
            _spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            DrawMap();

            
            base.Draw(gameTime);
        }

    }
}
