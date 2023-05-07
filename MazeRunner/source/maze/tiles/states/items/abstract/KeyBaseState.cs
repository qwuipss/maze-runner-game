using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class KeyBaseState : MazeItemBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.MazeTiles.MazeItems.Key;
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