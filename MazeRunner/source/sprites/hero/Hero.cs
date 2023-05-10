using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private const double MovePollingTimeMs = 50;

    private const int HitBoxOffsetX = 3;
    private const int HitBoxOffsetY = 4;

    private const int HitBoxWidth = 9;
    private const int HitBoxHeight = 12;

    private readonly MazeInfo _mazeInfo;

    private double _elapsedGameTimeMs;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(3, 3);
        }
    }

    protected override ISpriteState State { get; set; }

    public Hero(MazeRunnerGame game)
    {
        _mazeInfo = game.MazeInfo;

        State = new HeroIdleState();
    }

    public override Rectangle GetHitBox(Vector2 position)
    {
        return new Rectangle(
                (int)position.X + HitBoxOffsetX,
                (int)position.Y + HitBoxOffsetY,
                HitBoxWidth,
                HitBoxHeight);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        base.Update(game, gameTime);

        if (!KeyboardManager.IsPollingTimePassed(MovePollingTimeMs, ref _elapsedGameTimeMs, gameTime))
        {
            return;
        }

        var position = game.HeroInfo.Position;

        var movement = ProcessMovement(position);

        ProcessState(movement);
        ProcessFrameEffect(movement);

        position += movement;

        ProcessItemsColliding(position);

        game.HeroInfo.Position = position;
    }

    #region VisualProcessers
    private void ProcessState(Vector2 movement)
    {
        if (movement == Vector2.Zero)
        {
            if (State is not HeroIdleState)
            {
                State = new HeroIdleState();
            }
        }
        else
        {
            if (State is not HeroRunState)
            {
                State = new HeroRunState();
            }
        }
    }

    private void ProcessFrameEffect(Vector2 movement)
    {
        if (movement.X > 0)
        {
            FrameEffect = SpriteEffects.None;
        }
        else if (movement.X < 0)
        {
            FrameEffect = SpriteEffects.FlipHorizontally;
        }
    }
    #endregion

    #region Collidings
    private void ProcessItemsColliding(Vector2 position)
    {
        void ProcessKeyColliding(Vector2 position, Cell coords, Key key)
        {
            if (CollisionManager.CollidesWithKey(this, position, coords, key))
            {
                _mazeInfo.Maze.RemoveItem(coords);
                _mazeInfo.IsKeyCollected = true;
            }
        }

        if (CollisionManager.CollidesWithItems(this, position, _mazeInfo.Maze, out var itemInfo))
        {
            var (coords, item) = itemInfo;

            if (item is Key key)
            {
                ProcessKeyColliding(position, coords, key);
            }
        }
    }
    #endregion

    #region MovementCalculations
    private Vector2 ProcessMovement(Vector2 position)
    {
        static Vector2 NormalizeDiagonalSpeed(Vector2 speed, Vector2 movement)
        {
            if (movement.Abs() == speed)
            {
                return new Vector2((float)(movement.X / Math.Sqrt(2)), (float)(movement.Y / Math.Sqrt(2)));
            }

            return movement;
        }

        Vector2 GetTotalMovement(Vector2 movement, Vector2 position)
        {
            var maze = _mazeInfo.Maze;

            var totalMovement = Vector2.Zero;

            var movementX = new Vector2(movement.X, 0);
            var movementY = new Vector2(0, movement.Y);

            if (!CollisionManager.ColidesWithWalls(this, position, maze, movementX)
             && !CollisionManager.CollidesWithExit(this, position, maze, movementX))
            {
                totalMovement += movementX;
            }

            if (!CollisionManager.ColidesWithWalls(this, position, maze, movementY)
             && !CollisionManager.CollidesWithExit(this, position, maze, movementY))
            {
                totalMovement += movementY;
            }

            if (ProcessDiagonalMovement(totalMovement, position, movementX, movementY, out totalMovement))
            {
                return totalMovement;
            }

            return NormalizeDiagonalSpeed(Speed, totalMovement);
        }

        bool ProcessDiagonalMovement(Vector2 movement, Vector2 position, Vector2 movementX, Vector2 movementY, out Vector2 totalMovement) //
        {
            if (CollisionManager.ColidesWithWalls(this, position, _mazeInfo.Maze, movement))
            {
                if (RandomHelper.RandomBoolean())
                {
                    totalMovement = movementX;
                }
                else
                {
                    totalMovement = movementY;
                }

                return true;
            }

            totalMovement = movement;

            return false;
        }

        var movement = KeyboardManager.ProcessHeroMovement(this);
        var totalMovement = GetTotalMovement(movement, position);

        return totalMovement;
    }
    #endregion
}