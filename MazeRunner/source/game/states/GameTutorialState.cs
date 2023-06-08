using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Drawing.Writers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.GameBase.States;

public class GameTutorialState : GameBaseState
{
    private GraphicsDevice _graphicsDevice;

    private readonly Maze _maze;

    private readonly Hero _hero;

    private HeroCamera _heroCamera;

    private GameTutorialWriter _tutorialWriter;

    private HashSet<MazeRunnerGameComponent> _components;

    public override event Action<IGameState> ControlGiveUpNotify;

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        _graphicsDevice = graphicsDevice;
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.BeginDraw(_heroCamera);

        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();
    }

    public override void Update(GameTime gameTime)
    {
        HandleSecondaryButtons(gameTime);
    }

    private void InitializeHeroAndMaze()
    {
        //_maze = MazeGenerator.GenerateMaze();

        //MazeGenerator.MakeCyclic(_maze, GameParameters.MazeDeadEndsRemovePercentage);

        //MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), GameParameters.MazeBayonetTrapInsertingPercentage);
        //MazeGenerator.InsertTraps(_maze, () => new DropTrap(), GameParameters.MazeDropTrapInsertingPercentage);

        //MazeGenerator.InsertExit(_maze);

        //MazeGenerator.InsertItem(_maze, new Key());

        //_hero = new Hero(GameParameters.HeroHealth, GameParameters.ChalkUses);

        //MazeGenerator.InsertItems(_maze, () => new Chalk(_hero), GameParameters.ChalksInsertingPercentage);
        //MazeGenerator.InsertItems(_maze, () => new Food(_hero), GameParameters.FoodInsertingPercentage);

        var cell = MazeGenerator.GetRandomCell(_maze, _maze.IsFloor).First();
        var position = Maze.GetCellPosition(cell);

        _hero.Position = position;

        _hero.Initialize(_maze);

        _maze.Initialize(_hero);

        _maze.InitializeComponents();
    }

    private void InitializeCamera()
    {
        _heroCamera = new HeroCamera(_hero, ViewWidth, ViewHeight);
    }

    private void InitializeTextWriters()
    {
        _tutorialWriter = new GameTutorialWriter();
    }

    private void InitializeComponents()
    {
        _components = new HashSet<MazeRunnerGameComponent>
        {
            _hero, _maze, _heroCamera, _tutorialWriter,
        };
    }

    private void HandleSecondaryButtons(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (KeyboardManager.IsNextTutorialTextButtonPressed(gameTime))
        {

        }
    }
}
