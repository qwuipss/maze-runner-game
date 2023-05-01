#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites.States;

public interface ISpriteState
{
    public Texture2D Texture { get; }

    public int FramesCount { get; }

    public int FrameWidth { get; }

    public int FrameHeight { get; }

    public SpriteEffects FrameEffect { get; set; }

    public int FrameAnimationDelayMs { get; }

    public Point CurrentAnimationFramePoint { get; }

    public ISpriteState ProcessState();
}
