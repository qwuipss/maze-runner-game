#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private Vector2 _speed;

    public override ISpriteState State { get; set; }

    public override Vector2 Position { get; set; }

    public override Vector2 Speed
    {
        get
        {
            return _speed;
        }
    }

    public Hero()
    {
        _speed = new Vector2(3, 3);

        State = new HeroIdleState();
    }
}