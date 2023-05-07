using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles;

public class Exit : MazeTile
{
    public float FrameRotationAngle { get; init; }

    public Vector2 OriginFrameRotationVector { get; set; }

    public bool IsOpened
    {
        get
        {
            return State is ExitOpenedState;
        }
    }

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
        if (State is ExitClosedState)
        {
            State = new ExitOpeningState();
        }
    }
}
