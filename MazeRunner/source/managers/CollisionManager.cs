using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

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

    public static bool CollidesWithItems(Sprite sprite, Vector2 position, Maze maze, out (Cell ItemCell, MazeItem Item) itemInfo)
    {
        foreach (var (itemCell, item) in maze.ItemsInfo)
        {
            if (CollidesWithMazeTile(sprite, position, Vector2.Zero, item, itemCell))
            {
                itemInfo = (itemCell, item);

                return true;
            }
        }

        itemInfo = (new Cell(), null);
        return false;
    }

    public static bool CollidesWithKey(Sprite sprite, Vector2 position, Cell keyCell, Key key)
    {
        if (CollidesWithMazeTile(sprite, position, Vector2.Zero, key, keyCell))
        {
            return true;
        }

        return false;
    }

    private static bool CollidesWithMazeTile(Sprite sprite, Vector2 position, Vector2 movement, MazeTile tile, Cell tileCell)
    {
        var tilePosition = Maze.GetIndependentCellPosition(tile, tileCell);
        var tileHitBox = tile.GetHitBox(tilePosition);

        return GetExtendedHitBox(sprite, position, movement).Intersects(tileHitBox);
    }

    private static Rectangle GetExtendedHitBox(Sprite sprite, Vector2 position, Vector2 movement)
    {
        var hitBox = sprite.GetHitBox(position);

        var x = hitBox.X;
        var y = hitBox.Y;
        var width = hitBox.Width;
        var height = hitBox.Height;

        if (movement == Vector2.Zero)
        {
            goto _return;
        }
        
        if (movement.X > 0)
        {
            width += (int)movement.X;
        }
        else if (movement.X < 0)
        {
            x += (int)movement.X;
        }

        if (movement.Y > 0)
        {
            height += (int)movement.Y;
        }
        else if (movement.Y < 0)
        {
            y += (int)movement.Y;
        }

    _return:
        return new Rectangle(x, y, width, height);
    }
}