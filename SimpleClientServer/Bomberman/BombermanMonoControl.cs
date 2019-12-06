using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    public class BombermanMonoControl : MonoGame.Forms.Controls.MonoGameControl
    {
        public Bomberman_Character _character;

        protected override void Initialize()
        {
            base.Initialize();
            _character = new Bomberman_Character(Editor.Content);
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _character.Update(gameTime);
        }
        protected override void Draw()
        {
            base.Draw();
            Editor.spriteBatch.Begin();
            _character.Draw(Editor.spriteBatch);
            Editor.spriteBatch.End();
        }
    }
}
