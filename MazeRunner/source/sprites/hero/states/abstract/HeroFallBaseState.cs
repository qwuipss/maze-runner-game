using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class HeroFallBaseState : HeroBaseState
{
    protected HeroFallBaseState(ISpriteState previousState) : base(previousState)
    {
    }

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
