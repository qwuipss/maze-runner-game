#region Usings
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using System;
#endregion

namespace MazeRunner.MazeBase.Tiles;

public class Exit : MazeTile
{
    public float FrameRotationAngle { get; init; }

    public Vector2 OriginFrameRotationVector { get; set; }

    public Exit(float rotation)
    {
        State = new ExitClosedState();

        FrameRotationAngle = rotation;
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
