using MazeRunner.Components;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class MazeInfo : MazeRunnerGameComponent
{
    private const float ExitOpenDistanceCoeff = 2;

    private SpriteInfo _heroInfo;

    private float _exitOpenDistance;

    public Maze Maze { get; init; }

    public bool IsKeyCollected { get; set; }

    public SpriteInfo HeroInfo
    {
        set
        {
            _heroInfo = value;
            _exitOpenDistance = _heroInfo.Sprite.FrameSize * ExitOpenDistanceCoeff;
        }
    }

    public MazeInfo(Maze maze)
    {
        Maze = maze;
    }

    public override void Update(GameTime gameTime)
    {
        var exitInfo = Maze.ExitInfo;

        if (NeedOpenExit(exitInfo))
        {
            exitInfo.Exit.Open();
        }

        Maze.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Maze.Draw(gameTime);
    }

    private bool NeedOpenExit((Cell Cell, Exit Exit) exitInfo)
    {
        return IsKeyCollected
         && !exitInfo.Exit.IsOpened
         && _heroInfo is not null
         && Vector2.Distance(_heroInfo.Position, Maze.GetCellPosition(exitInfo.Cell)) < _exitOpenDistance;
    }
}