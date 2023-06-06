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
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public override event Action<IGameState> GameStateChanged;

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

    private Lazy<HeroCameraEffectDecreaser> _cameraEffectDecreaser;

    public GameParameters GameParameters { get; init; }

    public bool IsControlling { get; set; }

    public Hero Hero { get; private set; }

    public HeroCamera HeroCamera { get; private set; }

    public GameRunningState(GameParameters gameParameters)
    {
        GameParameters = gameParameters;

        IsControlling = true;
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
        InitializeComponentsList();
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
        void UpdateGameComponents()
        {
            foreach (var component in _gameComponents)
            {
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
        }

        void UpdateStaticComponents()
        {
            foreach (var staticComponent in _staticComponents)
            {
                staticComponent.Update(gameTime);
            }

            ProcessShadowerState(_staticComponents);
        }

        UpdateGameComponents();
        UpdateStaticComponents();

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
        _maze = MazeGenerator.GenerateMaze(GameParameters.MazeWidth, GameParameters.MazeHeight);


        MazeGenerator.MakeCyclic(_maze, GameParameters.MazeDeadEndsRemovePercentage);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), GameParameters.MazeBayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), GameParameters.MazeDropTrapInsertingPercentage);

        MazeGenerator.InsertExit(_maze);

        MazeGenerator.InsertItem(_maze, new Key());

        Hero = new Hero(GameParameters.HeroHealth, GameParameters.ChalkUses);
        
        MazeGenerator.InsertItems(_maze, () => new Chalk(Hero), GameParameters.ChalksInsertingPercentage);
        MazeGenerator.InsertItems(_maze, () => new Food(Hero), GameParameters.FoodInsertingPercentage);

        var cell = MazeGenerator.GetRandomCell(_maze, _maze.IsFloor).First();
        var position = _maze.GetCellPosition(cell);

        Hero.Position = position;

        Hero.Initialize(_maze);

        _maze.Initialize(Hero);

        _maze.InitializeComponentsList();
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
    }

    private void InitializeShadower()
    {
        Shadower = new EffectsHelper.Shadower(true);

        Shadower.TresholdReached += () => NeedShadowerDeactivate = true;
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
        void ProcessGameWin()
        {
            if (IsMazeEscaped())
            {
                WonGame();
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
        else if (sprite is Hero)
        {
            ProcessStateControl(gameTime);
            ProcessGameWin();
        }
        else
        {
            throw new NotImplementedException();
        }

        sprite.Update(gameTime);
    }

    private void UpdateTextWriter(TextWriter textWriter, GameTime gameTime)
    {
        if (textWriter.IsDead)
        {
            _deadGameComponents.Add(textWriter);
        }

        textWriter.Update(gameTime);
    }

    private bool IsMazeEscaped()
    {
        return SpriteBaseState.GetSpriteCell(Hero, _maze) == _maze.ExitInfo.Cell;
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

                GameStateChanged.Invoke(new GameOverState(this, HeroCamera.EffectTransparency));
            }
        }
    }

    private void WonGame()
    {
        Shadower = new EffectsHelper.Shadower(false);

        NeedShadowerActivate = true;

        Shadower.TresholdReached += () => GameStateChanged.Invoke(new GameWonState());
    }
}