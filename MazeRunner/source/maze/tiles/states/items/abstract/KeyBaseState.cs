using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class KeyBaseState : MazeItemBaseState
{
    public override Texture2D Texture => Textures.MazeTiles.MazeItems.Key;

    public override int FramesCount => 8;

    protected override double UpdateTimeDelayMs => 150;
}