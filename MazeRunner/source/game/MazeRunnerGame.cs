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
    public GraphicsDeviceManager Graphics { get; init; }
    #endregion

    #region MazeData
    public MazeInfo MazeInfo { get; private set; }
    #endregion

    #region HeroData
    public SpriteInfo HeroInfo { get; private set; }
    #endregion

    #region EnemiesData
    private HashSet<SpriteInfo> _enemiesInfo;
    #endregion

    #region FindKeyTextWriterData
    public TextWriterInfo FindKeyTextWriterInfo { get; private set; }
    #endregion

    #region CameraData
    private HeroCamera _heroCamera;
    #endregion

    #region GameComponentsData
    private HashSet<MazeRunnerGameComponent> _gameComponents;
    private List<MazeRunnerGameComponent> _deadGameComponents;
    #endregion

    public MazeRunnerGame()
    {
        IsFixedTimeStep = false;
        IsMouseVisible = true;
        Content.RootDirectory = "Content";

        Graphics = new GraphicsDeviceManager(this);
    }

    #region GameBase
    protected override void Initialize()
    {
        base.Initialize();

        SetFullScreen();

        InitializeMaze();
        InitializeHero();
        InitializeHeroCamera();
        InitializeEnemies();
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
        foreach (var component in _gameComponents)
        {
            component.Update(this, gameTime);
        }

        DisposeDeadGameComponents();

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
        Graphics.IsFullScreen = true;

        Graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        Graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        Graphics.ApplyChanges();
    }

    private void InitializeComponentsList()
    {
        _deadGameComponents = new List<MazeRunnerGameComponent>();

        _gameComponents = new HashSet<MazeRunnerGameComponent>()
        {
            MazeInfo, HeroInfo, FindKeyTextWriterInfo, _heroCamera,
        };

        foreach (var enemyInfo in _enemiesInfo)
        {
            _gameComponents.Add(enemyInfo);
        }

        foreach (var component in _gameComponents)
        {
            component.NeedDisposeNotify += AddGameComponentToDisposeList;
        }
    }

    private void InitializeMaze()
    {
        var maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);

        MazeGenerator.MakeCyclic(maze, 50);

        MazeGenerator.InsertTraps(maze, () => new BayonetTrap(), 7);
        MazeGenerator.InsertTraps(maze, () => new DropTrap(), 7);

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

        var heroCell = MazeGenerator.GetRandomCell(maze, maze.IsFloor).First();
        var heroPosition = maze.GetCellPosition(heroCell);

        var hero = Hero.GetInstance();

        HeroInfo = new SpriteInfo(hero, heroPosition);

        hero.Initialize(this);
    }

    private void InitializeEnemies()
    {
        void InitializeGuards(Maze maze, int guardsCount)
        {
            bool IsEnemyFreeFloorCell(Cell cell)
            {
                const float spawnDistanceCoeff = 3;

                if (!maze.IsFloor(cell))
                {
                    return false;
                }

                var cellPosition = maze.GetCellPosition(cell);
                var distanceToHero = Vector2.Distance(HeroInfo.Position, cellPosition);

                if (distanceToHero <= spawnDistanceCoeff)
                {
                    return false;
                }

                var noSpawnRadius = maze.Skeleton[cell.Y, cell.X].FrameSize * spawnDistanceCoeff;
                var isEnemyFree = _enemiesInfo.Where(enemyInfo => Vector2.Distance(enemyInfo.Position, cellPosition) <= noSpawnRadius).Count() is 0;

                return isEnemyFree;
            }

            for (int i = 0; i < guardsCount; i++)
            {
                var guard = new Guard();

                var guardCell = MazeGenerator.GetRandomCell(maze, IsEnemyFreeFloorCell).First();
                var guardPosition = maze.GetCellPosition(guardCell);

                var guardInfo = new SpriteInfo(guard, guardPosition);

                guard.Initialize(this, guardInfo);

                _enemiesInfo.Add(guardInfo);
            }
        }

        _enemiesInfo = new HashSet<SpriteInfo>();

        var guardsCount = 1;

        InitializeGuards(MazeInfo.Maze, guardsCount);
    }

    private void InitializeHeroCamera()
    {
        var scaleFactor = 7;
        var heroFrameSize = HeroInfo.Sprite.FrameSize;
        var shadowTreshold = heroFrameSize * 2.4f;

        _heroCamera = new HeroCamera(GraphicsDevice.Viewport, shadowTreshold, GraphicsDevice, scaleFactor);
    }

    private void InitializeTextWriters()
    {
        var findKeyTextWriter = FindKeyTextWriter.GetInstance();
        findKeyTextWriter.Initialize(this);

        FindKeyTextWriterInfo = new TextWriterInfo(findKeyTextWriter);
    }
    #endregion

    private void AddGameComponentToDisposeList(MazeRunnerGameComponent component)
    {
        _deadGameComponents.Add(component);
    }

    private void DisposeDeadGameComponents()
    {
        if (_deadGameComponents.Count is not 0)
        {
            foreach (var component in _deadGameComponents)
            {
                _gameComponents.Remove(component);
            }
        }

        _deadGameComponents.Clear();
    }

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