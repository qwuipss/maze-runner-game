using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
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

    private Maze _maze;

    private FindKeyTextWriter _findKeyTextWriter;

    private HeroHealthWriter _heroHealthWriter;

    private HeroChalkUsesWriter _heroChalkUsesWriter;

    private List<Enemy> _enemies;

    private HashSet<MazeRunnerGameComponent> _gameComponents;

    private List<Enemy> _respawnEnemies;

    private List<MazeRunnerGameComponent> _deadGameComponents;

    public GameParameters GameParameters { get; init; }

    public bool IsControlling { get; set; }

    public Hero Hero { get; private set; }

    public HeroCamera HeroCamera { get; private set; }

    public GameRunningState(GameParameters gameParameters)
    {
        GameParameters = gameParameters;

        IsControlling = true;
    }

    public static void SwitchCamera(ICamera camera)
    {
        Drawer.EndDraw();

        Drawer.BeginDraw(camera);
    }

    public void ContinueDraw()
    {
        Drawer.EndDraw();

        Drawer.BeginDraw(HeroCamera);
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
            if (component is MazeTile mazeTile)
            {
                UpdateMazeTile(mazeTile, gameTime);
                continue;
            }

            if (component is Sprite sprite)
            {
                UpdateSprite(sprite, gameTime);
                continue;
            }

            if (component is TextWriter textWriter)
            {
                UpdateTextWriter(textWriter, gameTime);
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
        _respawnEnemies = new List<Enemy>();

        _deadGameComponents = new List<MazeRunnerGameComponent>();

        _gameComponents = new HashSet<MazeRunnerGameComponent>()
        {
            _maze, _findKeyTextWriter, HeroCamera,
        };

        foreach (var enemy in _enemies)
        {
            _gameComponents.Add(enemy);
        }

        _gameComponents.Add(Hero);
        _gameComponents.Add(_heroHealthWriter);
        _gameComponents.Add(_heroChalkUsesWriter);
    }

    private void PreInitializeMaze()
    {
        _maze = MazeGenerator.GenerateMaze(GameParameters.MazeWidth, GameParameters.MazeHeight);

        MazeGenerator.MakeCyclic(_maze, GameParameters.MazeDeadEndsRemovePercentage);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), GameParameters.MazeBayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), GameParameters.MazeDropTrapInsertingPercentage);

        MazeGenerator.InsertExit(_maze);

        MazeGenerator.InsertItem(_maze, new Key());

        _maze.InitializeComponentsList();
    }

    private void PostInitializeMaze()
    {
        _maze.PostInitialize(Hero);
    }

    private void InitializeHero()
    {
        var cell = MazeGenerator.GetRandomCell(_maze, _maze.IsFloor).First();
        var position = _maze.GetCellPosition(cell);

        Hero = new Hero(GameParameters.HeroHealth, GameParameters.ChalkUses)
        {
            Position = position,
        };


        Hero.Initialize(_maze);
    }

    private void InitializeEnemies()
    {
        void InitializeGuards()
        {
            for (int i = 0; i < GameParameters.GuardSpawnCount; i++)
            {
                _enemies.Add(CreateGuard());
            }
        }

        _enemies = new List<Enemy>();

        InitializeGuards();
    }

    private void InitializeHeroCamera()
    {
        void InitializeCameraEffect()
        {
            var viewPort = _graphicsDevice.Viewport;

            var viewWidth = viewPort.Width;
            var viewHeight = viewPort.Height;

            var heroFrameSize = Hero.FrameSize;
            var shadowTreshold = heroFrameSize * 2.4f;

            _cameraEffect = EffectsHelper.CreateGradientCircleEffect(viewWidth, viewHeight, shadowTreshold, _graphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        HeroCamera = new HeroCamera(_graphicsDevice, Hero)
        {
            Effect = _cameraEffect,
        };
    }

    private void InitializeTextWriters()
    {
        _findKeyTextWriter = new FindKeyTextWriter(Hero, _maze);

        var scaleDivider = 450;

        _heroHealthWriter = new HeroHealthWriter(Hero, this, scaleDivider, _graphicsDevice);

        _heroChalkUsesWriter = new HeroChalkUsesWriter(Hero, this, _heroHealthWriter, scaleDivider, _graphicsDevice);
    }

    private Guard CreateGuard()
    {
        var cell = MazeGenerator.GetRandomCell(_maze, IsEnemyFreeFloorCell).First();
        var position = _maze.GetCellPosition(cell);

        var guard = new Guard()
        {
            Position = position,
        };

        guard.Initialize(Hero, _maze);

        return guard;
    }

    private bool IsEnemyFreeFloorCell(Cell cell)
    {
        if (!_maze.IsFloor(cell))
        {
            return false;
        }

        var cellPosition = _maze.GetCellPosition(cell);
        var distanceToHero = Vector2.Distance(Hero.Position, cellPosition);

        var mazeTile = _maze.Skeleton[cell.Y, cell.X];

        var spawnDistance = Optimization.GetEnemySpawnDistance(mazeTile);

        if (distanceToHero < spawnDistance)
        {
            return false;
        }

        var isEnemyFree = _enemies
            .Where(enemy => Vector2.Distance(enemy.Position, cellPosition) < spawnDistance)
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

    private void AddEnemyToRespawnList(Enemy enemy)
    {
        if (enemy is Guard)
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
        foreach (var enemy in _respawnEnemies)
        {
            _gameComponents.Add(enemy);
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

    private void UpdateSprite(Sprite sprite, GameTime gameTime)
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
                    _gameComponents.Remove(_heroHealthWriter);
                    _gameComponents.Remove(_heroChalkUsesWriter);

                    GameStateChanged.Invoke(new GameOverState(this));
                }
            }
        }

        if (sprite is Enemy enemy)
        {
            var distance = Vector2.Distance(enemy.Position, Hero.Position);

            if (distance > Optimization.GetEnemyUpdateDistance(enemy))
            {
                return;
            }

            if (enemy.IsDead
             && distance > Optimization.GetEnemyDisposingDistance(enemy))
            {
                _deadGameComponents.Add(enemy);

                AddEnemyToRespawnList(enemy);
            }
        }
        else
        {
            ProcessStateControl(sprite, gameTime);
        }

        sprite.Update(gameTime);
    }

    private void UpdateMazeTile(MazeTile mazeTile, GameTime gameTime)
    {
        var distance = Vector2.Distance(mazeTile.Position, Hero.Position);

        if (distance < Optimization.GetMazeTileUpdateDistance(mazeTile))
        {
            mazeTile.Update(gameTime);
        }
    }

    private void UpdateTextWriter(TextWriter textWriter, GameTime gameTime)
    {
        if (textWriter.IsDead)
        {
            _deadGameComponents.Add(textWriter);
        }

        textWriter.Update(gameTime);
    }
}