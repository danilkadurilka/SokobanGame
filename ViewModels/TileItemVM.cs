using System.Windows.Media;
using System.Windows.Input;

namespace SokobanGame.ViewModels
{
    public class TileItemVM : BaseVM
    {
        private int x;
        private int y;
        private string tileType;
        public int X
        {
            get 
            {
                return x; 
            }
            set 
            { 
                x = value; 
                OnPropertyChanged("X");
            }
        }
        public int Y
        {
            get
            { 
                return y; 
            }
            set 
            { 
                y = value;
                OnPropertyChanged("Y"); 
            }
        }
        public string TileType
        {
            get
            {
                return tileType;
            }
            set
            {
                tileType = value;
                OnPropertyChanged("TileType");
                OnPropertyChanged("BackgroundColor");
                OnPropertyChanged("TooltipText");
            }
        }
        public Brush BackgroundColor
        {
            get
            {
                if (TileType == "Wall")
                    return Brushes.DarkGray;
                else if (TileType == "Box")
                    return Brushes.SaddleBrown;
                else if (TileType == "BoxOnTarget")
                    return Brushes.Goldenrod;
                else if (TileType == "Target")
                    return Brushes.Gold;
                else if (TileType == "Player")
                    return Brushes.DodgerBlue;
                else if (TileType == "PlayerOnTarget")
                    return Brushes.LightBlue;
                else 
                    return Brushes.LightGray;
            }
        }
        public string TooltipText
        {
            get
            {
                if (TileType == "Wall")
                    return "Стена";
                else if (TileType == "Box")
                    return "Ящик";
                else if (TileType == "BoxOnTarget")
                    return "Ящик на цели";
                else if (TileType == "Target")
                    return "Цель";
                else if (TileType == "Player")
                    return "Игрок";
                else if (TileType == "PlayerOnTarget")
                    return "Игрок на цели";
                else
                    return "Пол";
            }
        }
        public TileItemVM(int x, int y, string tileType)
        {
            this.x = x;
            this.y = y;
            this.tileType = tileType;
        }
    }
}