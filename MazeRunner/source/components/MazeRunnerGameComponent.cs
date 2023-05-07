using Microsoft.Xna.Framework;

namespace MazeRunner.Components;

public abstract class MazeRunnerGameComponent
{
    public abstract void Update(MazeRunnerGame game, GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
}