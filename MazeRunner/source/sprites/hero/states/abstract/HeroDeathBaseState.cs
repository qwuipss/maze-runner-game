using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class HeroDeathBaseState : HeroBaseState
{
    protected HeroDeathBaseState(ISpriteState previousState) : base(previousState)
    {
    }

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
