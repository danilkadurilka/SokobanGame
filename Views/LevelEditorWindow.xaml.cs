using SokobanGame.ViewModels;
using System.Windows;

namespace SokobanGame.Views
{
    public partial class LevelEditorWindow : Window
    {
        public LevelEditorWindow(string playerName)
        {
            InitializeComponent();
            DataContext = new LevelEditorVM(this, playerName);
        }
    }
}
