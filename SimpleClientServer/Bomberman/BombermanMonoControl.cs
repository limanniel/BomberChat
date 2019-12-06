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
        Bomberman_Character _Character;

        protected override void Initialize()
        {
            base.Initialize();
            _Character = new Bomberman_Character(Editor.Content);
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _Character.Update(gameTime);
        }
        protected override void Draw()
        {
            base.Draw();
            Editor.spriteBatch.Begin();
            _Character.Draw(Editor.spriteBatch);
            Editor.spriteBatch.End();
        }

        public float GetCharacterPosX() { return _Character.GetCharacterPoxX(); }
    }
}
