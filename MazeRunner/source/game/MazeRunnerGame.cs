using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static MazeRunner.Settings;

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    #region GraphicsData
    private readonly GraphicsDeviceManager _graphics;
    #endregion

    #region MazeData
    private Maze _maze;
    public MazeInfo MazeInfo { get; private set; }
    #endregion

    #region HeroData
    private Hero Hero { get; set; }
    public SpriteInfo HeroInfo { get; private set; }
    #endregion

    #region CameraData
    private HeroCamera _heroCamera;
    #endregion

    #region GameComponentsList
    private List<MazeRunnerGameComponent> _components;
    #endregion

    #region FindKeyTextData
    public TextWriterInfo FindKeyTextWriterInfo;
    private FindKeyTextWriter _findKeyTextWriter;
    #endregion

    public MazeRunnerGame()
    {
        IsMouseVisible = true;

        Content.RootDirectory = "Content";

        _graphics = new GraphicsDeviceManager(this);
    }

    #region GameBase
    protected override void Initialize()
    {
        base.Initialize();

        SetFullScreen();

        InitializeHeroCamera();
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
        foreach (var component in _components)
        {
            component.Update(this, gameTime);
        }

        CheckDebugButtons();

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
            MazeInfo, HeroInfo, FindKeyTextWriterInfo, _heroCamera,
        };
    }

    private void InitializeMaze()
    {
        _maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), 3);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), 2);

        MazeGenerator.InsertExit(_maze);

        MazeGenerator.InsertItem(_maze, new Key());

        _maze.InitializeComponentsList();

        MazeInfo = new MazeInfo(_maze);
    }

    private void InitializeDrawer()
    {
        Drawer.Initialize(this);
    }

    private void InitializeHero()
    {
        var heroCell = MazeGenerator.GetRandomFloorCell(_maze);
        var heroPosition = _maze.GetCellPosition(heroCell);

        Hero = new Hero(this);

        HeroInfo = new SpriteInfo(Hero, heroPosition);
    }

    private void InitializeHeroCamera()
    {
        _heroCamera = new HeroCamera(GraphicsDevice.Viewport, 7);
    }

    private void InitializeTextWriters()
    {
        _findKeyTextWriter = FindKeyTextWriter.GetInstance();

        FindKeyTextWriterInfo = new TextWriterInfo(_findKeyTextWriter, Vector2.Zero);

        _findKeyTextWriter.Initialize(this);
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
            if (MazeInfo.IsKeyCollected)
            {
                _maze.ExitInfo.Exit.Open();
            }
        }
    }
}