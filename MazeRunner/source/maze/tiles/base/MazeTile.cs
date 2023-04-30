#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTile
{
    public abstract Texture2D Texture { get; }

    public abstract TileType TileType { get; }

    public virtual int FramesCount
    {
        get
        {
            return 1;
        }
    }

    public virtual int Width
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public virtual int Height
    {
        get
        {
            return Texture.Height;
        }
    }

    public virtual Point GetCurrentAnimationPoint(GameTime gameTime)
    {
        return Point.Zero;
    }
}
