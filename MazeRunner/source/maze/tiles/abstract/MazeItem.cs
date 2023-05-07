using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeItem : MazeTile
{
    public abstract ItemType ItemType { get; }

    public override TileType TileType
    {
        get
        {
            return TileType.Item;
        }
    }

    public abstract Rectangle GetHitBox(Vector2 position);
}
