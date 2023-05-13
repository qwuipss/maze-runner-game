using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private const double MovePollingTimeMs = 20;

    private const int HitBoxOffsetX = 3;
    private const int HitBoxOffsetY = 5;

    private const int HitBoxWidth = 10;
    private const int HitBoxHeight = 10;

    private double _movementPollingTimeMs;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(1, 1);
        }
    }

    protected override ISpriteState State { get; set; }

    public Hero()
    {
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

        if (!KeyboardManager.IsPollingTimePassed(MovePollingTimeMs, ref _movementPollingTimeMs, gameTime))
        {
            return;
        }

        var heroInfo = game.HeroInfo;
        var position = heroInfo.Position;

        var mazeInfo = game.MazeInfo;

        var movement = ProcessMovement(position, mazeInfo.Maze);

        FrameEffect = SpriteBaseState.ProcessFrameEffect(movement, FrameEffect);
        ProcessState(movement);

        if (movement != Vector2.Zero)
        {
            ProcessItemsColliding(position, movement, mazeInfo);

            position += movement;
            heroInfo.Position = position;
        }
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

    #region Collidings
    private void ProcessItemsColliding(Vector2 position, Vector2 movement, MazeInfo mazeInfo)
    {
        void ProcessKeyColliding(Vector2 position, Cell cell, Key key)
        {
            mazeInfo.Maze.RemoveItem(cell);
            mazeInfo.IsKeyCollected = true;
        }

        var maze = mazeInfo.Maze;

        if (CollisionManager.CollidesWithItems(this, position, movement, maze, out var itemInfo))
        {
            var (cell, item) = itemInfo;

            if (item is Key key)
            {
                ProcessKeyColliding(position, cell, key);
            }
        }
    }
    #endregion
}