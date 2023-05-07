using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Drawing;

public class FindKeyTextWriter : TextWriter
{
    private const double FindKeyTextMaxShowTimeMs = 3000;

    private static readonly Lazy<FindKeyTextWriter> _instance;

    private Maze _maze;

    private Vector2 _position;
    private Vector2 _heroPosition;

    private bool _mazeKeyCollected;

    private bool _findKeyTextShowed;

    private float _findKeyTextStringLength;

    private double _findKeyTextShowTimeMs;

    private float _findKeyTextShowDistance;

    static FindKeyTextWriter()
    {
        _instance = new Lazy<FindKeyTextWriter>(() => new FindKeyTextWriter());
    }

    private FindKeyTextWriter()
    {
        Font = Fonts.BaseFont;

        Color = Color.Black;
    }

    protected override string Text
    {
        get
        {
            return "i have to find the key";
        }
    }

    public static FindKeyTextWriter GetInstance()
    {
        return _instance.Value;
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawString(Text, _position, Color, DrawingPriority);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        _mazeKeyCollected = game.MazeKeyCollected;

        _position = GetDrawingPosition();
    }

    public void Initialize(MazeRunnerGame game)
    {
        _maze = game.Maze;

        _heroPosition = game.SpritesPositions[game.Hero];
    }

    private Vector2 GetDrawingPosition()
    {
        return Vector2.Zero;
    }

    private void ProcessFindKeyTextDrawing()
    {
        if (!_findKeyTextShowed && !_mazeKeyCollected)
        {
            if (CheckHeroExitLocatedNearby())
            {
                NeedWriting = true;
                _findKeyTextShowed = true;
            }
        }

        if (_mazeKeyCollected && NeedWriting)
        {
            NeedWriting = false;
        }
    }

    private void DrawFindKeyText(GameTime gameTime)
    {
        if (NeedWriting)
        {
            if (_findKeyTextShowTimeMs > FindKeyTextMaxShowTimeMs)
            {
                NeedWriting = false;
                return;
            }

            Drawer.DrawString(Text, _position, Color.Black, DrawingPriority);

            _findKeyTextShowTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }

    private bool CheckHeroExitLocatedNearby() //////////////////////////////////
    {
        var (coords, exit) = _maze.ExitInfo;

        var coordsAsVector = new Vector2(coords.X * exit.FrameWidth, coords.Y * exit.FrameHeight);

        return Vector2.Distance(coordsAsVector, _heroPosition) <= _findKeyTextShowDistance;
    }
}