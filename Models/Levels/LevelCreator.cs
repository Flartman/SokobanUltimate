using static Sokoban.Shared;

namespace Sokoban.Models.Levels
{
    public interface ILevelCreator
    {
        public void CreateLevel(string packageName, string author);
    }

    public class LevelCreator : AppState, ILevelCreator
    {
        private const int minWidthAndHeight = 3;

        private readonly int maxWidth;

        private readonly int maxHeight;

        private readonly int initialHeight;

        private readonly int initialWidth;

        private Level level;

        private string packageName;

        private string authorName;

        private int width;

        private int height;


        public LevelCreator(int maxWidth, int maxHeight, int initialHeight, int initialWidth)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            this.initialHeight = initialHeight;
            this.initialWidth = initialWidth;
        }

        public void CreateLevel(string packageName, string author)
        {
            this.packageName = packageName;
            authorName = author;
            width = initialWidth;
            height = initialHeight;
        }

        public override Shared.ExternalAppState GetAppState()
        {
            return Shared.ExternalAppState.Editor;
        }

        public override void ProcessCommand(InputCommand command)
        {
            switch (command)
            {
                case InputCommand.Left:
                    ChangeWidth(-1);
                    break;
                case InputCommand.Right:
                    ChangeWidth(1);
                    break;
                case InputCommand.Up:
                    ChangeHeight(1);
                    break;
                case InputCommand.Down:
                    ChangeHeight(-1);
                    break;
                case InputCommand.Back:
                    Back();
                    break;
                case InputCommand.Enter:
                    ToEditor();
                    break;
                default:
                    break;
            }
        }

        public override ViewMapData GetMapViewData()
        {
            var groundLayer = new GameElement[width, height];
            var collisionLayer = new GameElement[width, height];
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    groundLayer[x, y] = GameElement.Ground;
                    collisionLayer[x, y] = GameElement.Empty;
                }
            return new ViewMapData(groundLayer, collisionLayer);
        }

        private void ChangeWidth(int delta)
        {
            var newWidth = width + delta;
            if (newWidth >= minWidthAndHeight && newWidth <= maxWidth)
            {
                width = newWidth;
            }
        }

        private void ChangeHeight(int delta)
        {
            var newHeight = height + delta;
            if (newHeight >= minWidthAndHeight && newHeight <= maxHeight)
            {
                height = newHeight;
            }
        }

        private void Back()
        {
            Container.SetState(Container.InMenu);
        }

        private void ToEditor()
        {
            level = new Level(-1, packageName, authorName, width, height);
            Container.SetState(Container.InEditor);
            (Container.InEditor as ILevelEditor).Edit(level);
        }
    }
}
