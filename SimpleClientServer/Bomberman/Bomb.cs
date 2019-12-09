using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    class Bomb
    {
        Texture2D _texture;
        Rectangle _sourceRect;
        float _timer;
        public Vector2 _position;
        public int _playerID { get; private set; }
        const float _TIME_TO_EXPLODE = 2.5f;
        public bool _remove { get; private set; }

        public Bomb(ContentManager contentManager, Vector2 position, int playerID)
        {
            _texture = contentManager.Load<Texture2D>("Textures/Bomb_f01");
            _position = position;
            _playerID = playerID;
            _position.Y += 38.0f; // Offset to the base of character
            _sourceRect = new Rectangle(0, 0, 48, 48);
            _timer = 0.0f;
            _remove = false;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _TIME_TO_EXPLODE)
            {
                _remove = true;
                _timer = 0.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
            spriteBatch.End();
        }
    }
}
