using SokobanGame.Database;
using SokobanGame.Models;
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
            List<LevelRecordsDisplay>? levelsWithRecords = dbContext.Levels.Select(level => new LevelRecordsDisplay
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
                        .OrderBy(r => r.CountMoves)
                        .ThenBy(r => r.Time)
                        .ToList()
                })
                .Where(l => l.Records.Any())
                .ToList();
            RecordsListBox.ItemsSource = levelsWithRecords;
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}