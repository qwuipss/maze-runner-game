#region Usings
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    public override ISpriteState State { get; set; }

    public override Vector2 Position { get; set; }

    public Hero()
    {
        State = new IdleState();
    }
}