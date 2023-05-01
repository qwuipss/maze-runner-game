#region Usings
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Physics;

public static class CollisionManager
{
    public static bool ColidesWithWalls(Hero hero, Maze maze, Vector2 movement)
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

                if (GetExtendedHeroHitBox(hero, movement).Intersects(GetMazeTileHitBox(tile, x, y)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static Rectangle GetMazeTileHitBox(MazeTile mazeTile, int x, int y)
    {
        return new Rectangle(x * mazeTile.FrameWidth, y * mazeTile.FrameHeight,
                             mazeTile.FrameWidth, mazeTile.FrameHeight);
    }

    private static Rectangle GetExtendedHeroHitBox(Hero hero, Vector2 movement)
    {
        var hitBox = hero.HitBox;

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
