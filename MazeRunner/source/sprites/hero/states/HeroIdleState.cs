#region Usings
using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites.States;

public class HeroIdleState : BaseState
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
}
