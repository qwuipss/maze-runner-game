using Microsoft.Xna.Framework;

namespace MazeRunner.Components;

public abstract class MazeRunnerGameComponent
{
    public delegate void GameComponentProvider(MazeRunnerGameComponent component);

    public abstract event GameComponentProvider NeedDisposeNotify;

    public abstract void Update(MazeRunnerGame game, GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
}