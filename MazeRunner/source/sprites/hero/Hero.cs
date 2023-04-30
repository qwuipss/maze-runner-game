#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Sprites.Heroes;

public class Hero : Sprite
{
    public override ISpriteState State { get; set; }

    public override Vector2 Position { get; set; }

    public Hero()
    {
        State = new IdleState();
    }
}