using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Drawing;

public class FindKeyTextWriter : TextWriter
{
    private enum WritingSide
    {
        None,
        Left,
        Right,
    };

    private const double TextMaxShowTimeMs = 3000;

    private static readonly Lazy<FindKeyTextWriter> _instance;

    private readonly Vector2 _textStringLength;

    private Maze _maze;

    private Sprite _hero;

    private WritingSide _writingSide;

    private Vector2 _heroPosition;

    private bool _mazeKeyCollected;

    private bool _textShowed;

    private bool _needWriting;

    private double _textShowTimeMs;

    private float _textShowDistance;

    private int _mazeWidth;

    public override float ScaleFactor
    {
        get
        {
            return .2f;
        }
    }

    static FindKeyTextWriter()
    {
        _instance = new Lazy<FindKeyTextWriter>(() => new FindKeyTextWriter());
    }

    private FindKeyTextWriter()
    {
        Font = Fonts.BaseFont;

        Color = Color.Black;

        _textStringLength = Font.MeasureString(Text) * ScaleFactor;
    }

    public override string Text
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

    public override void Draw(GameTime gameTime, Vector2 position)
    {
        DrawIfNeeded(gameTime, position);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        if (_textShowed)
        {
            return;
        }

        _mazeKeyCollected = game.MazeInfo.IsKeyCollected;

        _heroPosition = game.HeroInfo.Position;

        if (_needWriting)
        {
            game.FindKeyTextWriterInfo.Position = GetDrawingPosition();
        }

        ProcessNeedDrawing();
    }

    public void Initialize(MazeRunnerGame game)
    {
        _hero = game.HeroInfo.Sprite;
        _maze = game.MazeInfo.Maze;

        _mazeKeyCollected = game.MazeInfo.IsKeyCollected;

        _textShowDistance = _maze.ExitInfo.Exit.FrameWidth * 2;

        _heroPosition = game.HeroInfo.Position;

        _mazeWidth = (int)_maze.GetCellPosition(new Cell(_maze.Skeleton.GetLength(1), 0)).X;
    }

    private Vector2 GetDrawingPosition()
    {
        var rightUpCorner = (int)_heroPosition.X + _hero.FrameWidth;
        var leftUpCorner = _heroPosition.X;

        var rightSideTextEndPos = rightUpCorner + _textStringLength.X;
        var leftSideTextStartPos = leftUpCorner - _textStringLength.X;

        switch (_writingSide)
        {
            case WritingSide.None:
                if (rightSideTextEndPos <= _mazeWidth)
                {
                    _writingSide = WritingSide.Right;
                    goto case WritingSide.Left;
                }
                else
                {
                    _writingSide = WritingSide.Left;
                    goto case WritingSide.Right;
                }
            case WritingSide.Left:
                if (leftSideTextStartPos >= 0)
                {
                    return new Vector2(leftSideTextStartPos, _heroPosition.Y);
                }
                else
                {
                    return new Vector2(rightUpCorner, _heroPosition.Y);
                }
            case WritingSide.Right:
                if (rightSideTextEndPos <= _mazeWidth)
                {
                    return new Vector2(rightUpCorner, _heroPosition.Y);
                }
                else
                {
                    return new Vector2(leftSideTextStartPos, _heroPosition.Y);
                }
            default:
                throw new NotImplementedException();
        }
    }

    private void DrawIfNeeded(GameTime gameTime, Vector2 position)
    {
        if (_needWriting)
        {
            if (_textShowTimeMs > TextMaxShowTimeMs)
            {
                _needWriting = false;
                _textShowed = true;

                return;
            }

            Drawer.DrawString(this, position);

            _textShowTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }

    private void ProcessNeedDrawing()
    {
        if (!_needWriting && !_mazeKeyCollected)
        {
            if (AreHeroExitLocatedNearby())
            {
                _needWriting = true;
            }
        }

        if (_mazeKeyCollected && _needWriting)
        {
            _needWriting = false;
            _textShowed = true;
        }
    }

    private bool AreHeroExitLocatedNearby()
    {
        var exitPosition = _maze.GetCellPosition(_maze.ExitInfo.Coords);

        var distance = Vector2.Distance(exitPosition, _heroPosition);

        return distance <= _textShowDistance;
    }
}