using SokobanGame.Database;
using System.Windows;

namespace SokobanGame.Views
{
    public partial class RecordsWindow : Window
    {
        private SokobanDbContext dbContext;
        public RecordsWindow()
        {
            InitializeComponent();
            dbContext = new SokobanDbContext();
            dbContext.EnsureDatabaseCreated();
            LoadRecords();
        }
        private void LoadRecords()
        {
            var levelsWithRecords = dbContext.Levels.Select(level => new LevelRecordsDisplay
                {
                    LevelName = level.Name,
                    Records = dbContext.Records
                        .Where(r => r.LevelId == level.Id)
                        .Select(r => new RecordDisplay
                        {
                            PlayerName = r.PlayerName,
                            CountMoves = r.CountMoves,
                            Time = r.Time,
                            CompletedAt = r.CompletedAt
                        })
                        .OrderBy(r => r.CountMoves)  // Сортируем по количеству ходов
                        .ThenBy(r => r.Time)         // Затем по времени
                        .ToList()
                })
                .Where(l => l.Records.Any())  // Только уровни, у которых есть рекорды
                .ToList();

            RecordsListBox.ItemsSource = levelsWithRecords;
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class LevelRecordsDisplay
    {
        public string LevelName { get; set; }
        public List<RecordDisplay> Records { get; set; }
    }

    public class RecordDisplay
    {
        public string PlayerName { get; set; }
        public int CountMoves { get; set; }
        public int Time { get; set; }
        public DateTime CompletedAt { get; set; }

        public string MovesText => $"Ходов: {CountMoves}";
        public string TimeText => $"Время: {System.TimeSpan.FromSeconds(Time):mm\\:ss}";
        public string RecordText => $"{PlayerName} | {MovesText} | {TimeText}";
    }
}