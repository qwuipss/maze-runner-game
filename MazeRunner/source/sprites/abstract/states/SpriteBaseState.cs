using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public abstract class SpriteBaseState : ISpriteState
{
    private static readonly SpriteEffects[] FrameEffects = new[] { SpriteEffects.None, SpriteEffects.FlipHorizontally };

    protected SpriteBaseState(ISpriteState previousState)
    {
        if (previousState is null)
        {
            FrameEffect = RandomHelper.Choice(FrameEffects);
        }
        else
        {
            FrameEffect = previousState.FrameEffect;
        }
    }

    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    public abstract double UpdateTimeDelayMs { get; }

    public SpriteEffects FrameEffect { get; set; }

    public int FrameSize
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public Rectangle CurrentAnimationFrame
    {
        get
        {
            return new Rectangle(CurrentAnimationFramePoint, new Point(FrameSize, FrameSize));
        }
    }

    protected Point CurrentAnimationFramePoint { get; set; }

    protected double ElapsedGameTimeMs { get; set; }

    protected abstract ISpriteState GetTrapCollidingState(TrapType trapType);

    protected static bool CollidesWithTraps(SpriteInfo spriteInfo, MazeInfo mazeInfo, bool needActivating, out TrapType trapType)
    {
        if (CollisionManager.CollidesWithTraps(spriteInfo.Sprite, spriteInfo.Position, mazeInfo.Maze, needActivating, out var trapInfo))
        {
            trapType = trapInfo.Trap.TrapType;

            return true;
        }

        trapType = TrapType.None;
        return false;
    }

    public virtual ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }

    protected void ProcessFrameEffect(Vector2 movement)
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
}