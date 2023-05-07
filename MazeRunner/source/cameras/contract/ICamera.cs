#region Usings
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Cameras;

public interface ICamera
{
    public Matrix GetTransformMatrix();
}
