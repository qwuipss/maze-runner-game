#region Usings
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Physics;

public static class CollisionManager
{
    public static bool ColidesWithWalls(Hero hero, Maze maze, Vector2 position, Vector2 movement)
    {
        var mazeSkeleton = maze.Skeleton;

        for (int y = 0; y < mazeSkeleton.GetLength(0); y++)
        {
            for (int x = 0; x < mazeSkeleton.GetLength(1); x++)
            {
                var tile = mazeSkeleton[y, x];

                if (tile.TileType is TileType.Floor)
                {
                    continue;
                }

                if (CollidesWithMazeTile(hero, position, movement, tile, x, y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool CollidesWithExit(Hero hero, Maze maze, Vector2 position, Vector2 movement)
    {
        var (coords, exit) = maze.ExitInfo;

        return CollidesWithMazeTile(hero, position, movement, exit, coords.X, coords.Y)
           && !exit.IsOpened;
    }

    public static bool CollidesWithItems(Hero hero, Maze maze, Vector2 position, out (Cell Coords, MazeItem Item) itemInfo)
    {
        foreach (var (coords, item) in maze.Items)
        {
            itemInfo = (coords, item);

            if (CollidesWithMazeTile(hero, position, Vector2.Zero, item, coords.X, coords.Y))
            {
                return true;
            }
        }

        itemInfo = (new Cell(), null);
        return false;
    }

    public static bool CollidesWithKey(Hero hero, Vector2 position, Cell itemCoords, Key key)
    {
        if (CollidesWithMazeTile(hero, position, Vector2.Zero, key, itemCoords.X, itemCoords.Y))
        {
            return true;
        }

        return false;
    }

    private static bool CollidesWithMazeTile(Hero hero, Vector2 heroPosition, Vector2 movement, MazeTile tile, int x, int y)
    {
        return GetExtendedHitBox(hero, heroPosition, movement).Intersects(GetHitBox(tile, x, y));
    }

    private static Rectangle GetHitBox(MazeTile mazeTile, int x, int y)
    {
        return new Rectangle(x * mazeTile.FrameWidth, y * mazeTile.FrameHeight,
                             mazeTile.FrameWidth, mazeTile.FrameHeight);
    }

    private static Rectangle GetExtendedHitBox(Hero hero, Vector2 position, Vector2 movement)
    {
        var hitBox = hero.GetHitBox(position);

        var x = hitBox.X;
        var y = hitBox.Y;
        var width = hitBox.Width;
        var height = hitBox.Height;

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

        return new Rectangle(x, y, width, height);
    }
}
