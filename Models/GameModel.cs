
using Sokoban.Models.Levels;
using Sokoban.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using static Sokoban.Shared;

namespace Sokoban.Models
{

    public class GameModel : AppState
    {
        private readonly App app;

        private readonly Stopwatch watch;

        private readonly Position player;

        private Map map;

        private Level level;

        private int boxCount;

        private int boxOnEndpointCount;

        private bool IsWin => boxCount == boxOnEndpointCount;


        public GameModel(App app)
        {
            this.app = app;
            watch = new Stopwatch();
            player = new Position(0, 0);
        }

        public override void ProcessCommand(InputCommand command)
        {
            switch (command)
            {
                case InputCommand.Left:
                    TryMovePlayer(new Step(-1, 0));
                    break;
                case InputCommand.Right:
                    TryMovePlayer(new Step(1, 0));
                    break;
                case InputCommand.Up:
                    TryMovePlayer(new Step(0, -1));
                    break;
                case InputCommand.Down:
                    TryMovePlayer(new Step(0, 1));
                    break;
                case InputCommand.Back:
                    Back();
                    break;
                case InputCommand.Enter:
                    if (IsWin)
                    {
                        Back();
                    }
                    break;
                case InputCommand.Restart:
                    Restart();
                    break;
                case InputCommand.Edit:
                    EditLevel();
                    break;
                default:
                    break;
            }

            if (IsWin)
            {
                SaveResultAndBack();
            }
        }

        public override ViewListData GetListViewData()
        {
            var viewDataList = new List<string>()
            {
                Math.Round(watch.Elapsed.TotalSeconds, 2).ToString() + " seconds",
            };
            return new ViewListData(viewDataList, -1);
        }

        public override ViewMapData GetMapViewData()
        {
            return  (IsWin) ? null : new ViewMapData(map.GroundLayer, map.CollisionLayer);
        }

        public void Start(Level level)
        {
            this.level = level;
            map = new Map(level.Width, level.Height);
            Restart();
        }

        public override ExternalAppState GetAppState()
        {
            return (IsWin) ? ExternalAppState.GameResult : ExternalAppState.Game;
        }

        private void TryMovePlayer(Step step)
        {
            var newPlayerX = player.X + step.dX;
            var newPlayerY = player.Y + step.dY;
            if (IsOutOfBounds(newPlayerX, newPlayerY)) return;

            if (map.CollisionLayer[newPlayerX, newPlayerY] is GameElement.Wall) return;

            if (map.CollisionLayer[newPlayerX, newPlayerY] is GameElement.Box)
            {
                var newBoxX = newPlayerX + step.dX;
                var newBoxY = newPlayerY + step.dY;
                if (!IsOutOfBounds(newBoxX, newBoxY))
                {
                    if (map.CollisionLayer[newBoxX, newBoxY] is GameElement.Empty)
                    {
                        DoStep(step.dX, step.dY);
                        map.CollisionLayer[newBoxX, newBoxY] = GameElement.Box;
                        if (map.GroundLayer[newBoxX, newBoxY] is GameElement.EndPoint)
                        {
                            boxOnEndpointCount++;
                        }
                        if (map.GroundLayer[newPlayerX, newPlayerY] is GameElement.EndPoint)
                        {
                            boxOnEndpointCount--;
                        }
                    }
                }
                return;
            }
            DoStep(step.dX, step.dY);
        }

        private void DoStep(int dx, int dy)
        {
            map.CollisionLayer[player.X, player.Y] = GameElement.Empty;
            player.X = player.X + dx;
            player.Y = player.Y + dy;
            map.CollisionLayer[player.X, player.Y] = GameElement.Player;
        }

        private void Back()
        {
            Container.SetState(Container.InMenu);
        }

        private void SaveResultAndBack()
        {
            watch.Stop();
            var result = new GameResult(level.Number, level.PackageName, watch.Elapsed.TotalSeconds);
            app.ProfileManager.CurrentProfile.UpdateResult(result);
        }

        private void Restart()
        {
            player.X = level.PlayerPosition.X;
            player.Y = level.PlayerPosition.Y;

            watch.Restart();

            for(var x = 0; x < map.Width; x++) 
                for(var y = 0; y < map.Height; y++)
                {
                    map.GroundLayer[x, y] = level.Map.GroundLayer[x, y];
                    map.CollisionLayer[x, y] = level.Map.CollisionLayer[x, y];
                }

            CalculateBoxesCounters();
        }
        
        private void CalculateBoxesCounters()
        {
            boxCount = 0;
            boxOnEndpointCount = 0;
            for (var x = 0; x < map.Width; x++)
                for (var y = 0; y < map.Height; y++)
                {
                    if(map.CollisionLayer[x, y] is GameElement.Box)
                    {
                        boxCount++;
                        if (map.GroundLayer[x, y] is GameElement.EndPoint)
                        {
                            boxOnEndpointCount++;
                        }
                    }
                }
        }

        private void EditLevel()
        {
            app.LevelEditor.Edit(level);   
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 
                || y < 0 
                || x >= map.Width
                || y >= map.Height;
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
