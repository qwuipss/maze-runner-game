#region Usings
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTrap : MazeTile
{
    public abstract double ActivateChance { get; }

    public abstract double DeactivateChance { get; }

    public virtual IMazeTrapState ProcessState()
    {
        return State.ProcessState();
    }

    public override Point GetCurrentAnimationPoint(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs >= AnimationDelayMs)
        {
            State = ProcessState();
            ElapsedGameTimeMs -= AnimationDelayMs;
        }

        return State.CurrentAnimationPoint;
    }

    protected abstract IMazeTrapState State { get; set; }

    protected abstract int AnimationDelayMs { get; }

    protected virtual double ElapsedGameTimeMs { get; set; }
}