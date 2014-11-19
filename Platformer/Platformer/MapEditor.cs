using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class MapEditor
    {
        static int tileSize = 64;
        Rectangle defaultRec = new Rectangle(0, 0, 32, 32);
        bool holdingObject;

        enum Place { Mouse, DumbCat, SmartCat, IntelligentCat, GeniusCat, Cheese, Wall, Floor }
        Place placeTool = Place.Wall;

        public void Update(GameWindow window, World world)
        {
            //hud.Update();
            //foreach (EditorButton b in hud.buttons)
            //{
            //    if (b.ButtonClicked())
            //        placeTool = (Place)b.type;
            //}
            if (KeyMouseReader.KeyPressed(Keys.Q))
            {
                ++placeTool;
                if ((int)placeTool == 8)
                    placeTool = Place.Mouse;
            }
            if (KeyMouseReader.KeyPressed(Keys.W))
            {
                --placeTool;
                if ((int)placeTool == -1)
                    placeTool = Place.Floor;
            }

            foreach (Platform p in world.platforms)
            {
                if (p.followingMouse)
                {
                    p.hitbox.X = KeyMouseReader.mousePos.X - tileSize/2;
                    p.hitbox.Y = KeyMouseReader.mousePos.Y - tileSize/2;
                }
                if (p.hitbox.Contains(KeyMouseReader.LeftClickPos) && !holdingObject)
                {
                    p.followingMouse = true;
                    holdingObject = true;
                    break;
                }
                else if (KeyMouseReader.LeftClick() && p.followingMouse)
                {
                    p.followingMouse = false;
                    SnapToGrid(p);
                    holdingObject = false;
                    break;
                }
            }
        }

        public void SnapToGrid(Platform p)
        {
            if (p.hitbox.X > 0)
                p.hitbox.X = ((p.hitbox.X + tileSize / 2) / tileSize) * tileSize;
            if (p.hitbox.Y > 0)
                p.hitbox.Y = ((p.hitbox.Y + tileSize / 2) / tileSize) * tileSize;

            if (p.hitbox.X <= 0)
                p.hitbox.X = ((p.hitbox.X - tileSize / 2) / tileSize) * tileSize;
            if (p.hitbox.Y <= 0)
                p.hitbox.Y = ((p.hitbox.Y - tileSize / 2) / tileSize) * tileSize;
        }
    }
}
