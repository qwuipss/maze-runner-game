using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles;

public sealed class Key : MazeItem
{
    private const int HitBoxOffsetX = 4;
    private const int HitBoxOffsetY = 4;

    private const int HitBoxWidth = 8;
    private const int HitBoxHeight = 8;

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

    public override Rectangle GetHitBox(Vector2 position)
    {
        return new Rectangle(
            (int)position.X + HitBoxOffsetX,
            (int)position.Y + HitBoxOffsetY,
            HitBoxWidth,
            HitBoxHeight);
    }
}
