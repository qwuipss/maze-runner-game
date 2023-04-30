#region Usings
using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MazeRunner.Settings;
#endregion

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    private const int KeyboardPollingDelayMs = 50;

    private GraphicsDeviceManager _graphics;

    private Drawer _drawer;

    private Maze _maze;

    private Hero _hero;

    private double _elapsedGameTimeMs;

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

        _hero = new Hero();
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        _elapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_elapsedGameTimeMs >= KeyboardPollingDelayMs)
        {
            _elapsedGameTimeMs -= KeyboardPollingDelayMs;
        }
        else
        {
            return;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            _hero.Position = new Vector2(_hero.Position.X, _hero.Position.Y - _hero.Speed.Y);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            _hero.Position = new Vector2(_hero.Position.X, _hero.Position.Y + _hero.Speed.Y);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            _hero.Position = new Vector2(_hero.Position.X - _hero.Speed.X, _hero.Position.Y);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            _hero.Position = new Vector2(_hero.Position.X + _hero.Speed.X, _hero.Position.Y);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _drawer.BeginDraw();

        _drawer.DrawMaze(_maze, gameTime);
        _drawer.DrawSprite(_hero, gameTime);

        _drawer.EndDraw();

        base.Draw(gameTime);
    }
}