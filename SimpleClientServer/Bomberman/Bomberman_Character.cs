using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Bomberman
{
    public class Bomberman_Character
    {
        public AnimatedSprite _animatedSprite;
        Texture2D _texture;
        Rectangle _sourceRect;
        public Vector2 _position;
        const float _CHARACTER_SPEED = 0.25f;
        public bool _isMoving { get; private set; }
        public int _direction { get; private set; }

        public Bomberman_Character(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("Textures/bomberman_spritesheet");
            _animatedSprite = new AnimatedSprite(_texture, 8, 4);
            _position = new Vector2(0.0f, 0.0f);
            _sourceRect = new Rectangle(0, 0, 64, 128);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.W))
            {
                _position.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                _animatedSprite.Update(gameTime, 1);
                _direction = 1;
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                _position.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                _animatedSprite.Update(gameTime, 0);
                _direction = 0;
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                _position.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                _animatedSprite.Update(gameTime, 3);
                _direction = 3;
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                _position.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                _animatedSprite.Update(gameTime, 2);
                _direction = 2;
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
            //spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
            _animatedSprite.Draw(spriteBatch, _position);
        }
    }
}
