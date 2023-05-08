using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static MazeRunner.Settings;

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    #region GraphicsData
    private readonly GraphicsDeviceManager _graphics;
    #endregion

    #region MazeData
    public Maze Maze { get; private set; }
    public bool MazeKeyCollected { get; private set; }
    #endregion

    #region SpritesData
    public Dictionary<Sprite, Vector2> SpritesPositions { get; init; }
    #endregion

    #region HeroData
    public Hero Hero { get; private set; }
    #endregion

    #region CameraData
    private HeroCamera _heroCamera;
    #endregion

    #region GameComponentsList
    private List<MazeRunnerGameComponent> _components;
    #endregion

    #region TextWritersData
    public Dictionary<TextWriter, Vector2> TextWritersPositions { get; private set; }
    #endregion

    #region FindKeyTextData
    private FindKeyTextWriter _findKeyTextWriter;
    #endregion

    public MazeRunnerGame()
    {
        IsMouseVisible = true;

        Content.RootDirectory = "Content";

        _graphics = new GraphicsDeviceManager(this);

        SpritesPositions = new Dictionary<Sprite, Vector2>();
        TextWritersPositions = new Dictionary<TextWriter, Vector2>();
    }

    #region GameBase
    protected override void Initialize()
    {
        base.Initialize();

        SetFullScreen();

        InitializeCamera();
        InitializeMaze();
        InitializeHero();

        InitializeTextWriters();

        InitializeComponentsList();

        InitializeDrawer();
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
        Fonts.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (KeyboardManager.IsPollingTimePassed(gameTime))
        {
            ProcessHeroMovement();
            ProcessHeroItemsColliding();

            CheckDebugButtons();
        }

        foreach (var component in _components)
        {
            component.Update(this, gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        Drawer.BeginDraw(_heroCamera);

        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();

        base.Draw(gameTime);
    }
    #endregion

    #region Initializers
    private void SetFullScreen()
    {
        _graphics.IsFullScreen = true;

        _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        _graphics.ApplyChanges();
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>()
        {
            Maze, Hero, _findKeyTextWriter, _heroCamera,
        };
    }

    private void InitializeMaze()
    {
        Maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);

        MazeGenerator.InsertTraps(Maze, () => new BayonetTrap(), 3);
        MazeGenerator.InsertTraps(Maze, () => new DropTrap(), 2);

        MazeGenerator.InsertExit(Maze);

        MazeGenerator.InsertItem(Maze, new Key());
    }

    private void InitializeDrawer()
    {
        Drawer.Initialize(this);
    }

    private void InitializeHero()
    {
        Hero = new Hero();

        var heroCell = MazeGenerator.GetRandomFloorCell(Maze);

        var heroPosition = Maze.GetCellPosition(heroCell);

        SpritesPositions.Add(Hero, heroPosition);
    }

    private void InitializeCamera()
    {
        _heroCamera = new HeroCamera(GraphicsDevice.Viewport, 7);
    }

    private void InitializeTextWriters()
    {
        _findKeyTextWriter = FindKeyTextWriter.GetInstance();
        TextWritersPositions.Add(_findKeyTextWriter, Vector2.Zero);

        _findKeyTextWriter.Initialize(this);
    }
    #endregion

    #region HeroCollisionCheckers
    private void ProcessHeroItemsColliding()
    {
        var heroPosition = SpritesPositions[Hero];

        if (CollisionManager.CollidesWithItems(Hero, heroPosition, Maze, out var itemInfo))
        {
            var (coords, item) = itemInfo;

            if (item is Key key)
            {
                ProcessHeroKeyColliding(coords, key);
            }
        }
    }

    private void ProcessHeroKeyColliding(Cell coords, Key key)
    {
        var heroPosition = SpritesPositions[Hero];

        if (CollisionManager.CollidesWithKey(Hero, heroPosition, coords, key))
        {
            Maze.RemoveItem(coords);
            MazeKeyCollected = true;
        }
    }

    private void ProcessHeroMovement()
    {
        var movement = KeyboardManager.ProcessHeroMovement(Hero);

        var totalMovement = GetTotalMovement(movement);

        SpritesPositions[Hero] += totalMovement;
        Hero.ProcessPositionChange(totalMovement);
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

        var heroPosition = SpritesPositions[Hero];

        var totalMovement = Vector2.Zero;

        var movementX = new Vector2(movement.X, 0);
        var movementY = new Vector2(0, movement.Y);

        if (!CollisionManager.ColidesWithWalls(Hero, heroPosition, Maze, movementX)
         && !CollisionManager.CollidesWithExit(Hero, heroPosition, Maze, movementX))
        {
            totalMovement += movementX;
        }

        if (!CollisionManager.ColidesWithWalls(Hero, heroPosition, Maze, movementY)
         && !CollisionManager.CollidesWithExit(Hero, heroPosition, Maze, movementY))
        {
            totalMovement += movementY;
        }

        if (ProcessDiagonalMovement(totalMovement, movementX, movementY, out totalMovement))
        {
            return totalMovement;
        }

        return NormalizeDiagonalSpeed(Hero.Speed, totalMovement);
    }

    private bool ProcessDiagonalMovement(Vector2 movement, Vector2 movementX, Vector2 movementY, out Vector2 totalMovement)
    {
        var heroPosition = SpritesPositions[Hero];

        if (CollisionManager.ColidesWithWalls(Hero, heroPosition, Maze, movement))
        {
            if (RandomHelper.RandomBoolean())
            {
                totalMovement = movementX;
            }
            else
            {
                totalMovement = movementY;
            }

            return true;
        }

        totalMovement = movement;

        return false;
    }
    #endregion

    private void CheckDebugButtons()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // exit
        {
            Exit();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.O)) // open exit
        {
            if (MazeKeyCollected)
            {
                Maze.ExitInfo.Exit.Open();
            }
        }
    }
}