using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using RectangleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.Sprites;

public abstract class Sprite : MazeRunnerGameComponent
{
    public abstract Vector2 Speed { get; }

    public abstract bool IsDead { get; }

    public virtual float DrawingPriority => .45f;

    public Texture2D Texture => State.Texture;

    public int FrameSize => State.FrameSize;

    public RectangleXna CurrentAnimationFrame => State.CurrentAnimationFrame;

    public SpriteEffects FrameEffect => State.FrameEffect;

    public ISpriteState State { get; set; }

    public Vector2 Position { get; set; }

    public abstract RectangleF GetHitBox(Vector2 position);

    public Vector2 GetMovement(Vector2 direction, GameTime gameTime)
    {
        return direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawSprite(this);
    }
}