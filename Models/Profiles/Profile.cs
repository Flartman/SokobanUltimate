using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.Models.Profiles
{
    public interface IProfile
    {
        public string Name { get; set; }

        public IReadOnlySet<GameResult> Results { get; }


        public void UpdateResult(GameResult result);
    }

    public record Profile : IProfile
    {
        public string Name { get; set; }

        private HashSet<GameResult> _results;

        public IReadOnlySet<GameResult> Results => _results;

        public Profile(string name) : this(name, new HashSet<GameResult>())
        {

        }


        public Profile(string name, HashSet<GameResult> results) 
        {
            Name = name;
            _results = results;
        }

        public void UpdateResult(GameResult newResult)
        {
            var oldResultList = 
                _results
                .Where(r => r.LevelNumber == newResult.LevelNumber && r.PackageName == newResult.PackageName)
                .ToList();

            if(oldResultList.Count > 1)
            {
                throw new Exception($"Profile contain {oldResultList.Count} records for level {newResult.LevelNumber} in package {newResult.PackageName}");
            }
            else if(oldResultList.Count == 1)
            {
                var oldResult = oldResultList[0];
                if (oldResult.TimeInSeconds > newResult.TimeInSeconds)
                {
                    _results.Remove(oldResult);
                    _results.Add(newResult);
                }
                else
                {
                    return;
                }
            }
            _results.Add(newResult);
        }

    }
}
