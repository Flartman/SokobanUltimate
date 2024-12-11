using static Sokoban.Shared;


namespace Sokoban.Models.Levels
{

    public class Map
    {
        public int Width { get; }

        public int Height { get; }

        public GameElement[,] GroundLayer { get; }

        public GameElement[,] CollisionLayer { get; }

        public Map(int width, int height)
        {
            GroundLayer = new GameElement[width, height];
            CollisionLayer = new GameElement[width, height];
            Width = GroundLayer.GetLength(0);
            Height = GroundLayer.GetLength(1);
            Initialize();
        }

        private void Initialize()
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    CollisionLayer[x, y] = GameElement.Empty;
                    GroundLayer[x, y] = GameElement.Ground;
                }
        }
    }

    public record Level
    {
        public int Number { get; set; }

        public string PackageName { get; }

        public string Author { get; }

        public Map Map { get; }

        public int Width => Map.Width;

        public int Height => Map.Height;

        public Position PlayerPosition { get; set; }

        public Level(int number, string packageName, string author, int width, int height)
        {
            Number = number;
            PackageName = packageName;
            PlayerPosition = new Position(0, 0);
            Author = author;
            Map = new Map(width, height);
        }
    }
}
