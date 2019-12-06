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

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void Draw()
        {
            base.Draw();
            Editor.spriteBatch.Begin();
            Editor.spriteBatch.End();
        }
    }
}
