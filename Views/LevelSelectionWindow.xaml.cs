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
        private ObservableCollection<LevelToDisplay> levelsStatistics;
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
            List<Level> levels = dbContext.Levels.OrderBy(l => l.Id).ToList();
            levelsStatistics = new ObservableCollection<LevelToDisplay>();
            foreach (Level level in levels)
            {
                Record? bestRecord = dbContext.Records
                    .Where(r => r.LevelId == level.Id && r.PlayerName == playerName)
                    .OrderBy(r => r.CountMoves)
                    .ThenBy(r => r.Time)
                    .FirstOrDefault();
                bool isCompleted;
                string bestMovesText;
                string bestTimeText;
                string bestResultText;
                if (bestRecord == null)
                {
                    isCompleted = false;
                    bestMovesText = "";
                    bestTimeText = "";
                    bestResultText = "Вы ещё не проходили этот уровень";
                }
                else
                {
                    isCompleted = true;
                    bestMovesText = $"Ходов: {bestRecord.CountMoves}";
                    bestTimeText = (TimeSpan.FromSeconds(bestRecord.Time)).ToString(@"mm\:ss");
                    bestResultText = "Это ваш лучший результат!";
                }
                levelsStatistics.Add(new LevelToDisplay
                {
                    Id = level.Id,
                    Name = level.Name,
                    Width = level.Width,
                    Height = level.Height,
                    IsDefault = level.IsDefault,
                    IsCompleted = isCompleted,
                    BestMovesText = bestMovesText,
                    BestTimeText = bestTimeText,
                    BestResultText = bestResultText,
                    OriginalLevel = level
                });
            }
            LevelsListBox.ItemsSource = levelsStatistics;
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (LevelsListBox.SelectedItem is LevelToDisplay selectedLevel)
            {
                GameWindow gameWindow = new(selectedLevel.OriginalLevel, playerName);
                gameWindow.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("Пожалуйста, выберите уровень!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
