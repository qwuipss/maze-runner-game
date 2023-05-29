using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles;

public class Exit : MazeTile
{
    private float _drawingPriority;

    public override float DrawingPriority => _drawingPriority;

    public bool IsOpened => State is ExitOpenedState;

    public override TileType TileType => TileType.Exit;

    public Exit()
    {
        _drawingPriority = .6f;

        State = new ExitClosedState();
    }

    public static float GetFrameRotationAngle(Cell cell, Maze maze)
    {
        if (cell.X is 0)
        {
            return -MathHelper.PiOver2;
        }

        var skeleton = maze.Skeleton;

        if (cell.X == skeleton.GetLength(1) - 1)
        {
            return MathHelper.PiOver2;
        }

        return 0;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void Open()
    {
        if (State is ExitClosedState)
        {
            _drawingPriority = .4f;

            State = new ExitOpeningState();
        }
    }
}