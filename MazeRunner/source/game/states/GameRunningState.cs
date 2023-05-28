using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
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

    private readonly GameParameters _gameParameters;

    private GraphicsDevice _graphicsDevice;

    private MazeInfo _mazeInfo;

    private SpriteInfo _heroInfo;

    private TextWriterInfo _findKeyTextWriterInfo;

    private readonly TextWriterInfo _openExitTextWriterInfo;

    private HeroCamera _heroCamera;

    private List<SpriteInfo> _enemiesInfo;

    private HashSet<MazeRunnerGameComponent> _gameComponents;

    private List<SpriteInfo> _respawnEnemies;

    private List<MazeRunnerGameComponent> _deadGameComponents;

    public GameRunningState(GameParameters gameParameters)
    {
        _gameParameters = gameParameters;
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
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
        Drawer.BeginDraw(_heroCamera);

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
            if (component is MazeTileInfo tileInfo
             && Vector2.Distance(tileInfo.Position, _heroInfo.Position) < Optimization.GetMazeTileUpdateDistance(tileInfo.MazeTile))
            {
                continue;
            }

            if (component is SpriteInfo spriteInfo)
            {
                var sprite = spriteInfo.Sprite;

                if (sprite is not Hero)
                {
                    var distance = Vector2.Distance(spriteInfo.Position, _heroInfo.Position);

                    if (distance > Optimization.GetEnemyUpdateDistance(spriteInfo))
                    {
                        continue;
                    }

                    if (sprite.IsDead
                     && distance > Optimization.GetEnemyDisposingDistance(spriteInfo))
                    {
                        _deadGameComponents.Add(component);

                        AddEnemyToRespawnList(spriteInfo);
                    }
                }
            }

            if (component is TextWriterInfo textWriterInfo)
            {
                if (textWriterInfo.TextWriter.IsDead)
                {
                    _deadGameComponents.Add(component);
                }
            }

            component.Update(gameTime);
        }

        HandleSecondaryButtons();

        RespawnEnemies();
        DisposeDeadGameComponents();
    }

    private void InitializeComponentsList()
    {
        _respawnEnemies = new List<SpriteInfo>();

        _deadGameComponents = new List<MazeRunnerGameComponent>();

        _gameComponents = new HashSet<MazeRunnerGameComponent>()
        {
            _mazeInfo, _findKeyTextWriterInfo, _heroCamera,
        };

        foreach (var enemyInfo in _enemiesInfo)
        {
            _gameComponents.Add(enemyInfo);
        }

        _gameComponents.Add(_heroInfo);
    }

    private void PreInitializeMaze()
    {
        var maze = MazeGenerator.GenerateMaze(_gameParameters.MazeWidth, _gameParameters.MazeHeight);

        MazeGenerator.MakeCyclic(maze, _gameParameters.MazeDeadEndsRemovePercentage);

        MazeGenerator.InsertTraps(maze, () => new BayonetTrap(), _gameParameters.MazeBayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(maze, () => new DropTrap(), _gameParameters.MazeDropTrapInsertingPercentage);

        MazeGenerator.InsertExit(maze);

        MazeGenerator.InsertItem(maze, new Key());

        maze.InitializeComponentsList();

        _mazeInfo = new MazeInfo(maze);
    }

    private void PostInitializeMaze()
    {
        _mazeInfo.HeroInfo = _heroInfo;
    }

    private void InitializeHero()
    {
        var maze = _mazeInfo.Maze;

        var heroCell = MazeGenerator.GetRandomCell(maze, maze.IsFloor).First();
        var heroPosition = maze.GetCellPosition(heroCell);

        var hero = Hero.GetInstance();

        _heroInfo = new SpriteInfo(hero, heroPosition);

        hero.Initialize(_heroInfo, _mazeInfo, _gameParameters.HeroHalfHeartsHealth);
    }

    private void InitializeEnemies()
    {
        void InitializeGuards()
        {
            for (int i = 0; i < _gameParameters.GuardSpawnCount; i++)
            {
                _enemiesInfo.Add(CreateGuard());
            }
        }

        _enemiesInfo = new List<SpriteInfo>();

        InitializeGuards();
    }

    private void InitializeHeroCamera()
    {
        var heroFrameSize = _heroInfo.Sprite.FrameSize;
        var shadowTreshold = heroFrameSize * _gameParameters.HeroCameraShadowTresholdCoeff;

        _heroCamera = new HeroCamera(_graphicsDevice, shadowTreshold, _heroInfo, _gameParameters.HeroCameraScaleFactor);
    }

    private void InitializeTextWriters()
    {
        void InitializeFindKeyTextWriter()
        {
            var findKeyTextWriter = FindKeyTextWriter.GetInstance();

            _findKeyTextWriterInfo = new TextWriterInfo(findKeyTextWriter);

            findKeyTextWriter.Initialize(_heroInfo, _mazeInfo, _findKeyTextWriterInfo);
        }

        InitializeFindKeyTextWriter();
    }

    private SpriteInfo CreateGuard()
    {
        var guard = new Guard(_gameParameters.GuardHalfHeartsDamage);

        var maze = _mazeInfo.Maze;

        var guardCell = MazeGenerator.GetRandomCell(maze, IsEnemyFreeFloorCell).First();
        var guardPosition = maze.GetCellPosition(guardCell);

        var guardInfo = new SpriteInfo(guard, guardPosition);

        guard.Initialize(guardInfo, _heroInfo, _mazeInfo);

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
        var distanceToHero = Vector2.Distance(_heroInfo.Position, cellPosition);

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

    private void HandleSecondaryButtons()
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            GameStateChanged.Invoke(new GameMenuState());
        }

        if (keyboardState.IsKeyDown(Keys.Tab))
        {
            Environment.Exit(0);
        }
    }
}