#region Usings
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public class BayonetTrap : MazeTrap
{
    public BayonetTrap()
    {
        State = new TrapDeactivatedState(this);
    }

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
            return TileType.BayonetTrap;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 5;
        }
    }

    public override double ActivateChance
    {
        get
        {
            return 1e-1 / 3;
        }
    }

    public override double DeactivateChance
    {
        get
        {
            return 1e-2 / 2;
        }
    }

    public override int AnimationFrameDelayMs
    {
        get
        {
            return 75;
        }
    }

    public override int CurrentAnimationFrameX { get; set; } = 0;

    protected override double ElapsedGameTime { get; set; } = 0;

    protected override IMazeTrapState State { get; set; }
}