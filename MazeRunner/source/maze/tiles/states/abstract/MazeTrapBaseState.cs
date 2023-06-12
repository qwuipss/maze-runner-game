using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class MazeTrapBaseState : MazeTileBaseState
{
    protected Hero Hero { get; init; }

    protected MazeTrap Trap { get; init; }

    protected MazeTrapBaseState(Hero hero, MazeTrap trap)
    {
        Hero = hero;
        Trap = trap;
    }

    protected float GetDistanceToHero()
    {
        if (Hero is null)
        {
            return float.MaxValue;
        }

        return Vector2.Distance(Hero.Position, Trap.Position);
    }
}