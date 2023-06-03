using MazeGenBE.Models;
using System;
using System.Collections.Generic;

namespace MazeGenBE.Logic
{
    public class MazeGenerator
    {
        private Maze maze;
        private int minSize = 10;
        private int maxSize = 1000;

        public MazeGenerator()
        {
        }

        public Maze GenerateMaze(int rows, int columns, string? seed = null)
        {
            // Size restrictions
            rows = Math.Max(rows, minSize);
            rows = Math.Min(rows, maxSize);
            columns = Math.Max(columns, minSize);
            columns = Math.Min(columns, maxSize);

            maze = new Maze
            {
                Rows = rows,
                Columns = columns,
                Cells = CreateEmptyMazeCells(rows, columns)
            };

            int randomSeed = seed == null ? GenerateRandomSeed() : seed.GetHashCode();
            Random random = new Random(randomSeed);
            maze.Seed = randomSeed;

            MazeCell startCell = GetRandomCell(rows, columns, random);
            startCell.Type = CellType.Start;
            maze.StartCell = startCell;

            MazeCell endCell = GetRandomCell(rows, columns, random);
            endCell.Type = CellType.End;
            maze.EndCell = endCell;

            CarvePassages(startCell, random);

            return maze;
        }
        
        private MazeCell[][] CreateEmptyMazeCells(int rows, int columns)
        {
            MazeCell[][] cells = new MazeCell[rows][];

            for (int i = 0; i < rows; i++)
            {
                cells[i] = new MazeCell[columns];
                for (int j = 0; j < columns; j++)
                {
                    cells[i][j] = new MazeCell
                    {
                        Row = i,
                        Column = j,
                        Visited = false,
                        Type = CellType.Wall
                    };
                }
            }

            return cells;
        }
        
        private int GenerateRandomSeed()
        {
            return Guid.NewGuid().GetHashCode();
        }

        private MazeCell GetRandomCell(int rows, int columns, Random random)
        {
            int row = random.Next(rows);
            int column = random.Next(columns);
            return maze.Cells[row][column];
        }
        
        private void CarvePassages(MazeCell currentCell, Random random) // DFS
        {
            Stack<MazeCell> stack = new Stack<MazeCell>();
            stack.Push(currentCell);

            while (stack.Count > 0)
            {
                MazeCell cell = stack.Pop();
                cell.Visited = true;

                List<MazeCell> neighbors = GetUnvisitedNeighbors(cell);
                if (neighbors.Count > 0)
                {
                    stack.Push(cell);

                    MazeCell nextCell = neighbors[random.Next(neighbors.Count)];
                    RemoveWallBetweenCells(cell, nextCell);
                    stack.Push(nextCell);
                }
            }
        }

        private List<MazeCell> GetUnvisitedNeighbors(MazeCell cell)
        {
            int row = cell.Row;
            int column = cell.Column;

            List<MazeCell> neighbors = new List<MazeCell>();

            if (IsInBounds(row - 2, column) && !maze.Cells[row - 2][column].Visited)
                neighbors.Add(maze.Cells[row - 2][column]);
            if (IsInBounds(row + 2, column) && !maze.Cells[row + 2][column].Visited)
                neighbors.Add(maze.Cells[row + 2][column]);
            if (IsInBounds(row, column - 2) && !maze.Cells[row][column - 2].Visited)
                neighbors.Add(maze.Cells[row][column - 2]);
            if (IsInBounds(row, column + 2) && !maze.Cells[row][column + 2].Visited)
                neighbors.Add(maze.Cells[row][column + 2]);

            return neighbors;
        }

        private bool IsInBounds(int row, int column)
        {
            return row >= 0 && row < maze.Rows && column >= 0 && column < maze.Columns;
        }

        private void RemoveWallBetweenCells(MazeCell cell1, MazeCell cell2)
        {
            int passageRow = cell1.Row + (cell2.Row - cell1.Row) / 2;
            int passageColumn = cell1.Column + (cell2.Column - cell1.Column) / 2;
            if(maze.Cells[passageRow][passageColumn].Type != CellType.End)
            {
                maze.Cells[passageRow][passageColumn].Type = CellType.Empty;
            }
            if (cell2.Type != CellType.End)
            {
                cell2.Type = CellType.Empty;
            }
        }
    }
}
