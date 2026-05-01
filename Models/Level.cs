namespace SokobanGame.Models
{
    public class Level
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string MapData { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }
    public class LevelToDisplay
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCompleted { get; set; }
        public string BestMovesText { get; set; }
        public string BestTimeText { get; set; }
        public string BestResultText { get; set; }
        public int BestMoves { get; set; }
        public Level OriginalLevel { get; set; }
    }
}
