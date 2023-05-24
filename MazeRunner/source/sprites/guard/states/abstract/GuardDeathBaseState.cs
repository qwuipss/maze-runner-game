using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class GuardDeathBaseState : GuardBaseState
{
    public GuardDeathBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Guard.Dead;
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
