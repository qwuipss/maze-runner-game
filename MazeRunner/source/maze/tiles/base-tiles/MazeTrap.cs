#region Usings
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

        if (CurrentAnimationFrameX == FrameWidth)
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
