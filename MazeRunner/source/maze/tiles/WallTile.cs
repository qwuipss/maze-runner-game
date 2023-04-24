#region Usings
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public class WallTile : MazeTile
{
    public override Texture2D Texture
    {
        get
        {
            return TilesTextures.Wall;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Wall;
        }
    }
}
