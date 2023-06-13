using MazeRunner.GameBase;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Drawing;

namespace MazeRunner.Sprites;

public class Guard : Enemy
{
    private const float HitBoxOffset = 5;

    private const float HitBoxSizeX = 5;

    private const float HitBoxSizeY = 11;

    private const float AttackDistanceCoeff = .85f;

    private const float ElongateAttackDistanceCoeff = 1.25f;

    public const float AttackDistance = AttackDistanceCoeff * GameConstants.AssetsFrameSize;

    public const float ElongatedAttackDistance = AttackDistance * ElongateAttackDistanceCoeff;

    public static int Damage => 1;

    private float _drawingPriority;

    public override event Action EnemyDiedNotify;

    public override bool IsDead => State is GuardDiedState or GuardFellState or GuardFallingState or GuardDyingState;

    public override float DrawingPriority => _drawingPriority;

    public override Vector2 Speed => new(15, 15);

    public SoundManager.SoundEffectData RunSoundEffectData { get; init; }

    public Guard()
    {
        _drawingPriority = base.DrawingPriority;

        RunSoundEffectData = SoundManager.Sprites.Guard.RunSoundData;
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSizeX, HitBoxSizeY);
    }

    public void Initialize(Hero hero, Maze maze)
    {
        State = new GuardIdleState(hero, this, maze);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_drawingPriority == base.DrawingPriority && IsDead)
        {
            _drawingPriority += .1f;

            EnemyDiedNotify.Invoke();
        }
    }
}
