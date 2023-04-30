#region Usings
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MazeRunner.MazeBase;

public static class MazeGenerator
{
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
        var (tiles, floorsInserted) = GetDefaultCells(width, height);

        var currentCell = GetRandomCellWithPrefferedType(tiles, TileType.Floor);
        visitedCells.Add(currentCell);

        while (visitedCells.Count != floorsInserted)
        {
            var adjacentCells = GetAdjacentCellsWithPrefferedType(currentCell, tiles, TileType.Floor, 2)
                               .Where(cell => !visitedCells.Contains(cell))
                               .ToList();

            if (adjacentCells.Count is not 0)
            {
                backtrackingCells.Push(currentCell);

                var adjacentCell = adjacentCells[RandomHelper.Next(0, adjacentCells.Count)];

                RemoveWallBetween(currentCell, adjacentCell, tiles);

                visitedCells.Add(adjacentCell);
                currentCell = adjacentCell;
            }
            else
            {
                currentCell = backtrackingCells.Pop();
            }
        }

        return new Maze(tiles);
    }

    public static void InsertTiles(Maze maze, Func<MazeTile> tile, int percentage)
    {
        var floorsCount = GetFloorsCount(maze);
        var insertionsCount = floorsCount * percentage / 100;

        for (int i = 0; i < insertionsCount; i++)
        {
            var cell = GetRandomCellWithPrefferedType(maze.Tiles, TileType.Floor);

            maze.Tiles[cell.Y, cell.X] = tile.Invoke();
        }
    }

    private static int GetFloorsCount(Maze maze)
    {
        var floorsCount = 0;

        foreach (var tile in maze.Tiles)
        {
            if (tile.TileType is TileType.Floor)
            {
                floorsCount++;
            }
        }

        return floorsCount;
    }

    private static (MazeTile[,] Tiles, int FloorsInserted) GetDefaultCells(int width, int height)
    {
        var tiles = new MazeTile[height, width];
        var emptiesInserted = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y % 2 != 0 && x % 2 != 0
                 && y.InRange(0, height - 1) && x.InRange(0, width - 1))
                {
                    tiles[y, x] = new Floor();
                    emptiesInserted++;
                }
                else
                {
                    tiles[y, x] = new Wall();
                }
            }
        }

        return (tiles, emptiesInserted);
    }

    private static void RemoveWallBetween(Cell first, Cell second, MazeTile[,] tiles)
    {
        var dx = second.X - first.X;
        var dy = second.Y - first.Y;

        var moveX = dx is 0 ? 0 : dx / Math.Abs(dx);
        var moveY = dy is 0 ? 0 : dy / Math.Abs(dy);

        var wallCoords = new Cell(moveX + first.X, moveY + first.Y);

        tiles[wallCoords.Y, wallCoords.X] = new Floor();
    }

    private static Cell GetRandomCellWithPrefferedType(MazeTile[,] tiles, TileType prefferedType)
    {
        var cell = new Cell(RandomHelper.Next(0, tiles.GetLength(1)), RandomHelper.Next(0, tiles.GetLength(0)));

        return GetRandomCellWithPrefferedType(tiles, cell, prefferedType);
    }

    private static Cell GetRandomCellWithPrefferedType(MazeTile[,] tiles, Cell cell, TileType prefferedType)
    {
        var searchingQueue = new Queue<Cell>();
        var visitedCells = new HashSet<Cell>();

        searchingQueue.Enqueue(cell);
        visitedCells.Add(cell);

        while (searchingQueue.Count is not 0)
        {
            var currentCell = searchingQueue.Dequeue();

            if (tiles[currentCell.Y, currentCell.X].TileType == prefferedType)
            {
                return currentCell;
            }

            foreach (var adjCell in GetAdjacentCells(currentCell, tiles, 1))
            {
                if (!visitedCells.Contains(adjCell))
                {
                    visitedCells.Add(adjCell);
                    searchingQueue.Enqueue(adjCell);
                }
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

    private static IEnumerable<Cell> GetAdjacentCellsWithPrefferedType(Cell cell, MazeTile[,] tiles, TileType prefferedType, int cellOffset)
    {
        return GetAdjacentCells(cell, tiles, cellOffset)
              .Where(cell => tiles[cell.Y, cell.X].TileType == prefferedType);
    }
}