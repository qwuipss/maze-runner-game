#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
#endregion

namespace MazeRunner;

public class Drawer
{
    private static readonly Lazy<Drawer> _instance = new(() => new Drawer());

    private const int MazeTileWidth = Settings.MazeTileWidth;
    private const int MazeTileHeight = Settings.MazeTileHeight;

    private SpriteBatch _spriteBatch;

    private Dictionary<CellType, Texture2D> _mazeTextures;

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

    public void LoadContent(Game game)
    {
        _mazeTextures = new()
        {
            { CellType.Floor, game.Content.Load<Texture2D>("floor") },
            { CellType.Wall, game.Content.Load<Texture2D>("wall") },
        };
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
                if (_mazeTextures.TryGetValue(maze[x, y], out var texture))
                {
                    _spriteBatch.Draw(texture, new Rectangle(x * MazeTileWidth, y * MazeTileHeight, MazeTileWidth, MazeTileHeight), Color.White);
                }
                else
                {
                    throw new NotImplementedException($"texture for {maze[x, y]} is not loaded");
                }
            }
        }
    }
}
