using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class DropTrapBaseState : MazeTrapBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.MazeTraps.Drop;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 8;
        }
    }

    protected override double UpdateTimeDelayMs
    {
        get
        {
            return 10;
        }
    }
}