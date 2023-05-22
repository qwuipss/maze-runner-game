using MazeRunner.Helpers;
using MazeRunner.Sprites.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.Sprites;

public class Guard : Sprite
{
    private const int HitBoxOffsetX = 7;
    private const int HitBoxOffsetY = 7;

    private const int HitBoxSizeX = 1;
    private const int HitBoxSizeY = 1;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(15, 15);
        }
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffsetX, HitBoxOffsetY, HitBoxSizeX, HitBoxSizeY);
    }

    public void Initialize(MazeRunnerGame game, SpriteInfo selfInfo)
    {
        State = new GuardIdleState(game.HeroInfo, selfInfo, game.MazeInfo);
    }
}
