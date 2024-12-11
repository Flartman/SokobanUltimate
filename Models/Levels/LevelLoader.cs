using SharpDX.Direct3D9;
using Sokoban.Models.Menu;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using static Sokoban.Shared;

namespace Sokoban.Models.Levels
{
    public interface ILevelLoader
    {
        public Level LoadLevel(int levelNumber, string packageName);

        public void SaveLevel(Level level);

        public void SaveAllInFile();

        public IReadOnlyList<int> GetLevelNumbers();

        public IReadOnlyDictionary<string, IReadOnlyList<int>> GetPackageNameToLevelNumbers();
    }

    public class LevelLoader : ILevelLoader
    {
        private readonly Dictionary<string, Dictionary<int, string>> _packageNameToLevelNumbersToLevelMap;

        private readonly IMenuModel menuModel;

        private readonly App app;

        public LevelLoader(App app)
        {
            this.app = app;
            _packageNameToLevelNumbersToLevelMap = new Dictionary<string, Dictionary<int, string>>();
            LoadDirectory();
        }

        public IReadOnlyDictionary<string, IReadOnlyList<int>> GetPackageNameToLevelNumbers()
        {
            var packages = new Dictionary<string, IReadOnlyList<int>>();
            foreach (var packageName in _packageNameToLevelNumbersToLevelMap.Keys)
            {
                var package = _packageNameToLevelNumbersToLevelMap[packageName].Keys;
                packages.Add(packageName, package.ToList());
            }
            return packages;
        }

        public void SaveAllInFile()
        {
            string pathToDirectory = "LevelPackages";

            if (!Directory.Exists(pathToDirectory))
            {
                Directory.CreateDirectory(pathToDirectory);
            }

            foreach (var packageName in _packageNameToLevelNumbersToLevelMap.Keys)
            {
                string pathToFile = $"{pathToDirectory}\\{packageName}.txt";
                using (StreamWriter writer = new StreamWriter(pathToFile, false))
                {
                    foreach (var level in _packageNameToLevelNumbersToLevelMap[packageName].Values)
                    {
                        writer.WriteLineAsync(level);
                    }
                }
            }

        }

        public IReadOnlyList<int> GetLevelNumbers()
        {
            var result = new List<int>();
            foreach (var numbers in _packageNameToLevelNumbersToLevelMap.Values)
            {
                foreach (var number in numbers.Keys)
                {
                    result.Add(number);
                }
            }
            return result;
        }

        public void SaveLevel(Level level)
        {
            if (_packageNameToLevelNumbersToLevelMap[level.PackageName].ContainsKey(level.Number))
            {
                _packageNameToLevelNumbersToLevelMap[level.PackageName][level.Number] = ConvertToString(level);
            }
            else
            {
                level.Number = GetNextNewLevelNumber(level.PackageName);
                _packageNameToLevelNumbersToLevelMap[level.PackageName].Add(level.Number, ConvertToString(level));
                app.MenuModel.AddMenuItem(level.Number, level.PackageName);
            }
        }

        public Level LoadLevel(int levelNumber, string packageName)
        {
            _packageNameToLevelNumbersToLevelMap.TryGetValue(packageName, out var result);
            result.TryGetValue(levelNumber, out var text);
            return ConvertFromString(text, packageName);
        }

        private int GetNextNewLevelNumber(string packageName)
        {
            return (_packageNameToLevelNumbersToLevelMap[packageName].Count == 0) ? 
                1 :
                _packageNameToLevelNumbersToLevelMap[packageName].Keys.Max() + 1;
        }

        private void LoadDirectory()
        {
            var path = "LevelPackages";
            foreach (var file in Directory.GetFiles(path))
            {
                LoadFile(file);
            }
        }

        private void LoadFile(string path)
        {
            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name;
            var levelByNumber = new Dictionary<int, string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var count = int.Parse(line.Split(',')[0]);
                    levelByNumber.Add(count, line);
                }
            }

            var packageName = fileName.Split('.')[0]; ;
            _packageNameToLevelNumbersToLevelMap.Add(packageName, levelByNumber);
        }

        private static Level ConvertFromString(string line, string packageName)
        {
            var dataBlocks = line.Split(':');
            var attributes = dataBlocks[0].Split(",");
            var number = int.Parse(attributes[0]);
            var author = attributes[1];
            var width = int.Parse(attributes[2]);
            var height = int.Parse(attributes[3]);
            var playerPositionX = int.Parse(attributes[4]);
            var playerPositionY = int.Parse(attributes[5]);
            var level = new Level(number, packageName, author, width, height);

            AddMap(level, dataBlocks[1]);

            return level;
        }

        private static void AddMap(Level level, string map)
        {
            for (int y = 0; y < level.Height; y++)
                for (int x = 0; x < level.Width; x++)
                {
                    var indexOfCharToGround = y * level.Width + x;
                    var indexOfCharToCollision = indexOfCharToGround + map.Length / 2;
                    level.Map.GroundLayer[x, y] = ConvertCharToElement(map[indexOfCharToGround]);
                    level.Map.CollisionLayer[x, y] = ConvertCharToElement(map[indexOfCharToCollision]);
                    if (level.Map.CollisionLayer[x, y] is GameElement.Player)
                    {
                        level.PlayerPosition.X = x;
                        level.PlayerPosition.Y = y;
                    }
                }
        }

        private static string ConvertToString(Level level)
        {
            var result = new StringBuilder();
            result.Append(level.Number);
            result.Append(",");
            result.Append(level.Author);
            result.Append(",");
            result.Append(level.Width);
            result.Append(",");
            result.Append(level.Height);
            result.Append(",");
            result.Append(level.PlayerPosition.X);
            result.Append(",");
            result.Append(level.PlayerPosition.Y);
            result.Append(":");

            for (int y = 0; y < level.Height; y++)
                for (int x = 0; x < level.Width; x++)
                {
                    result.Append(ConvertElementToChar(level.Map.GroundLayer[x, y]));
                }

            for (int y = 0; y < level.Height; y++)
                for (int x = 0; x < level.Width; x++)
                {
                    result.Append(ConvertElementToChar(level.Map.CollisionLayer[x, y]));
                }

            var levelInText = result.ToString();
            return levelInText;
        }

        private static char ConvertElementToChar(GameElement element)
        {
            return element switch
            {
                GameElement.Box => 'B',
                GameElement.Empty => '.',
                GameElement.Wall => 'W',
                GameElement.Player => 'P',
                GameElement.Ground => 'G',
                GameElement.EndPoint => 'E',
            };
        }

        private static GameElement ConvertCharToElement(char c)
        {
            return c switch
            {
                'P' => GameElement.Player,
                'W' => GameElement.Wall,
                'E' => GameElement.EndPoint,
                'G' => GameElement.Ground,
                'B' => GameElement.Box,
                '.' => GameElement.Empty,
                _ => GameElement.Empty,
            };
        }
    }
}
