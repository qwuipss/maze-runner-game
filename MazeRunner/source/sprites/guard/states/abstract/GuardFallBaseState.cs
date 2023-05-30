using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class GuardFallBaseState : GuardBaseState
{
    protected GuardFallBaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Fall;

    public override int FramesCount => 4;
}
