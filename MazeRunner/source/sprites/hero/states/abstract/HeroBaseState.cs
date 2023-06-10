using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Sprites.States;

public abstract class HeroBaseState : SpriteBaseState
{
    public static event Action HeroDrewWithChalkNotify;

    protected Hero Hero { get; set; }

    protected Maze Maze { get; set; }

    protected HeroBaseState(ISpriteState previousState, Hero hero, Maze maze) : base(previousState)
    {
        Hero = hero;
        Maze = maze;
    }

    protected override HeroBaseState GetTrapCollidingState(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Bayonet => new HeroDyingState(this, Hero, Maze),
            TrapType.Drop => new HeroFallingState(this, Hero, Maze),
            _ => throw new NotImplementedException()
        };
    }

    protected Vector2 ProcessMovement(GameTime gameTime)
    {
        static bool IsMovementAccepted(Sprite hero, Vector2 position, Vector2 movement, Maze maze)
        {
            return !CollisionManager.CollidesWithWalls(hero, position, movement, maze)
                && !CollisionManager.CollidesWithExit(hero, position, movement, maze);
        }

        var movementDirection = KeyboardManager.ProcessHeroMovement();
        var movement = Hero.GetMovement(movementDirection, gameTime);

        if (movementDirection == Vector2.Zero)
        {
            return Vector2.Zero;
        }

        if (IsMovementAccepted(Hero, Hero.Position, movement, Maze))
        {
            movementDirection.Normalize();
            movement = Hero.GetMovement(movementDirection, gameTime);

            return movement;
        }

        var movementX = new Vector2(movement.X, 0);

        if (IsMovementAccepted(Hero, Hero.Position, movementX, Maze))
        {
            return movementX;
        }

        var movementY = new Vector2(0, movement.Y);

        if (IsMovementAccepted(Hero, Hero.Position, movementY, Maze))
        {
            return movementY;
        }

        return Vector2.Zero;
    }

    protected void HandleChalkDrawing(GameTime gameTime)
    {
        if (Hero.ChalkUses > 0 && KeyboardManager.IsChalkDrawingButtonPressed(gameTime))
        {
            var cell = GetSpriteCell(Hero);

            if (Maze.CanInsertMark(cell))
            {
                var mark = new ChalkMark
                {
                    Position = Maze.GetCellPosition(cell)
                };

                Maze.InsertMark(mark, cell);

                Hero.ChalkUses--;

                HeroDrewWithChalkNotify.Invoke();
            }
        }
    }
}