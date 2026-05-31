using SokobanGame.Models;
using SokobanGame.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace SokobanGame.Views
{
    public partial class GameWindow : Window
    {
        private GameVM viewModel;
        public GameWindow(Level level, string playerName)
        {
            InitializeComponent();
            viewModel = new GameVM(level, playerName, this);
            DataContext = viewModel;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                viewModel.MoveUpCommand.Execute(null);
            }
            else if (e.Key == Key.Down || e.Key == Key.S)
            {
                viewModel.MoveDownCommand.Execute(null);
            }
            else if (e.Key == Key.Left || e.Key == Key.A)
            {
                viewModel.MoveLeftCommand.Execute(null);
            }
            else if (e.Key == Key.Right || e.Key == Key.D)
            {
                viewModel.MoveRightCommand.Execute(null);
            }
            else if (e.Key == Key.R)
            {
                MessageBoxResult result = MessageBox.Show("Нажата клавиша R.\nВы действительно хотите сбросить текущий прогресс?", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                    viewModel.RestartCommand.Execute(null);
                else
                    return;
            }
            else if (e.Key == Key.Escape)
            {
                MessageBoxResult result = MessageBox.Show("Нажата клавиша Escape.\nВы действительно хотите вернуться в главное меню?\nЭто приведет к сбросу текущей игры.", "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                    viewModel.BackToMenuCommand.Execute(null);
                else
                    return;
            }
        }
    }
}
