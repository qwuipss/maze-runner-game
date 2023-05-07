#region Usings
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.Cameras;

public interface ICamera
{
    public void Follow(Sprite sprite, Vector2 position);

    public Matrix GetTransformMatrix();
}
