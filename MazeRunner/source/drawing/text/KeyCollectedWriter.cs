using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Drawing.Writers;

public class KeyCollectedWriter : TextWriter
{
    private static readonly Texture2D _keyCollectedTexture;

    private readonly float _scaleFactor;

    private readonly Maze _maze;

    private bool _needDrawing;

    public override float ScaleFactor => _scaleFactor;

    public override string Text => throw new NotImplementedException();

#pragma warning disable CS0067 // The event 'KeyCollectedWriter.WriterDiedNotify' is never used
    public override event Action WriterDiedNotify;
#pragma warning restore CS0067 // The event 'KeyCollectedWriter.WriterDiedNotify' is never used

    static KeyCollectedWriter()
    {
        _keyCollectedTexture = Textures.Gui.StateShowers.KeyCollected;
    }

    public KeyCollectedWriter(Maze maze, HeroChalkUsesWriter chalkUsesWriter, float scaleDivider, int viewWidth)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _maze = maze;

        _needDrawing = _maze.IsKeyCollected;

        _scaleFactor = viewWidth / scaleDivider;

        var topOffset = 1.5f;

        Position = new Vector2(
            0,
            chalkUsesWriter.ChalkTextureDrawingPosition.Y + HeroChalkUsesWriter.ChalkTexture.Height * chalkUsesWriter.ScaleFactor * topOffset);
    }

    public override void Draw(GameTime gameTime)
    {
        if (_needDrawing)
        {
            Drawer.Draw(
                _keyCollectedTexture,
                Position,
                new Rectangle(0, 0, _keyCollectedTexture.Width, _keyCollectedTexture.Height),
                DrawingPriority,
                scale: _scaleFactor);
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (_maze.IsKeyCollected && !_needDrawing)
        {
            _needDrawing = true;
        }
    }
}
