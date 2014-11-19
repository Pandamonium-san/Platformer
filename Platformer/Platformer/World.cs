using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class World
    {
        public List<Platform> platforms;
        public List<int[]> mapData;

        public void LoadWorld()
        {
            mapData = MapHandler.GetMapFromText(MapHandler.lvl1);
            platforms = new List<Platform>();

            for (int i = 0; i < mapData.Count(); i++)
            {
                platforms.Add(new Platform(ObjectManager.tileTexture, new Vector2(mapData[i][0], mapData[i][1]), mapData[i][2], mapData[i][3]));
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var p in platforms)
            p.Draw(spriteBatch);
        }
    }
}
