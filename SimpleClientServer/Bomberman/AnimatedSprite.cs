using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    public class AnimatedSprite
    {
        public Texture2D _texture { get; set; }
        public int _rows { get; set; }
        public int _columns { get; set; }
        private int _currentFrame;
        private int _activeColumn;
        private float _time;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            _texture = texture;
            _rows = rows;
            _columns = columns;
            _currentFrame = 0;
            _activeColumn = 0;
            _time = 0;
        }

        public void Update(GameTime gameTime, int column = 0)
        {
            _activeColumn = column;
            _time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_time > 50.0f)
            {
                _time = 0;
                _currentFrame++;
                if (_currentFrame == _rows)
                    _currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            int width = _texture.Width / _columns;
            int height = _texture.Height / _rows;

            Rectangle sourceRectangle = new Rectangle(width * _activeColumn, height * _currentFrame, width, height);
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
