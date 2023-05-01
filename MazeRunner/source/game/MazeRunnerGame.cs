#region Usings
using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using static MazeRunner.Settings;
#endregion

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    private GraphicsDeviceManager _graphics;

    private Drawer _drawer;

    private Maze _maze;

    private Hero _hero;

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

        _hero = new Hero(new Vector2(16,16));
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (KeyboardManager.IsPollingTimePassed(gameTime))
        {
            var movement = KeyboardManager.ProcessHeroMovement(_hero, gameTime);
            if (!CheckMazeCollisions())
            {
                _hero.Position += movement;
            }
        }

        CheckMazeCollisions();

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

    private bool CheckMazeCollisions()
    {
        var skeleton = _maze.Skeleton;

        var position = _hero.Position;

        for (int y = 0; y < skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < skeleton.GetLength(1); x++)
            {
                var tile = skeleton[y, x];

                if (tile is Floor)
                {
                    continue;
                }

                var tileHitBox = new Rectangle(
                    x * tile.FrameWidth, 
                    y * tile.FrameHeight, 
                    tile.FrameWidth, 
                    tile.FrameHeight);

                if (tileHitBox.Intersects(_hero.HitBox))
                {
                    return true;
                }

            }
        }

        return false;
    }
}