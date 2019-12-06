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
        List<Bomberman_Character> _CharacterList;

        protected override void Initialize()
        {
            base.Initialize();
            _CharacterList = new List<Bomberman_Character>();
            for (var i = 0; i < 2; i++)
            {
                _CharacterList.Add(new Bomberman_Character(Editor.Content));
            }
            _CharacterList[0]._activated = true;
            _CharacterList[1]._activated = false;
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _CharacterList.ForEach(cl =>
            {
                cl.Update(gameTime);
            });
        }
        protected override void Draw()
        {
            base.Draw();
            Editor.spriteBatch.Begin();
            _CharacterList.ForEach(cl =>
            {
                cl.Draw(Editor.spriteBatch);
            });
            Editor.spriteBatch.End();
        }

        public Vector2 GetCharacterPosition(int index) { return _CharacterList[index].GetCharacterPosition(); }
        public void SetCharacterPosition(float x, float y, int index) { _CharacterList[index].SetCharacterPosition(x, y); }
    }
}
