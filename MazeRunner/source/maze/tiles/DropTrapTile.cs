#region Usings
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public class DropTrapTile : MazeTrap
{
    public DropTrapTile()
    {
        State = new TrapDeactivatedState(this);
    }

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

    public override int FramesCount
    {
        get
        {
            return 8;
        }
    }

    public override double ActivateChance
    {
        get
        {
            return 1e-1;
        }
    }

    public override double DeactivateChance
    {
        get
        {
            return 1e-2;
        }
    }

    public override int AnimationFrameDelayMs
    {
        get
        {
            return 35;
        }
    }

    protected override IMazeTrapState State { get; set; }
}