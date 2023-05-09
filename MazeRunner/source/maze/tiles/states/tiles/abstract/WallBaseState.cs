using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class WallBaseState : MazeTileBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.Wall;
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