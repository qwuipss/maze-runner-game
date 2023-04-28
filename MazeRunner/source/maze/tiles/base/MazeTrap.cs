#region Usings
using Microsoft.Xna.Framework;
using System;
#endregion

namespace MazeRunner;

public abstract class MazeTrap : MazeTile
{
    protected abstract double ActivateChance { get; }

    protected abstract double DeactivateChance { get; }

    protected abstract Random Random { get; }

    protected abstract int AnimationFrameDelayMs { get; }

    protected abstract int CurrentAnimationFrameX { get; set; }

    protected abstract double ElapsedGameTime { get; set; }

    protected abstract TrapCondition Condition { get; set; }

    public override Point GetCurrentAnimationFrame(GameTime gameTime)
    {
        ElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTime >= AnimationFrameDelayMs)
        {
            switch (Condition)
            {
                case TrapCondition.Active:
                    DeactivateWithChance(ActivateChance);
                    break;
                case TrapCondition.Inactive:
                    ActivateWithChance(DeactivateChance);
                    break;
                case TrapCondition.Activating:
                    ContinueActivating();
                    break;
                case TrapCondition.Deactivating:
                    ContinueDeactivating();
                    break;
                default:
                    break;
            }

            ElapsedGameTime -= AnimationFrameDelayMs;
        }

        return new Point(CurrentAnimationFrameX, 0);
    }

    protected virtual void ContinueActivating()
    {
        CurrentAnimationFrameX += FrameWidth;

        if (CurrentAnimationFrameX == FrameWidth * FramesCount - FrameWidth)
        {
            Condition = TrapCondition.Active;
        }
    }

    protected virtual void ContinueDeactivating()
    {
        CurrentAnimationFrameX -= FrameWidth;

        if (CurrentAnimationFrameX is 0)
        {
            Condition = TrapCondition.Inactive;
        }
    }

    protected virtual void ActivateWithChance(double chance)
    {
        SwitchConditionWithChance(chance, TrapCondition.Activating);
    }

    protected virtual void DeactivateWithChance(double chance)
    {
        SwitchConditionWithChance(chance, TrapCondition.Deactivating);
    }

    protected virtual void SwitchConditionWithChance(double chance, TrapCondition newCondition)
    {
        var randomValue = Random.NextDouble();

        if (chance > randomValue)
        {
            Condition = newCondition;
        }
    }
}