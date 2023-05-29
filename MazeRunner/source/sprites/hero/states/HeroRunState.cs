using MazeRunner.Content;
using MazeRunner.Managers;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroRunState : HeroBaseState
{
    private readonly SpriteInfo _heroInfo;
    private readonly MazeInfo _mazeInfo;

    public override Texture2D Texture => Textures.Sprites.Hero.Run;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 85;

    public HeroRunState(ISpriteState previousState, SpriteInfo heroInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;
        _mazeInfo = mazeInfo;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(_heroInfo, _mazeInfo.Maze, gameTime);

        if (movement == Vector2.Zero)
        {
            return new HeroIdleState(this, _heroInfo, _mazeInfo);
        }

        _heroInfo.Position += movement;

        ProcessFrameEffect(movement);

        if (CollidesWithTraps(_heroInfo, _mazeInfo, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        ProcessItemsColliding(_heroInfo, _mazeInfo);

        base.ProcessState(gameTime);

        return this;
    }

    private static void ProcessItemsColliding(SpriteInfo heroInfo, MazeInfo mazeInfo)
    {
        if (CollisionManager.CollidesWithItems(heroInfo.Sprite, heroInfo.Position, mazeInfo.Maze, out var itemInfo))
        {
            var (cell, item) = itemInfo;

            item.ProcessCollecting(mazeInfo, cell);
        }
    }
}