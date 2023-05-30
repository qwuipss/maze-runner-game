using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
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

    private readonly float _textShowDistance;

    private readonly int _mazeWidth;

    private readonly Vector2 _textStringLength;

    private readonly Maze _maze;

    private readonly SpriteInfo _heroInfo;

    private WritingSide _writingSide;

    private bool _textShowed;

    private bool _needWriting;

    private double _textShowTimeMs;

    public override float ScaleFactor => .2f;

    public FindKeyTextWriter(SpriteInfo heroInfo, Maze maze)
    {
        Font = Fonts.BaseFont;
        Color = Color.White;

        _textStringLength = Font.MeasureString(Text) * ScaleFactor;

        _heroInfo = heroInfo;
        _maze = maze;

        _textShowDistance = maze.ExitInfo.Exit.FrameSize * 2;

        var skeleton = maze.Skeleton;

        var sideCell = new Cell(skeleton.GetLength(1) - 1, 0);
        var sideCellPosX = (int)maze.GetCellPosition(sideCell).X;

        _mazeWidth = sideCellPosX + skeleton[sideCell.Y, sideCell.X].FrameSize;
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
            IsDead = true;

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
        var hero = _heroInfo.Sprite;
        var position = _heroInfo.Position;

        var rightUpCorner = position.X + hero.FrameSize;
        var leftUpCorner = position.X;

        var rightSideTextEndPos = rightUpCorner + _textStringLength.X;
        var leftSideTextStartPos = leftUpCorner - _textStringLength.X;

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

            Drawer.DrawString(this, Position);

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
        var position = _heroInfo.Position;

        var exitPosition = _maze.GetCellPosition(_maze.ExitInfo.Cell);
        var distance = Vector2.Distance(exitPosition, position);

        return distance <= _textShowDistance;
    }
}