using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using RectangleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract Vector2 Speed { get; }

    public virtual float DrawingPriority
    {
        get
        {
            return .45f;
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

    public virtual RectangleXna CurrentAnimationFrame
    {
        get
        {
            return State.CurrentAnimationFrame;
        }
    }

    public virtual ISpriteState State { get; set; }

    public virtual bool IsDead { get; protected set; }

    public abstract RectangleF GetHitBox(Vector2 position);

    public virtual Vector2 GetMovement(Vector2 direction, GameTime gameTime)
    {
        return direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Update(MazeRunnerGame game, GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }
}