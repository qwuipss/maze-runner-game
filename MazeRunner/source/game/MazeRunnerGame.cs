using MazeRunner.Content;
using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.source.managers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static MazeRunner.Settings;

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    #region GraphicsData
    private readonly GraphicsDeviceManager _graphics;
    #endregion

    #region DrawerData
    private Drawer _drawer;
    #endregion

    #region MazeData
    private Maze _maze;
    private bool _mazeKeyCollected;
    #endregion

    #region HeroData
    private Hero _hero;
    private Vector2 _heroPosition;
    #endregion    
    
    #region CameraData
    private Camera _camera;
    #endregion

    #region FindKeyTextData
    private const string FindKeyText = "i have to find the key";
    private float _findKeyTextStringLength;

    private double _findKeyTextShowTime;
    private float _findKeyTextShowDistance;

    private bool _findKeyTextShowed;
    private bool _needDrawFindKeyText;
    #endregion

    public MazeRunnerGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    #region GameBase
    protected override void Initialize()
    {
        base.Initialize();

        SetFullScreen();
        InitializeDrawer();
        InitializeMaze();
        InitializeHero();
        InitializeCamera();

        InitializeFindKeyTextData();
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

        _camera.Follow(_hero, _heroPosition);
        ProcessFindKeyTextDrawing();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _drawer.BeginDraw(_camera);

        _drawer.DrawMaze(_maze, gameTime);
        _drawer.DrawSprite(_hero, _heroPosition, gameTime);

        DrawFindKeyText(gameTime);

        _drawer.EndDraw();

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

    private void InitializeMaze()
    {
        _maze = MazeGenerator.GenerateMaze(MazeWidth, MazeHeight);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), 3);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), 2);

        MazeGenerator.InsertExit(_maze);

        MazeGenerator.InsertItem(_maze, new Key());
    }

    private void InitializeDrawer()
    {
        _drawer = Drawer.GetInstance();
        _drawer.Initialize(this);

        _drawer.SetSpriteFont(Fonts.BaseFont);
    }

    private void InitializeHero()
    {
        _hero = new Hero();

        var heroCell = MazeGenerator.GetRandomFloorCell(_maze);

        _heroPosition = new Vector2(heroCell.X * _hero.FrameWidth, heroCell.Y * _hero.FrameHeight);
    }

    private void InitializeCamera()
    {
        _camera = new Camera(GraphicsDevice.Viewport, 7);
    }

    private void InitializeFindKeyTextData()
    {
        _findKeyTextShowDistance = _maze.ExitInfo.Exit.FrameWidth * 2;
        _findKeyTextStringLength = Fonts.BaseFont.MeasureString(FindKeyText).Length();
    }
    #endregion

    #region HeroCollisionCheckers
    private void ProcessHeroItemsColliding()
    {
        if (CollisionManager.CollidesWithItems(_hero, _maze, _heroPosition, out var itemInfo))
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
        if (CollisionManager.CollidesWithKey(_hero, _heroPosition, coords, key))
        {
            _maze.RemoveItem(coords);
            _mazeKeyCollected = true;
        }
    }

    private void ProcessHeroMovement()
    {
        var movement = KeyboardManager.ProcessHeroMovement(_hero);

        var totalMovement = GetTotalMovement(movement);

        _heroPosition += totalMovement;
        _hero.ProcessPositionChange(totalMovement);
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

        var totalMovement = Vector2.Zero;

        var movementX = new Vector2(movement.X, 0);
        var movementY = new Vector2(0, movement.Y);

        if (!CollisionManager.ColidesWithWalls(_hero, _maze, _heroPosition, movementX)
         && !CollisionManager.CollidesWithExit(_hero, _maze, _heroPosition, movementX))
        {
            totalMovement += movementX;
        }

        if (!CollisionManager.ColidesWithWalls(_hero, _maze, _heroPosition, movementY)
         && !CollisionManager.CollidesWithExit(_hero, _maze, _heroPosition, movementY))
        {
            totalMovement += movementY;
        }

        if (ProcessDiagonalMovement(totalMovement, movementX, movementY, out totalMovement))
        {
            return totalMovement;
        }

        return NormalizeDiagonalSpeed(_hero.Speed, totalMovement);
    }

    private bool ProcessDiagonalMovement(Vector2 movement, Vector2 movementX, Vector2 movementY, out Vector2 totalMovement)
    {
        if (CollisionManager.ColidesWithWalls(_hero, _maze, _heroPosition, movement))
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

    #region DrawingPreprocessers
    private void ProcessFindKeyTextDrawing()
    {
        if (!_findKeyTextShowed && !_mazeKeyCollected)
        {
            if (CheckHeroExitLocatedNearby())
            {
                _needDrawFindKeyText = true;
                _findKeyTextShowed = true;
            }
        }

        if (_mazeKeyCollected && _needDrawFindKeyText)
        {
            _needDrawFindKeyText = false;
        }
    }

    private void DrawFindKeyText(GameTime gameTime)
    {
        const float windowTopIndentCoeff = .75f;

        if (_needDrawFindKeyText)
        {
            if (_findKeyTextShowTime > FindKeyTextMaxShowTimeMs)
            {
                _needDrawFindKeyText = false;
                return;
            }

            var positionX = (_graphics.PreferredBackBufferWidth - _findKeyTextStringLength) / 2;
            var positionY = (float)(_graphics.PreferredBackBufferHeight * windowTopIndentCoeff);

            var position = new Vector2(positionX, positionY);

            _drawer.DrawString(FindKeyText, position, Color.Black);

            _findKeyTextShowTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }

    private bool CheckHeroExitLocatedNearby()
    {
        var (coords, exit) = _maze.ExitInfo;

        var coordsAsVector = new Vector2(coords.X * exit.FrameWidth, coords.Y * exit.FrameHeight);

        return Vector2.Distance(coordsAsVector, _heroPosition) <= _findKeyTextShowDistance;
    }
    #endregion

    private void CheckDebugButtons()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // exit
        {
            Exit();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.G)) // generate maze
        {
            InitializeMaze();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.O)) // open exit
        {
            if (_mazeKeyCollected)
            {
                _maze.ExitInfo.Exit.Open();
            }
        }
    }
}