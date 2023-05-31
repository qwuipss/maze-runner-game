using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class ChalkBaseState : MazeItemBaseState
{
    public override Texture2D Texture => Textures.MazeTiles.MazeItems.Chalk;

    public override int FramesCount => 1;

    protected override double UpdateTimeDelayMs => double.MaxValue;
}