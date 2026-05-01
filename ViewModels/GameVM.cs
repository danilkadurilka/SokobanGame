using SokobanGame.Database;
using SokobanGame.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SokobanGame.ViewModels
{
    public class GameVM : BaseVM
    {
        private GameModel gameModel;
        private string playerName;
        private Level currentLevel;
        private System.Windows.Threading.DispatcherTimer timer;
        private Window currentWindow;
        private bool isGameActive;
        private SokobanDbContext dbContext;
        private ObservableCollection<ObservableCollection<TileVM>> rows;
        private ICommand moveUpCommand;
        private ICommand moveDownCommand;
        private ICommand moveLeftCommand;
        private ICommand moveRightCommand;
        private ICommand restartCommand;
        private ICommand backToMenuCommand;
        public ObservableCollection<ObservableCollection<TileVM>> Rows
        {
            get 
            {
                return rows; 
            }
            set
            {
                rows = value;
                OnPropertyChanged("Rows");
            }
        }
        public int Moves
        {
            get
            {
                return gameModel.CountMoves;
            }
        }
        public string CurrentTime
        {
            get
            {
                if (gameModel.StartTime == default)
                    return "00:00";
                TimeSpan elapsed = DateTime.Now - gameModel.StartTime;
                return $"{elapsed:mm\\:ss}";
            }
        }
        public string PlayerName
        {
            get 
            { 
                return playerName; 
            }
        }
        public string LevelName
        {
            get
            {
                return currentLevel.Name;
            }
        }
        public ICommand MoveUpCommand
        {
            get
            {
                if (moveUpCommand == null)
                    moveUpCommand = new RelayCommand(ExecuteMoveUp, CanExecuteMove);
                return moveUpCommand;
            }
        }
        public ICommand MoveDownCommand
        {
            get
            {
                if (moveDownCommand == null)
                    moveDownCommand = new RelayCommand(ExecuteMoveDown, CanExecuteMove);
                return moveDownCommand;
            }
        }
        public ICommand MoveLeftCommand
        {
            get
            {
                if (moveLeftCommand == null)
                    moveLeftCommand = new RelayCommand(ExecuteMoveLeft, CanExecuteMove);
                return moveLeftCommand;
            }
        }
        public ICommand MoveRightCommand
        {
            get
            {
                if (moveRightCommand == null)
                    moveRightCommand = new RelayCommand(ExecuteMoveRight, CanExecuteMove);
                return moveRightCommand;
            }
        }
        public ICommand RestartCommand
        {
            get
            {
                if (restartCommand == null)
                    restartCommand = new RelayCommand(ExecuteRestart);
                return restartCommand;
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
        public GameVM(Level level, string playerName, Window currentWindow)
        {
            currentLevel = level;
            this.playerName = playerName;
            this.currentWindow = currentWindow;
            isGameActive = true;
            dbContext = new SokobanDbContext();
            dbContext.EnsureDatabaseCreated();
            gameModel = new GameModel
            {
                StartTime = DateTime.Now,
                CountMoves = 0
            };
            LoadLevel(currentLevel);
            timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isGameActive)
                OnPropertyChanged("CurrentTime");
        }
        private void LoadLevel(Level level)
        {
            string[] mapRows = level.MapData.Split('\n');
            gameModel.Height = level.Height;
            gameModel.Width = level.Width;
            gameModel.TileMap = new string[level.Width, level.Height];
            Rows = new ObservableCollection<ObservableCollection<TileVM>>();
            for (int y = 0; y < level.Height; y++)
            {
                ObservableCollection<TileVM> row = [];
                string currentRow;
                if (y < mapRows.Length)
                    currentRow = mapRows[y];
                else
                    currentRow = "";
                for (int x = 0; x < level.Width; x++)
                {
                    char c;
                    if (x < currentRow.Length)
                        c = currentRow[x];
                    else
                        c = ' ';
                    string tileType;
                    bool hasPlayer = false;
                    if (c == '#')
                        tileType = "Wall";
                    else if (c == '$')
                        tileType = "Box";
                    else if (c == '.')
                        tileType = "Target";
                    else if (c == '@')
                    {
                        tileType = "Floor";
                        hasPlayer = true;
                        gameModel.PlayerX = x;
                        gameModel.PlayerY = y;
                    }
                    else if (c == '*')
                        tileType = "BoxOnTarget";
                    else if (c == '+')
                    {
                        tileType = "Target";
                        hasPlayer = true;
                        gameModel.PlayerX = x;
                        gameModel.PlayerY = y;
                    }
                    else
                        tileType = "Floor";
                    gameModel.TileMap[x, y] = tileType;
                    row.Add(new TileVM(tileType, hasPlayer));
                }
                Rows.Add(row);
            }
            OnPropertyChanged("Rows");
            OnPropertyChanged("Moves");
        }
        private bool CanExecuteMove(object parameter)
        {
            return isGameActive;
        }

        private void ExecuteMoveUp(object parameter)
        {
            if (isGameActive)
                Move(0, -1);
        }

        private void ExecuteMoveDown(object parameter)
        {
            if (isGameActive)
                Move(0, 1);
        }

        private void ExecuteMoveLeft(object parameter)
        {
            if (isGameActive)
                Move(-1, 0);
        }

        private void ExecuteMoveRight(object parameter)
        {
            if (isGameActive)
                Move(1, 0);
        }

        private void ExecuteRestart(object parameter)
        {
            RestartGame();
        }

        private void ExecuteBackToMenu(object parameter)
        {
            BackToMenu();
        }

        private void Move(int dx, int dy)
        {
            int newX = gameModel.PlayerX + dx;
            int newY = gameModel.PlayerY + dy;
            if (IsWall(newX, newY))
                return;
            if (IsBox(newX, newY))
            {
                int pushX = newX + dx;
                int pushY = newY + dy;
                if (CanPushBox(pushX, pushY))
                {
                    PushBox(newX, newY, pushX, pushY);
                    MovePlayer(newX, newY);
                    gameModel.CountMoves++;
                    OnPropertyChanged("Moves");
                    UpdateTilesDisplay();
                    CheckWinCondition();
                }
            }
            else
            {
                MovePlayer(newX, newY);
                gameModel.CountMoves++;    
                OnPropertyChanged("Moves");
                UpdateTilesDisplay();
            }
        }
        private bool IsWall(int x, int y)
        {
            if (x < 0 || x >= gameModel.Width || y < 0 || y >= gameModel.Height)
                return true;
            if (gameModel.TileMap[x, y] == "Wall")
                return true;
            else
                return false;
        }
        private bool IsBox(int x, int y)
        {
            if (x < 0 || x >= gameModel.Width || y < 0 || y >= gameModel.Height)
                return true;
            if (gameModel.TileMap[x, y] == "Box" || gameModel.TileMap[x, y] == "BoxOnTarget")
                return true;
            else
                return false;
        }
        private bool CanPushBox(int x, int y)
        {
            if (x < 0 || x >= gameModel.Width || y < 0 || y >= gameModel.Height)
                return true;
            if (!IsWall(x, y) && !IsBox(x, y))
                return true;
            else
                return false;
        }
        private void PushBox(int boxX, int boxY, int pushX, int pushY)
        {
            bool wasOnTarget;
            bool pushOnTarget;
            if (gameModel.TileMap[boxX, boxY] == "BoxOnTarget")
                wasOnTarget = true;
            else
                wasOnTarget = false;
            if (gameModel.TileMap[pushX, pushY] == "Target")
                pushOnTarget = true;
            else
                pushOnTarget = false;
            if (pushOnTarget)
                gameModel.TileMap[pushX, pushY] = "BoxOnTarget";
            else
                gameModel.TileMap[pushX, pushY] = "Box";
            if (wasOnTarget)
                gameModel.TileMap[boxX, boxY] = "Target";
            else
                gameModel.TileMap[boxX, boxY] = "Floor";
        }
        private void MovePlayer(int x, int y)
        {
            bool wasOnTarget;
            bool moveToTarget;
            if (gameModel.TileMap[gameModel.PlayerX, gameModel.PlayerY] == "PlayerOnTarget")
                wasOnTarget = true;
            else
                wasOnTarget = false;
            if (gameModel.TileMap[x, y] == "Target")
                moveToTarget = true;
            else
                moveToTarget = false;
            if (wasOnTarget)
                gameModel.TileMap[gameModel.PlayerX, gameModel.PlayerY] = "Target";
            else
                gameModel.TileMap[gameModel.PlayerX, gameModel.PlayerY] = "Floor";
            if (moveToTarget)
                gameModel.TileMap[x, y] = "PlayerOnTarget";
            else
                gameModel.TileMap[x, y] = "Player";
            gameModel.PlayerX = x;
            gameModel.PlayerY = y;
        }
        private void UpdateTilesDisplay()
        {
            for (int y = 0; y < gameModel.Height; y++)
            {
                for (int x = 0; x < gameModel.Width; x++)
                {
                    bool hasPlayer;
                    if (x == gameModel.PlayerX && y == gameModel.PlayerY)
                        hasPlayer = true;
                    else
                        hasPlayer = false;
                    Rows[y][x].TileType = gameModel.TileMap[x, y];
                    Rows[y][x].HasPlayer = hasPlayer;
                }
            }
        }
        private void CheckWinCondition()
        {
            bool allBoxesOnTarget = true;
            for (int y = 0; y < gameModel.Height; y++)
            {
                for (int x = 0; x < gameModel.Width; x++)
                {
                    if (gameModel.TileMap[x, y] == "Box")
                    {
                        allBoxesOnTarget = false;
                        break;
                    }
                }
                if (!allBoxesOnTarget)
                    break;
            }
            if (allBoxesOnTarget)
            {
                isGameActive = false;
                timer.Stop();
                int timeSeconds = (int)gameModel.CurrentTime.TotalSeconds;
                Record newRecord = new()
                {
                    PlayerName = playerName,
                    LevelId = currentLevel.Id,
                    CountMoves = gameModel.CountMoves,
                    Time = timeSeconds,
                    CompletedAt = DateTime.Now
                };
                dbContext.Records.Add(newRecord);
                dbContext.SaveChanges();
                string message = "Поздравляем! Уровень пройден!\nХодов: " + gameModel.CountMoves + "\nВремя: " + CurrentTime;
                MessageBox.Show(message, "Победа!");
                BackToMenu();
            }
        }
        private void RestartGame()
        {
            gameModel = new GameModel
            {
                StartTime = DateTime.Now,
                CountMoves = 0
            };
            LoadLevel(currentLevel);
            OnPropertyChanged("Moves");
            OnPropertyChanged("CurrentTime");
        }
        private void BackToMenu()
        {
            timer?.Stop();
            currentWindow.Close();
        }
    }
}