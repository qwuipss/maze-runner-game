#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract ISpriteState State { get; set; }

    public abstract Vector2 Position { get; set; }

    public virtual Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public virtual int Width
    {
        get
        {
            return State.Width;
        }
    }

    public virtual int Height
    {
        get
        {
            return State.Height;
        }
    }

    public virtual Point GetCurrentAnimationPoint(GameTime gameTime)
    {
        return State.GetCurrentAnimationPoint(gameTime);
    }

    public virtual ISpriteState ProcessState()
    {
        return State.ProcessState();
    }
}