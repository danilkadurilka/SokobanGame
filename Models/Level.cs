using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public bool IsCompleted { get; set; }
        [NotMapped]
        public string BestMovesText
        {
            get
            {
                if (BestRecord == null)
                    return string.Empty;
                return $"Ходов: {BestRecord.CountMoves}";
            }
        }
        [NotMapped]
        public string BestTimeText
        {
            get
            {
                if (BestRecord == null)
                    return string.Empty;
                return TimeSpan.FromSeconds(BestRecord.Time).ToString(@"mm\:ss");
            }
        }
        [NotMapped]
        public string BestResultText
        {
            get
            {
                if (BestRecord == null)
                    return "Вы ещё не проходили этот уровень";
                return "Это ваш лучший результат!";
            }
        }
        [NotMapped]
        public Record BestRecord { get; set; }
    }
}