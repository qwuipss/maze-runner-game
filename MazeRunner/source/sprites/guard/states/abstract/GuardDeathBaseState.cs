using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class GuardDeathBaseState : GuardBaseState
{
    public override Texture2D Texture => Textures.Sprites.Guard.Dead;

    public override int FramesCount => 4;

    protected GuardDeathBaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
    }
}
