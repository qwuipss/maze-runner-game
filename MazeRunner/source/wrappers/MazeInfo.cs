using MazeRunner.Components;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class MazeInfo : MazeRunnerGameComponent
{
#pragma warning disable CS0067
    public override event GameComponentProvider NeedDisposeNotify;
#pragma warning restore

    public Maze Maze { get; init; }

    public bool IsKeyCollected { get; set; }

    public MazeInfo(Maze maze)
    {
        Maze = maze;
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        Maze.Update(game, gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Maze.Draw(gameTime);
    }
}