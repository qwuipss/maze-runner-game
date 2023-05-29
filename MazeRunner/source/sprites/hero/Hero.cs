using MazeRunner.Helpers;
using MazeRunner.Sprites.States;
using MazeRunner.Wrappers;
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

    private SpriteInfo _selfInfo;

    private int _halfHeartsHealth;

    public override bool IsDead => State is HeroDeadState or HeroFalledState;

    public override Vector2 Speed => new(40, 40);

    public override float DrawingPriority => .5f;

    public bool IsTakingDamage => State is HeroDamageTakingState;

    static Hero()
    {
        _instance = new Lazy<Hero>(() => new Hero());
    }

    private Hero()
    {
        _halfHeartsHealth = 6;
    }

    public static Hero GetInstance()
    {
        return _instance.Value;
    }

    public void Initialize(SpriteInfo selfInfo, MazeInfo mazeInfo, int halfHeartsHealth)
    {
        _halfHeartsHealth = halfHeartsHealth;

        _selfInfo = selfInfo;

        State = new HeroIdleState(_selfInfo, mazeInfo);
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
        _halfHeartsHealth -= damage;

        if (_halfHeartsHealth <= 0)
        {
            State = new HeroDyingState(State);
        }

        State = new HeroDamageTakingState(State);
    }
}