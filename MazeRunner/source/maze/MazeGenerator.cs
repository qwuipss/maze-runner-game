#region Usings
using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
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

        return maze;
    }

    public static void InsertTraps(Maze maze, Func<MazeTrap> trapSource, int percentage)
    {
        var insertionsCount = maze.GetFloorsCount() * percentage / 100;

        for (int i = 0; i < insertionsCount; i++)
        {
            var cell = GetRandomFloorCell(maze);

            maze.InsertTrap(trapSource.Invoke(), cell);
        }
    }

    public static void InsertExit(Maze maze)
    {
        var sideCell = GetRandomSideCell(maze);
        var rotation = GetExitFrameRotationAngle(sideCell, maze);

        var exit = new Exit(rotation);

        exit.OriginFrameRotationVector = GetExitOriginFrameRotationVector(exit);

        maze.InsertExit(exit, sideCell);
    }

    public static void InsertItem(Maze maze, MazeItem item)
    {
        maze.InsertItem(item, GetRandomFloorCell(maze));
    }

    public static Cell GetRandomFloorCell(Maze maze)
    {
        var cell = new Cell(RandomHelper.Next(0, maze.Skeleton.GetLength(1)), RandomHelper.Next(0, maze.Skeleton.GetLength(0)));

        return GetRandomFloorCell(maze, cell);
    }

    private static (int Width, int Height) GetMazeDimension(Maze maze)
    {
        var skeleton = maze.Skeleton;

        return (skeleton.GetLength(1), skeleton.GetLength(0));
    }

    private static Cell GetRandomSideCell(Maze maze)
    {
        var (width, height) = GetMazeDimension(maze);

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

    private static Cell GetRandomSideCell(Maze maze, Cell sideCell)
    {
        var (width, height) = GetMazeDimension(maze);

        var searchingQueue = new Queue<Cell>();
        var visitedCells = new HashSet<Cell>();

        searchingQueue.Enqueue(sideCell);
        visitedCells.Add(sideCell);

        while (searchingQueue.Count is not 0)
        {
            var currentCell = searchingQueue.Dequeue();
            var adjCells = GetAdjacentCells(currentCell, maze, 1);

            if (adjCells.Where(cell => !maze.IsWall(cell)).Count() is not 0)
            {
                return currentCell;
            }

            if (currentCell.IsCornerOf(maze.Skeleton))
            {
                AddCellInSearchingQueue(adjCells, visitedCells, searchingQueue);
            }
            else
            {
                AddCellInSearchingQueue(GetSideAdjacentCells(currentCell, maze), visitedCells, searchingQueue);
            }
        }

        throw new KeyNotFoundException($"unable to find appropriate cell in {nameof(maze)}");
    }

    private static IEnumerable<Cell> GetSideAdjacentCells(Cell cell, Maze maze)
    {
        var (width, height) = GetMazeDimension(maze);

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
            throw new NotImplementedException();
        }
    }

    private static Vector2 GetExitOriginFrameRotationVector(Exit exit)
    {
        if (exit.FrameRotationAngle is (float)Math.PI / 2)
        {
            return new Vector2(0, exit.FrameHeight);
        }

        if (exit.FrameRotationAngle is (float)-Math.PI / 2)
        {
            return new Vector2(exit.FrameWidth, 0);
        }

        return Vector2.Zero;
    }

    private static float GetExitFrameRotationAngle(Cell exitCell, Maze maze)
    {
        if (exitCell.X is 0)
        {
            return (float)-Math.PI / 2;
        }

        if (exitCell.X == maze.Skeleton.GetLength(1) - 1)
        {
            return (float)Math.PI / 2;
        }

        return 0;
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

    private static Cell GetRandomFloorCell(Maze maze, Cell cell)
    {
        var searchingQueue = new Queue<Cell>();
        var visitedCells = new HashSet<Cell>();

        searchingQueue.Enqueue(cell);
        visitedCells.Add(cell);

        while (searchingQueue.Count is not 0)
        {
            var currentCell = searchingQueue.Dequeue();

            if (maze.IsFloor(currentCell))
            {
                return currentCell;
            }

            AddCellInSearchingQueue(GetAdjacentCells(currentCell, maze, 1), visitedCells, searchingQueue);
        }

        throw new KeyNotFoundException($"cell with type: {nameof(TileType.Floor)} doesn't exist in {nameof(maze)}");
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
}