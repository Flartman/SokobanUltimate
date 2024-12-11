namespace Sokoban
{
    public class Shared
    {
        public enum ExternalAppState
        {
            Menu,
            Game,
            Editor,
            Packages,
            Profiles,
            Input,
            GameResult,
            Statistics,
            Library,
        }

        public enum InputCommand
        {
            Left,
            Right,
            Up,
            Down,
            Enter,
            Back,
            Restart,
            PrimaryAction,
            SecondaryAction,
            Edit,
            CreateLevel,
        }

        public enum GameElement
        {
            Empty,
            Wall,
            Box,
            Player,
            Ground,
            EndPoint,
        }

        public class Step
        {
            public int dX { get; }

            public int dY { get; }

            public Step(int dx, int dy)
            {
                dX = dx;
                dY = dy;
            }
        }
    }
}
