using Microsoft.EntityFrameworkCore;
using SokobanGame.Database;
using SokobanGame.Models;
using SokobanGame.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SokobanGame.ViewModels
{
    public class LevelEditorVM : BaseVM
    {
        private SokobanDbContext dbContext;
        private Window currentWindow;
        private string currentPlayerName;
        private int width;
        private int height;
        private string levelName;
        private string selectedTileCommand;
        private ObservableCollection<ObservableCollection<TileVM>> tileRows;
        private ObservableCollection<Level> levels;
        private Level selectedLevel;
        private bool isFloorSelected = true;
        private bool isWallSelected;
        private bool isTargetSelected;
        private bool isBoxSelected;
        private bool isPlayerSelected;
        private ICommand newLevelCommand;
        private ICommand saveLevelCommand;
        private ICommand deleteLevelCommand;
        private ICommand backToMenuCommand;
        private ICommand setTileCommand;

        public int Width
        {
            get 
            { 
                return width;
            }
            set
            {
                if (value > 0 && value <= 50 && value != width)
                {
                    width = value;
                    OnPropertyChanged("Width");
                    ResizeMap();
                }
            }
        }
        public int Height
        {
            get 
            { 
                return height; 
            }
            set
            {
                if (value > 0 && value <= 50 && value != height)
                {
                    height = value;
                    OnPropertyChanged("Height");
                    ResizeMap();
                }
            }
        }
        public string LevelName
        {
            get
            { 
                return levelName;
            }
            set
            {
                levelName = value;
                OnPropertyChanged("LevelName");
            }
        }
        public ObservableCollection<ObservableCollection<TileVM>> TileRows
        {
            get
            { 
                return tileRows; 
            }
            set 
            {
                tileRows = value; 
                OnPropertyChanged("TileRows"); 
            }
        }

        public ObservableCollection<Level> Levels
        {
            get
            {
                return levels; 
            }
        }

        public Level SelectedLevel
        {
            get 
            { 
                return selectedLevel; 
            }
            set
            {
                selectedLevel = value;
                OnPropertyChanged("SelectedLevel");
                if (value != null)
                {
                    LoadLevel(value);
                }
            }
        }

        public bool IsFloorSelected
        {
            get 
            { 
                return isFloorSelected; 
            }
            set
            {
                isFloorSelected = value;
                OnPropertyChanged("IsFloorSelected");
                if (value) 
                    selectedTileCommand = "Floor";
            }
        }

        public bool IsWallSelected
        {
            get 
            { 
                return isWallSelected;
            }
            set
            {
                isWallSelected = value;
                OnPropertyChanged("IsWallSelected");
                if (value) 
                    selectedTileCommand = "Wall";
            }
        }

        public bool IsTargetSelected
        {
            get
            { 
                return isTargetSelected;
            }
            set
            {
                isTargetSelected = value;
                OnPropertyChanged("IsTargetSelected");
                if (value) 
                    selectedTileCommand = "Target";
            }
        }

        public bool IsBoxSelected
        {
            get 
            { 
                return isBoxSelected;
            }
            set
            {
                isBoxSelected = value;
                OnPropertyChanged("IsBoxSelected");
                if (value)
                    selectedTileCommand = "Box";
            }
        }

        public bool IsPlayerSelected
        {
            get 
            { 
                return isPlayerSelected; 
            }
            set
            {
                isPlayerSelected = value;
                OnPropertyChanged("IsPlayerSelected");
                if (value) 
                    selectedTileCommand = "Player";
            }
        }


        public ICommand NewLevelCommand
        {
            get
            {
                if (newLevelCommand == null)
                    newLevelCommand = new RelayCommand(ExecuteNewLevel);
                return newLevelCommand;
            }
        }

        public ICommand SaveLevelCommand
        {
            get
            {
                if (saveLevelCommand == null)
                    saveLevelCommand = new RelayCommand(ExecuteSaveLevel);
                return saveLevelCommand;
            }
        }

        public ICommand DeleteLevelCommand
        {
            get
            {
                if (deleteLevelCommand == null)
                    deleteLevelCommand = new RelayCommand(ExecuteDeleteLevel);
                return deleteLevelCommand;
            }
        }

        public ICommand BackToMenuCommand
        {
            get
            {
                if (backToMenuCommand == null)
                    backToMenuCommand = new RelayCommand(ExecuteBackToMenu);
                return backToMenuCommand;
            }
        }

        public ICommand SetTileCommand
        {
            get
            {
                if (setTileCommand == null)
                    setTileCommand = new RelayCommand(ExecuteSetTile);
                return setTileCommand;
            }
        }
        public LevelEditorVM(Window currentWindow, string playerName)
        {
            this.currentWindow = currentWindow;
            this.currentPlayerName = playerName;
            dbContext = new SokobanDbContext();
            dbContext.EnsureDatabaseCreated();
            levels = new ObservableCollection<Level>(dbContext.Levels.OrderBy(l => l.Id).ToList());
            width = 10;
            height = 10;
            selectedTileCommand = "Floor";
            InitializeMap();
        }
        private void InitializeMap()
        {
            tileRows = new ObservableCollection<ObservableCollection<TileVM>>();
            for (int y = 0; y < Height; y++)
            {
                ObservableCollection<TileVM> row = new ObservableCollection<TileVM>();
                for (int x = 0; x < Width; x++) 
                    row.Add(new TileVM("Floor", false, x,y));
                tileRows.Add(row);
            }
            OnPropertyChanged("TileRows");
        }

        private void ResizeMap()
        {
            if (tileRows == null)
            {
                InitializeMap();
                return;
            }

            ObservableCollection<ObservableCollection<TileVM>> newRows = new ObservableCollection<ObservableCollection<TileVM>>();

            for (int y = 0; y < Height; y++)
            {
                ObservableCollection<TileVM> newRow = new ObservableCollection<TileVM>();
                for (int x = 0; x < Width; x++)
                {
                    if (y < tileRows.Count && x < tileRows[y].Count)
                        newRow.Add(tileRows[y][x]);
                    else
                        newRow.Add(new TileVM("Floor", false, x, y));
                }
                newRows.Add(newRow);
            }
            tileRows = newRows;
            OnPropertyChanged("TileRows");
        }

        private void ExecuteSetTile(object parameter)
        {
            TileVM tile = parameter as TileVM;
            if (tile == null) 
                return;
            if (selectedTileCommand == "Player" || selectedTileCommand == "PlayerOnTarget")
                RemoveExistingPlayer();
            tile.TileType = selectedTileCommand;
        }

        private void RemoveExistingPlayer()
        {
            for (int y = 0; y < tileRows.Count; y++)
            {
                for (int x = 0; x < tileRows[y].Count; x++)
                {
                    if (tileRows[y][x].TileType == "Player" || tileRows[y][x].TileType == "PlayerOnTarget")
                        tileRows[y][x].TileType = "Floor";
                }
            }
        }

        private void LoadLevel(Level level)
        {
            if (level == null || string.IsNullOrEmpty(level.MapData)) 
                return;
            string[] mapRows = level.MapData.Split('\n');
            Width = level.Width;
            Height = level.Height;
            levelName = level.Name;
            ObservableCollection<ObservableCollection<TileVM>> newRows = new ObservableCollection<ObservableCollection<TileVM>>();
            for (int y = 0; y < Height; y++)
            {
                ObservableCollection<TileVM> newRow = new ObservableCollection<TileVM>();
                string rowData;
                if (y < mapRows.Length)
                    rowData = mapRows[y];
                else
                    rowData = "";
                for (int x = 0; x < Width; x++)
                {
                    char c;
                    if (x < rowData.Length)
                        c = rowData[x];
                    else
                        c = ' ';
                    string tileType;
                    if (c == '#')
                        tileType = "Wall";
                    else if (c == '.')
                        tileType = "Target";
                    else if (c == '$')
                        tileType = "Box";
                    else if (c == '@')
                        tileType = "Player";
                    else if (c == '*')
                        tileType = "BoxOnTarget";
                    else if (c == '+')
                        tileType = "PlayerOnTarget";
                    else
                        tileType = "Floor";
                    newRow.Add(new TileVM(tileType, false, x,y));
                }
                newRows.Add(newRow);
            }
            tileRows = newRows;
            OnPropertyChanged("TileRows");
        }
        private void ExecuteNewLevel(object parameter)
        {
            SelectedLevel = null;
            Width = 10;
            Height = 10;
            InitializeMap();
        }
        private void ExecuteSaveLevel(object parameter)
        {
            if (string.IsNullOrWhiteSpace(LevelName))
            {
                MessageBox.Show("Введите название уровня!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (levels.Any(l => l.Name == LevelName.Trim()))
            {
                MessageBox.Show("Уровень с таким названием уже существует! Введите другое название.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!ValidateMap())
            {
                MessageBox.Show("На карте должен быть игрок!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            string mapData = ConvertMapToString();
            if (SelectedLevel != null && SelectedLevel.Id > 0 && !SelectedLevel.IsDefault)
            {
                SelectedLevel.Width = Width;
                SelectedLevel.Height = Height;
                SelectedLevel.MapData = mapData;
                dbContext.Entry(SelectedLevel).State = EntityState.Modified;
                dbContext.SaveChanges();
                int index = levels.IndexOf(SelectedLevel);
                levels[index] = SelectedLevel;
            }
            else
            {
                Level newLevel = new Level
                {
                    Name = $"Уровень (создан игроком {currentPlayerName})",
                    Width = Width,
                    Height = Height,
                    MapData = mapData,
                    IsDefault = false
                };
                dbContext.Levels.Add(newLevel);
                dbContext.SaveChanges();
                levels.Add(newLevel);
                SelectedLevel = newLevel;
            }
            MessageBox.Show("Уровень сохранен!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private bool ValidateMap()
        {
            for (int y = 0; y < tileRows.Count; y++)
            {
                for (int x = 0; x < tileRows[y].Count; x++)
                {
                    string tileType = tileRows[y][x].TileType;
                    if (tileType == "Player" || tileType == "PlayerOnTarget")
                        return true;
                }
            }
            return false;
        }
        private string ConvertMapToString()
        {
            string result = "";
            for (int y = 0; y < tileRows.Count; y++)
            {
                for (int x = 0; x < tileRows[y].Count; x++)
                {
                    char symbol = ' ';
                    string tileType = tileRows[y][x].TileType;

                    if (tileType == "Wall")
                        symbol = '#';
                    else if (tileType == "Target")
                        symbol = '.';
                    else if (tileType == "Box")
                        symbol = '$';
                    else if (tileType == "Player")
                        symbol = '@';
                    else if (tileType == "BoxOnTarget")
                        symbol = '*';
                    else if (tileType == "PlayerOnTarget")
                        symbol = '+';
                    else
                        symbol = ' ';

                    result += symbol;
                }
                if (y < tileRows.Count - 1) result += "\n";
            }
            return result;
        }
        private void ExecuteDeleteLevel(object parameter)
        {
            if (SelectedLevel == null) 
                return;
            if (SelectedLevel.IsDefault)
            {
                MessageBox.Show("Нельзя удалить стандартный уровень!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Удалить уровень \"{SelectedLevel.Name}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                dbContext.Levels.Remove(SelectedLevel);
                dbContext.SaveChanges();
                levels.Remove(SelectedLevel);
                SelectedLevel = null;
                InitializeMap();
                MessageBox.Show("Уровень удален!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ExecuteBackToMenu(object parameter)
        {
            dbContext.Dispose();
            currentWindow.Close();
        }
    }
}