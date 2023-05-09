using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class FloorBaseState : MazeTileBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.Floor;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 1;
        }
    }

    protected override int UpdateTimeDelayMs
    {
        get
        {
            return int.MaxValue;
        }
    }
}