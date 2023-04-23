#region Usings
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public class WallTile : MazeTile
{
    private static readonly Texture2D _texture = TilesTextures.Wall;

    public override Texture2D Texture
    {
        get
        {
            return _texture;
        }
    }

    public override CellType CellType
    {
        get
        {
            return CellType.Wall;
        }
    }
}
