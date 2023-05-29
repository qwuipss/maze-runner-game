using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.GameBase.States;

public interface IGameState
{
    public event Action<IGameState> GameStateChanged;

    public void Initialize(GraphicsDevice graphicsDevice, Game game);

    public void Update(GameTime gameTime);

    public void Draw(GameTime gameTime);
}