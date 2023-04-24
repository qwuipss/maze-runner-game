#region Usings
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public class FloorTile : MazeTile
{
    private static readonly Texture2D _texture = TilesTextures.Floor;

    public override Texture2D Texture
    {
        get
        {
            return _texture;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Floor;
        }
    }
}
