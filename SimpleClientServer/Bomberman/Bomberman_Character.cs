using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Bomberman
{
    public class Bomberman_Character
    {
        Texture2D _texture;
        Rectangle _sourceRect;
        public Vector2 _position;
        const float _CHARACTER_SPEED = 0.5f;
        public bool _isMoving { get; private set; }

        public Bomberman_Character(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("Textures/Bman_F_f00");
            _position = new Vector2(0.0f, 0.0f);
            _sourceRect = new Rectangle(0, 0, 64, 128);
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

            if (kbState.GetPressedKeys().Length == 0)
            {
                _isMoving = false;
            }
            else
            {
                _isMoving = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
        }
    }
}
