using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.GameBase;
using MazeRunner.GameBase.States;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Wrappers;

public class MazeTileInfo : MazeRunnerGameComponent
{
#pragma warning disable CS0067
    public override event GameComponentProvider NeedDisposeNotify;
#pragma warning restore

    public MazeTile MazeTile { get; init; }

    public Vector2 Position { get; init; }

    public MazeTileInfo(MazeTile mazeTile, Vector2 position)
    {
        MazeTile = mazeTile;
        Position = position;
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawMazeTile(MazeTile, Position);
    }

    public override void Update(GameTime gameTime)
    {
        MazeTile.Update(gameTime);
    }
}