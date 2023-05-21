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

    public GuardIdleState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;

        _guardInfo = guardInfo;
        _mazeInfo = mazeInfo;
    }

    public GuardIdleState(SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : this(null, heroInfo, guardInfo, mazeInfo)
    {

    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Guard.Idle;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return 650;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            if (animationPoint.X == (FramesCount - 1) * FrameSize && framePosX is 0 && RandomHelper.RandomBoolean())
            {
                return new GuardWalkState(this, _heroInfo, _guardInfo, _mazeInfo);
            }

            if (IsHeroNearby(_heroInfo, _guardInfo))
            {
                return new GuardWalkState(this, _heroInfo, _guardInfo, _mazeInfo);
                //return new GuardChaseState(this, _heroInfo, _guardInfo, _mazeInfo);
            }

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
