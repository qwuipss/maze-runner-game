#region Usings
using MazeRunner.Content;
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class BayonetTrap : MazeTrap
{
    public BayonetTrap()
    {
        State = new DeactivatedState(this);
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.MazeTraps.BayonetTrap;
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
            return 1e-1 / 2;
        }
    }

    public override double DeactivateChance
    {
        get
        {
            return 1e-2 * 4;
        }
    }

    protected override int AnimationDelayMs
    {
        get
        {
            return 75;
        }
    }

    protected override IMazeTrapState State { get; set; }
}