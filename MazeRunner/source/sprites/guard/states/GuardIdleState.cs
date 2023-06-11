﻿using MazeRunner.Content;
using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class GuardIdleState : GuardBaseState
{
    private readonly ISpriteState _previousState;

    private bool IsAttackOnCooldown => _previousState is GuardAttackState;

    public override Texture2D Texture => Textures.Sprites.Guard.Idle;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 650;

    public GuardIdleState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
        _previousState = previousState;
    }

    public GuardIdleState(Hero hero, Guard guard, Maze maze) : this(null, hero, guard, maze)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(Guard, Maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (CollidesWithTraps(Guard, Maze, false, out var _))
        {
            return new GuardWalkState(this, Hero, Guard, Maze);
        }

        if (IsHeroNearby(out var _))
        {
            return new GuardChaseState(this, Hero, Guard, Maze, IsAttackOnCooldown);
        }

        var elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

        ElapsedGameTimeMs += elapsedTime;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            if (framePosX is 0 && animationPoint.X == (FramesCount - 1) * FrameSize && RandomHelper.RandomBoolean())
            {
                return new GuardWalkState(this, Hero, Guard, Maze);
            }

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
