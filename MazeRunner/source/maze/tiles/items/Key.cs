using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.MazeBase.Tiles;

public sealed class Key : MazeItem
{
    private const float HitBoxOffset = 4;
    private const float HitBoxSize = 8;

    public Key()
    {
        State = new KeyIdleState();
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Item;
        }
    }

    public override ItemType ItemType
    {
        get
        {
            return ItemType.Key;
        }
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSize, HitBoxSize);
    }

    public override void ProcessCollecting(Maze maze, Cell cell)
    {
        base.ProcessCollecting(maze, cell);

        maze.IsKeyCollected = true;
    }
}
