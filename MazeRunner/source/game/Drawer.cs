#region Usings
using MazeRunner.MazeBase;
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
        for (int y = 0; y < maze.Tiles.GetLength(0); y++)
        {
            for (int x = 0; x < maze.Tiles.GetLength(1); x++)
            {
                var mazeTile = maze.Tiles[y, x];

                _spriteBatch.Draw(
                    mazeTile.Texture,
                    new Vector2(x * mazeTile.Width, y * mazeTile.Height),
                    new Rectangle(mazeTile.GetCurrentAnimationPoint(gameTime),
                                  new Point(mazeTile.Width, mazeTile.Height)),
                    Color.White);
            }
        }
    }
}