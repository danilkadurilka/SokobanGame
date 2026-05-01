using SokobanGame.Database;
using System.Windows;

namespace SokobanGame.Views
{
    public partial class NameInputWindow : Window
    {
        private SokobanDbContext dbContext;
        private bool isForEditor;
        public NameInputWindow(bool isForEditor = false)
        {
            InitializeComponent();
            dbContext = new SokobanDbContext();
            dbContext.EnsureDatabaseCreated();
            this.isForEditor = isForEditor;
        }

        private void NextStage_Click(object sender, RoutedEventArgs e)
        {
            string playerName = PlayerNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                MessageBox.Show("Пожалуйста, введите ваше имя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (isForEditor)
            {
                LevelEditorWindow levelEditor = new LevelEditorWindow(playerName);
                levelEditor.ShowDialog();
                this.Close();
            }
            else
            {
                LevelSelectionWindow levelSelection = new LevelSelectionWindow(playerName);
                levelSelection.ShowDialog();
                this.Close();
            }
        }
    }
}