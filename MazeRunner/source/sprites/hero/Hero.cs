using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace MazeRunner.Sprites;

public class Hero : Sprite
{
    private const float HitBoxOffsetX = 5;

    private const float HitBoxOffsetY = 5;

    private const float HitBoxWidth = 7;

    private const float HitBoxHeight = 10;

    private Maze _maze;

    public override bool IsDead => State is HeroDiedState or HeroFellState or HeroFallingState or HeroDyingState;

    public override Vector2 Speed => new(40, 40);

    public override float DrawingPriority => .5f;

    public bool IsTakingDamage => State is HeroDamageTakingState;

    public int Health { get; set; }

    public int ChalkUses { get; set; }

    public Hero(int health, int chalkUses)
    {
        Health = health;
        ChalkUses = chalkUses;
    }

    public void Initialize(Maze maze)
    {
        _maze = maze;

        State = new HeroIdleState(this, maze);
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
        SoundManager.Sprites.Hero.PlayGetHitSoundWithDelay();

        Health -= damage;

        if (Health <= 0)
        {
            State = new HeroDyingState(State, this, _maze);
        }

        State = new HeroDamageTakingState(State, this, _maze);
    }
}