using SokobanGame.Views;
using System.Windows;
using System.Windows.Input;

namespace SokobanGame.ViewModels
{
    public class MainMenuVM : BaseVM
    {
        private ICommand newGameCommand;
        private ICommand levelEditorCommand;
        private ICommand recordsCommand;
        private ICommand instructionsCommand;
        private ICommand exitCommand;
        public ICommand NewGameCommand //VS рекомендует составной оператор назначения вместо if-null
        {
            get
            {
                newGameCommand ??= new RelayCommand(ExecuteNewGame, CanExecuteNewGame);
                return newGameCommand;
            }
        }
        public ICommand LevelEditorCommand
        {
            get
            {
                levelEditorCommand ??= new RelayCommand(ExecuteLevelEditor, CanExecuteLevelEditor);
                return levelEditorCommand;
            }
        }
        public ICommand RecordsCommand
        {
            get
            {
                recordsCommand ??= new RelayCommand(ExecuteRecords, CanExecuteRecords);
                return recordsCommand;
            }
        }
        public ICommand InstructionsCommand
        {
            get
            {
                instructionsCommand ??= new RelayCommand(ExecuteInstructions, CanExecuteInstructions);
                return instructionsCommand;
            }
        }
        public ICommand ExitCommand
        {
            get
            {
                exitCommand ??= new RelayCommand(ExecuteExit, CanExecuteExit);
                return exitCommand;
            }
        }
        private void ExecuteNewGame(object parameter)
        {
            OpenNewGame();
        }
        private bool CanExecuteNewGame(object parameter)
        {
            return true;
        }
        private void ExecuteLevelEditor(object parameter)
        {
            OpenLevelEditor();
        }
        private bool CanExecuteLevelEditor(object parameter)
        {
            return true;
        }
        private void ExecuteRecords(object parameter)
        {
            OpenRecords();
        }
        private bool CanExecuteRecords(object parameter)
        {
            return true;
        }
        private void ExecuteInstructions(object parameter)
        {
            OpenInstructions();
        }
        private bool CanExecuteInstructions(object parameter)
        {
            return true;
        }
        private void ExecuteExit(object parameter)
        {
            ExitApplication();
        }
        private bool CanExecuteExit(object parameter)
        {
            return true;
        }
        private void OpenNewGame()
        {
            var nameInputWindow = new NameInputWindow(isForEditor: false);
            nameInputWindow.ShowDialog();
        }
        private void OpenLevelEditor()
        {
            var nameInputWindow = new NameInputWindow(isForEditor: true);
            nameInputWindow.ShowDialog();
        }
        private void OpenRecords()
        {
            var recordsWindow = new RecordsWindow();
            recordsWindow.ShowDialog();
        }
        private void OpenInstructions()
        {
            var instructionsWindow = new InstructionsWindow();
            instructionsWindow.ShowDialog();
        }
        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}