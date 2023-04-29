#region Usings
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public class FloorTile : MazeTile
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.Floor;
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
