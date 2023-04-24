#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Channels;
#endregion

namespace MazeRunner;

public class DropTrapTile : MazeTile
{
    private enum TrapCondition
    {
        Opened,
        Closed,
        Opening,
        Closing,
    }

    private const int AnimationFrameDelayMs = 50;

    private static readonly Texture2D _texture = TilesTextures.DropTrap;
    private static readonly Random _random = new();

    private TrapCondition _condition = TrapCondition.Closed;

    private double _elapsedGameTime = 0;
    private int _currentAnimationFrame = 0;

    public override Texture2D Texture
    {
        get
        {
            return _texture;
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
                    CloseTrapWithChance(1e-1);
                    break;
                case TrapCondition.Closed:
                    OpenTrapWithChance(1e-2);
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

        return new Point(_currentAnimationFrame, 0);
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
        _currentAnimationFrame += FrameWidth;

        if (_currentAnimationFrame == FrameWidth * FramesCount - FrameWidth)
        {
            _condition = TrapCondition.Opened;
        }
    }

    private void ContinueClosing()
    {
        _currentAnimationFrame -= FrameWidth;

        if (_currentAnimationFrame == FrameWidth)
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
