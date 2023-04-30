#region Usings
using Microsoft.Xna.Framework;
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public interface IMazeTrapState
{
    public Point CurrentAnimationPoint { get; }

    public IMazeTrapState ProcessState();
}