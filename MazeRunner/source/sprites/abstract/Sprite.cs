using MazeRunner.Extensions;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract Vector2 Speed { get; }

    public abstract ISpriteState State { get; set; }

    public virtual bool IsDead { get; set; }

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

    public virtual SpriteEffects FrameEffect
    {
        get
        {
            return State.FrameEffect;
        }
    }

    public virtual Rectangle CurrentAnimationFrame
    {
        get
        {
            return State.CurrentAnimationFrame;
        }
    }

    public abstract FloatRectangle GetHitBox(Vector2 position);

    public virtual Vector2 GetTravelledDistance(Vector2 direction, GameTime gameTime)
    {
        return direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Update(MazeRunnerGame game, GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }
}