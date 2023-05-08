using MazeRunner.MazeBase;

namespace MazeRunner.Wrappers;

public record class MazeInfo(Maze Maze)
{
    public bool KeyCollected { get; set; }
}