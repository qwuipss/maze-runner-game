using MazeRunner.Cameras;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MazeRunner.Drawing;

public static class Drawer
{
    private static SpriteBatch _spriteBatch;

    private static Dictionary<Sprite, Vector2> _spritesPositions;

    private static Dictionary<TextWriter, Vector2> _textWritersPositions;

    public static void Initialize(MazeRunnerGame game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);

        _spritesPositions = game.SpritesPositions;
        _textWritersPositions = game.TextWritersPositions;
    }

    public static void BeginDraw(ICamera camera)
    {
        _spriteBatch.Begin(transformMatrix: camera.GetTransformMatrix(), samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront);
    }

    public static void EndDraw()
    {
        _spriteBatch.End();
    }

    public static void DrawString(TextWriter textWriter)
    {
        _spriteBatch.DrawString(
            textWriter.Font, textWriter.Text,
            _textWritersPositions[textWriter], textWriter.Color,
            0, Vector2.Zero, textWriter.ScaleFactor,
            SpriteEffects.None, textWriter.DrawingPriority);
    }

    public static void DrawMaze(Maze maze, GameTime gameTime)
    {
        DrawMazeSkeleton(maze);
        DrawTraps(maze);
        DrawExit(maze);
        DrawItems(maze);
    }

    public static void DrawSprite(Sprite sprite, GameTime gameTime)
    {
        Draw(sprite.Texture,
             _spritesPositions[sprite],
             new Rectangle(sprite.CurrentAnimationFramePoint,
                           new Point(sprite.FrameWidth, sprite.FrameHeight)),
             sprite.DrawingPriority,
             spriteEffects: sprite.FrameEffect);
    }

    private static void DrawMazeSkeleton(Maze maze)
    {
        for (int y = 0; y < maze.Skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < maze.Skeleton.GetLength(1); x++)
            {
                DrawMazeTile(maze.Skeleton[y, x], new Cell(x, y), maze);
            }
        }
    }

    private static void DrawTraps(Maze maze)
    {
        foreach (var (coords, trap) in maze.Traps)
        {
            DrawMazeTile(trap, coords, maze);
        }
    }

    private static void DrawExit(Maze maze)
    {
        var (coords, exit) = maze.ExitInfo;

        DrawMazeTile(exit, coords, maze, exit.FrameRotationAngle, exit.OriginFrameRotationVector);
    }

    private static void DrawItems(Maze maze)
    {
        foreach (var (coords, item) in maze.Items)
        {
            DrawMazeTile(item, coords, maze);
        }
    }

    private static void DrawMazeTile(MazeTile mazeTile, Cell cell, Maze maze, float rotation = 0, Vector2 origin = default)
    {
        var position = maze.GetCellPosition(cell);

        Draw(
             mazeTile.Texture,
             position,
             new Rectangle(mazeTile.CurrentAnimationFramePoint,
                           new Point(mazeTile.FrameWidth, mazeTile.FrameHeight)),
             mazeTile.DrawingPriority,
             rotation,
             origin);
    }

    private static void Draw(
        Texture2D texture,
        Vector2 position, Rectangle sourceRectangle,
        float layerDepth, float rotation = 0, Vector2 origin = default,
        SpriteEffects spriteEffects = default)
    {
        _spriteBatch.Draw(texture, position, sourceRectangle, Color.White, rotation, origin, Vector2.One, spriteEffects, layerDepth);
    }
}