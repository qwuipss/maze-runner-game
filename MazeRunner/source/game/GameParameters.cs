using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.GameBase;

public class GameParameters
{
    public int MazeWidth;
    public int MazeHeight;

    public int MazeDeadEndsRemovePercentage;

    public int MazeBayonetTrapInsertingPercentage;
    public int MazeDropTrapInsertingPercentage;

    public float HeroCameraScaleFactor;
    public float HeroCameraShadowTresholdCoeff;

    public int GuardSpawnCount;
    public int GuardHalfHeartsDamage;

    public int HeroHalfHeartsHealth;

    public GraphicsDevice GraphicsDevice;
}
