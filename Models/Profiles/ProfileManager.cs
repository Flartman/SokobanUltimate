using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sokoban.Shared;

namespace Sokoban.Models.Profiles
{
    public interface IProfileManager
    {
        public void SelectProfile(IProfile profile);

        public void Add(string profileName);

        public void DeleteProfile(string name);

        public IReadOnlyList<IProfile> Profiles { get; }

        public IProfile CurrentProfile { get; }

        public void SaveProfilesInFile();
    }

    public class ProfileManager : IProfileManager
    {
        private readonly List<IProfile> _profiles;

        private IProfile _selectedProfile;
        
        private int _selectedProfileIndex;

        public IReadOnlyList<IProfile> Profiles => _profiles;

        public IProfile CurrentProfile { get; private set; }

        public ProfileManager() 
        {
             _profiles = new List<IProfile>();
            LoadProfilesFromFile();
            CurrentProfile = _profiles[0];
            _selectedProfile = CurrentProfile;
        }    

        private void DeleteProfile(string profileName)
        {
            throw new NotImplementedException(); 
        }

        private void AddProfile(IProfile profile)
        {
            if(!_profiles.Contains(profile))
            {
                _profiles.Add(profile);
            }
            else
            {
                throw new Exception("This profile already exist");
            }
        }

        public void SaveProfilesInFile()
        {
            string pathToDirectory = "Profiles";

            if (!Directory.Exists(pathToDirectory))
            {
                Directory.CreateDirectory(pathToDirectory);
            }

            foreach (var profile in _profiles)
            {
                string pathToFile = $"{pathToDirectory}\\{profile.Name}.txt";
                using (StreamWriter writer = new StreamWriter(pathToFile, false))
                {
                    foreach (var result in profile.Results)
                    {
                        writer.WriteLineAsync($"{result.LevelNumber}\t{result.PackageName}\t{result.TimeInSeconds}");
                    }
                }
            }
        }

        private void LoadProfilesFromFile()
        {
            var path = "Profiles";
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    LoadProfileFromFile(file);
                }
            }
        }

        private void LoadProfileFromFile(string path)
        {
            var results = new HashSet<GameResult>();
            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name;
            var profileName = fileName.Split('.')[0];

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var blocks = line.Split('\t');
                    var levelNumber = int.Parse(blocks[0]);
                    var packageName = blocks[1];
                    var timeInSeconds = double.Parse(blocks[2]);
                    results.Add(new GameResult(levelNumber, packageName, timeInSeconds));
                }
            }
            _profiles.Add(new Profile(profileName, results));
        }

        public void SelectProfile(IProfile profile)
        {
            if (Profiles.Contains(profile))
            {
                CurrentProfile = profile;
            }
            else
            {
                throw new ArgumentException("Profile was not created in ProfileManager");
            }
        }

        public void Add(string profileName)
        {
            AddProfile(new Profile(profileName));
        }

        void IProfileManager.DeleteProfile(string name)
        {
            throw new NotImplementedException();
        }
    }
}
