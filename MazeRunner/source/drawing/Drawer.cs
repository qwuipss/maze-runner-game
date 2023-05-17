using MazeRunner.Cameras;
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using ColorXna = Microsoft.Xna.Framework.Color;
using RectagleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.Drawing;

public static class Drawer
{
    private static SpriteBatch _spriteBatch;

    private static RectangleF _viewBox;

    private static int _viewWidth;
    private static int _viewHeight;

    public static void Initialize(MazeRunnerGame game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);

        var graphics = game.Graphics;

        _viewWidth = graphics.GraphicsDevice.Viewport.Width;
        _viewHeight = graphics.GraphicsDevice.Viewport.Height;
    }

    public static void BeginDraw(ICamera camera)
    {
        _viewBox = DrawHelper.GetViewBox(camera.Position, _viewWidth, _viewHeight);

        _spriteBatch.Begin(transformMatrix: camera.TransformMatrix, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront);
    }

    public static void EndDraw()
    {
        _spriteBatch.End();
    }

    public static void DrawString(TextWriter textWriter, Vector2 position)
    {
        _spriteBatch.DrawString(
            textWriter.Font,
            textWriter.Text,
            position,
            textWriter.Color,
            0,
            Vector2.Zero,
            textWriter.ScaleFactor,
            SpriteEffects.None,
            textWriter.DrawingPriority);
    }

    public static void DrawSprite(Sprite sprite, Vector2 position)
    {
        Draw(
             sprite.Texture,
             position,
             sprite.CurrentAnimationFrame,
             sprite.DrawingPriority,
             spriteEffects: sprite.FrameEffect);
    }

    public static void DrawMazeTile(MazeTile mazeTile, Vector2 position)
    {
        Draw(
             mazeTile.Texture,
             position,
             mazeTile.CurrentAnimationFrame,
             mazeTile.DrawingPriority,
             mazeTile.FrameRotationAngle,
             mazeTile.OriginFrameRotationVector);
    }

    private static void Draw(
        Texture2D texture,
        Vector2 position,
        RectagleXna sourceRectangle,
        float layerDepth,
        float rotation = 0,
        Vector2 origin = default,
        SpriteEffects spriteEffects = default)
    {
        if (DrawHelper.IsInViewBox(position, sourceRectangle, _viewBox))
        {
            _spriteBatch.Draw(texture, position, sourceRectangle, ColorXna.White, rotation, origin, Vector2.One, spriteEffects, layerDepth);
        }
    }
}