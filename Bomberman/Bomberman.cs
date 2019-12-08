using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Bomberman
{
    class Bomberman
    {
        Texture2D _texture;
        Vector2 _position;
        Rectangle _sourceRect;
        SpriteBatch _spriteBatchRef;
        ContentManager _contentManager;
        const float _CHARACTER_SPEED = 0.5f;
        Bomb _bomb;
        bool _canSpawnBomb;
        public bool _active { get; set; }

        List<Bomberman> _playerListRef;

        public Bomberman(ref SpriteBatch spritebatch, ContentManager contentmanager, ref List<Bomberman> playerslistref)
        {
            _spriteBatchRef = spritebatch;
            _contentManager = contentmanager;
            _playerListRef = playerslistref;
            _sourceRect = new Rectangle(0, 0, 64, 128);
            _position.X = 0.0f;
            _position.Y = 0.0f;
            _canSpawnBomb = true;
            _active = true;

            LoadTexture();
        }

        public void LoadTexture()
        {
            _texture = _contentManager.Load<Texture2D>("Textures/Bman_F_f00");
        }

        public void Update(ref GameTime gametime)
        {
            if (_bomb != null && _bomb._exploded)
            {
                _bomb = null;
                _canSpawnBomb = true;
            }

            if (_bomb != null)
            {
                _bomb.Update(gametime);
            }

            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.W))
            {
                _position.Y -= (float)gametime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                _position.Y += (float)gametime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                _position.X -= (float)gametime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                _position.X += (float)gametime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }

            if (kbState.IsKeyDown(Keys.Space) && _canSpawnBomb)
            {
                _bomb = new Bomb(new Vector2(_position.X + 8.0f, _position.Y + 80.0f), ref _spriteBatchRef, _contentManager, ref _playerListRef);
                _canSpawnBomb = false;
            }
            
            _sourceRect.X = (int)_position.X;
            _sourceRect.Y = (int)_position.Y;
        }

        public void Draw()
        {
            if (_active)
            {
                _spriteBatchRef.Draw(_texture, _position, new Rectangle(0, 0, 64, 128), Color.White);
            }
            if (_bomb != null)
            {
                _bomb.Draw();
            }
        }

        public Vector2 GetPosition() { return _position; }
        public Rectangle GetSourceRect() { return _sourceRect; }
    }
}
