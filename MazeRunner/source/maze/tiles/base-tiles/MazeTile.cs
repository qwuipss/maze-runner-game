#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public abstract class MazeTile
{
    public abstract Texture2D Texture { get; }

    public abstract TileType TileType { get; }

    public virtual int FrameWidth
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public virtual int FrameHeight
    {
        get
        {
            return Texture.Height;
        }
    }

    public virtual Point GetCurrentAnimationFrame(GameTime gameTime)
    {
        return Point.Zero;
    }

    protected virtual int FramesCount
    {
        get
        {
            return 1;
        }
    }
}
