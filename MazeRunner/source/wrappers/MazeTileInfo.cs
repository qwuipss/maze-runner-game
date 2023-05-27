using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class MazeTileInfo : MazeRunnerGameComponent
{
    public MazeTile MazeTile { get; init; }

    public Vector2 Position { get; init; }

    public MazeTileInfo(MazeTile mazeTile, Vector2 position)
    {
        MazeTile = mazeTile;
        Position = position;
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawMazeTile(MazeTile, Position);
    }

    public override void Update(GameTime gameTime)
    {
        MazeTile.Update(gameTime);
    }
}