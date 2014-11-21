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
        static int tileSize = 32;
        Rectangle defaultRec = new Rectangle(0, 0, 32, 32);
        bool holdingObject, objectWasRemoved;

        enum Place { Platform, Slime }
        Place placeTool = Place.Platform;

        public void Update(GameWindow window, ObjectManager obj)
        {
            objectWasRemoved = false;
            //hud.Update();
            //foreach (EditorButton b in hud.buttons)
            //{
            //    if (b.ButtonClicked())
            //        placeTool = (Place)b.type;
            //}
            if (KeyMouseReader.KeyPressed(Keys.Q))
            {
                ++placeTool;
                if ((int)placeTool == 2)
                    placeTool = Place.Platform;
            }
            if (KeyMouseReader.KeyPressed(Keys.W))
            {
                --placeTool;
                if ((int)placeTool == -1)
                    placeTool = Place.Slime;
            }
                if(KeyMouseReader.RightClick())
                {
                    foreach (Platform p in ObjectManager.platforms)
                    {
                        if ((p.hitbox.Contains(KeyMouseReader.mapEditRightClickPos) && !holdingObject))
                        {
                            ObjectManager.platforms.Remove(p);
                            objectWasRemoved = true;
                            break;
                        }
                    }
                    foreach (Monster m in ObjectManager.monsters)
                    {
                        if ((m.hitbox.Contains(KeyMouseReader.mapEditRightClickPos) && !holdingObject))
                        {
                            ObjectManager.monsters.Remove(m);
                            objectWasRemoved = true;
                            break;
                        }
                    }
                    if (!objectWasRemoved && !holdingObject)
                        PlaceObject();
                }
            
                
            foreach (Platform p in ObjectManager.platforms)
            {
                MoveObject(p);
            }
            foreach (Monster m in ObjectManager.monsters)
            {
                MoveObject(m);
            }
            MoveObject(obj.player);
        }

        private void PlaceObject()
        {
            switch (placeTool)
            {
                case Place.Platform:
                    Platform p = new Platform(ObjectManager.tileTexture, new Vector2(KeyMouseReader.mapEditMousePos.X, KeyMouseReader.mapEditMousePos.Y), 64, 64);
                    ObjectManager.platforms.Add(p);
                    SnapToGrid(p);
                    break;
                case Place.Slime:
                    ObjectManager.monsters.Add(new Monster(ObjectManager.slimeTexture, new Vector2(KeyMouseReader.mapEditMousePos.X, KeyMouseReader.mapEditMousePos.Y)));
                    break;
            }
        }

        private void MoveObject(GameObject p)
        {
            if (p.followingMouse)
            {
                p.pos.X = KeyMouseReader.mapEditMousePos.X - p.hitbox.Width / 2;
                p.pos.Y = KeyMouseReader.mapEditMousePos.Y - p.hitbox.Height / 2;
                p.hitbox.X = KeyMouseReader.mapEditMousePos.X - p.hitbox.Width / 2;
                p.hitbox.Y = KeyMouseReader.mapEditMousePos.Y - p.hitbox.Height / 2;
            }
            if (p.hitbox.Contains(KeyMouseReader.mapEditLeftClickPos) && !holdingObject)
            {
                p.followingMouse = true;
                holdingObject = true;
                return;
            }
            else if (KeyMouseReader.LeftClick() && p.followingMouse)
            {
                p.followingMouse = false;
                SnapToGrid(p);
                holdingObject = false;
                return;
            }
        }

        public void SnapToGrid(GameObject p)
        {
            if (p.hitbox.X > 0)
                p.hitbox.X = ((p.hitbox.X + tileSize / 2) / tileSize) * tileSize;
            if (p.hitbox.Y > 0)
                p.hitbox.Y = ((p.hitbox.Y + tileSize / 2) / tileSize) * tileSize;

            if (p.hitbox.X <= 0)
                p.hitbox.X = ((p.hitbox.X - tileSize / 2) / tileSize) * tileSize;
            if (p.hitbox.Y <= 0)
                p.hitbox.Y = ((p.hitbox.Y - tileSize / 2) / tileSize) * tileSize;

            p.pos.X = p.hitbox.X;
            p.pos.Y = p.hitbox.Y;
        }
    }
}
