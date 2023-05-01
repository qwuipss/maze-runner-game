#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private Vector2 _position;

    private const int HitBoxWidth = 7;
    private const int HitBoxHeight = 9;

    public override ISpriteState State { get; set; }

    public override Vector2 Position
    {
        get
        {
            return _position;
        }
        set
        {
            if (value != Vector2.Zero)
            {
                var movement = value - _position;

                ProcessState(movement);
                ProcessFrameEffect(movement);

                _position = value;
            }
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
                (int)_position.X + (FrameWidth - HitBoxWidth), 
                (int)_position.Y + (FrameHeight - HitBoxHeight), 
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