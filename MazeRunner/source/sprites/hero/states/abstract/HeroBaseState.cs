using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Sprites.States;

public abstract class HeroBaseState : SpriteBaseState
{
    protected HeroBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    protected static Vector2 ProcessMovement(SpriteInfo heroInfo, Maze maze, GameTime gameTime)
    {
        static bool IsMovementAccepted(Sprite hero, Vector2 position, Vector2 movement, Maze maze)
        {
            return !CollisionManager.CollidesWithWalls(hero, position, movement, maze)
                && !CollisionManager.CollidesWithExit(hero, position, movement, maze);
        }

        var hero = heroInfo.Sprite;
        var position = heroInfo.Position;

        var movementDirection = KeyboardManager.ProcessHeroMovement();
        var movement = hero.GetMovement(movementDirection, gameTime);

        if (movementDirection == Vector2.Zero)
        {
            return Vector2.Zero;
        }

        if (IsMovementAccepted(hero, position, movement, maze))
        {
            movementDirection.Normalize();
            movement = hero.GetMovement(movementDirection, gameTime);

            return movement;
        }

        var movementX = new Vector2(movement.X, 0);

        if (IsMovementAccepted(hero, position, movementX, maze))
        {
            return movementX;
        }

        var movementY = new Vector2(0, movement.Y);

        if (IsMovementAccepted(hero, position, movementY, maze))
        {
            return movementY;
        }

        return Vector2.Zero;
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