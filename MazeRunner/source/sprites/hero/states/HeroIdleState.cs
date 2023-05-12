using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroIdleState : SpriteBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Hero.HeroIdle;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 6;
        }
    }

    public override int UpdateTimeDelayMs
    {
        get
        {
            return 65;
        }
    }
}
