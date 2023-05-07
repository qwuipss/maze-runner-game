#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Cameras;

public interface ICamera
{
    public Matrix GetTransformMatrix();
}
