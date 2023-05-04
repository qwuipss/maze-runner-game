#region Usings
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace MazeRunner;

public class Drawer
{
    private static readonly Lazy<Drawer> _instance = new(() => new Drawer());

    private SpriteBatch _spriteBatch;

    private Drawer()
    {
    }

    public static Drawer GetInstance()
    {
        return _instance.Value;
    }

    public void Initialize(Game game)
    {
        _spriteBatch = new(game.GraphicsDevice);
    }

    public void BeginDraw()
    {
        _spriteBatch.Begin();
    }

    public void EndDraw()
    {
        _spriteBatch.End();
    }

    public void DrawMaze(Maze maze, GameTime gameTime)
    {
        DrawMazeSkeleton(maze, gameTime);
        DrawTraps(maze, gameTime);
        DrawExit(maze, gameTime);
    }

    public void DrawSprite(Sprite sprite, GameTime gameTime)
    {
        _spriteBatch.Draw(
                sprite.Texture,
                sprite.Position,
                new Rectangle(sprite.GetCurrentAnimationFramePoint(gameTime),
                              new Point(sprite.FrameWidth, sprite.FrameHeight)),
                Color.White,
                0,
                Vector2.Zero,
                Vector2.One,
                sprite.FrameEffect,
                1);
    }

    private void DrawMazeSkeleton(Maze maze, GameTime gameTime)
    {
        for (int y = 0; y < maze.Skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < maze.Skeleton.GetLength(1); x++)
            {
                DrawMazeTile(maze.Skeleton[y, x], x, y, gameTime);
            }
        }
    }

    private void DrawTraps(Maze maze, GameTime gameTime)
    {
        foreach (var trap in maze.Traps)
        {
            DrawMazeTile(trap.Value, trap.Key.X, trap.Key.Y, gameTime);
        }
    }

    private void DrawExit(Maze maze, GameTime gameTime)
    {
        var exitInfo = maze.ExitInfo;

        var exit = exitInfo.Exit;
        var coords = exitInfo.Coords;

        _spriteBatch.Draw(
            exit.Texture,
            new Vector2(coords.X * exit.FrameWidth, coords.Y * exit.FrameHeight),
            new Rectangle(exit.GetCurrentAnimationFramePoint(gameTime),
            new Point(exit.FrameWidth, exit.FrameHeight)),
            Color.White,
            exit.FrameRotationAngle,
            exit.OriginFrameRotationVector,
            Vector2.One,
            SpriteEffects.None,
            0);
    }

    private void DrawMazeTile(MazeTile mazeTile, int x, int y, GameTime gameTime)
    {
        _spriteBatch.Draw(
            mazeTile.Texture,
            new Vector2(x * mazeTile.FrameWidth, y * mazeTile.FrameHeight),
            new Rectangle(mazeTile.GetCurrentAnimationFramePoint(gameTime),
                          new Point(mazeTile.FrameWidth, mazeTile.FrameHeight)),
            Color.White);
    }
}