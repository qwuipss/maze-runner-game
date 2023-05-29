using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.GameBase.States;

public class GameRunningState : IGameState
{
    public event Action<IGameState> GameStateChanged;

    private const double StateSwitchAfterHeroDeadDelayMs = 2000;

    private static Texture2D _cameraEffect;

    private double _heroAfterDeadElapsedTimeMs;

    private bool _isGameOver;

    private GraphicsDevice _graphicsDevice;

    private MazeInfo _mazeInfo;

    private TextWriterInfo _findKeyTextWriterInfo;

    private List<SpriteInfo> _enemiesInfo;

    private HashSet<MazeRunnerGameComponent> _gameComponents;

    private List<SpriteInfo> _respawnEnemies;

    private List<MazeRunnerGameComponent> _deadGameComponents;

    public GameParameters GameParameters { get; init; }

    public bool IsControlling { get; set; }

    public SpriteInfo HeroInfo { get; private set; }

    public HeroCamera HeroCamera { get; private set; }

    public GameRunningState(GameParameters gameParameters)
    {
        GameParameters = gameParameters;

        IsControlling = true;
    }

    public void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        if (game.IsMouseVisible)
        {
            game.IsMouseVisible = false;
        }

        if (_graphicsDevice is not null)
        {
            return;
        }

        _graphicsDevice = graphicsDevice;

        PreInitializeMaze();
        InitializeHero();
        PostInitializeMaze();
        InitializeHeroCamera();
        InitializeEnemies();
        InitializeTextWriters();
        InitializeComponentsList();
    }

    public void Draw(GameTime gameTime)
    {
        Drawer.BeginDraw(HeroCamera);

        foreach (var component in _gameComponents)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var component in _gameComponents)
        {
            if (component is MazeTileInfo tileInfo)
            {
                UpdateMazeTileInfo(tileInfo, gameTime);
                continue;
            }

            if (component is SpriteInfo spriteInfo)
            {
                UpdateSpriteInfo(spriteInfo, gameTime);
                continue;
            }

            if (component is TextWriterInfo textWriterInfo)
            {
                UpdateTextWriterInfo(textWriterInfo, gameTime);
                continue;
            }

            component.Update(gameTime);
        }

        if (!IsControlling)
        {
            return;
        }

        HandleSecondaryButtons(gameTime);

        RespawnEnemies();
        DisposeDeadGameComponents();
    }

    private void InitializeComponentsList()
    {
        _respawnEnemies = new List<SpriteInfo>();

        _deadGameComponents = new List<MazeRunnerGameComponent>();

        _gameComponents = new HashSet<MazeRunnerGameComponent>()
        {
            _mazeInfo, _findKeyTextWriterInfo, HeroCamera,
        };

        foreach (var enemyInfo in _enemiesInfo)
        {
            _gameComponents.Add(enemyInfo);
        }

        _gameComponents.Add(HeroInfo);
    }

    private void PreInitializeMaze()
    {
        var maze = MazeGenerator.GenerateMaze(GameParameters.MazeWidth, GameParameters.MazeHeight);

        MazeGenerator.MakeCyclic(maze, GameParameters.MazeDeadEndsRemovePercentage);

        MazeGenerator.InsertTraps(maze, () => new BayonetTrap(), GameParameters.MazeBayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(maze, () => new DropTrap(), GameParameters.MazeDropTrapInsertingPercentage);

        MazeGenerator.InsertExit(maze);

        MazeGenerator.InsertItem(maze, new Key());

        maze.InitializeComponentsList();

        _mazeInfo = new MazeInfo(maze);
    }

    private void PostInitializeMaze()
    {
        _mazeInfo.HeroInfo = HeroInfo;
    }

    private void InitializeHero()
    {
        var maze = _mazeInfo.Maze;

        var heroCell = MazeGenerator.GetRandomCell(maze, maze.IsFloor).First();
        var heroPosition = maze.GetCellPosition(heroCell);

        var hero = Hero.GetInstance();

        HeroInfo = new SpriteInfo(hero, heroPosition);

        hero.Initialize(HeroInfo, _mazeInfo, GameParameters.HeroHalfHeartsHealth);
    }

    private void InitializeEnemies()
    {
        void InitializeGuards()
        {
            for (int i = 0; i < GameParameters.GuardSpawnCount; i++)
            {
                _enemiesInfo.Add(CreateGuard());
            }
        }

        _enemiesInfo = new List<SpriteInfo>();

        InitializeGuards();
    }

    private void InitializeHeroCamera()
    {
        if (_cameraEffect is null)
        {
            var viewPort = _graphicsDevice.Viewport;

            var viewWidth = viewPort.Width;
            var viewHeight = viewPort.Height;

            var heroFrameSize = HeroInfo.Sprite.FrameSize;
            var shadowTreshold = heroFrameSize * GameParameters.HeroCameraShadowTresholdCoeff;

            _cameraEffect = EffectsHelper.CreateGradientCircleEffect(viewWidth, viewHeight, shadowTreshold, _graphicsDevice);
        }

        HeroCamera = new HeroCamera(_graphicsDevice, HeroInfo, GameParameters.HeroCameraScaleFactor)
        {
            Effect = _cameraEffect,
        };
    }

    private void InitializeTextWriters()
    {
        void InitializeFindKeyTextWriter()
        {
            var findKeyTextWriter = FindKeyTextWriter.GetInstance();

            _findKeyTextWriterInfo = new TextWriterInfo(findKeyTextWriter);

            findKeyTextWriter.Initialize(HeroInfo, _mazeInfo, _findKeyTextWriterInfo);
        }

        InitializeFindKeyTextWriter();
    }

    private SpriteInfo CreateGuard()
    {
        var guard = new Guard();

        var maze = _mazeInfo.Maze;

        var guardCell = MazeGenerator.GetRandomCell(maze, IsEnemyFreeFloorCell).First();
        var guardPosition = maze.GetCellPosition(guardCell);

        var guardInfo = new SpriteInfo(guard, guardPosition);

        guard.Initialize(guardInfo, HeroInfo, _mazeInfo);

        return guardInfo;
    }

    private bool IsEnemyFreeFloorCell(Cell cell)
    {
        var maze = _mazeInfo.Maze;

        if (!maze.IsFloor(cell))
        {
            return false;
        }

        var mazeTile = maze.Skeleton[cell.Y, cell.X];
        var cellPosition = maze.GetCellPosition(cell);
        var distanceToHero = Vector2.Distance(HeroInfo.Position, cellPosition);

        var spawnDistance = Optimization.GetEnemySpawnDistance(mazeTile);

        if (distanceToHero < spawnDistance)
        {
            return false;
        }

        var isEnemyFree = _enemiesInfo
            .Where(enemyInfo => Vector2.Distance(enemyInfo.Position, cellPosition) < spawnDistance)
            .Count() is 0;

        return isEnemyFree;
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

    private void AddEnemyToRespawnList(SpriteInfo enemyInfo)
    {
        if (enemyInfo.Sprite is Guard)
        {
            _respawnEnemies.Add(CreateGuard());
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private void RespawnEnemies()
    {
        foreach (var enemyInfo in _respawnEnemies)
        {
            _gameComponents.Add(enemyInfo);
        }

        _respawnEnemies.Clear();
    }

    private void HandleSecondaryButtons(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (KeyboardManager.IsGamePauseSwitched(gameTime))
        {
            GameStateChanged.Invoke(new GamePausedState(this));

            GameStateChanged = null;
        }
    }

    private void UpdateSpriteInfo(SpriteInfo spriteInfo, GameTime gameTime)
    {
        void ProcessStateControl(Sprite hero, GameTime gameTime)
        {
            if (hero.IsDead && !_isGameOver)
            {
                _isGameOver = true;
            }

            if (_isGameOver && IsControlling)
            {
                _heroAfterDeadElapsedTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_heroAfterDeadElapsedTimeMs > StateSwitchAfterHeroDeadDelayMs)
                {
                    GameStateChanged.Invoke(new GameOverState(this));
                }
            }
        }

        var sprite = spriteInfo.Sprite;

        if (sprite is not Hero)
        {
            var distance = Vector2.Distance(spriteInfo.Position, HeroInfo.Position);

            if (distance > Optimization.GetEnemyUpdateDistance(spriteInfo))
            {
                return;
            }

            if (sprite.IsDead
             && distance > Optimization.GetEnemyDisposingDistance(spriteInfo))
            {
                _deadGameComponents.Add(spriteInfo);

                AddEnemyToRespawnList(spriteInfo);
            }
        }
        else
        {
            ProcessStateControl(sprite, gameTime);
        }

        spriteInfo.Update(gameTime);
    }

    private void UpdateMazeTileInfo(MazeTileInfo tileInfo, GameTime gameTime)
    {
        var distance = Vector2.Distance(tileInfo.Position, HeroInfo.Position);

        if (distance < Optimization.GetMazeTileUpdateDistance(tileInfo.MazeTile))
        {
            tileInfo.Update(gameTime);
        }
    }

    private void UpdateTextWriterInfo(TextWriterInfo writerInfo, GameTime gameTime)
    {
        if (writerInfo.TextWriter.IsDead)
        {
            _deadGameComponents.Add(writerInfo);
        }

        writerInfo.Update(gameTime);
    }
}