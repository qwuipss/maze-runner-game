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
        if (CollisionManager.CollidesWithTraps(heroInfo, mazeInfo.Maze, out var trapInfo))
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

        var totalMovement = Vector2.Zero;

        foreach (var movementDirection in KeyboardManager.ProcessHeroMovement())
        {
            if (!CollisionManager.CollidesWithWalls(heroInfo, movementDirection, maze)
             && !CollisionManager.CollidesWithExit(heroInfo, movementDirection, maze))
            {
                var travelledDistance = hero.GetTravelledDistance(movementDirection, gameTime);

                position += travelledDistance;
                totalMovement += travelledDistance;
            }
        }

        return totalMovement == Vector2.Zero ? totalMovement : Vector2.Normalize(totalMovement);
    }

    protected override HeroBaseState GetTrapCollidingState(TrapType trapType, ISpriteState previousState)
    {
        return trapType switch
        {
            TrapType.Bayonet => new HeroDyingState(previousState),
            TrapType.Drop => new HeroFallingState(previousState),
            _ => throw new NotImplementedException()
        };
    }
}
