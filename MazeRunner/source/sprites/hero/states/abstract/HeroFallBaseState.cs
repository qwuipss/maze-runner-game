using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class HeroFallBaseState : HeroBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Hero.Fall;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }
}
