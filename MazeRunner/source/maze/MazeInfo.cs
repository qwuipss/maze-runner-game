namespace MazeRunner.MazeBase;

public record class MazeInfo(Maze Maze)
{
    public bool KeyCollected { get; set; }
}