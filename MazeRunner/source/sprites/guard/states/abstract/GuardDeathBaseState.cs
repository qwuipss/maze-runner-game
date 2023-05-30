using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class GuardDeathBaseState : GuardBaseState
{
    protected GuardDeathBaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Dead;

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }
}
