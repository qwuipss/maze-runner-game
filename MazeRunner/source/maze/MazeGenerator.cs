#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MazeRunner;

public static class MazeGenerator
{
    private static readonly Random _random = new();

    /// <param name="width">
    /// Only odd. If even then will be rounded up to the nearest odd number
    /// </param>
    /// <param name="height">
    /// Only odd. If even then will be rounded up to the nearest odd number
    /// </param>
    /// <returns></returns>
    public static Maze GenerateMaze(int width, int height)
    {
        static (int Width, int Height) RoundUpToOdd(int width, int height)
        {
            var newWidth = width % 2 is 0 ? width + 1 : width;
            var newHeight = height % 2 is 0 ? height + 1 : height;

            return (newWidth, newHeight);
        }

        var visitedCells = new HashSet<Cell>();
        var backtrackingCells = new Stack<Cell>();

        (width, height) = RoundUpToOdd(width, height);
        var (tiles, emptiesInserted) = GetDefaultCells(width, height);

        var currentCell = GetRandomCellWithPrefferedType(tiles, CellType.Floor);
        visitedCells.Add(currentCell);

        while (visitedCells.Count != emptiesInserted)
        {
            var adjacentCells = GetAdjacentCellsWithPrefferedType(currentCell, tiles, CellType.Floor, 2)
                               .Where(cell => !visitedCells.Contains(cell))
                               .ToList();

            if (adjacentCells.Count is not 0)
            {
                backtrackingCells.Push(currentCell);

                var adjacentCell = adjacentCells[_random.Next(0, adjacentCells.Count)];

                RemoveWallBetween(currentCell, adjacentCell, tiles);

                visitedCells.Add(adjacentCell);
                currentCell = adjacentCell;
            }
            else
            {
                currentCell = backtrackingCells.Pop();
            }
        }

        return new Maze(tiles.Transpose());
    }

    private static (MazeTile[,] Tiles, int EmptiesInserted) GetDefaultCells(int width, int height)
    {
        var cells = new MazeTile[width, height];
        var emptiesInserted = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x % 2 != 0 && y % 2 != 0
                 && x.InRange(0, width - 1) && y.InRange(0, height - 1))
                {
                    cells[x, y] = new FloorTile();
                    emptiesInserted++;
                }
                else
                {
                    cells[x, y] = new WallTile();
                }
            }
        }

        return (cells, emptiesInserted);
    }

    private static void RemoveWallBetween(Cell first, Cell second, MazeTile[,] tiles)
    {
        var dx = second.X - first.X;
        var dy = second.Y - first.Y;

        var moveX = dx is 0 ? 0 : dx / Math.Abs(dx);
        var moveY = dy is 0 ? 0 : dy / Math.Abs(dy);

        var wallCoords = new Cell(moveX + first.X, moveY + first.Y);

        tiles[wallCoords.X, wallCoords.Y] = new FloorTile();
    }

    private static Cell GetRandomCellWithPrefferedType(MazeTile[,] tiles, CellType prefferedType)
    {
        var cell = new Cell(_random.Next(0, tiles.GetLength(0)), _random.Next(0, tiles.GetLength(1)));

        return GetRandomCellWithPrefferedType(tiles, cell, prefferedType);
    }

    private static Cell GetRandomCellWithPrefferedType(MazeTile[,] tiles, Cell cell, CellType prefferedType)
    {
        var searchingQueue = new Queue<Cell>();
        var visitedCells = new HashSet<Cell>();

        searchingQueue.Enqueue(cell);
        visitedCells.Add(cell);

        while (searchingQueue.Count is not 0)
        {
            var currentCell = searchingQueue.Dequeue();

            if (tiles[currentCell.X, currentCell.Y].CellType == prefferedType)
            {
                return currentCell;
            }

            foreach (var adjCell in GetAdjacentCells(currentCell, tiles, 1)
                                   .Where(cell => !visitedCells.Contains(cell)))
            {
                searchingQueue.Enqueue(adjCell);
            }
        }

        throw new KeyNotFoundException($"cell with type: {prefferedType} doesn't exist in {nameof(tiles)}");
    }

    private static IEnumerable<Cell> GetAdjacentCells(Cell cell, MazeTile[,] tiles, int cellOffset)
    {
        return new List<Cell>()
        {
            cell with { X = cell.X + cellOffset },
            cell with { X = cell.X - cellOffset },
            cell with { Y = cell.Y + cellOffset },
            cell with { Y = cell.Y - cellOffset },
        }
        .Where(cell => cell.InBoundsOf(tiles));
    }

    private static IEnumerable<Cell> GetAdjacentCellsWithPrefferedType(Cell cell, MazeTile[,] tiles, CellType prefferedType, int cellOffset)
    {
        return GetAdjacentCells(cell, tiles, cellOffset)
              .Where(cell => tiles[cell.X, cell.Y].CellType == prefferedType);
    }
}