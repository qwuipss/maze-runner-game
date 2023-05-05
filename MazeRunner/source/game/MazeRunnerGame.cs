#region Usings
using MazeRunner.Content;
using MazeRunner.Extensions;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Physics;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
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
        _graphics = new GraphicsDeviceManager(this)
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

        InitializeDrawer();
        InitializeMaze();
        InitializeHero();
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

        CheckDebugButtons();

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

    private void InitializeMaze()
    {
        _maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), 3);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), 2);

        MazeGenerator.InsertExit(_maze);
        MazeGenerator.InsertItem(_maze, new Key());
    }

    private void InitializeDrawer()
    {
        _drawer = Drawer.GetInstance();
        _drawer.Initialize(this);
    }

    private void InitializeHero()
    {
        _hero = new Hero();

        var heroCell = MazeGenerator.GetRandomFloorCell(_maze);

        _heroPosition = new Vector2(heroCell.X * _hero.FrameWidth, heroCell.Y * _hero.FrameHeight);
    }

    private void ProcessHeroMovement(GameTime gameTime)
    {
        var movement = KeyboardManager.ProcessHeroMovement(_hero, gameTime);

        var totalMovement = GetTotalMovement(movement);

        _heroPosition += totalMovement;
        _hero.ProcessPositionChange(totalMovement);
    }

    private Vector2 GetTotalMovement(Vector2 movement)
    {
        static Vector2 NormalizeDiagonalSpeed(Vector2 speed, Vector2 movement)
        {
            if (movement.Abs() == speed)
            {
                return new Vector2((float)(movement.X / Math.Sqrt(2)), (float)(movement.Y / Math.Sqrt(2)));
            }

            return movement;
        }

        var totalMovement = Vector2.Zero;

        var movementX = new Vector2(movement.X, 0);
        var movementY = new Vector2(0, movement.Y);

        if (!CollisionManager.ColidesWithWalls(_hero, _maze, _heroPosition, movementX)
         && !CollisionManager.CollidesWithExit(_hero, _maze, _heroPosition, movementX))
        {
            totalMovement += movementX;
        }

        if (!CollisionManager.ColidesWithWalls(_hero, _maze, _heroPosition, movementY)
         && !CollisionManager.CollidesWithExit(_hero, _maze, _heroPosition, movementY))
        {
            totalMovement += movementY;
        }

        return NormalizeDiagonalSpeed(_hero.Speed, totalMovement);
    }

    private void CheckDebugButtons()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.G)) // generate maze
        {
            InitializeMaze();
        }
        if (Keyboard.GetState().IsKeyDown(Keys.O)) // open exit
        {
            _maze.ExitInfo.Exit.Open();
        }
    }
}