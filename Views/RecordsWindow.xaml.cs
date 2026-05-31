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
           var levelsWithRecords = dbContext.Levels.Where(l => l.Records.Any())
                .Select(level => new
                {
                    LevelName = level.Name,
                    Records = level.Records.OrderBy(r => r.CountMoves).ThenBy(r => r.Time).ToList()
                }).ToList();

            RecordsListBox.ItemsSource = levelsWithRecords;
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}