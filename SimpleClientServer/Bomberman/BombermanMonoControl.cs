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
        List<Bomb> _bombList;

        protected override void Initialize()
        {
            base.Initialize();
            _characterList = new List<Bomberman_Character>();
            _bombList = new List<Bomb>();
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (var i = 0; i < _characterList.Count; i++)
            {
                _characterList[i].Update(gameTime);
            }
            //_characterList.ForEach(cl =>
            //{
            //    cl.Update(gameTime);
            //});

            for (var i = 0; i < _bombList.Count; i++)
            {
                if (_bombList[i]._remove)
                {
                    int index = _characterList.FindIndex(cl => cl._id == _bombList[i]._playerID);
                    if (index != -1)
                    {
                        _characterList[index]._canSpawnBomb = true;
                    }

                    _bombList.RemoveAt(i);
                    continue;
                }

                _bombList[i].Update(gameTime);
            }
        }
        protected override void Draw()
        {
            base.Draw();
            for (var i = 0; i < _characterList.Count; i++)
            {
                _characterList[i].Draw(Editor.spriteBatch);
            }
            //_characterList.ForEach(cl =>
            //{
            //    cl.Draw(Editor.spriteBatch);
            //});
            _bombList.ForEach(bl =>
            {
                bl.Draw(Editor.spriteBatch);
            });

            //Editor.spriteBatch.Begin();
           // Editor.spriteBatch.End();
        }

        public void CreateCharacter(int id, int r, int g, int b)
        {
            Color color = new Color(r, g, b, 255);
            _characterList.Add(new Bomberman_Character(Editor.Content, id, color));
        }

        public void RemoveCharacet(int id)
        {
            _characterList.RemoveAt(id);
        }

        public void SpawnBomb(Vector2 position, int id)
        {
            _bombList.Add(new Bomb(Editor.Content, position, id));
        }

        public void SpawnBomb(float posX, float posY, int id)
        {
            Vector2 position = new Vector2(posX, posY);
            _bombList.Add(new Bomb(Editor.Content, position, id));
        }
    }
}
