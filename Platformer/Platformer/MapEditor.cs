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
        int platformID = 1;

        enum Place { Platform, Slime, Red_Slime, Skeleton, Gargoyle, Dagger, Ruby_Axe, Bow }
        Place placeTool = Place.Platform;

        public void Update(GameWindow window, ObjectManager obj)
        {
            objectWasRemoved = false;
            if (KeyMouseReader.KeyPressed(Keys.W))
            {
                if (placeTool == Place.Platform)
                {
                    ++platformID;
                    if(platformID >= 8)
                    {
                        platformID = 7;
                        ++placeTool;
                    }
                }
                else
                {
                    ++placeTool;
                    if ((int)placeTool == 8)
                    {
                        placeTool = Place.Platform;
                        platformID = 1;
                    }
                }
            }
            if (KeyMouseReader.KeyPressed(Keys.Q))
            {
                if (placeTool == Place.Platform)
                {
                    --platformID;
                    if (platformID <= 0)
                    {
                        platformID = 1;
                        placeTool = Place.Bow;
                    }
                }
                else
                {
                    --placeTool;
                    if ((int)placeTool <= -1)
                        placeTool = Place.Bow;
                }
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
                    foreach (Weapon w in ObjectManager.weapons)
                        if ((w.hitbox.Contains(KeyMouseReader.mapEditRightClickPos) && !holdingObject))
                        {
                            ObjectManager.weapons.Remove(w);
                            objectWasRemoved = true;
                            break;
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
            foreach (Weapon w in ObjectManager.weapons)
            {
                MoveObject(w);
            }
            MoveObject(obj.player);
        }

        private void PlaceObject()
        {
            Vector2 mousePos = new Vector2(KeyMouseReader.mapEditMousePos.X, KeyMouseReader.mapEditMousePos.Y);
            switch (placeTool)
            {
                case Place.Platform:
                    Platform p = new Platform(ObjectManager.tileTexture, mousePos, platformID);
                    ObjectManager.platforms.Add(p);
                    SnapToGrid(p);
                    break;
                case Place.Slime:
                    ObjectManager.monsters.Add(new Slime(ObjectManager.slimeTexture, mousePos));
                    break;
                case Place.Red_Slime:
                    ObjectManager.monsters.Add(new RedSlime(ObjectManager.redSlimeTexture, mousePos));
                    break;
                case Place.Skeleton:
                    ObjectManager.monsters.Add(new Skeleton(ObjectManager.monsterTexture, mousePos));
                    break;
                case Place.Gargoyle:
                    ObjectManager.monsters.Add(new Gargoyle(ObjectManager.gargoyleTexture, mousePos));
                    break;
                case Place.Dagger:
                    ObjectManager.weapons.Add(new Dagger(ObjectManager.daggerTexture, mousePos));
                    break;
                case Place.Ruby_Axe:
                    ObjectManager.weapons.Add(new RubyAxe(ObjectManager.axeTexture, mousePos));
                    break;
                case Place.Bow:
                    ObjectManager.weapons.Add(new Bow(ObjectManager.bowTexture, mousePos));
                    break;
            }
        }

        private void MoveObject(StaticGameObject p)
        {
            if (p.followingMouse)
            {
                p.pos.X = KeyMouseReader.mapEditMousePos.X - p.hitbox.Width / 2 + p.spriteRec.Width/2;
                p.pos.Y = KeyMouseReader.mapEditMousePos.Y - p.hitbox.Height / 2 + p.spriteRec.Height/2;
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
                if (p is Platform)
                SnapToGrid(p);
                holdingObject = false;
                return;
            }
        }

        public void SnapToGrid(StaticGameObject p)
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if(placeTool == Place.Platform)
                spriteBatch.DrawString(Game1.font, placeTool.ToString() + " " + platformID.ToString(), Vector2.Zero, Color.Red);
            else
                spriteBatch.DrawString(Game1.font, placeTool.ToString(), Vector2.Zero, Color.Red);
            spriteBatch.End();
        }
    }
}
