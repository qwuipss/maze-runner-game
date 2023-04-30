#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites.States;

public interface ISpriteState
{
    public Texture2D Texture { get; }

    public int FramesCount { get; }

    public int Width { get; }

    public int Height { get; }

    public int AnimationDelayMs { get; }

    public Point CurrentAnimationPoint { get; }

    public ISpriteState ProcessState();
}
