using MazeRunner.Extensions;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract Vector2 Speed { get; }

    protected abstract ISpriteState State { get; set; }

    public virtual float DrawingPriority
    {
        get
        {
            return .5f;
        }
    }

    public virtual Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public virtual int FrameSize
    {
        get
        {
            return State.FrameSize;
        }
    }

    public virtual SpriteEffects FrameEffect { get; set; }

    public virtual Rectangle CurrentAnimationFrame
    {
        get
        {
            return State.CurrentAnimationFrame;
        }
    }

    public abstract FloatRectangle GetHitBox(Vector2 position);

    public virtual void Update(MazeRunnerGame game, GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }
}