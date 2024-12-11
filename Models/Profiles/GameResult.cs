namespace Sokoban.Models.Profiles
{
    public record GameResult
    {
        public int LevelNumber { get; }

        public string PackageName {  get; }

        public double TimeInSeconds { get; }

        public GameResult(int levelNumber, string packageName, double timeInSeconds)
        {
            LevelNumber = levelNumber;
            PackageName = packageName;
            TimeInSeconds = timeInSeconds;
        }
    }
}
