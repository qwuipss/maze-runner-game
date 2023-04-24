#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Channels;
#endregion

namespace MazeRunner;

public class DropTrapTile : MazeTile
{
    private const int AnimationFrameDelayMs = 35;

    private const double OpenChance = 1e-1;
    private const double CloseChance = 1e-2;

    private static readonly Random _random = new();

    private TrapCondition _condition = TrapCondition.Closed;

    private double _elapsedGameTime = 0;
    private int _currentAnimationFrameX = 0;

    public override Texture2D Texture
    {
        get
        {
            return TilesTextures.DropTrap;
        }
    }

    public override TileType TileType
    {
        get
        {
            return TileType.DropTrap;
        }
    }

    public override Point GetCurrentAnimationFrame(GameTime gameTime)
    {
        _elapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_elapsedGameTime >= AnimationFrameDelayMs)
        {
            switch (_condition)
            {
                case TrapCondition.Opened:
                    CloseTrapWithChance(OpenChance);
                    break;
                case TrapCondition.Closed:
                    OpenTrapWithChance(CloseChance);
                    break;
                case TrapCondition.Opening:
                    ContinueOpening();
                    break;
                case TrapCondition.Closing:
                    ContinueClosing();
                    break;
                default:
                    break;
            }
            
            _elapsedGameTime -= AnimationFrameDelayMs;
        }

        return new Point(_currentAnimationFrameX, 0);
    }

    protected override int FramesCount
    {
        get
        {
            return 8;
        }
    }

    private void ContinueOpening()
    {
        _currentAnimationFrameX += FrameWidth;

        if (_currentAnimationFrameX == FrameWidth * FramesCount - FrameWidth)
        {
            _condition = TrapCondition.Opened;
        }
    }

    private void ContinueClosing()
    {
        _currentAnimationFrameX -= FrameWidth;

        if (_currentAnimationFrameX == FrameWidth)
        {
            _condition = TrapCondition.Closed;
        }
    }

    private void OpenTrapWithChance(double chance)
    {
        SwitchTrapConditionWithChance(chance, TrapCondition.Opening);
    }

    private void CloseTrapWithChance(double chance)
    {
        SwitchTrapConditionWithChance(chance, TrapCondition.Closing);
    }

    private void SwitchTrapConditionWithChance(double chance, TrapCondition newCondition)
    {
        var randomValue = _random.NextDouble();

        if (chance > randomValue)
        {
            _condition = newCondition;
        }
    }
}
