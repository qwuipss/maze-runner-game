using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using RectangleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.Sprites;

public abstract class Sprite
{
    public abstract Vector2 Speed { get; }

    public abstract bool IsDead { get; }

    public virtual float DrawingPriority => .45f;

    public virtual Texture2D Texture => State.Texture;

    public virtual int FrameSize => State.FrameSize;

    public virtual SpriteEffects FrameEffect => State.FrameEffect;

    public virtual RectangleXna CurrentAnimationFrame => State.CurrentAnimationFrame;

    public ISpriteState State { get; set; }

    public abstract RectangleF GetHitBox(Vector2 position);

    public Vector2 GetMovement(Vector2 direction, GameTime gameTime)
    {
        return direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }
}