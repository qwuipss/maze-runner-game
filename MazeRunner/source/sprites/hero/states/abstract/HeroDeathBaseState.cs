using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class HeroDeathBaseState : HeroBaseState
{
    public override Texture2D Texture => Textures.Sprites.Hero.Dead;

    public override int FramesCount => 5;

    protected HeroDeathBaseState(ISpriteState previousState) : base(previousState)
    {
    }
}