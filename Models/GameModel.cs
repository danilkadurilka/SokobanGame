using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SokobanGame.Models
{
    public class GameModel : INotifyPropertyChanged
    {
        private int width;
        private int height;
        private string[,] tileMap;
        private int playerX;
        private int playerY;
        private int countMoves;
        private DateTime startTime;
        public GameModel()
        {
            StartTime = DateTime.Now;
        }
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");
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
                height = value;
                OnPropertyChanged("Height");
            }
        }
        public string[,] TileMap
        {
            get
            {
                return tileMap;
            }
            set
            {
                tileMap = value;
                OnPropertyChanged("TileMap");
            }
        }
        public int PlayerX
        {
            get
            {
                return playerX;
            }
            set
            {
                playerX = value;
                OnPropertyChanged("PlayerX");
            }
        }
        public int PlayerY
        {
            get
            {
                return playerY;
            }
            set
            {
                playerY = value;
                OnPropertyChanged("PlayerY");
            }
        }
        public int CountMoves
        {
            get
            {
                return countMoves;
            }
            set
            {
                countMoves = value;
                OnPropertyChanged("CountMoves");
            }
        }
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        public TimeSpan CurrentTime
        {
            get
            {
                return DateTime.Now - startTime;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
