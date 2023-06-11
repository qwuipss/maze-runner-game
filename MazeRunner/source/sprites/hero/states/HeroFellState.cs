using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Sprites.States;

public class HeroFellState : HeroFallBaseState
{
    public static event Action HeroFellNotify;

    public override double UpdateTimeDelayMs => double.MaxValue;

    public HeroFellState(ISpriteState previousState, Hero hero, Maze maze, bool needNotifyingAboutFall) : base(previousState, hero, maze)
    {
        if (needNotifyingAboutFall)
        {
            HeroFellNotify.Invoke();
        }

        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
