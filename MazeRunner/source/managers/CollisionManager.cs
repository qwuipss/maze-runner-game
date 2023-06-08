using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeRunner.Managers;

public static class CollisionManager
{
    public static bool CollidesWithWalls(Sprite sprite, Vector2 position, Vector2 movement, Maze maze)
    {
        var skeleton = maze.Skeleton;
        var collideArea = maze.GetCollideArea();

        for (int y = collideArea.Top; y <= collideArea.Bottom; y++)
        {
            for (int x = collideArea.Left; x <= collideArea.Right; x++)
            {
                var tile = skeleton[y, x];

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
        if (CollidesWith(maze.HoverTilesInfo, sprite, position, maze, mazeTile => mazeTile.TileType is TileType.Item, out var tileInfo))
        {
            itemInfo = (tileInfo.Cell, (MazeItem)tileInfo.Tile);

            return true;
        }

        itemInfo = (new Cell(), null);

        return false;
    }

    public static bool CollidesWithTraps(Sprite sprite, Vector2 position, Maze maze, bool needActivating, out (Cell Cell, MazeTrap Trap) trapInfo)
    {
        if (CollidesWith(maze.HoverTilesInfo, sprite, position, maze, mazeTile => mazeTile.TileType is TileType.Trap, out var tileInfo))
        {
            if (((MazeTrap)tileInfo.Tile).IsActivated == needActivating)
            {
                trapInfo = (tileInfo.Cell, (MazeTrap)tileInfo.Tile);

                return true;
            }
        }

        trapInfo = (new Cell(), null);

        return false;
    }

    private static bool CollidesWith(
        IDictionary<Cell, MazeTile> sourceInfo, Sprite sprite, Vector2 position, Maze maze, Func<MazeTile, bool> selector, out (Cell Cell, MazeTile Tile) tileInfo)
    {
        var collideArea = maze.GetCollideArea();

        for (int y = collideArea.Top; y <= collideArea.Bottom; y++)
        {
            for (int x = collideArea.Left; x <= collideArea.Right; x++)
            {
                var cell = new Cell(x, y);

                if (sourceInfo.TryGetValue(cell, out var tile))
                {
                    if (selector.Invoke(tile) && CollidesWithMazeTile(sprite, position, tile, cell))
                    {
                        tileInfo = (cell, tile);

                        return true;
                    }
                }
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
        var tilePosition = Maze.GetCellPosition(tileCell);
        var tileHitBox = mazeTile.GetHitBox(tilePosition);

        var spriteExtendedHitBox = GetExtendedHitBox(sprite, position, movement);

        return spriteExtendedHitBox.IntersectsWith(tileHitBox);
    }

    private static RectangleF GetExtendedHitBox(Sprite sprite, Vector2 position, Vector2 movement)
    {
        var hitBox = sprite.GetHitBox(position);

        if (movement == Vector2.Zero)
        {
            return hitBox;
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

        return hitBox;
    }
}