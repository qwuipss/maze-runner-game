using MazeRunner.Content;
using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class GuardIdleState : GuardBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;

    private readonly Maze _maze;

    public override Texture2D Texture => Textures.Sprites.Guard.Idle;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 650;

    public GuardIdleState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, Maze maze) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _maze = maze;
    }

    public GuardIdleState(SpriteInfo heroInfo, SpriteInfo guardInfo, Maze maze) : this(null, heroInfo, guardInfo, maze)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (CollidesWithTraps(_guardInfo, _maze, false, out var _))
        {
            return new GuardWalkState(this, _heroInfo, _guardInfo, _maze);
        }

        if (IsHeroNearby(_heroInfo, _guardInfo, _maze, out var _))
        {
            return new GuardChaseState(this, _heroInfo, _guardInfo, _maze);
        }

        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            if (framePosX is 0 && animationPoint.X == (FramesCount - 1) * FrameSize && RandomHelper.RandomBoolean())
            {
                return new GuardWalkState(this, _heroInfo, _guardInfo, _maze);
            }

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
