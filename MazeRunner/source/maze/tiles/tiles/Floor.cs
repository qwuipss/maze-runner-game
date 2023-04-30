#region Usings
using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class Floor : MazeTile
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.Floor;
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
