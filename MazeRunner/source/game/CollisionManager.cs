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

                if (HeroCollidesWithMazeTile(hero, position, movement, tile, x, y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool CollidesWithExit(Hero hero, Maze maze, Vector2 position, Vector2 movement)
    {
        var exitInfo = maze.ExitInfo;

        var exit = exitInfo.Exit;
        var coords = exitInfo.Coords;

        return HeroCollidesWithMazeTile(hero, position, movement, exit, coords.X, coords.Y)
           && !exit.IsOpened;
    }

    private static bool HeroCollidesWithMazeTile(Hero hero, Vector2 heroPosition, Vector2 movement, MazeTile tile, int x, int y)
    {
        return GetExtendedHeroHitBox(hero, heroPosition, movement).Intersects(GetMazeTileHitBox(tile, x, y));
    }

    private static Rectangle GetMazeTileHitBox(MazeTile mazeTile, int x, int y)
    {
        return new Rectangle(x * mazeTile.FrameWidth, y * mazeTile.FrameHeight,
                             mazeTile.FrameWidth, mazeTile.FrameHeight);
    }

    private static Rectangle GetExtendedHeroHitBox(Hero hero, Vector2 position, Vector2 movement)
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
