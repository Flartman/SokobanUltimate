namespace SokobanUltimate.Models
{
    public record GameResult
    {
        public int LevelId { get; }

        public double TimeInSec {  get; }

        public GameResult(int levelId, double timeInSec)
        {
            LevelId = levelId;
            TimeInSec = timeInSec;
        }
    }
}
