using static Sokoban.Shared;


namespace Sokoban.Models.Levels

{
    public interface ILevelEditor
    {
        public Position SelectedElement { get; }

        public int Width { get; }

        public int Height { get; }

        public Level CurrentLevel { get; }

        public void ProcessCommand(InputCommand command);

        public void Edit(Level level);
    }

    public class LevelEditor : AppState, ILevelEditor
    {
        private int PlayersCount;

        private Level level;

        private readonly ILevelLoader levelLoader;

        public Position SelectedElement { get; private set; }

        public int Width => level.Width;

        public int Height => level.Height;

        public Level CurrentLevel => level;

        public override ViewMapData GetMapViewData()
        {
            var groundLayer = new GameElement[level.Width, level.Height];
            var collisionLayer = new GameElement[level.Width, level.Height];
            for (var y = 0; y < level.Height; y++)
            {
                for (var x = 0; x < level.Width; x++)
                {
                    groundLayer[x, y] = level.Map.GroundLayer[x, y];
                    collisionLayer[x, y] = level.Map.CollisionLayer[x, y];
                }
            }

            return new ViewMapData(groundLayer, collisionLayer, SelectedElement.X, SelectedElement.Y);
        }

        public LevelEditor(ILevelLoader loader)
        {
            levelLoader = loader;
            SelectedElement = new Position(0, 0);
        }

        public override void ProcessCommand(InputCommand command)
        {
            switch (command)
            {
                case InputCommand.Left:
                    Move(new Step(-1, 0));
                    break;
                case InputCommand.Right:
                    Move(new Step(1, 0));
                    break;
                case InputCommand.Up:
                    Move(new Step(0, -1));
                    break;
                case InputCommand.Down:
                    Move(new Step(0, 1));
                    break;
                case InputCommand.Back:
                    BackToMenuWithoutSaving();
                    break;
                case InputCommand.Enter:
                    SaveAndBackToMenu();
                    break;
                case InputCommand.PrimaryAction:
                    ChangeCollisionElementType();
                    break;
                case InputCommand.SecondaryAction:
                    ChangeGroundElementType();
                    break;
                default:
                    break;
            }
        }

        public void Edit(Level level)
        {
            Container.SetState(this);
            this.level = level;
            PlayersCount = CountElementOnMap(GameElement.Player);
        }

        public override ExternalAppState GetAppState()
        {
            return ExternalAppState.Editor;
        }

        private int CountElementOnMap(GameElement element)
        {
            var result = 0;
            foreach (var currentElement in level.Map.CollisionLayer)
            {
                if (currentElement == element)
                    result++;
            }

            foreach (var currentElement in level.Map.GroundLayer)
            {
                if (currentElement == element)
                    result++;
            }
            return result;
        }

        private void BackToMenuWithoutSaving()
        {
            Container.SetState(Container.InMenu);
        }

        private void SaveAndBackToMenu()
        {
            var boxCount = CountElementOnMap(GameElement.Box);
            var endPointCount = CountElementOnMap(GameElement.EndPoint);
            if (PlayersCount == 1 
                && boxCount > 0
                && boxCount == endPointCount)
            {
                levelLoader.SaveLevel(level);
                BackToMenuWithoutSaving();
            }
        }

        private void ChangeCollisionElementType()
        {
            var layer = level.Map.CollisionLayer;
            if (layer[SelectedElement.X, SelectedElement.Y] is GameElement.Player)
            {
                PlayersCount--;
            }

            if (PlayersCount > 0 && layer[SelectedElement.X, SelectedElement.Y] is GameElement.Box)
            {
                layer[SelectedElement.X, SelectedElement.Y] = GameElement.Empty;
            }
            else
            {
                layer[SelectedElement.X, SelectedElement.Y] =
                    layer[SelectedElement.X, SelectedElement.Y] switch
                    {
                        GameElement.Empty => GameElement.Wall,
                        GameElement.Wall => GameElement.Box,
                        GameElement.Box => GameElement.Player,
                        GameElement.Player => GameElement.Empty,
                    };
            }

            if (layer[SelectedElement.X, SelectedElement.Y] is GameElement.Player)
            {
                PlayersCount++;
                level.PlayerPosition.X = SelectedElement.X;
                level.PlayerPosition.Y = SelectedElement.Y;
            }
        }

        private void ChangeGroundElementType()
        {
            var layer = level.Map.GroundLayer;
            layer[SelectedElement.X, SelectedElement.Y] =
                layer[SelectedElement.X, SelectedElement.Y] switch
                {
                    GameElement.Ground => GameElement.EndPoint,
                    GameElement.EndPoint => GameElement.Ground,
                };
        }

        private void Move(Step step)
        {
            var newX = SelectedElement.X + step.dX;
            var newY = SelectedElement.Y + step.dY;

            (SelectedElement.X, SelectedElement.Y) =
                IsOutOfBounds(newX, newY) ?
                (SelectedElement.X, SelectedElement.Y) :
                (newX, newY);
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0
                || y < 0
                || x >= Width
                || y >= Height;
        }
    }
}
