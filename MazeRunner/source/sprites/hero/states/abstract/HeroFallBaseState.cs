using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Sprites.States;

public abstract class HeroFallBaseState : HeroBaseState
{
    public override Texture2D Texture => Textures.Sprites.Hero.Fall;

    public override int FramesCount => 4;

    protected HeroFallBaseState(ISpriteState previousState, Hero hero, Maze maze) : base(previousState, hero, maze)
    {
        Hero.Health = 0;
    }
}
