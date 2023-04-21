#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MazeRunner;

public static class MazeGenerator
{
    private static readonly Random _random = new(123);

    public static Maze GenerateMaze(int width, int height)
    {
        var visitedCells = new HashSet<Cell>();
        var backtrackingCells = new Stack<Cell>();
        var cells = GetDefaultMaze(width, height);

        var startCell = new Cell(1, 1);

        var currentCell = startCell;
        visitedCells.Add(currentCell);

        while (visitedCells.Count != width * height) // todo
        {
            var adjacentCells = GetAdjacentEmptyNotVisitedCells(currentCell, cells, visitedCells);

            if (adjacentCells.Count is not 0)
            {
                backtrackingCells.Push(currentCell);

                var randomAdjacentCell = adjacentCells[_random.Next(0, adjacentCells.Count)];

                RemoveWallBetween(currentCell, randomAdjacentCell, cells);

                visitedCells.Add(randomAdjacentCell);
                currentCell = randomAdjacentCell;

                new Maze(cells).LoadToFile(new System.IO.FileInfo("maze.txt")); // debug
            }
            else
            {
                currentCell = backtrackingCells.Pop();
            }
        }

        return new Maze(cells);
    }

    private static CellType[,] GetDefaultMaze(int width, int height)
    {
        var cells = new CellType[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x % 2 != 0 && y % 2 != 0
                 && x.InRange(0, width - 1) && y.InRange(0, height - 1))
                {
                    cells[x, y] = CellType.Empty;
                }
                else
                {
                    cells[x, y] = CellType.Wall;
                }
            }
        }

        return cells;
    }

    private static void RemoveWallBetween(Cell first, Cell second, CellType[,] cells)
    {
        var dx = second.X - first.X;
        var dy = second.Y - first.Y;

        var moveX = dx is 0 ? 0 : dx / Math.Abs(dx);
        var moveY = dy is 0 ? 0 : dy / Math.Abs(dy);

        var wallCoords = new Cell(moveX + first.X, moveY + first.Y);

        cells[wallCoords.X, wallCoords.Y] = CellType.Empty;
    }

    private static List<Cell> GetAdjacentEmptyNotVisitedCells(Cell cell, CellType[,] cells, HashSet<Cell> visitedCells)
    {
        const int CellsDistance = 2;

        return new List<Cell>()
        {
            cell with { X = cell.X + CellsDistance },
            cell with { X = cell.X - CellsDistance },
            cell with { Y = cell.Y + CellsDistance },
            cell with { Y = cell.Y - CellsDistance },
        }
        .Where(cell =>
               cell.InBoundsOf(cells)
            && cells[cell.X, cell.Y] is CellType.Empty // test without this
            && !visitedCells.Contains(cell))
        .ToList();
    }
}