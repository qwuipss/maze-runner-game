using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Sprites.States;

public class HeroDiedState : HeroDeathBaseState
{
    public static event Action HeroDiedNotify;

    public override double UpdateTimeDelayMs => double.MaxValue;

    public HeroDiedState(ISpriteState previousState, Hero hero, Maze maze) : base(previousState, hero, maze)
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(Hero, Maze, true, out var trapType))
        {
            if (trapType is TrapType.Drop)
            {
                return new HeroFallingState(this, Hero, Maze);
            }
        }

        return this;
    }
}