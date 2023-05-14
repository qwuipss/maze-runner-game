using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class HeroDeathBaseState : HeroBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Hero.Dead;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 5;
        }
    }
}
