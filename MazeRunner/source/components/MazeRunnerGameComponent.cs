using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Components;

public abstract class MazeRunnerGameComponent
{
    public Vector2 Position { get; set; }

    public static bool IsInArea(Rectangle area, MazeRunnerGameComponent component)
    {
        var cell = Maze.GetCellByPosition(component.Position);

        if (cell.X >= area.Left && cell.X <= area.Right && cell.Y >= area.Top)
        {
            return cell.Y <= area.Bottom;
        }

        return false;
    }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
}