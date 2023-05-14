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
            return 8;
        }
    }

    protected override int UpdateTimeDelayMs
    {
        get
        {
            return 150;
        }
    }
}