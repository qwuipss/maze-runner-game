using MazeRunner.Content;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class DropTrapBaseState : MazeTrapBaseState
{
    public override Texture2D Texture => Textures.MazeTiles.MazeTraps.Drop;

    public override int FramesCount => 8;

    protected override double UpdateTimeDelayMs => 20;

    protected DropTrapBaseState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
    }
}