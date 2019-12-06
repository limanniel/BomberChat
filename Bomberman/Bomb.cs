using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Collections.Generic;

namespace Bomberman
{
    class Bomb
    {
        Texture2D _texture;
        Vector2 _position;
        Rectangle _sourceRect;
        SpriteBatch _spriteBatchRef;
        float _currentDetonateTime;
        const float _DETONATE_TIME = 5000.0f;
        public bool _exploded { get; private set; }
        List<Bomberman> _playersListRef;

        public Bomb(Vector2 position, ref SpriteBatch spritebatch, ContentManager cm, ref List<Bomberman> playerlistref)
        {
            _exploded = false;
            _position = position;
            _sourceRect = new Rectangle((int)_position.X, (int)_position.Y, 48, 48);
            _spriteBatchRef = spritebatch;
            _playersListRef = playerlistref;
            _texture = cm.Load<Texture2D>("Textures/Bomb_f01");
        }

        public void Update(GameTime gametime)
        {
            _currentDetonateTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            Debug.WriteLine(_currentDetonateTime);
            if (_currentDetonateTime >= _DETONATE_TIME)
            {
                _playersListRef.ForEach(pl =>
                {
                    if (_sourceRect.Intersects(pl.GetSourceRect()))
                    {
                        pl._active = false;
                    }
                });
                _exploded = true;
            }
        }

        public void Draw()
        {
            _spriteBatchRef.Draw(_texture, _position, new Rectangle(0, 0, 48, 48), Color.White);
        }
    }
}
