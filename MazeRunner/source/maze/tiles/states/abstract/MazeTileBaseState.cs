﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class MazeTileBaseState : IMazeTileState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    protected abstract int UpdateTimeDelayMs { get; }

    public virtual int FrameSize
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public Rectangle CurrentAnimationFrame
    {
        get
        {
            return new Rectangle(CurrentAnimationFramePoint, new Point(FrameSize, FrameSize));
        }
    }

    protected virtual Point CurrentAnimationFramePoint { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public abstract IMazeTileState ProcessState(GameTime gameTime);
}