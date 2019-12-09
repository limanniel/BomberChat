using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    public class Bomberman_Character
    {
        public AnimatedSprite _animatedSprite;
        Texture2D _texture;
        public Vector2 _position;
        const float _CHARACTER_SPEED = 0.25f;
        public int _id { get; set; }
        public bool _isMoving { get; private set; }
        public bool _possessed { get; set; }
        public int _direction { get; private set; }
        public bool _canSpawnBomb { get; set; }

        public Bomberman_Character(ContentManager contentManager, int id, Color color)
        {
            _texture = contentManager.Load<Texture2D>("Textures/bomberman_spritesheet");
            _animatedSprite = new AnimatedSprite(_texture, 8, 4, color);
            _id = id;
            _position = new Vector2(0.0f, 0.0f);
            _possessed = false;
            _canSpawnBomb = true;
        }

        public void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            KeyboardState kbState = Keyboard.GetState();
            if (_possessed)
            {
                if (kbState.IsKeyDown(Keys.W) && _position.Y > 0)
                {
                    _position.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                    _animatedSprite.Update(gameTime, 1);
                    _direction = 1;
                }
                if (kbState.IsKeyDown(Keys.S) && (_position.Y + 90 < windowHeight))
                {
                    _position.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                    _animatedSprite.Update(gameTime, 0);
                    _direction = 0;
                }
                if (kbState.IsKeyDown(Keys.A) && _position.X > 0)
                {
                    _position.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                    _animatedSprite.Update(gameTime, 3);
                    _direction = 3;
                }
                if (kbState.IsKeyDown(Keys.D) && (_position.X + 52) < windowWidth)
                {
                    _position.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * _CHARACTER_SPEED;
                    _animatedSprite.Update(gameTime, 2);
                    _direction = 2;
                }
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
            _animatedSprite.Draw(spriteBatch, _position);
        }

        public void UpdateAnimation(GameTime gametime, int direction)
        {
            _animatedSprite.Update(gametime, direction);
        }
    }
}
