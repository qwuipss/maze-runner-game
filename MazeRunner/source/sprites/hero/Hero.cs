using MazeRunner.Helpers;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private static readonly Lazy<Hero> _instance;

    private const int HitBoxOffsetX = 5;
    private const int HitBoxOffsetY = 5;

    private const int HitBoxWidth = 7;
    private const int HitBoxHeight = 10;

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(40, 40);
        }
    }

    public override ISpriteState State { get; set; }

    static Hero()
    {
        _instance = new Lazy<Hero>(() => new Hero());
    }

    private Hero()
    {
    }

    public static Hero GetInstance()
    {
        return _instance.Value;
    }

    public void Initialize(MazeRunnerGame game)
    {
        State = new HeroIdleState(game.HeroInfo, game.MazeInfo);
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffsetX, HitBoxOffsetY, HitBoxWidth, HitBoxHeight);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        base.Update(game, gameTime);
    }
}