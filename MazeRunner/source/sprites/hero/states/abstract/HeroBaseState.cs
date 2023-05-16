using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Sprites.States;

public abstract class HeroBaseState : SpriteBaseState
{
    protected HeroBaseState(ISpriteState previousState)
    {
        if (previousState is null)
        {
            FrameEffect = RandomHelper.Choice(new[] { SpriteEffects.None, SpriteEffects.FlipHorizontally });
        }
        else
        {
            FrameEffect = previousState.FrameEffect;
        }
    }

    protected static bool CollidesWithTraps(SpriteInfo heroInfo, MazeInfo mazeInfo, out TrapType trapType)
    {
        if (CollisionManager.CollidesWithTraps(heroInfo.Sprite, heroInfo.Position, mazeInfo.Maze, out var trapInfo))
        {
            trapType = trapInfo.Trap.TrapType;

            return true;
        }

        trapType = TrapType.None;
        return false;
    }

    protected static Vector2 ProcessMovement(SpriteInfo heroInfo, Maze maze, GameTime gameTime)
    {
        var hero = heroInfo.Sprite;
        var position = heroInfo.Position;

        var directions = Vector2.Zero;

        foreach (var movementDirection in KeyboardManager.ProcessHeroMovement())
        {
            var travelledDistance = hero.GetTravelledDistance(movementDirection, gameTime);

            if (!CollisionManager.CollidesWithWalls(hero, position, travelledDistance, maze)
             && !CollisionManager.CollidesWithExit(hero, position, travelledDistance, maze))
            {
                directions += movementDirection;
                position += travelledDistance;
            }
        }

        if (directions != Vector2.Zero)
        {
            directions.Normalize();
        }

        return hero.GetTravelledDistance(directions, gameTime);
    }

    protected override HeroBaseState GetTrapCollidingState(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Bayonet => new HeroDyingState(this),
            TrapType.Drop => new HeroFallingState(this),
            _ => throw new NotImplementedException()
        };
    }
}
