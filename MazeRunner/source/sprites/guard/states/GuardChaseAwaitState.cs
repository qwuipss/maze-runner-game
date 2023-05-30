﻿using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class GuardChaseAwaitState : GuardMoveBaseState
{
    public override Texture2D Texture => Textures.Sprites.Guard.Idle;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => double.MaxValue;

    public GuardChaseAwaitState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
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

        if (!PathToHeroExist(Hero, Guard, Maze, out var pathToHero))
        {
            return new GuardIdleState(this, Hero, Guard, Maze);
        }

        var direction = GetMovementDirection(pathToHero);

        if (ProcessMovement(direction, gameTime))
        {
            return new GuardChaseState(this, Hero, Guard, Maze);
        }

        return this;
    }
}