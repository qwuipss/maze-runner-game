using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles;

public sealed class Key : MazeItem
{
    private const int HitBoxOffset = 4;
    private const int HitBoxSize = 8;


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

    public override FloatRectangle GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSize, HitBoxSize);
    }

    public override void ProcessCollecting(MazeInfo mazeInfo, Cell cell)
    {
        base.ProcessCollecting(mazeInfo, cell);

        mazeInfo.IsKeyCollected = true;
    }
}
