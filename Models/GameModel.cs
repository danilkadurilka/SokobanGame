using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SokobanGame.Models
{
    public class GameModel : INotifyPropertyChanged
    {
        private int width;
        private int height;
        private TileType[,] tileMap;
        private Player player;
        private int countMoves;
        private DateTime startTime;
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
        public TileType[,] TileMap
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
        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
                OnPropertyChanged("Player");
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
