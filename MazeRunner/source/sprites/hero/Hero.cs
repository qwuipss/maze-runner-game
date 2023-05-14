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
    private static readonly Lazy<Hero> _instance;

    private const double MovementPollingTimeMs = 20;

    private const int HitBoxOffsetX = 4;
    private const int HitBoxOffsetY = 5;

    private const int HitBoxWidth = 8;
    private const int HitBoxHeight = 10;

    private bool _isDead;

    private double _movementPollingElapsedTimeMs;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(1, 1);
        }
    }

    protected override ISpriteState State { get; set; }

    static Hero()
    {
        _instance = new Lazy<Hero>(() => new Hero());
    }

    private Hero()
    {
        FrameEffect = RandomHelper.Choice(new[] { SpriteEffects.None, SpriteEffects.FlipHorizontally });

        State = new HeroIdleState();
    }

    public static Hero GetInstance()
    {
        return _instance.Value;
    }

    public override Rectangle GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffsetX, HitBoxOffsetY, HitBoxWidth, HitBoxHeight);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        base.Update(game, gameTime);

        if (_isDead || !KeyboardManager.IsPollingTimePassed(MovementPollingTimeMs, ref _movementPollingElapsedTimeMs, gameTime))
        {
            return;
        }

        var heroInfo = game.HeroInfo;
        var position = heroInfo.Position;

        var mazeInfo = game.MazeInfo;

        var movement = ProcessMovement(position, mazeInfo.Maze);

        FrameEffect = SpriteBaseState.ProcessFrameEffect(movement, FrameEffect);
        ProcessState(movement);

        ProcessTrapsColliding(position, mazeInfo);

        if (movement != Vector2.Zero)
        {
            ProcessItemsColliding(position, mazeInfo);

            position += movement;
            heroInfo.Position = position;
        }
    }

    private static HeroBaseState GetStateBasedOnTrapType(TrapType trapType)
    {
        return trapType switch
        {
            TrapType.Bayonet => new HeroDyingState(),
            TrapType.Drop => new HeroFallingState(),
            _ => throw new NotImplementedException()
        };
    }

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

    private Vector2 ProcessMovement(Vector2 position, Maze maze)
    {
        var totalMovement = Vector2.Zero;

        foreach (var movement in KeyboardManager.ProcessHeroMovement(this))
        {
            if (!CollisionManager.CollidesWithWalls(this, position, movement, maze)
             && !CollisionManager.CollidesWithExit(this, position, movement, maze))
            {
                position += movement;
                totalMovement += movement;
            }
        }

        return totalMovement;
    }

    #region CollidingsProcessors
    private void ProcessTrapsColliding(Vector2 position, MazeInfo mazeInfo)
    {
        if (!_isDead && CollisionManager.CollidesWithTraps(this, position, mazeInfo.Maze, out var trapInfo))
        {
            _isDead = true;

            State = GetStateBasedOnTrapType(trapInfo.Trap.TrapType);
        }
    }

    private void ProcessItemsColliding(Vector2 position, MazeInfo mazeInfo)
    {
        if (CollisionManager.CollidesWithItems(this, position, mazeInfo.Maze, out var itemInfo))
        {
            var (cell, item) = itemInfo;

            item.ProcessCollecting(mazeInfo, cell);
        }
    }
    #endregion
}