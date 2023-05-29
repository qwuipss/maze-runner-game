using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class GuardFallBaseState : GuardBaseState
{
    protected GuardFallBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Fall;

    public override int FramesCount => 4;
}
