#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner;

public abstract class MazeTrap : MazeTile, IMazeTrapState
{
    public abstract double ActivateChance { get; }

    public abstract double DeactivateChance { get; }

    public abstract int AnimationFrameDelayMs { get; }

    public virtual int CurrentAnimationFrameX { get; set; }

    protected virtual double ElapsedGameTime { get; set; }

    protected abstract IMazeTrapState State { get; set; }

    public virtual IMazeTrapState ProcessState()
    {
        return State.ProcessState();
    }

    public override Point GetCurrentAnimationFrame(GameTime gameTime)
    {
        ElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTime >= AnimationFrameDelayMs)
        {
            State = ProcessState();
            ElapsedGameTime -= AnimationFrameDelayMs;
        }

        return new Point(CurrentAnimationFrameX, 0);
    }
}