using MazeGenBE.Models;
using System.Collections.Generic;

namespace MazeGenBE.Logic
{
    public class MazeSolver
    {
        public Maze maze { get; set; }

        public MazeSolver(Maze maze)
        {
            this.maze = maze;
            ResetVisited();
        }

        // During generation this value is already set to true, needs to be reset
        public void ResetVisited()
        {
            foreach (var row in maze.Cells)
            {
                foreach (var cell in row)
                {
                    cell.Visited = false;
                }
            }
        }
        public bool SolveMaze() // BFS
        {
            MazeCell start = maze.StartCell;
            MazeCell end = maze.EndCell;

            Queue<MazeCell> queue = new Queue<MazeCell>();
            Dictionary<MazeCell, MazeCell> prevCell = new Dictionary<MazeCell, MazeCell>();

            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                MazeCell current = queue.Dequeue();
                current.Visited = true;

                if (current.Row == end.Row && current.Column == end.Column)
                {
                    // Protects end cell from being overwritten
                    current = prevCell[current]; 

                    while (current.Row != start.Row || current.Column != start.Column)
                    {
                        current.Type = CellType.Path;
                        current = prevCell[current];
                    }
                    
                    return true;
                }

                foreach (MazeCell neighbor in GetUnvisitedNeighbors(current))
                {
                    if (!prevCell.ContainsKey(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        prevCell[neighbor] = current;
                    }
                }
            }
            return false;  // No path found
        }


        private List<MazeCell> GetUnvisitedNeighbors(MazeCell cell)
        {
            int row = cell.Row;
            int column = cell.Column;

            List<MazeCell> neighbors = new List<MazeCell>();

            if (IsValidAndUnvisited(row - 1, column))
                neighbors.Add(maze.Cells[row - 1][column]);
            if (IsValidAndUnvisited(row + 1, column))
                neighbors.Add(maze.Cells[row + 1][column]);
            if (IsValidAndUnvisited(row, column - 1))
                neighbors.Add(maze.Cells[row][column - 1]);
            if (IsValidAndUnvisited(row, column + 1))
                neighbors.Add(maze.Cells[row][column + 1]);

            return neighbors;
        }

        private bool IsValidAndUnvisited(int row, int column)
        {
            if (row >= 0 && row < maze.Rows && column >= 0 && column < maze.Columns)
            {
                MazeCell cell = maze.Cells[row][column];
                return (cell.Type == CellType.Empty || cell.Type == CellType.End) && !cell.Visited;
            }
            return false;
        }
    }
}
