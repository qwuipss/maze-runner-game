using MazeRunner.GameBase.States;
using MazeRunner.Helpers;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private static readonly Lazy<Hero> _instance;

    private const float HitBoxOffsetX = 5;
    private const float HitBoxOffsetY = 5;

    private const float HitBoxWidth = 7;
    private const float HitBoxHeight = 10;

    public int HalfHeartsHealth { get; set; }

    public override bool IsDead
    {
        get
        {
            return State is HeroDeadState || State is HeroFalledState;
        }
    }

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(40, 40);
        }
    }

    public override float DrawingPriority
    {
        get
        {
            return .5f;
        }
    }

    public bool IsTakingDamage
    {
        get
        {
            return State is HeroDamageTakingState;
        }
    }

    static Hero()
    {
        _instance = new Lazy<Hero>(() => new Hero());
    }

    private Hero()
    {
        HalfHeartsHealth = 6;
    }

    public static Hero GetInstance()
    {
        return _instance.Value;
    }

    public void Initialize(GameRunningState game)
    {
        State = new HeroIdleState(game.HeroInfo, game.MazeInfo);
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffsetX, HitBoxOffsetY, HitBoxWidth, HitBoxHeight);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void TakeDamage(int damage)
    {
        HalfHeartsHealth -= damage;

        if (HalfHeartsHealth <= 0)
        {
            State = new HeroDyingState(State);
        }

        State = new HeroDamageTakingState(State);
    }
}