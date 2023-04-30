#region Usings
using MazeRunner.Content;
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class DropTrap : MazeTrap
{
    public DropTrap()
    {
        State = new DeactivatedState(this);
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.DropTrap;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 8;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.DropTrap;
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

    protected override int AnimationDelayMs
    {
        get
        {
            return 35;
        }
    }

    protected override IMazeTrapState State { get; set; }
}