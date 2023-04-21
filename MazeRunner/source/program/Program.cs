namespace MazeRunner;

internal class Program
{
    public static void Main(string[] args)
    {
        MazeGenerator.GenerateMaze(15, 15).LoadToFile(new System.IO.FileInfo("maze.txt"));

        //using var game = new MazeRunnerGame();
        //game.Run();
    }
}
