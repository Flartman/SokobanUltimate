using C3.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sokoban.Models;
using System;
using static Sokoban.Shared;

namespace Sokoban.View
{
    public class MVPAppView : IAppView
    {
        private const int windowHeight = 812;
        private const int windowWidth = 1440;
        private readonly App _app;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly int _spriteSize = 64;

        private Texture2D backGroundTexture;
        private Texture2D playerTexture;
        private Texture2D boxTexture;
        private Texture2D endPointTexture;
        private Texture2D groundTexture;
        private Texture2D wallTexture;
        private Texture2D deliveredBoxTexture;
        private Texture2D frameTexture;
        private Texture2D playButton;
        private Texture2D exitButton;
        private Texture2D settingsButton;
        private Texture2D statisticsButton;
        private Texture2D profilesButton;
        private Texture2D scoreTableButton;
        private Texture2D soundOnButton;
        private Texture2D soundOffButton;
        private Texture2D homeButton;

        private SpriteFont font;

        public MVPAppView(App app)
        {
            _app = app;
            _graphics = new GraphicsDeviceManager(app);
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.PreferredBackBufferHeight = windowHeight;
            app.Content.RootDirectory = "Content";
            app.IsMouseVisible = false;
        }

        public void Initialize()
        {
            _spriteBatch = new SpriteBatch(_app.GraphicsDevice);
            LoadContent();
        }

        

        public void Draw()
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack);
            _spriteBatch.Draw(backGroundTexture, new Vector2(0, 0), null, Color.White);
            _app.GraphicsDevice.Clear(Color.White);
            DrawMenuItem(new Vector2(50, windowHeight - 100), _app.CurrentProfileName);

            var state = _app.GetState();
            switch (state)
            {
                case Shared.ExternalAppState.Menu:
                    DrawGridMenu();
                    break;
                case Shared.ExternalAppState.Packages:
                    DrawVerticalMenu();
                    break;
                case Shared.ExternalAppState.Profiles:
                    DrawVerticalMenu();
                    break;
                case Shared.ExternalAppState.Game:
                    DrawMap();
                    break;
                case Shared.ExternalAppState.Editor:
                    DrawEditor();
                    break;
                case Shared.ExternalAppState.GameResult:
                    DrawVerticalMenu();
                    break;
                case Shared.ExternalAppState.Statistics:
                    DrawVerticalMenu();
                    break;
            }
            _spriteBatch.End();
        }
        private void LoadContent()
        {
            backGroundTexture = _app.Content.Load<Texture2D>("BackGround");
            playerTexture = _app.Content.Load<Texture2D>("Character");
            wallTexture = _app.Content.Load<Texture2D>("Wall_Gray");
            boxTexture = _app.Content.Load<Texture2D>("CrateDark_Brown");
            endPointTexture = _app.Content.Load<Texture2D>("EndPoint_Brown");
            groundTexture = _app.Content.Load<Texture2D>("Ground_Grass");
            deliveredBoxTexture = _app.Content.Load<Texture2D>("Crate_Brown");
            playButton = _app.Content.Load<Texture2D>("Play@2x");
            exitButton = _app.Content.Load<Texture2D>("Exit@2x");
            settingsButton = _app.Content.Load<Texture2D>("Gear@2x");
            statisticsButton = _app.Content.Load<Texture2D>("Player@2x");
            profilesButton = _app.Content.Load<Texture2D>("Multiplayer@2x");
            scoreTableButton = _app.Content.Load<Texture2D>("Podium@2x");
            soundOnButton = _app.Content.Load<Texture2D>("Sound-Three@2x");
            homeButton = _app.Content.Load<Texture2D>("Home@2x");
            frameTexture = _app.Content.Load<Texture2D>("Frame");

            font = _app.Content.Load<SpriteFont>("Font");
        }

        private void DrawVerticalMenu()
        {
            DrawGridMenu(1);
        }

        private void DrawGridMenu(int numberOfColumns = 10)
        {
            var viewData = _app.GetListViewData();
            var selectedItemNumber = viewData.SelectedItemNumber;
            var count = viewData.Items.Count;

            var itemMaxSize = GetItemMaxSize(viewData);
            var rangeBetweenItems = itemMaxSize * 0.5f;// 
            var centerOfWindow = new Vector2(windowWidth / 2, windowHeight / 2);
            var realColumns = (count / numberOfColumns > 0) ? numberOfColumns : count % numberOfColumns;
            var rows = (int)Math.Ceiling((double)count / numberOfColumns);

            var initialCorrection = new Vector2
            {
                X = realColumns * itemMaxSize.X / 2 + (realColumns - 1) / 2 * rangeBetweenItems.X,
                Y = rows * itemMaxSize.Y / 2 + (rows - 1) / 2 * rangeBetweenItems.Y,
            };
            var position = centerOfWindow - initialCorrection;

            var stepCorrection = itemMaxSize + rangeBetweenItems;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < numberOfColumns; j++)
                {
                    var index = i * numberOfColumns + j;
                    if (index >= count) break;
                    var scale = (selectedItemNumber == index) ? 1.2f : 1.0f;
                    var size = GetItemsSize(viewData.Items[index]);
                    var scaleCorrection = size * (scale - 1.0f) / 2;
                    position -= scaleCorrection;
                    DrawMenuItem(position, viewData.Items[index], scale);
                    position += scaleCorrection;
                    position.X += stepCorrection.X;
                }
                position.Y += stepCorrection.Y;
                position.X = centerOfWindow.X - initialCorrection.X;
            }
        }

        private Vector2 GetItemsSize(string itemName)
        {
            var texture = GetMenuTexture(itemName);
            return (texture == null) ?
                font.MeasureString(itemName) * 0.75f :
                new Vector2(texture.Width, texture.Height);
        }

        private Texture2D GetMenuTexture(string name)
        {
            return name switch
            {
                "Play" => playButton,
                "Exit" => exitButton,
                "Settings" => settingsButton,
                "Sound" => soundOnButton,
                "Home" => homeButton,
                "Profiles" => profilesButton,
                "Statistics" => statisticsButton,
                "scoreTable" => scoreTableButton,
                _ => null,
            };
        }

        private Vector2 GetItemMaxSize(ViewListData viewListData)
        {
            var maxSize = Vector2.Zero;
            foreach (var item in viewListData.Items)
            {
                var size = GetItemsSize(item);
                maxSize.X = (size.X > maxSize.X) ? size.X : maxSize.X;
                maxSize.Y = (size.Y > maxSize.Y) ? size.Y : maxSize.Y;
            }
            return maxSize;
        }

        private void DrawMenuItem(Vector2 position, string itemName, float scale = 1.0f)
        {
            var texture = GetMenuTexture(itemName);
            if (texture == null)
            {
                _spriteBatch.DrawString(
                        font,
                        itemName,
                        position,
                        Color.Black,
                        0f,
                        new Vector2(0, 0),
                        scale,
                        SpriteEffects.None,
                        0.9f);
            }
            else
            {
                _spriteBatch.Draw(
                      texture,
                      position,
                      null,
                      Color.Black,
                      0f,
                      new Vector2(0, 0),
                      scale,
                      SpriteEffects.None,
                      0.9f);
            }
        }

        private void DrawEditor()
        {
            var viewData = _app.GetMapViewData();
            var startX = windowWidth / 2 - _spriteSize * viewData.ColumnsNumber / 2;
            var startY = windowHeight / 2 - _spriteSize * viewData.RowsNumber / 2;
            var frameX = startX + viewData.SelectedItemColumn * _spriteSize;
            var frameY = startY + viewData.SelectedItemRow * _spriteSize;
            var framePosition = new Vector2(frameX, frameY);
            _spriteBatch.Draw(
                            frameTexture,
                            framePosition,
                            null,
                            Color.White,
                            0f,
                            new Vector2(0, 0),
                            Vector2.One,
                            SpriteEffects.None,
                            1f);
            var topBorderY = startY;
            var leftBorderX = startX;
            var bottomBorderY = startY + viewData.RowsNumber * _spriteSize;
            var rightBorderX = startX + viewData.ColumnsNumber * _spriteSize;
            for (var y = 0; y <= viewData.RowsNumber; y++)
            {
                var currentLineY = topBorderY + y * _spriteSize;
                _spriteBatch.DrawLine(leftBorderX, currentLineY, rightBorderX, currentLineY, Color.Black);
            }
            for (var x = 0; x <= viewData.ColumnsNumber; x++)
            {
                var currentLineX = leftBorderX + x * _spriteSize;
                _spriteBatch.DrawLine(currentLineX, topBorderY, currentLineX, bottomBorderY, Color.Black);
            }

            DrawMap(viewData);
        }

        private void DrawMap()
        {
            DrawMap(_app.GetMapViewData());
        }

        private void DrawMap(ViewMapData viewData)
        {
            var groundLayer = viewData.GroundLayer;
            var collisionLayer = viewData.CollisionLayer;
            var startX = windowWidth / 2 - _spriteSize * viewData.ColumnsNumber / 2;
            var startY = windowHeight / 2 - _spriteSize * viewData.RowsNumber / 2;
            var startPosition = new Vector2(startX, startY);
            for (var y = 0; y < viewData.RowsNumber; y++)
                for (var x = 0; x < viewData.ColumnsNumber; x++)
                {
                    var position = startPosition + new Vector2(x * _spriteSize, y * _spriteSize);
                    var texture = GetTileTexture(groundLayer[x, y]);
                    if (texture != null)
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

                    texture = GetTileTexture(collisionLayer[x, y]);
                    if (collisionLayer[x, y] is GameElement.Box
                        && groundLayer[x, y] is GameElement.EndPoint)
                    {
                        texture = deliveredBoxTexture;
                    }

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
                        0.8f);
                    }
                }
        }

        private Texture2D GetTileTexture(GameElement element)
        {
            return element switch
            {
                GameElement.Player => playerTexture,
                GameElement.Wall => wallTexture,
                GameElement.Box => boxTexture,
                GameElement.EndPoint => endPointTexture,
                GameElement.Ground => groundTexture,
                _ => null,
            };
        }
    }
}
