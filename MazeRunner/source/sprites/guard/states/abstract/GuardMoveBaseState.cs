using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class GuardMoveBaseState : GuardBaseState
{
    protected GuardMoveBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Guard.Run;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return 150;
        }
    }
}
