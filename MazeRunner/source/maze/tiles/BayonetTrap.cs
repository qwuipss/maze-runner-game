#region Usings
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace MazeRunner;

public class BayonetTrap : MazeTrap
{
    private static readonly Random _random = new();

    public override Texture2D Texture
    {
        get
        {
            return TilesTextures.BayonetTrap;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.DropTrap;
        }
    }

    protected override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    protected override double ActivateChance
    {
        get
        {
            return 1e-1 / 3;
        }
    }

    protected override double DeactivateChance
    {
        get
        {
            return 1e-2 / 2;
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
            return 75;
        }
    }

    protected override int CurrentAnimationFrameX { get; set; } = 0;

    protected override double ElapsedGameTime { get; set; } = 0;

    protected override TrapCondition Condition { get; set; } = TrapCondition.Inactive;
}