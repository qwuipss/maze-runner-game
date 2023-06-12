using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Drawing.Writers;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using RectangleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.GameBase.States;

public class GameRunningState : GameBaseState
{
    private class HeroCameraEffectDecreaser
    {
        public const float TransparencyDecreasingTreshold = 1 / 2.25f;

        private const double DecreasingDelayMs = EffectsHelper.Shadower.StepAddDelay * 2.25;

        private const float DecreasingStep = EffectsHelper.Shadower.Step;

        private readonly HeroCamera _camera;

        private double _elapsedTimeMs;

        public HeroCameraEffectDecreaser(HeroCamera camera)
        {
            _camera = camera;
        }

        public bool Update(GameTime gameTime)
        {
            _elapsedTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTimeMs > DecreasingDelayMs)
            {
                _camera.EffectTransparency -= DecreasingStep;

                _elapsedTimeMs -= DecreasingDelayMs;
            }

            if (_camera.EffectTransparency < TransparencyDecreasingTreshold)
            {
                return true;
            }

            return false;
        }
    }

    public const int UpdateAreaWidthRadius = 7;

    public const int UpdateAreaHeightRadius = 5;

    public static event Action GameStartedNotify;

    public static event Action GameWonNotify;

    public static event Action GameOveredNotify;

    public override event Action<IGameState> ControlGiveUpNotify;

    private static Texture2D _cameraEffect;

    private bool _isGameOver;

    private Maze _maze;

    private StaticCamera _staticCamera;

    private FindKeyWriter _findKeyTextWriter;

    private HeroHealthWriter _heroHealthWriter;

    private HeroChalkUsesWriter _heroChalkUsesWriter;

    private List<Enemy> _enemies;

    private HashSet<MazeRunnerGameComponent> _gameComponents;

    private HashSet<MazeRunnerGameComponent> _staticComponents;

    private List<Enemy> _respawnEnemies;

    private List<MazeRunnerGameComponent> _deadGameComponents;

    private HashSet<Enemy> _pendingDisposeEnemies;

    private Lazy<HeroCameraEffectDecreaser> _cameraEffectDecreaser;

    public GameParameters GameParameters { get; init; }

    public bool IsControlling { get; set; }

    public Hero Hero { get; private set; }

    public HeroCamera HeroCamera { get; private set; }

    public GameRunningState(GameParameters gameParameters)
    {
        GameParameters = gameParameters;

        IsControlling = true;

        GameStartedNotify.Invoke();
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        TurnOffMouseVisible(game);

        if (GraphicsDevice is not null)
        {
            return;
        }

        base.Initialize(graphicsDevice, game);

        InitializeHeroAndMaze();
        InitializeCameras();
        InitializeEnemies();
        InitializeTextWriters();
        InitializeShadower();
        InitializeComponents();
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.BeginDraw(HeroCamera);

        foreach (var component in _gameComponents)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();

        Drawer.BeginDraw(_staticCamera);

        foreach (var staticComponent in _staticComponents)
        {
            staticComponent.Draw(gameTime);
        }

        Drawer.EndDraw();
    }

    public override void Update(GameTime gameTime)
    {
        void UpdateGameComponents(RectangleXna updatableArea)
        {
            foreach (var component in _gameComponents)
            {
                if (MazeRunnerGameComponent.IsInArea(updatableArea, component))
                {
                    component.Update(gameTime);
                }
            }
        }

        void UpdateStaticComponents()
        {
            foreach (var staticComponent in _staticComponents)
            {
                staticComponent.Update(gameTime);
            }

            ProcessShadowerState(_staticComponents);
        }

        var heroCell = SpriteBaseState.GetSpriteCell(Hero);
        var updatableArea = HitBoxHelper.GetArea(heroCell, UpdateAreaWidthRadius, UpdateAreaHeightRadius, _maze.Skeleton);

        UpdateGameComponents(updatableArea);
        UpdateStaticComponents();

        ProcessStateControl(gameTime);

        if (!IsControlling)
        {
            return;
        }

        ProcessGameWin();

        MarkPendingDisposeEnemiesAsDead();

        HandleSecondaryButtons(gameTime);

        RespawnEnemies();
        DisposeDeadGameComponents();
    }

    private void InitializeComponents()
    {
        _respawnEnemies = new List<Enemy>();
        _pendingDisposeEnemies = new HashSet<Enemy>();
        _deadGameComponents = new List<MazeRunnerGameComponent>();

        _gameComponents = new HashSet<MazeRunnerGameComponent>
        {
            _maze, _findKeyTextWriter, HeroCamera, Hero,
        };

        foreach (var enemy in _enemies)
        {
            _gameComponents.Add(enemy);
        }

        _staticComponents = new HashSet<MazeRunnerGameComponent>
        {
            _heroHealthWriter, _heroChalkUsesWriter, Shadower,
        };
    }

    private void InitializeHeroAndMaze()
    {
        void GenerateMaze()
        {
            _maze = MazeGenerator.GenerateMaze(GameParameters.MazeWidth, GameParameters.MazeHeight);

            MazeGenerator.MakeCyclic(_maze, GameParameters.MazeDeadEndsRemovePercentage);
        }

        void InsertKey()
        {
            var key = new Key();

            var keyCollectedActions = () =>
            {
                _maze.IsKeyCollected = true;
            };

            MazeGenerator.InsertItem(_maze, key, keyCollectedActions);
        }

        void InsertTraps()
        {
            MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), GameParameters.MazeBayonetTrapInsertingPercentage);
            MazeGenerator.InsertTraps(_maze, () => new DropTrap(), GameParameters.MazeDropTrapInsertingPercentage);
        }

        void InsertItems()
        {
            MazeGenerator.InsertItems(_maze, () => new Chalk(Hero), GameParameters.ChalksInsertingPercentage);
            MazeGenerator.InsertItems(_maze, () => new Food(Hero), GameParameters.FoodInsertingPercentage);
        }

        void InsertExit()
        {
            MazeGenerator.InsertExit(_maze);
        }

        void InitializeHero()
        {
            var cell = MazeGenerator.GetRandomCell(_maze, _maze.IsFloor).First();
            var position = Maze.GetCellPosition(cell);

            Hero.Initialize(_maze);

            Hero.Position = position;
        }

        void CreateHero()
        {
            Hero = new Hero(GameParameters.HeroHealth, GameParameters.ChalkUses);
        }

        void InitializeMaze()
        {
            _maze.Initialize(Hero);
            _maze.InitializeComponents();
        }

        GenerateMaze();

        InsertTraps();
        InsertExit();
        InsertKey();

        CreateHero();

        InsertItems();

        InitializeHero();
        InitializeMaze();
    }

    private void InitializeEnemies()
    {
        void InitializeGuards()
        {
            for (int i = 0; i < GameParameters.GuardSpawnCount; i++)
            {
                var guard = CreateGuard();

                _enemies.Add(guard);

                guard.EnemyDiedNotify += () => AddEnemyToDisposeList(guard);
            }
        }

        _enemies = new List<Enemy>();

        InitializeGuards();
    }

    private void InitializeCameras()
    {
        void InitializeCameraEffect()
        {
            var shadowTreshold = Hero.FrameSize * 2.4f;

            _cameraEffect = EffectsHelper.CreateGradientCircleEffect(ViewWidth, ViewHeight, shadowTreshold, GraphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        _staticCamera = new StaticCamera(ViewWidth, ViewHeight);

        HeroCamera = new HeroCamera(Hero, ViewWidth, ViewHeight)
        {
            Effect = _cameraEffect,
        };

        _cameraEffectDecreaser = new Lazy<HeroCameraEffectDecreaser>(() => new HeroCameraEffectDecreaser(HeroCamera));
    }

    private void InitializeTextWriters()
    {
        _findKeyTextWriter = new FindKeyWriter(Hero, _maze);

        var scaleDivider = 450;

        _heroHealthWriter = new HeroHealthWriter(Hero, scaleDivider, ViewWidth);

        _heroChalkUsesWriter = new HeroChalkUsesWriter(Hero, _heroHealthWriter, scaleDivider, ViewWidth);

        _findKeyTextWriter.WriterDiedNotify += () => AddComponentToDeadList(_findKeyTextWriter);
    }

    private void InitializeShadower()
    {
        Shadower = new EffectsHelper.Shadower(true);

        Shadower.TresholdReached += () => NeedShadowerDeactivate = true;
    }

    private Guard CreateGuard()
    {
        var cell = MazeGenerator.GetRandomCell(_maze, IsEnemyFreeFloorCell).First();
        var position = Maze.GetCellPosition(cell);

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

        var cellPosition = Maze.GetCellPosition(cell);
        var distanceToHero = Vector2.Distance(Hero.Position, cellPosition);

        var mazeTile = _maze.Skeleton[cell.Y, cell.X];

        if (distanceToHero < GameRules.EnemySpawnDistance)
        {
            return false;
        }

        var isEnemyFree = _enemies
            .Where(enemy => Vector2.Distance(enemy.Position, cellPosition) < GameRules.EnemySpawnDistance)
            .Count() is 0;

        return isEnemyFree;
    }

    private void DisposeDeadGameComponents()
    {
        if (_deadGameComponents.Count is 0)
        {
            return;
        }

        foreach (var component in _deadGameComponents)
        {
            _gameComponents.Remove(component);
        }

        _deadGameComponents.Clear();
    }

    private void MarkPendingDisposeEnemiesAsDead()
    {
        if (_pendingDisposeEnemies.Count is 0)
        {
            return;
        }

        var deadComponentsCount = _deadGameComponents.Count;

        foreach (var enemy in _pendingDisposeEnemies)
        {
            var distance = Vector2.Distance(enemy.Position, Hero.Position);

            if (distance > GameRules.EnemyDisposeDistance)
            {
                _deadGameComponents.Add(enemy);

                AddEnemyToRespawnList(enemy);
            }
        }

        for (int i = deadComponentsCount; i < _deadGameComponents.Count; i++)
        {
            _pendingDisposeEnemies.Remove((Enemy)_deadGameComponents[i]);
        }
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

    private void AddEnemyToDisposeList(Enemy enemy)
    {
        _pendingDisposeEnemies.Add(enemy);
    }

    private void RespawnEnemies()
    {
        if (_respawnEnemies.Count is 0)
        {
            return;
        }

        foreach (var enemy in _respawnEnemies)
        {
            _gameComponents.Add(enemy);
        }

        _respawnEnemies.Clear();
    }

    private void HandleSecondaryButtons(GameTime gameTime)
    {
        if (KeyboardManager.IsGamePauseSwitched(gameTime))
        {
            PauseGame();
        }
    }

    private bool IsMazeEscaped()
    {
        return SpriteBaseState.GetSpriteCell(Hero) == _maze.ExitInfo.Cell;
    }

    private void ProcessStateControl(GameTime gameTime)
    {
        if (Hero.IsDead && !_isGameOver)
        {
            _isGameOver = true;
        }

        if (_isGameOver && IsControlling)
        {
            if (_cameraEffectDecreaser.Value.Update(gameTime))
            {
                _staticComponents.Clear();

                OverGame();
            }
        }
    }

    private void PauseGame()
    {
        ControlGiveUpNotify.Invoke(new GamePausedState(this));

        ControlGiveUpNotify = null;
    }

    private void WonGame()
    {
        Shadower = new EffectsHelper.Shadower(false);

        NeedShadowerActivate = true;

        Shadower.TresholdReached += () =>
        {
            GameWonNotify.Invoke();
            ControlGiveUpNotify.Invoke(new GameWonState());
        };
    }

    private void OverGame()
    {
        GameOveredNotify.Invoke();
        ControlGiveUpNotify.Invoke(new GameOverState(this, HeroCamera.EffectTransparency));
    }

    private void ProcessGameWin()
    {
        if (IsMazeEscaped())
        {
            WonGame();
        }
    }

    private void AddComponentToDeadList(MazeRunnerGameComponent component)
    {
        _deadGameComponents.Add(component);
    }
}