#region Usings
using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

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

    public override int FrameAnimationDelayMs
    {
        get
        {
            return int.MaxValue;
        }
    }
}