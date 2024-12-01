using System;
using System.Collections.Generic;
using static SokobanUltimate.Game1;

namespace SokobanUltimate.Models
{

    public class Map
    {
        public int Width { get; }

        public int Height{ get; }


        public Element[,] GroundLayer { get; }

        public Element[,] CollisionLayer { get; }

        public Map(Element[,] groundLayer, Element[,] collisionLayer  )
        {
            GroundLayer = groundLayer;
            CollisionLayer = collisionLayer;
            Width = GroundLayer.GetLength(0);
            Height = GroundLayer.GetLength(1);
        }


    }

    public record Level
    {
        public string Name { get; }

        public string Author { get; }

        public Map Map { get; }

        public int LevelWidth { get; }

        public int LevelHeight { get; }

        public Position PlayerPosition { get; private set; }

        public Level(string name, string author, string groundMapInText, string collisionMapInText, int width, int height)
        {
            Name = name;
            Author = author;
            
            var groundMap = AssembleMap(groundMapInText, width, height);
            var collisionMap = AssembleMap(collisionMapInText, width, height);
            Map = new Map(groundMap, collisionMap);
            LevelWidth = width;
            LevelHeight = height;
        }

        
        public Element[,] AssembleMap(string mapInText, int width, int height)
        {
            if(IsMapIncorrect(mapInText, width, height)) throw new ArgumentException("Incorrect map");

            var map = new Element[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var c = mapInText[y * width + x];
                    var tile = ConvertCharToTileType(c);
                    if(tile == Element.Player )
                    {
                        if (PlayerPosition is not null) throw new Exception("Map must contain only one player");
                        PlayerPosition = new Position(x, y);
                    }
                    map[x, y] = tile;
                }
            }

            //if (PlayerPosition is null) throw new ArgumentException("Map must contain at least one player");

            return map;
        }
        private static bool IsMapIncorrect(string input, int width, int height)
        {
            return (input.Length == 0
                || width == 0
                || height == 0
                || input.Length % width != 0
                || input.Length % height != 0);
        }

        private static Element ConvertCharToTileType(char c)
        {
            return c switch
            {
                'P' => Element.Player,
                'W' => Element.Wall,
                'E' => Element.EndPoint,
                'G' => Element.Ground,
                'B' => Element.Box,
                '.' => Element.Empty,
                _ => Element.Empty,
            };
        }
    }

    public class LevelPack
    {
        public string Name { get; }

        public string Author { get; }

        private readonly Dictionary<Guid, Level> _pack;

        public void Add(Level level)
        {
            var id = new Guid();
            _pack[id] = level;
        }

        public Level GetLevel(Guid id)
        {
            _pack.TryGetValue(id, out var level);
            return level;
        }
    }
}
