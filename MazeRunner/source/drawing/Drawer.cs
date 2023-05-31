using MazeRunner.Cameras;
using MazeRunner.Drawing.Writers;
using MazeRunner.Gui.Buttons;
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

    public static void Initialize(Game game)
    {
        _spriteBatch = new SpriteBatch(game.GraphicsDevice);
    }

    public static void BeginDraw(ICamera camera)
    {
        _viewBox = DrawHelper.GetViewBox(camera);

        _spriteBatch.Begin(transformMatrix: camera.TransformMatrix, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront);
    }

    public static void EndDraw()
    {
        _spriteBatch.End();
    }

    public static void DrawButton(Button button)
    {
        Draw(
            button.Texture,
            button.Position,
            button.CurrentAnimationFrame,
            Button.DrawingPriority,
            scale: button.BoxScale);
    }

    public static void DrawString(TextWriter textWriter)
    {
        _spriteBatch.DrawString(
            textWriter.Font,
            textWriter.Text,
            textWriter.Position,
            textWriter.Color,
            0,
            Vector2.Zero,
            textWriter.ScaleFactor,
            SpriteEffects.None,
            textWriter.DrawingPriority);
    }

    public static void DrawSprite(Sprite sprite)
    {
        Draw(
             sprite.Texture,
             sprite.Position,
             sprite.CurrentAnimationFrame,
             sprite.DrawingPriority,
             spriteEffects: sprite.FrameEffect);
    }

    public static void DrawMazeTile(MazeTile mazeTile)
    {
        Draw(
             mazeTile.Texture,
             mazeTile.Position,
             mazeTile.CurrentAnimationFrame,
             mazeTile.DrawingPriority,
             mazeTile.FrameRotationAngle,
             mazeTile.OriginFrameRotationVector);
    }

    public static void Draw(
        Texture2D texture,
        Vector2 position,
        RectagleXna sourceRectangle,
        float layerDepth,
        float rotation = 0,
        Vector2 origin = default,
        float scale = 1,
        SpriteEffects spriteEffects = default,
        float transparency = 1)
    {
        if (DrawHelper.IsInViewBox(position, sourceRectangle, _viewBox))
        {
            _spriteBatch.Draw(texture, position, sourceRectangle, new ColorXna(ColorXna.White, transparency), rotation, origin, scale, spriteEffects, layerDepth);
        }
    }
}