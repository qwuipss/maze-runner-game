using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.GameBase.States;

public class GameRunningState : IGameState
{
    private readonly GameParameters _gameParameters;

    public MazeInfo MazeInfo { get; private set; }

    public SpriteInfo HeroInfo { get; private set; }

    public TextWriterInfo FindKeyTextWriterInfo { get; private set; }

    private HeroCamera _heroCamera;

    private HashSet<SpriteInfo> _enemiesInfo;

    private HashSet<MazeRunnerGameComponent> _gameComponents;

    private List<MazeRunnerGameComponent> _deadGameComponents;

    public GameRunningState(GameParameters gameParameters)
    {
        _gameParameters = gameParameters;

        InitializeMaze();
        InitializeHero();
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

    public IGameState ProcessState(GameTime gameTime)
    {
        foreach (var component in _gameComponents)
        {
            component.Update(this, gameTime);
        }

        DisposeDeadGameComponents();

        return this;
    }

    private void InitializeComponentsList()
    {
        _deadGameComponents = new List<MazeRunnerGameComponent>();

        _gameComponents = new HashSet<MazeRunnerGameComponent>()
        {
            MazeInfo, FindKeyTextWriterInfo, _heroCamera,
        };

        foreach (var enemyInfo in _enemiesInfo)
        {
            _gameComponents.Add(enemyInfo);
        }

        foreach (var component in _gameComponents)
        {
            component.NeedDisposeNotify += AddGameComponentToDisposeList;
        }

        _gameComponents.Add(HeroInfo);
    }

    private void InitializeMaze()
    {
        var maze = MazeGenerator.GenerateMaze(_gameParameters.MazeWidth, _gameParameters.MazeHeight);

        MazeGenerator.MakeCyclic(maze, _gameParameters.MazeDropTrapInsertingPercentage);

        MazeGenerator.InsertTraps(maze, () => new BayonetTrap(), _gameParameters.MazeBayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(maze, () => new DropTrap(), _gameParameters.MazeDropTrapInsertingPercentage);

        MazeGenerator.InsertExit(maze);

        MazeGenerator.InsertItem(maze, new Key());

        maze.InitializeComponentsList();

        MazeInfo = new MazeInfo(maze);
    }

    private void InitializeHero()
    {
        var maze = MazeInfo.Maze;

        var heroCell = MazeGenerator.GetRandomCell(maze, maze.IsFloor).First();
        var heroPosition = maze.GetCellPosition(heroCell);

        var hero = Hero.GetInstance();

        hero.HalfHeartsHealth = _gameParameters.HeroHalfHeartsHealth;

        HeroInfo = new SpriteInfo(hero, heroPosition);

        hero.Initialize(this);
    }

    private void InitializeEnemies()
    {
        void InitializeGuards(Maze maze, int guardsCount)
        {
            bool IsEnemyFreeFloorCell(Cell cell)
            {
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

            for (int i = 0; i < guardsCount; i++)
            {
                var guard = new Guard(_gameParameters.GuardHalfHeartsDamage);

                var guardCell = MazeGenerator.GetRandomCell(maze, IsEnemyFreeFloorCell).First();
                var guardPosition = maze.GetCellPosition(guardCell);

                var guardInfo = new SpriteInfo(guard, guardPosition);

                guard.Initialize(this, guardInfo);

                _enemiesInfo.Add(guardInfo);
            }
        }

        _enemiesInfo = new HashSet<SpriteInfo>();

        InitializeGuards(MazeInfo.Maze, _gameParameters.GuardSpawnCount);
    }

    private void InitializeHeroCamera()
    {
        var heroFrameSize = HeroInfo.Sprite.FrameSize;
        var shadowTreshold = heroFrameSize * _gameParameters.HeroCameraShadowTresholdCoeff;

        _heroCamera = new HeroCamera(_gameParameters.GraphicsDevice.Viewport, shadowTreshold, 
                                     _gameParameters.GraphicsDevice, _gameParameters.HeroCameraScaleFactor);
    }

    private void InitializeTextWriters()
    {
        var findKeyTextWriter = FindKeyTextWriter.GetInstance();
        findKeyTextWriter.Initialize(this);

        FindKeyTextWriterInfo = new TextWriterInfo(findKeyTextWriter);
    }

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
}