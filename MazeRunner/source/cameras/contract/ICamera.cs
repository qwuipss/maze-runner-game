#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Cameras;

public interface ICamera
{
    public Vector2 Position { get; }

    public Matrix TransformMatrix { get; }
}
