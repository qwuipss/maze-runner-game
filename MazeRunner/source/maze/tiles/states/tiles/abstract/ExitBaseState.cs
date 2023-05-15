using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class ExitBaseState : MazeTileBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.Exit;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 14;
        }
    }

    protected override double UpdateTimeDelayMs
    {
        get
        {
            return double.MaxValue;
        }
    }
}