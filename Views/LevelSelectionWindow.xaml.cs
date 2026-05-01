using SokobanGame.Database;
using SokobanGame.Models;
using System.Windows;

namespace SokobanGame.Views
{
    public partial class LevelSelectionWindow : Window
    {
        private string playerName;
        private SokobanDbContext dbContext;
        public LevelSelectionWindow(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;
            dbContext = new SokobanDbContext();
            dbContext.EnsureDatabaseCreated();
            LevelsListBox.ItemsSource = dbContext.Levels.OrderBy(l => l.Id).ToList();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (LevelsListBox.SelectedItem is Level selectedLevel)
            {
                GameWindow gameWindow = new(selectedLevel, playerName);
                gameWindow.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("Пожалуйста, выберите уровень!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
