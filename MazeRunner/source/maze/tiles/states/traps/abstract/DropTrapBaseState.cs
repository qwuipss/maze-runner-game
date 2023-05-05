#region Usings
using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class DropTrapBaseState : MazeTrapBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.MazeTraps.DropTrap;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 8;
        }
    }

    public override int FrameAnimationDelayMs
    {
        get
        {
            return 35;
        }
    }
}