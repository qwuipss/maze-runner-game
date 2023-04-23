#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            
            PreferredBackBufferWidth = 17 * 16,
            PreferredBackBufferHeight = 9 * 16
        };

        _graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _maze = MazeGenerator.GenerateMaze(17, 9);
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
                if (_maze[x, y] is CellType.Empty)
                {
                    _spriteBatch.Draw(floor, new Rectangle(x * 16, y * 16, 16, 16), Color.White);
                }
                else if (_maze[x, y] is CellType.Wall)
                {
                    _spriteBatch.Draw(wall, new Rectangle(x * 16, y * 16, 16, 16), Color.White);
                }
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}