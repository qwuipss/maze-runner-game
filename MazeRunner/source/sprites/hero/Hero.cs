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
    private static readonly Lazy<Hero> _instance;

    private const int HitBoxOffsetX = 5;
    private const int HitBoxOffsetY = 5;

    private const int HitBoxWidth = 7;
    private const int HitBoxHeight = 10;

    private bool _isDead;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(40, 40);
        }
    }

    protected override ISpriteState State { get; set; }

    static Hero()
    {
        _instance = new Lazy<Hero>(() => new Hero());
    }

    private Hero()
    {
        State = new HeroIdleState
        {
            FrameEffect = RandomHelper.Choice(new[] { SpriteEffects.None, SpriteEffects.FlipHorizontally })
        };
    }

    public static Hero GetInstance()
    {
        return _instance.Value;
    }

    public override FloatRectangle GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffsetX, HitBoxOffsetY, HitBoxWidth, HitBoxHeight);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        base.Update(game, gameTime);

        if (_isDead)
        {
            return;
        }

        var heroInfo = game.HeroInfo;
        var position = heroInfo.Position;

        var mazeInfo = game.MazeInfo;

        var movement = ProcessMovement(position, mazeInfo.Maze);

        State.FrameEffect = SpriteBaseState.ProcessFrameEffect(movement, FrameEffect);//
        ProcessState(movement);

        ProcessTrapsColliding(position, mazeInfo);

        if (movement != Vector2.Zero)
        {
            ProcessItemsColliding(position, mazeInfo);

            movement.Normalize();

            position += movement * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
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

        foreach (var movement in KeyboardManager.ProcessHeroMovement())
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
        if (CollisionManager.CollidesWithTraps(this, position, mazeInfo.Maze, out var trapInfo))
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