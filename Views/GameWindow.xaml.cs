using SokobanGame.Models;
using SokobanGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SokobanGame.Views
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
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
                viewModel.RestartCommand.Execute(null);
            }
            else if (e.Key == Key.Escape)
            {
                viewModel.BackToMenuCommand.Execute(null);
            }
        }
    }
}
