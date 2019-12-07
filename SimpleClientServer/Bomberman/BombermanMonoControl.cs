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
        public List<Bomberman_Character> _characterList;

        protected override void Initialize()
        {
            base.Initialize();
            _characterList = new List<Bomberman_Character>();
            _characterList.Add(new Bomberman_Character(Editor.Content, Color.White));
            _characterList.Add(new Bomberman_Character(Editor.Content, Color.BlueViolet));
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _characterList.ForEach(cl =>
            {
                cl.Update(gameTime);
            });
        }
        protected override void Draw()
        {
            base.Draw();
            //Editor.spriteBatch.Begin();
            _characterList.ForEach(cl =>
            {
                cl.Draw(Editor.spriteBatch);
            });
           // Editor.spriteBatch.End();
        }
    }
}
