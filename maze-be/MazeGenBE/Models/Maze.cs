namespace MazeGenBE.Models
{
    public class Maze
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int? Seed { get; set; }
        public MazeCell[][] Cells { get; set; }
        public MazeCell StartCell { get; set; }
        public MazeCell EndCell { get; set; }
    }
}
