using Microsoft.VisualBasic;

namespace MazeGenBE.Models
{
    public class MazeCell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Visited { get; set; }
        public CellType Type { get; set; }
    }
}
