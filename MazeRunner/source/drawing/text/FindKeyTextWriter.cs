using MazeRunner.Content;
using MazeRunner.GameBase.States;
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

    private static readonly Lazy<FindKeyTextWriter> _instance;

    private readonly Vector2 _textStringLength;

    private MazeInfo _mazeInfo;

    private SpriteInfo _heroInfo;

    private WritingSide _writingSide;

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
        Color = Color.White;

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
            //FindKeyTextWriterInfo.Position = GetDrawingPosition();
        }
    }

    public void Initialize(GameRunningState game)
    {
        _heroInfo = game.HeroInfo;
        _mazeInfo = game.MazeInfo;

        var maze = _mazeInfo.Maze;
        var exit = maze.ExitInfo.Exit;

        _textShowDistance = exit.FrameSize * 2;

        var skeleton = maze.Skeleton;
        var sideCell = new Cell(skeleton.GetLength(1) - 1, 0);
        var sideCellPosX = (int)maze.GetCellPosition(sideCell).X;

        _mazeWidth = sideCellPosX + skeleton[sideCell.Y, sideCell.X].FrameSize;
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

    private void ProcessNeedWriting()
    {
        var keyCollected = _mazeInfo.IsKeyCollected;

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
        var maze = _mazeInfo.Maze;
        var position = _heroInfo.Position;

        var exitPosition = maze.GetCellPosition(maze.ExitInfo.Cell);
        var distance = Vector2.Distance(exitPosition, position);

        return distance <= _textShowDistance;
    }
}