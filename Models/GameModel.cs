using System.Threading;
using static SokobanUltimate.Game1;

namespace SokobanUltimate.Models
{

    public class GameModel
    {
        public readonly Element[,] _groundMap;
        public readonly Element[,] _collisionMap;
        public readonly Map Map;

        private Position _player;
        private Level _level;


        public GameModel(Level level)
        {
            Map = level.Map;
            _player = level.PlayerPosition;
        }

        public bool TryMovePlayer(int dx, int dy)
        {
            var newPlayerX = _player.X + dx;
            var newPlayerY = _player.Y + dy;
            if (IsOutOfBounds(newPlayerX, newPlayerY)) return false;

            if (Map.CollisionLayer[newPlayerX, newPlayerY] is Element.Box)
            {
                var newBoxX = newPlayerX + dx;
                var newBoxY = newPlayerY + dy;
                if (!IsOutOfBounds(newBoxX, newBoxY))
                {
                    if (Map.CollisionLayer[newBoxX, newBoxY] is Element.Empty)
                    {
                        DoStep(dx, dy);
                        Map.CollisionLayer[newBoxX, newBoxY] = Element.Box;
                        return true;
                    }
                }
                return false;
            }

            if (Map.CollisionLayer[newPlayerX, newPlayerY] is Element.Wall) return false;

            DoStep(dx, dy);
            return true;
        }

        private void DoStep(int dx, int dy)
        {
            Map.CollisionLayer[_player.X, _player.Y] = Element.Empty;
            _player.X = _player.X + dx;
            _player.Y = _player.Y + dy;
            Map.CollisionLayer[_player.X, _player.Y] = Element.Player;
        }

        public bool IsOutOfBounds(int x, int y)
        {
            return x < 0 
                || y < 0 
                || x >= Map.Width
                || y >= Map.Height;
        }
    }



    public record Position 
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
