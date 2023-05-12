using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroRunState : SpriteBaseState
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
