using SokobanGame.Database;
using SokobanGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SokobanGame.Views
{
    /// <summary>
    /// Логика взаимодействия для LevelSelectionWindow.xaml
    /// </summary>
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
                gameWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show("Пожалуйста, выберите уровень!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
