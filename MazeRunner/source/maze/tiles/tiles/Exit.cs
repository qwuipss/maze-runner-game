#region Usings
using MazeRunner.MazeBase.Tiles.States;
using System;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class Exit : MazeTile
{
    public float FrameRotationAngle { get; set; }

    public Exit()
    {
        State = new ExitClosedState();
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Exit;
        }
    }

    public void Open()
    {
        if (State is ExitOpenedState)
        {
            throw new InvalidOperationException($"exit state already {nameof(ExitOpenedState)}");
        }

        State = new ExitOpeningState();
    }
}
