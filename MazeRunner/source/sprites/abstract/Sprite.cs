using MazeRunner.Components;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites;

public abstract class Sprite : MazeRunnerGameComponent
{
    public abstract Vector2 Speed { get; }

    protected abstract ISpriteState State { get; set; }

    public virtual float DrawingPriority
    {
        get
        {
            return 0;
        }
    }

    public virtual Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public virtual int FrameWidth
    {
        get
        {
            return State.FrameWidth;
        }
    }

    public virtual int FrameHeight
    {
        get
        {
            return State.FrameHeight;
        }
    }

    public virtual SpriteEffects FrameEffect { get; set; }

    public virtual Point CurrentAnimationFramePoint
    {
        get
        {
            return State.CurrentAnimationFramePoint;
        }
    }

    public abstract Rectangle GetHitBox(Vector2 position);

    public virtual ISpriteState ProcessState(GameTime gameTime)
    {
        return State.ProcessState(gameTime);
    }
}