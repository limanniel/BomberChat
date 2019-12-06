using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Bomberman
{
    class Bomberman_Character
    {
        Texture2D _texture;
        Rectangle _sourceRect;
        Vector2 _position;
        const float _CHARACTER_SPEED = 0.5f;

        public Bomberman_Character(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("Textures/Bman_F_f00");
            _sourceRect = new Rectangle(0, 0, 64, 128);
            _position.X = 0.0f;
            _position.Y = 0.0f;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.W))
            {
                _position.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                _position.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                _position.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                _position.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
        }

        public float GetCharacterPoxX() { return _position.X; }
    }
}
