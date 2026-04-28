using SokobanGame.ViewModels;
using System.Windows;
namespace SokobanGame.Views
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            DataContext = new MainMenuVM();
        }
    }
}
