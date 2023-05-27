using MazeRunner.GameBase;

namespace MazeRunner;

internal class Program
{
    public static void Main(string[] args)
    {
        using var game = new MazeRunnerGame();
        game.Run();
    }
}
