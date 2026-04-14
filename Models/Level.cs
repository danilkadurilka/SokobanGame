namespace SokobanGame.Models
{
    public class Level
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string MapData { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }
}
