#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Cameras;

public interface ICamera
{
    public Vector2 ViewPosition { get; }

    public int ViewWidth { get; }

    public int ViewHeight { get; }

    public Matrix TransformMatrix { get; }
}
