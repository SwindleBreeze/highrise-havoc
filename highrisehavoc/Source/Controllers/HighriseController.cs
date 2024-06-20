// Inside HighriseController.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using System;

namespace highrisehavoc
{
    public class HighriseController
    {
        private readonly Highrise _highrise;
        private readonly HighriseRenderer _highriseRenderer;
        private readonly HighriseAttachmentRenderer _highriseAttachmentRenderer;
        private readonly HighriseLevelRenderer _highriseLevelRenderer;
        public bool _canBuildAttachment = false;
        public bool _canBuildLevel = false;
        private float _attachment_y;
        private float _attachment_x;
        private float _level_y;
        private float _level_x;
        public HighriseController(Highrise highrise, HighriseRenderer highriseRenderer, HighriseAttachmentRenderer highriseAttachmentRenderer, HighriseLevelRenderer highriseLevelRenderer)
        {
            _highrise = highrise;
            _highriseRenderer = highriseRenderer;
            _highriseAttachmentRenderer = highriseAttachmentRenderer;
            _highriseLevelRenderer = highriseLevelRenderer;


            _attachment_y = (_highrise.SpritePosition.Y + (int)(_highrise.SourceRectangle.Height * _highriseRenderer.TextureScale.Y) - 150);
            _attachment_x = (_highrise.SpritePosition.X + (int)(_highrise.SourceRectangle.Width * _highriseRenderer.TextureScale.X) + 48);

            _level_y = _highrise.SpritePosition.Y ;
            _level_x = _highrise.SpritePosition.X + ((int)(_highrise.SourceRectangle.Width * _highriseRenderer.TextureScale.X) / 2);
        }

        public void Update(GameTime gameTime)
        {
            // Example: Move the highrise based on user input or other game logic
            // _highrise.SpritePosition += new Vector2(1, 0);

            for(int i = 0; i < _highrise.Levels.Count; i++)
            {
                if (_highrise.Levels[i].IsBuilt && i != _highrise.Levels.Count - 1)
                {
                    _highrise.Levels[i+1].canBeBuilt = true;
                }
                if (_highrise.Levels[i].IsBuilt)
                {
                    _highrise.Levels[i].BalconyAttachment.canBeBuilt = true;
                }
            }
            int count = 0;
            while (_highrise.AttachmentPoints.Count < 4 && _attachment_y > (int)(_highrise.SourceRectangle.Height * _highriseRenderer.TextureScale.Y) + 120 && count < 10)
            {
                Console.WriteLine("_attachment_x is: "+ _attachment_x.ToString());
                // Console.WriteLine(_highrise.AttachmentPoints.Count);
                if (_attachment_y > ((int)(_highrise.SourceRectangle.Height * _highriseRenderer.TextureScale.Y) + 100))
                {
                    Console.WriteLine("Adding attachment point");
                    _highrise.AttachmentPoints.Add(new HighriseAttachment(new Vector2(_attachment_x - 100, _attachment_y + 25), new Vector2(_attachment_x + 20*_highriseRenderer.TextureScale.X, _attachment_y + 20 * _highriseRenderer.TextureScale.Y), true, _highriseRenderer.TextureScale));
                    _attachment_y -= 90;
                }
                count++;
            }
            count = 0;
            while (_highrise.Levels.Count < 3 && count < 10)
            {
                if (_level_y > 50)
                {
                    _highrise.Levels.Add(new HighriseLevel(new Vector2((_level_x - (_highrise.SourceRectangle.Width * _highriseLevelRenderer.TextureScale.X / 4.2f)), _level_y), new Vector2(_level_x + 20 * _highriseRenderer.TextureScale.X, _level_y + 20 * _highriseRenderer.TextureScale.Y), _highriseRenderer.TextureScale));
                    if (_highrise.Levels.Count > 0) { _highrise.Levels[0].canBeBuilt = true; }
                    _level_y -= (int)(147 * _highriseRenderer.TextureScale.Y);
                }
                count++;
            }
        }

        public void Draw()
        {
            _highriseRenderer.Draw(_highrise);

            _highriseAttachmentRenderer.Draw(_highrise.AttachmentPoints, _canBuildAttachment);

            _highriseLevelRenderer.Draw(_highrise.Levels, _canBuildLevel);
        }

        public Highrise GetHighrise()
        {
            return _highrise;
        }

        public HighriseController ReturnCopy()
        {
            return new HighriseController(_highrise.ReturnCopy(), _highriseRenderer, _highriseAttachmentRenderer, _highriseLevelRenderer);
        }
    }
}
