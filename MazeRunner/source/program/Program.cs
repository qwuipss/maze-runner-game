namespace MazeRunner;

internal class Program
{
    public static void Main(string[] args)
    {
        MazeGenerator.GenerateMaze(15, 10);

        //using var game = new MazeRunnerGame();
        //game.Run();
    }
}
