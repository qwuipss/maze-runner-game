#region Usings
using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class Wall : MazeTile
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.Wall;
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
