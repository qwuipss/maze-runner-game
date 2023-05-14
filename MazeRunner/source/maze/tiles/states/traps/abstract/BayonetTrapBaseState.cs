using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class BayonetTrapBaseState : MazeTrapBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.MazeTraps.Bayonet;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 5;
        }
    }

    protected override int UpdateTimeDelayMs
    {
        get
        {
            return 40;
        }
    }
}