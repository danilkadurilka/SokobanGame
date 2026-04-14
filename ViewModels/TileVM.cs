using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SokobanGame.ViewModels
{
    public class TileVM : BaseVM
    {
        private string tileType;
        private bool hasPlayer;
        private ImageSource tileImage;
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
                UpdateTileImage();
            }
        }

        public bool HasPlayer
        {
            get { return hasPlayer; }
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
                {
                    return "Player";
                }
                else
                {
                    return TileType;
                }
            }
        }

        public ImageSource TileImage
        {
            get 
            { 
                return tileImage; 
            }
            set
            {
                tileImage = value;
                OnPropertyChanged("TileImage");
            }
        }

        public TileVM(string tileType, bool hasPlayer = false)
        {
            this.tileType = tileType;
            this.hasPlayer = hasPlayer;
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
            {
                return null;
            }
        }
    }
}
