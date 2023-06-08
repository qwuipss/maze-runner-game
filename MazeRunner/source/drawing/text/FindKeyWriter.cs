using MazeRunner.Content;
using MazeRunner.GameBase;
using MazeRunner.MazeBase;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MazeRunner.Drawing.Writers;

public class FindKeyWriter : TextWriter
{
    private enum WritingSide
    {
        None,
        Left,
        Right,
    };

    private const double TextMaxShowTimeMs = 3000;

    private const float TextShowDistance = GameConstants.AssetsFrameSize * 2;

    private readonly int _mazeWidth;

    private readonly Vector2 _stringSize;

    private readonly Maze _maze;

    private readonly Hero _hero;

    private WritingSide _writingSide;

    private bool _textShowed;

    private bool _needWriting;

    private double _textShowTimeMs;

    public override event Action WriterDiedNotify;

    public override float ScaleFactor => .2f;

    public FindKeyWriter(Hero hero, Maze maze)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _stringSize = Font.MeasureString(Text) * ScaleFactor;

        _hero = hero;
        _maze = maze;

        var skeleton = maze.Skeleton;

        var sideCell = new Cell(skeleton.GetLength(1), 0);
        _mazeWidth = (int)Maze.GetCellPosition(sideCell).X;
    }

    public override string Text
    {
        get
        {
            return "i have to find the key";
        }
    }

    public override void Draw(GameTime gameTime)
    {
        DrawIfNeeded(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
        if (_textShowed)
        {
            WriterDiedNotify.Invoke();

            return;
        }

        ProcessNeedWriting();

        if (_needWriting)
        {
            Position = GetDrawingPosition();
        }
    }

    private Vector2 GetDrawingPosition()
    {
        var position = _hero.Position;

        var rightUpCorner = position.X + _hero.FrameSize;
        var leftUpCorner = position.X;

        var rightSideTextEndPos = rightUpCorner + _stringSize.X;
        var leftSideTextStartPos = leftUpCorner - _stringSize.X;

        switch (_writingSide)
        {
            case WritingSide.None:
                if (rightSideTextEndPos <= _mazeWidth)
                {
                    _writingSide = WritingSide.Right;
                    goto case WritingSide.Right;
                }
                else
                {
                    _writingSide = WritingSide.Left;
                    goto case WritingSide.Left;
                }
            case WritingSide.Left:
                if (leftSideTextStartPos >= 0)
                {
                    return new Vector2(leftSideTextStartPos, position.Y);
                }
                else
                {
                    return new Vector2(rightUpCorner, position.Y);
                }
            case WritingSide.Right:
                if (rightSideTextEndPos <= _mazeWidth)
                {
                    return new Vector2(rightUpCorner, position.Y);
                }
                else
                {
                    return new Vector2(leftSideTextStartPos, position.Y);
                }
            default:
                throw new NotImplementedException();
        }
    }

    private void DrawIfNeeded(GameTime gameTime)
    {
        if (_needWriting)
        {
            if (_textShowTimeMs > TextMaxShowTimeMs)
            {
                _needWriting = false;
                _textShowed = true;

                return;
            }

            Drawer.DrawString(this);

            _textShowTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }

    private void ProcessNeedWriting()
    {
        var keyCollected = _maze.IsKeyCollected;

        if (!_needWriting && !keyCollected)
        {
            if (AreHeroExitLocatedNearby())
            {
                _needWriting = true;
            }
        }

        if (keyCollected)
        {
            _needWriting = false;
            _textShowed = true;
        }
    }

    private bool AreHeroExitLocatedNearby()
    {
        var position = _hero.Position;

        var exitPosition = Maze.GetCellPosition(_maze.ExitInfo.Cell);
        var distance = Vector2.Distance(exitPosition, position);

        return distance <= TextShowDistance;
    }
}