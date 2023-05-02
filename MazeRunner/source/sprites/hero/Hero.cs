#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private Vector2 _position;

    private const int HitBoxOffsetX = 3;
    private const int HitBoxOffsetY = 4;

    private const int HitBoxWidth = 9;
    private const int HitBoxHeight = 11;

    public override ISpriteState State { get; set; }

    public override Vector2 Position
    {
        get
        {
            return _position;
        }
        set
        {
            var movement = value - _position;

            ProcessState(movement);
            ProcessFrameEffect(movement);

            _position = value;
        }
    }

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(3, 3);
        }
    }

    public override Rectangle HitBox
    {
        get
        {
            return new Rectangle(
                (int)_position.X + HitBoxOffsetX,
                (int)_position.Y + HitBoxOffsetY,
                HitBoxWidth,
                HitBoxHeight);
        }
    }

    public Hero(Vector2 position)
    {
        _position = position;

        State = new HeroIdleState();
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