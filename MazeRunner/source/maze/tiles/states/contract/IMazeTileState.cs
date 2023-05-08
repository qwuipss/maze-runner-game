using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public interface IMazeTileState
{
    public Texture2D Texture { get; }

    public int FramesCount { get; }

    public int FrameWidth { get; }

    public int FrameHeight { get; }

    public Point CurrentAnimationFramePoint { get; }

    public int FrameAnimationDelayMs { get; }

    public IMazeTileState ProcessState(GameTime gameTime);
}