using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Wrappers;

public class MazeTileInfo : MazeRunnerGameComponent
{
    public MazeTile MazeTile { get; init; }

    public Vector2 Position { get; init; }

    public MazeTileInfo(MazeTile mazeTile, Vector2 position)
    {
        MazeTile = mazeTile;
        Position = position;
    }

    public override bool Equals(object other)
    {
        if (other is null)
        {
            return false;
        }

        var tileInfo = other as MazeTileInfo;

        if (tileInfo.MazeTile == MazeTile && tileInfo.Position == Position)
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MazeTile, Position);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawMazeTile(MazeTile, Position);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        MazeTile.Update(gameTime);
    }
}