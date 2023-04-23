#region Usings
using static MazeRunner.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
#endregion

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Maze _maze;

    private Texture2D wall;
    private Texture2D floor;

    public MazeRunnerGame()
    {
        _graphics = new(this)
        {
            PreferredBackBufferWidth = WindowWidth,
            PreferredBackBufferHeight = WindowHeight,
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);
        _maze.LoadToFile(new System.IO.FileInfo("maze.txt"));
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        wall = Content.Load<Texture2D>("wall");
        floor = Content.Load<Texture2D>("floor");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _spriteBatch.Begin();

        for (int x = 0; x < _maze.Width; x++)
        {
            for (int y = 0; y < _maze.Height; y++)
            {
                Texture2D texture;

                if (_maze[x, y] is CellType.Empty)
                {
                    texture = floor;
                }
                else if (_maze[x, y] is CellType.Wall)
                {
                    texture = wall;
                }
                else
                {
                    throw new NotImplementedException();
                }

                _spriteBatch.Draw(texture, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), Color.White);
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}