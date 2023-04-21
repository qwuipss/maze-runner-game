using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner;

public static class MazeGenerator
{
    private static readonly Random _random = new(123);

    public static Maze GenerateMaze(int width, int height)
    {
        var visitedCells = new HashSet<MazeCell>();
        var cellsStack = new Stack<MazeCell>();
        var cells = GetDefaultMaze(width, height);

        var startCell = new MazeCell(1, 1);

        var currentCell = startCell;
        visitedCells.Add(currentCell);

        while (visitedCells.Count != width * height) // todo
        {
            var adjacentCells = GetAdjacentCells(currentCell)
                               .Where(cell => IsCellEmptyNotVisited(cell, visitedCells) 
                                           && cell.InBoundsOf(cells))
                               .ToList();

            if (adjacentCells.Count is not 0)
            {
                var randomAdjacentCell = adjacentCells[_random.Next(0, adjacentCells.Count)];
                cellsStack.Push(currentCell);

                var removedWall = RemoveWallBetween(currentCell, randomAdjacentCell, cells);

                visitedCells.Add(randomAdjacentCell);
                currentCell = randomAdjacentCell;

                new Maze(cells).LoadToFile(new System.IO.FileInfo("maze.txt"));
            }
            else
            {
                currentCell = cellsStack.Pop();
            }
        }

        return new Maze(cells);
    }


    private static bool IsCellEmptyNotVisited(MazeCell cell, HashSet<MazeCell> visitedCells)
    {
        return cell.CellType is CellType.Empty && !visitedCells.Contains(cell);
    }

    private static MazeCell[,] GetDefaultMaze(int width, int height)
    {
        var cells = new MazeCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x % 2 != 0 && y % 2 != 0
                 && x.InRange(0, width - 1) && y.InRange(0, height - 1))
                {
                    cells[x, y] = new MazeCell(x, y, CellType.Empty);
                }
                else
                {
                    cells[x, y] = new MazeCell(x, y, CellType.Wall);
                }
            }
        }

        return cells;
    }

    /// <returns> 
    /// MazeCell with coordinates of removed wall
    /// </returns>
    private static MazeCell RemoveWallBetween(MazeCell first, MazeCell second, MazeCell[,] cells)
    {
        var dx = second.X - first.X;
        var dy = second.Y - first.Y;

        var moveX = dx is 0 ? 0 : dx / Math.Abs(dx);
        var moveY = dy is 0 ? 0 : dy / Math.Abs(dy);

        var wallCoords = new MazeCell(moveX + first.X, moveY + first.Y);

        cells[wallCoords.X, wallCoords.Y] = wallCoords;

        return wallCoords;
    }

    private static List<MazeCell> GetAdjacentCells(MazeCell cell)
    {
        const int CellsDistance = 2;

        return new List<MazeCell>()
        {
            cell with { X = cell.X + CellsDistance },
            cell with { X = cell.X - CellsDistance },
            cell with { Y = cell.Y + CellsDistance },
            cell with { Y = cell.Y - CellsDistance },
        };
    }
}