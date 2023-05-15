namespace MazeRunner.Extensions;

public readonly record struct FloatRectangle(float X, float Y, float Width, float Height)
{
    private float Left
    {
        get
        {
            return X;
        }
    }

    private float Right
    {
        get
        {
            return X + Width;
        }
    }

    private float Top
    {
        get
        {
            return Y;
        }
    }

    private float Bottom
    {
        get
        {
            return Y + Height;
        }
    }

    public bool Intersects(FloatRectangle other)
    {
        if (other.Left < Right && Left < other.Right && other.Top < Bottom)
        {
            return Top < other.Bottom;
        }

        return false;
    }
}
