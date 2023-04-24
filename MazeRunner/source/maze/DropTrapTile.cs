using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner;

public class DropTrapTile : MazeTile
{
    private static readonly Texture2D _texture = TilesTextures.DropTrap;

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
            return TileType.DropTrap;
        }
    }
}
