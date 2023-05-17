using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System.Collections.Immutable;
using System.Drawing;

namespace MazeRunner.Managers;

public static class CollisionManager
{
    public static bool CollidesWithWalls(Sprite sprite, Vector2 position, Vector2 movement, Maze maze)
    {
        var mazeSkeleton = maze.Skeleton;

        for (int y = 0; y < mazeSkeleton.GetLength(0); y++)
        {
            for (int x = 0; x < mazeSkeleton.GetLength(1); x++)
            {
                var tile = mazeSkeleton[y, x];

                if (tile.TileType is not TileType.Wall)
                {
                    continue;
                }

                if (CollidesWithMazeTile(sprite, position, movement, tile, new Cell(x, y)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool CollidesWithExit(Sprite sprite, Vector2 position, Vector2 movement, Maze maze)
    {
        var (exitCell, exit) = maze.ExitInfo;

        return CollidesWithMazeTile(sprite, position, movement, exit, exitCell)
           && !exit.IsOpened;
    }

    public static bool CollidesWithItems(Sprite sprite, Vector2 position, Maze maze, out (Cell Cell, MazeItem Item) itemInfo)
    {
        if (CollidesWith(maze.ItemsInfo, sprite, position, out var tileInfo))
        {
            itemInfo = (tileInfo.Cell, (MazeItem)tileInfo.Tile);
            return true;
        }

        itemInfo = (new Cell(), null);
        return false;
    }

    public static bool CollidesWithTraps(Sprite sprite, Vector2 position, Maze maze, out (Cell Cell, MazeTrap Trap) trapInfo)
    {
        if (CollidesWith(maze.TrapsInfo, sprite, position, out var tileInfo) && ((MazeTrap)tileInfo.Tile).IsActivated)
        {
            trapInfo = (tileInfo.Cell, (MazeTrap)tileInfo.Tile);
            return true;
        }

        trapInfo = (new Cell(), null);
        return false;
    }

    private static bool CollidesWith(ImmutableDictionary<Cell, MazeTile> sourceInfo, Sprite sprite, Vector2 position, out (Cell Cell, MazeTile Tile) tileInfo)
    {
        foreach (var (cell, tile) in sourceInfo)
        {
            if (CollidesWithMazeTile(sprite, position, tile, cell))
            {
                tileInfo = (cell, tile);

                return true;
            }
        }

        tileInfo = (new Cell(), null);
        return false;
    }

    private static bool CollidesWithMazeTile(Sprite sprite, Vector2 position, MazeTile mazeTile, Cell tileCell)
    {
        return CollidesWithMazeTile(sprite, position, Vector2.Zero, mazeTile, tileCell);
    }

    private static bool CollidesWithMazeTile(Sprite sprite, Vector2 position, Vector2 movement, MazeTile mazeTile, Cell tileCell)
    {
        var tilePosition = Maze.GetIndependentCellPosition(mazeTile, tileCell);
        var tileHitBox = mazeTile.GetHitBox(tilePosition);

        return GetExtendedHitBox(sprite, position, movement).IntersectsWith(tileHitBox);
    }

    private static RectangleF GetExtendedHitBox(Sprite sprite, Vector2 position, Vector2 movement)
    {
        var hitBox = sprite.GetHitBox(position);

        if (movement == Vector2.Zero)
        {
            goto _return;
        }

        if (movement.X > 0)
        {
            hitBox.Width += movement.X;
        }
        else if (movement.X < 0)
        {
            hitBox.X += movement.X;
        }

        if (movement.Y > 0)
        {
            hitBox.Height += movement.Y;
        }
        else if (movement.Y < 0)
        {
            hitBox.Y += movement.Y;
        }

    _return:
        return hitBox;
    }
}