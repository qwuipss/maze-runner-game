using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.MazeBase;

public static class MazeGenerator
{
    /// <param name="width">
    /// Only odd. If even then will be rounded up to the nearest odd number
    /// </param>
    /// <param name="height">
    /// Only odd. If even then will be rounded up to the nearest odd number
    /// </param>
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
        var (tiles, floorsInserted) = GetMazeTemplate(width, height);

        var maze = new Maze(tiles);

        var currentCell = GetRandomFloorCell(maze);
        visitedCells.Add(currentCell);

        while (visitedCells.Count != floorsInserted)
        {
            var adjacentCells = GetAdjacentFloorCells(currentCell, maze, 2)
                               .Where(cell => !visitedCells.Contains(cell))
                               .ToList();

            if (adjacentCells.Count is not 0)
            {
                backtrackingCells.Push(currentCell);

                var adjacentCellIndex = RandomHelper.Next(0, adjacentCells.Count);
                var adjacentCell = adjacentCells[adjacentCellIndex];

                RemoveWallBetween(currentCell, adjacentCell, tiles);

                visitedCells.Add(adjacentCell);
                currentCell = adjacentCell;
            }
            else
            {
                currentCell = backtrackingCells.Pop();
            }
        }

        return maze;
    }

    private static void RemoveWallBetween(Cell first, Cell second, MazeTile[,] tiles)
    {
        var dx = second.X - first.X;
        var dy = second.Y - first.Y;

        var moveX = dx is 0 ? 0 : dx / Math.Abs(dx);
        var moveY = dy is 0 ? 0 : dy / Math.Abs(dy);

        var wallCoords = new Cell(moveX + first.X, moveY + first.Y);

        var floor = new Floor();

        tiles[wallCoords.Y, wallCoords.X] = floor;
    }

    #region Inserters
    public static void InsertTraps(Maze maze, Func<MazeTrap> trapSource, int percentage)
    {
        var floorsCount = maze.GetFloorsCount();
        var insertionsCount = floorsCount * percentage / 100;

        for (int i = 0; i < insertionsCount; i++)
        {
            var cell = GetRandomFloorCell(maze);

            maze.InsertTrap(trapSource.Invoke(), cell);
        }
    }

    public static void InsertExit(Maze maze)
    {
        var sideCell = GetRandomSideCell(maze);

        var exit = new Exit
        {
            FrameRotationAngle = Exit.GetFrameRotationAngle(sideCell, maze)
        };

        exit.OriginFrameRotationVector = MazeTile.GetOriginFrameRotationVector(exit);

        maze.InsertExit(exit, sideCell);
    }

    public static void InsertItem(Maze maze, MazeItem item)
    {
        var floorCell = GetRandomFloorCell(maze);

        maze.InsertItem(item, floorCell);
    }
    #endregion

    #region Utilities
    private static (int Width, int Height) GetMazeDimensions(Maze maze)
    {
        var skeleton = maze.Skeleton;

        return (skeleton.GetLength(1), skeleton.GetLength(0));
    }

    private static void AddCellInSearchingQueue(IEnumerable<Cell> cells, HashSet<Cell> visitedCells, Queue<Cell> searchingQueue)
    {
        foreach (var cell in cells)
        {
            if (!visitedCells.Contains(cell))
            {
                searchingQueue.Enqueue(cell);
                visitedCells.Add(cell);
            }
        }
    }
    #endregion

    #region CellsGetters
    public static Cell GetRandomFloorCell(Maze maze)
    {
        static Cell GetRandomFloorCell(Maze maze, Cell floorCell)
        {
            var searchingQueue = new Queue<Cell>();
            var visitedCells = new HashSet<Cell>();

            searchingQueue.Enqueue(floorCell);
            visitedCells.Add(floorCell);

            while (searchingQueue.Count is not 0)
            {
                var currentCell = searchingQueue.Dequeue();

                if (maze.IsFloor(currentCell))
                {
                    return currentCell;
                }

                var adjacentCells = GetAdjacentCells(currentCell, maze, 1);

                AddCellInSearchingQueue(adjacentCells, visitedCells, searchingQueue);
            }

            throw new KeyNotFoundException($"cell with type: {nameof(TileType.Floor)} doesn't exist in {nameof(maze)}");
        }

        var skeleton = maze.Skeleton;
        var randomCell = new Cell(RandomHelper.Next(0, skeleton.GetLength(1)), RandomHelper.Next(0, skeleton.GetLength(0)));

        return GetRandomFloorCell(maze, randomCell);
    }

    private static (MazeTile[,] Template, int FloorsInserted) GetMazeTemplate(int width, int height)
    {
        var floorsInserted = 0;
        var tiles = new MazeTile[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y % 2 != 0 && x % 2 != 0
                 && y.InRange(0, height - 1) && x.InRange(0, width - 1))
                {
                    var floor = new Floor();

                    tiles[y, x] = floor;
                    floorsInserted++;
                }
                else
                {
                    var wall = new Wall();

                    tiles[y, x] = wall;
                }
            }
        }

        return (tiles, floorsInserted);
    }

    private static IEnumerable<Cell> GetAdjacentCells(Cell cell, Maze maze, int cellOffset)
    {
        return new List<Cell>()
        {
            cell with { X = cell.X + cellOffset },
            cell with { X = cell.X - cellOffset },
            cell with { Y = cell.Y + cellOffset },
            cell with { Y = cell.Y - cellOffset },
        }
        .Where(cell => cell.InBoundsOf(maze.Skeleton));
    }

    private static IEnumerable<Cell> GetAdjacentFloorCells(Cell cell, Maze maze, int cellOffset)
    {
        return GetAdjacentCells(cell, maze, cellOffset)
              .Where(cell => maze.IsFloor(cell));
    }

    private static IEnumerable<Cell> GetSideAdjacentCells(Cell cell, Maze maze)
    {
        var (width, height) = GetMazeDimensions(maze);

        if ((cell.X is 0 || cell.X == width - 1) && cell.Y.InRange(1, height - 1))
        {
            yield return cell with { Y = cell.Y - 1 };
            yield return cell with { Y = cell.Y + 1 };
        }
        else if ((cell.Y is 0 || cell.Y == height - 1) && cell.X.InRange(1, width - 1))
        {
            yield return cell with { X = cell.X - 1 };
            yield return cell with { X = cell.X + 1 };
        }
        else
        {
            throw new ArgumentException($"cell {cell} is not side cell of the {nameof(maze)}");
        }
    }

    private static Cell GetRandomSideCell(Maze maze)
    {
        static Cell GetRandomSideCell(Maze maze, Cell sideCell)
        {
            var (width, height) = GetMazeDimensions(maze);

            var searchingQueue = new Queue<Cell>();
            var visitedCells = new HashSet<Cell>();

            searchingQueue.Enqueue(sideCell);
            visitedCells.Add(sideCell);

            while (searchingQueue.Count is not 0)
            {
                var currentCell = searchingQueue.Dequeue();
                var adjacentCells = GetAdjacentCells(currentCell, maze, 1);

                if (adjacentCells.Where(cell => !maze.IsWall(cell)).Count() is not 0)
                {
                    return currentCell;
                }

                if (currentCell.IsCornerOf(maze.Skeleton))
                {
                    AddCellInSearchingQueue(adjacentCells, visitedCells, searchingQueue);
                }
                else
                {
                    var sideAdjacentCells = GetSideAdjacentCells(currentCell, maze);

                    AddCellInSearchingQueue(sideAdjacentCells, visitedCells, searchingQueue);
                }
            }

            throw new KeyNotFoundException($"unable to find appropriate cell in {nameof(maze)}");
        }

        var (width, height) = GetMazeDimensions(maze);

        int coordX, coordY;

        if (RandomHelper.RandomBoolean())
        {
            coordX = RandomHelper.Next(1, width);
            coordY = RandomHelper.RandomBoolean() ? 0 : height - 1;
        }
        else
        {
            coordY = RandomHelper.Next(1, height);
            coordX = RandomHelper.RandomBoolean() ? 0 : width - 1;
        }

        return GetRandomSideCell(maze, new Cell(coordX, coordY));
    }
    #endregion
}