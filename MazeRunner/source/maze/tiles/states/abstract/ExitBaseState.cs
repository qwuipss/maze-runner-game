#region Usings
using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

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
            return 5;
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