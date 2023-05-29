using MazeRunner.Content;
using MazeRunner.Helpers;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class GuardIdleState : GuardBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;

    private readonly MazeInfo _mazeInfo;

    public override Texture2D Texture => Textures.Sprites.Guard.Idle;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 650;

    public GuardIdleState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _mazeInfo = mazeInfo;
    }

    public GuardIdleState(SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : this(null, heroInfo, guardInfo, mazeInfo)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _mazeInfo, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (CollidesWithTraps(_guardInfo, _mazeInfo, false, out var _))
        {
            return new GuardWalkState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        if (IsHeroNearby(_heroInfo, _guardInfo, _mazeInfo, out var _))
        {
            return new GuardChaseState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            if (framePosX is 0 && animationPoint.X == (FramesCount - 1) * FrameSize && RandomHelper.RandomBoolean())
            {
                return new GuardWalkState(this, _heroInfo, _guardInfo, _mazeInfo);
            }

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
