using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanGame.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int LevelId { get; set; }
        public int Moves { get; set; }
        public int Time { get; set; }
    }
}
