#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private const int HitBoxOffsetX = 3; //
    private const int HitBoxOffsetY = 4; //

    private const int HitBoxWidth = 9;
    private const int HitBoxHeight = 11;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(3, 3);
        }
    }

    protected override ISpriteState State { get; set; }

    public override Rectangle GetHitBox(Vector2 position)
    {
        return new Rectangle(
            (int)position.X + HitBoxOffsetX,
            (int)position.Y + HitBoxOffsetY,
            HitBoxWidth,
            HitBoxHeight);
    }

    public Hero()
    {
        State = new HeroIdleState();
    }

    public void ProcessPositionChange(Vector2 movement)
    {
        ProcessState(movement);
        ProcessFrameEffect(movement);
    }

    private void ProcessState(Vector2 movement)
    {
        if (movement == Vector2.Zero)
        {
            if (State is not HeroIdleState)
            {
                State = new HeroIdleState();
            }
        }
        else
        {
            if (State is not HeroRunState)
            {
                State = new HeroRunState();
            }
        }
    }

    private void ProcessFrameEffect(Vector2 movement)
    {
        if (movement.X > 0)
        {
            FrameEffect = SpriteEffects.None;
        }
        else if (movement.X < 0)
        {
            FrameEffect = SpriteEffects.FlipHorizontally;
        }
    }
}