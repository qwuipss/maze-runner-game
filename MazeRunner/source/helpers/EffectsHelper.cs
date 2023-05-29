using MazeRunner.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Helpers;

public static class EffectsHelper
{
    public static Texture2D CreateTransparentBackground(int width, int height, byte transparency, GraphicsDevice graphicsDevice)
    {
        var effectData = new Color[height, width];

        for (int y = 0; y < effectData.GetLength(0); y++)
        {
            for (int x = 0; x < effectData.GetLength(1); x++)
            {
                effectData[y, x] = Color.Black;

                effectData[y, x].A = transparency;
            }
        }

        return GetTexture(effectData, width, height, graphicsDevice);
    }

    public static Texture2D CreateGradientCircleEffect(int width, int height, float shadowTreshold, GraphicsDevice graphicsDevice)
    {
        var effectData = new Color[height, width];
        var centerPixel = new Vector2(width / 2, height / 2);

        for (int y = 0; y < effectData.GetLength(0); y++)
        {
            for (int x = 0; x < effectData.GetLength(1); x++)
            {
                var currentPixel = new Vector2(x, y);
                var distance = Vector2.Distance(centerPixel, currentPixel);

                if (distance >= shadowTreshold)
                {
                    effectData[y, x] = Color.Black;
                }
                else
                {
                    var transparentCoeff = distance / shadowTreshold;
                    var transparency = (byte)(byte.MaxValue * transparentCoeff);

                    effectData[y, x] = Color.Black;
                    effectData[y, x].A = transparency;
                }
            }
        }

        return GetTexture(effectData, width, height, graphicsDevice);
    }

    private static Texture2D GetTexture(Color[,] effectData, int width, int height, GraphicsDevice graphicsDevice)
    {
        var effectTexture = new Texture2D(graphicsDevice, width, height);

        effectTexture.SetData(effectData.ToLinear());

        return effectTexture;
    }
}
