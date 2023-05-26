using Microsoft.Xna.Framework;

namespace MazeRunner.GameBase.States;

public interface IGameState
{
    public IGameState ProcessState(GameTime gameTime);

    public void Draw(GameTime gameTime);
}