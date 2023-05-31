using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Helpers;

public static class EffectsHelper
{
    public class Shadower : MazeRunnerGameComponent
    {
        public event Action TresholdReached; 

        private const double StepAddDelay = 50;

        private const float Step = .05f;

        private static Texture2D _blackBackground;

        private float _transparency;

        private double _elapsedTimeMs;

        private bool _notified;

        public Shadower(GraphicsDevice graphicsDevice, int viewWidth, int viewHeight, float startTransparency)
        {
            _transparency = startTransparency;

            if (_blackBackground is null)
            {
                InitializeBlackBackground(viewWidth, viewHeight, graphicsDevice);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Drawer.Draw(
                _blackBackground, Vector2.Zero, new Rectangle(0, 0, _blackBackground.Width, _blackBackground.Height), 0, transparency: _transparency);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_notified && !_transparency.InRange(0, 1))
            {
                _notified = true;

                TresholdReached.Invoke();

                return;
            }

            _elapsedTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTimeMs > StepAddDelay)
            {
                _transparency += Step;

                _elapsedTimeMs = 0;
            }
        }
        
        private static void InitializeBlackBackground(int width, int height, GraphicsDevice graphicsDevice)
        {
            _blackBackground = CreateTransparentBackground(width, height, byte.MaxValue, graphicsDevice);
        }
    }


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
