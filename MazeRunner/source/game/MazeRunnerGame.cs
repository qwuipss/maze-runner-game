#region Usings
using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MazeRunner.Settings;
#endregion

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    private GraphicsDeviceManager _graphics;

    private Drawer _drawer;

    private Maze _maze;

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

        _drawer = Drawer.GetInstance();
        _drawer.Initialize(this);

        _maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), 3);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), 2);

        _maze.LoadToFile(new System.IO.FileInfo("maze.txt"));
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
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

        _drawer.BeginDraw();

        _drawer.DrawMaze(_maze, gameTime);

        _drawer.EndDraw();

        base.Draw(gameTime);
    }
}