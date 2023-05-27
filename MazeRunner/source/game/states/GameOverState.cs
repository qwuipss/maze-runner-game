using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.GameBase.States;

public class GameOverState : IGameState
{
    public event Action<IGameState> GameStateChanged;

    public void Draw(GameTime gameTime)
    {
        throw new NotImplementedException();
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
        throw new NotImplementedException();
    }

    public void ProcessState(GameTime gameTime)
    {
        throw new NotImplementedException();
    }
}
