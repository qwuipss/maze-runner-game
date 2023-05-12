﻿using MazeRunner.Helpers;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapActivatedState : BayonetTrapBaseState
{
    private readonly int _updateTimeDelayMs;

    protected override int UpdateTimeDelayMs
    {
        get
        {
            return _updateTimeDelayMs;
        }
    }

    public BayonetTrapActivatedState()
    {
        var minUpdateTimeMs = 1000;
        var maxUpdateTimeMs = 3000;

        _updateTimeDelayMs = RandomHelper.Next(minUpdateTimeMs, maxUpdateTimeMs);

        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            return new BayonetTrapDeactivatingState();
        }

        return this;
    }
}