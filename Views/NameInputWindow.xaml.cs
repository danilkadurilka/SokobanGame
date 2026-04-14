using SokobanGame.Database;
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
    /// Логика взаимодействия для NameInputWindow.xaml
    /// </summary>
    public partial class NameInputWindow : Window
    {
        private SokobanDbContext dbContext;
        public NameInputWindow()
        {
            InitializeComponent();
            dbContext = new SokobanDbContext();
            dbContext.EnsureDatabaseCreated();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            string playerName = PlayerNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(playerName))
            {
                MessageBox.Show("Пожалуйста, введите ваше имя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LevelSelectionWindow levelSelection = new(playerName);
            levelSelection.Show();
            this.Close();
        }

    }
}
