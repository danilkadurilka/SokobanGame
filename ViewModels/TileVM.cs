using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SokobanGame.ViewModels
{
    public class TileVM : BaseVM
    {
        private string tileType;
        private bool hasPlayer;
        private ImageSource tileImage;
        private int x;
        private int y;
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
                UpdateTileImage();
            }
        }
        public bool HasPlayer
        {
            get 
            { 
                return hasPlayer; 
            }
            set
            {
                hasPlayer = value;
                OnPropertyChanged("HasPlayer");
                OnPropertyChanged("DisplayType");
                UpdateTileImage();
            }
        }
        public string DisplayType
        {
            get
            {
                if (HasPlayer)
                    return "Player";
                else
                    return TileType;
            }
        }
        public ImageSource TileImage
        {
            get { return tileImage; }
            set
            {
                tileImage = value;
                OnPropertyChanged("TileImage");
            }
        }
        public TileVM(string tileType, bool hasPlayer = false, int x = 0, int y = 0)
        {
            this.tileType = tileType;
            this.hasPlayer = hasPlayer;
            this.x = x;
            this.y = y;
            UpdateTileImage();
        }
        private void UpdateTileImage()
        {
            string imageName = GetImageName();
            if (string.IsNullOrEmpty(imageName))
            {
                TileImage = null;
                return;
            }
            try
            {
                string imagePath = $"pack://application:,,,/Images/{imageName}";
                TileImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                TileImage = null;
            }
        }
        private string GetImageName()
        {
            string displayType = DisplayType;
            if (displayType == "Wall")
                return "wall.png";
            else if (displayType == "Box")
                return "box.png";
            else if (displayType == "BoxOnTarget")
                return "box_on_target.png";
            else if (displayType == "Target")
                return "target.png";
            else if (displayType == "Player")
                return "player.png";
            else if (displayType == "PlayerOnTarget")
                return "player_on_target.png";
            else
                return null;
        }
    }
}