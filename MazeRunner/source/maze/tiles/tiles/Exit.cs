using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles;

public class Exit : MazeTile
{
    public override float DrawingPriority
    {
        get
        {
            return .8f;
        }
    }

    public bool IsOpened
    {
        get
        {
            return State is ExitOpenedState;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Exit;
        }
    }

    public Exit()
    {
        State = new ExitClosedState();
    }

    public static Vector2 GetOriginFrameRotationVector(Exit exit)
    {
        var rotation = exit.FrameRotationAngle;

        if (rotation is MathHelper.PiOver2)
        {
            return new Vector2(0, exit.FrameSize);
        }

        if (rotation is -MathHelper.PiOver2)
        {
            return new Vector2(exit.FrameSize, 0);
        }

        return Vector2.Zero;
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
            State = new ExitOpeningState();
        }
    }
}