#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace MazeRunner;

public class Drawer
{
    private static readonly Lazy<Drawer> _instance = new(() => new Drawer());

    private SpriteBatch _spriteBatch;

    private Drawer()
    {
    }

    public static Drawer GetInstance()
    {
        return _instance.Value;
    }

    public void Initialize(Game game)
    {
        _spriteBatch = new(game.GraphicsDevice);
    }

    public void BeginDraw()
    {
        _spriteBatch.Begin();
    }

    public void EndDraw()
    {
        _spriteBatch.End();
    }

    public void DrawMaze(Maze maze, GameTime gameTime)
    {
        for (int x = 0; x < maze.Width; x++)
        {
            for (int y = 0; y < maze.Height; y++)
            {
                var mazeTile = maze[x, y];

                _spriteBatch.Draw(
                    mazeTile.Texture,
                    new Vector2(x * mazeTile.FrameWidth, y * mazeTile.FrameHeight),
                    new Rectangle(mazeTile.GetCurrentAnimationFrame(gameTime),
                                  new Point(mazeTile.FrameWidth, mazeTile.FrameHeight)),
                    Color.White);
            }
        }
    }
}