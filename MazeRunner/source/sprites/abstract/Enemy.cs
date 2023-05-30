namespace MazeRunner.Sprites;

public abstract class Enemy : Sprite
{
    public abstract int Damage { get; }

    public abstract float GetAttackDistance();
}
