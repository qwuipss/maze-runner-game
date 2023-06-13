using MazeRunner.GameBase;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MazeRunner.Sprites.States;

public abstract class GuardBaseState : SpriteBaseState
{
    protected Hero Hero { get; set; }

    protected Guard Guard { get; set; }

    protected Maze Maze { get; set; }

    protected GuardBaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState)
    {
        Hero = hero;
        Guard = guard;
        Maze = maze;
    }

    protected override GuardBaseState GetTrapCollidingState(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Drop => new GuardFallingState(this, Hero, Guard, Maze),
            TrapType.Bayonet => new GuardDyingState(this, Hero, Guard, Maze),
            _ => throw new NotImplementedException()
        };
    }

    protected bool IsHeroNearby(out IEnumerable<Vector2> pathToHero)
    {
        if (Hero.IsDead)
        {
            pathToHero = null;

            return false;
        }

        var distance = Vector2.Distance(Hero.Position, Guard.Position);

        if (distance > GameRules.GuardHeroDetectionDistance)
        {
            pathToHero = null;

            return false;
        }

        var pathExist = GuardMoveBaseState.PathToHeroExist(Hero, Guard, Maze, out pathToHero);

        return pathExist;
    }

    protected float GetDistanceToHero()
    {
        var distance = Vector2.Distance(Hero.Position, Guard.Position);

        return distance;
    }
}