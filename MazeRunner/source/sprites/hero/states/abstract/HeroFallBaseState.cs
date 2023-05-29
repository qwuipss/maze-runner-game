using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class HeroFallBaseState : HeroBaseState
{
    public override Texture2D Texture => Textures.Sprites.Hero.Fall;

    public override int FramesCount => 4;

    protected HeroFallBaseState(ISpriteState previousState) : base(previousState)
    {
    }
}
