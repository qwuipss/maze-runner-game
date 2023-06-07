using System;

namespace MazeRunner.Sprites;

public abstract class Enemy : Sprite
{
    public abstract event Action EnemyDiedNotify;

    public abstract int Damage { get; }
}
