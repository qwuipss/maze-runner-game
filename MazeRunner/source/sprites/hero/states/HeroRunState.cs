using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroRunState : BaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Hero.HeroRun;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 6;
        }
    }
}
