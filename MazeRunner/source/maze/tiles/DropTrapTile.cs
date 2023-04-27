#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace MazeRunner;

public class DropTrapTile : MazeTrap
{
    private static readonly Random _random = new();

    public override Texture2D Texture
    {
        get
        {
            return TilesTextures.DropTrap;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.DropTrap;
        }
    }

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

    protected override int FramesCount
    {
        get
        {
            return 8;
        }
    }

    protected override double ActivateChance
    {
        get
        {
            return 1e-1;
        }
    }

    protected override double DeactivateChance
    {
        get
        {
            return 1e-2;
        }
    }

    protected override Random Random
    {
        get
        {
            return _random;
        }
    }

    protected override int AnimationFrameDelayMs
    {
        get
        {
            return 35;
        }
    }

    protected override int CurrentAnimationFrameX { get; set; } = 0;

    protected override double ElapsedGameTime { get; set; } = 0;

    protected override TrapCondition Condition { get; set; } = TrapCondition.Inactive;
}
