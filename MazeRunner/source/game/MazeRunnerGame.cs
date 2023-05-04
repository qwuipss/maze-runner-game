#region Usings
using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Physics;
using MazeRunner.Sprites;
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

    private Hero _hero;
    private Vector2 _heroPosition;

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

        MazeGenerator.InsertExit(_maze);

        _hero = new Hero();
        _heroPosition = new Vector2(16, 16);
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (KeyboardManager.IsPollingTimePassed(gameTime))
        {
            ProcessHeroMovement(gameTime);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.G))
        {
            _maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);
            MazeGenerator.InsertExit(_maze);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _drawer.BeginDraw();

        _drawer.DrawMaze(_maze, gameTime);
        _drawer.DrawSprite(_hero, _heroPosition, gameTime);

        _drawer.EndDraw();

        base.Draw(gameTime);
    }

    private void ProcessHeroMovement(GameTime gameTime)
    {
        var totalMovement = Vector2.Zero;

        var movement = KeyboardManager.ProcessHeroMovement(_hero, gameTime);

        var movementX = new Vector2(movement.X, 0);
        var movementY = new Vector2(0, movement.Y);

        if (!CollisionManager.ColidesWithWalls(_hero, _maze, _heroPosition, movementX))
        {
            totalMovement += movementX;
        }

        if (!CollisionManager.ColidesWithWalls(_hero, _maze, _heroPosition, movementY))
        {
            totalMovement += movementY;
        }

        _heroPosition += totalMovement;
        _hero.ProcessPositionChange(totalMovement);
    }
}