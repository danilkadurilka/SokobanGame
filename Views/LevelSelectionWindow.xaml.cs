using SokobanGame.Database;
using SokobanGame.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace SokobanGame.Views
{
    public partial class LevelSelectionWindow : Window
    {
        private string playerName;
        private SokobanDbContext dbContext;
        private ObservableCollection<Level> levels;
        public LevelSelectionWindow(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
            PlayerNameText.Text = $"Игрок: {playerName}";
            dbContext = new SokobanDbContext();
            dbContext.EnsureDatabaseCreated();
            LoadLevelsWithStatistics();
        }
        private void LoadLevelsWithStatistics()
        {
            List<Level> levelsFromDb = dbContext.Levels.OrderBy(l => l.Id).ToList();
            levels = new ObservableCollection<Level>();

            foreach (Level level in levelsFromDb)
            {
                Record? bestRecord = dbContext.Records
                    .Where(r => r.LevelId == level.Id && r.PlayerName == playerName)
                    .OrderBy(r => r.CountMoves)
                    .ThenBy(r => r.Time)
                    .FirstOrDefault();

                level.BestRecord = bestRecord;
                if (bestRecord != null)
                    level.IsCompleted = true;
                else
                    level.IsCompleted = false;
                levels.Add(level);
            }

            LevelsListBox.ItemsSource = levels;
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (LevelsListBox.SelectedItem is Level selectedLevel)
            {
                GameWindow gameWindow = new(selectedLevel, playerName);
                gameWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show("Пожалуйста, выберите уровень!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
