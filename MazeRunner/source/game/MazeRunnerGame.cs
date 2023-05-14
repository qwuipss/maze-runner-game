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
using System.Linq;
using static MazeRunner.Settings;

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    #region GraphicsData
    private readonly GraphicsDeviceManager _graphics;
    #endregion

    #region MazeData
    public MazeInfo MazeInfo { get; private set; }
    #endregion

    #region HeroData
    public SpriteInfo HeroInfo { get; private set; }
    #endregion

    #region FindKeyTextWriterData
    public TextWriterInfo FindKeyTextWriterInfo { get; private set; }
    #endregion

    #region CameraData
    private HeroCamera _heroCamera;
    #endregion

    #region GameComponentsList
    private List<MazeRunnerGameComponent> _gameComponents;
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
        void RemoveDeadGameComponents() //
        {
            _gameComponents = _gameComponents.Where(component => !component.IsDead).ToList();
        }

        foreach (var component in _gameComponents)
        {
            component.Update(this, gameTime);
        }

        RemoveDeadGameComponents();

        CheckDebugButtons();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Drawer.BeginDraw(_heroCamera);

        foreach (var component in _gameComponents)
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
        _gameComponents = new List<MazeRunnerGameComponent>()
        {
            MazeInfo, HeroInfo, FindKeyTextWriterInfo, _heroCamera,
        };
    }

    private void InitializeMaze()
    {
        var maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);

        MazeGenerator.InsertTraps(maze, () => new BayonetTrap(), 3);
        MazeGenerator.InsertTraps(maze, () => new DropTrap(), 2);

        MazeGenerator.InsertExit(maze);

        MazeGenerator.InsertItem(maze, new Key());

        maze.InitializeComponentsList();

        MazeInfo = new MazeInfo(maze);
    }

    private void InitializeDrawer()
    {
        Drawer.Initialize(this);
    }

    private void InitializeHero()
    {
        var maze = MazeInfo.Maze;

        var heroCell = MazeGenerator.GetRandomFloorCell(maze);
        var heroPosition = maze.GetCellPosition(heroCell);

        var hero = Hero.GetInstance();

        HeroInfo = new SpriteInfo(hero, heroPosition);
    }

    private void InitializeHeroCamera()
    {
        var scaleFactor = 7;

        _heroCamera = new HeroCamera(GraphicsDevice.Viewport, scaleFactor);
    }

    private void InitializeTextWriters()
    {
        var findKeyTextWriter = FindKeyTextWriter.GetInstance();
        findKeyTextWriter.Initialize(this);

        FindKeyTextWriterInfo = new TextWriterInfo(findKeyTextWriter);
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
                MazeInfo.Maze.ExitInfo.Exit.Open();
            }
        }
    }
}