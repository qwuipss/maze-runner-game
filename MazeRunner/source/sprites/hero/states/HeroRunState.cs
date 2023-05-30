using MazeRunner.Content;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroRunState : HeroBaseState
{
    private readonly SpriteInfo _heroInfo;
    private readonly Maze _maze;

    public override Texture2D Texture => Textures.Sprites.Hero.Run;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 85;

    public HeroRunState(ISpriteState previousState, SpriteInfo heroInfo, Maze maze) : base(previousState)
    {
        _heroInfo = heroInfo;
        _maze = maze;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(_heroInfo, _maze, gameTime);

        if (movement == Vector2.Zero)
        {
            return new HeroIdleState(this, _heroInfo, _maze);
        }

        _heroInfo.Position += movement;

        ProcessFrameEffect(movement);

        if (CollidesWithTraps(_heroInfo, _maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        ProcessItemsColliding(_heroInfo, _maze);

        base.ProcessState(gameTime);

        return this;
    }

    private static void ProcessItemsColliding(SpriteInfo heroInfo, Maze maze)
    {
        if (CollisionManager.CollidesWithItems(heroInfo.Sprite, heroInfo.Position, maze, out var itemInfo))
        {
            var (cell, item) = itemInfo;

            item.ProcessCollecting(maze, cell);
        }
    }
}