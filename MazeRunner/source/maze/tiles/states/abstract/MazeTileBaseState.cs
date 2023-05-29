using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class MazeTileBaseState : IMazeTileState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    protected abstract double UpdateTimeDelayMs { get; }

    public int FrameSize => Texture.Width / FramesCount;

    public Rectangle CurrentAnimationFrame => new Rectangle(CurrentAnimationFramePoint, new Point(FrameSize, FrameSize));

    protected Point CurrentAnimationFramePoint { get; set; }

    protected double ElapsedGameTimeMs { get; set; }

    public abstract IMazeTileState ProcessState(GameTime gameTime);
}