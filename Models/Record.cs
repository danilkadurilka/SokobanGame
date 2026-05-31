namespace SokobanGame.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int LevelId { get; set; }
        public int CountMoves { get; set; }
        public int Time { get; set; }
        public DateTime CompletedAt { get; set; }
        public virtual Level Level { get; set; }
        public string MovesText
        {
            get
            {
                return $"Ходов: {CountMoves}";
            }
        }
        public string TimeText
        {
            get
            {
                return $"Время: {TimeSpan.FromSeconds(Time):mm\\:ss}";
            }
        }
        public string RecordText
        {
            get
            {
                return $"{PlayerName} | {MovesText} | {TimeText}";
            }
        }
    }
}