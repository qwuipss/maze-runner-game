using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class ExitBaseState : MazeTileBaseState
{
    public override Texture2D Texture => Textures.MazeTiles.Exit;

    public override int FramesCount => 14;

    protected override double UpdateTimeDelayMs => double.MaxValue;
}