#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
#endregion

namespace MazeRunner;

public class Drawer
{
    private static readonly Lazy<Drawer> _instance = new(() => new Drawer());

    private const int MazeTileWidth = Settings.MazeTileWidth;
    private const int MazeTileHeight = Settings.MazeTileHeight;

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

    public void DrawMaze(Maze maze)
    {
        for (int x = 0; x < maze.Width; x++)
        {
            for (int y = 0; y < maze.Height; y++)
            {
                _spriteBatch.Draw(
                    maze[x, y].Texture, 
                    new Rectangle(x * MazeTileWidth, y * MazeTileHeight, MazeTileWidth, MazeTileHeight), 
                    Color.White);
            }
        }
    }
}
