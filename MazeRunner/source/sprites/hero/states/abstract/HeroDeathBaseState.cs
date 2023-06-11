using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class HeroDeathBaseState : HeroBaseState
{
    public override Texture2D Texture => Textures.Sprites.Hero.Dead;

    public override int FramesCount => 5;

    protected HeroDeathBaseState(ISpriteState previousState, Hero hero, Maze maze) : base(previousState, hero, maze)
    {
        Hero.Health = 0;
    }
}