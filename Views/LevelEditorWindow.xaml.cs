using SokobanGame.ViewModels;
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
    /// Логика взаимодействия для LevelEditorWindow.xaml
    /// </summary>
    public partial class LevelEditorWindow : Window
    {
        public LevelEditorWindow(string playerName)
        {
            InitializeComponent();
            DataContext = new LevelEditorVM(this, playerName);
        }
    }
}
