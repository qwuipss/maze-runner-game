using MazeRunner.Components;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Cameras;

public class HeroCamera : MazeRunnerGameComponent, ICamera
{
    private readonly Vector3 _origin;

    private readonly Matrix _scale;

    private readonly Matrix _bordersOffset;

    private Matrix _transformMatrix;

    public HeroCamera(Viewport viewPort, float scaleCoeff = 1)
    {
        _scale = Matrix.CreateScale(new Vector3(scaleCoeff, scaleCoeff, 0));

        _origin = new Vector3(viewPort.Width / 2, viewPort.Height / 2, 0);
        _bordersOffset = Matrix.CreateTranslation(_origin);
    }

    public override void Draw(GameTime gameTime)
    {
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        var hero = game.Hero;

        Follow(hero, game.SpritesPositions[hero]);
    }

    private void Follow(Sprite sprite, Vector2 spritePosition)
    {
        var cameraPosition = Matrix.CreateTranslation(
            -spritePosition.X - (sprite.FrameWidth / 2),
            -spritePosition.Y - (sprite.FrameHeight / 2),
            0);

        _transformMatrix = cameraPosition * _scale * _bordersOffset;
    }

    public Matrix GetTransformMatrix()
    {
        return _transformMatrix;
    }
}