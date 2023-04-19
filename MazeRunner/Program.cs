namespace MazeRunner;

internal class Program
{
    public static void Main(string[] args)
    {
        using var game = new MazeRunner.MazeRunnerGame();
        game.Run();
    }
}
