﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class ObjectManager
    {
        public static Texture2D tileTexture, swordTexture, actorTexture, slimeTexture;
        public static List<Platform> platforms;
        public static List<Monster> monsters;

        public Player player;
        public bool lose, win;

        public void LoadContent(ContentManager Content)
        {
            tileTexture = Content.Load<Texture2D>("tile");
            swordTexture = Content.Load<Texture2D>("swordSheet");
            actorTexture = Content.Load<Texture2D>("Actor1");
            slimeTexture = Content.Load<Texture2D>("slime");
            platforms = new List<Platform>();
            monsters = new List<Monster>();
            player = new Player(actorTexture, MapHandler.startingPos);
        }

        public void Start(string path)
        {
            win = false;
            lose = false;


            platforms = new List<Platform>();
            monsters = new List<Monster>();
            LoadWorld(MapHandler.GetMapData(path));
            LoadMonsters(MapHandler.GetMonsterData(path));

            player = new Player(actorTexture, MapHandler.startingPos);
        }

        public void LoadWorld(List<int[]> mapData)
        {
            if (mapData != null)
                for (int i = 0; i < mapData.Count(); i++)
                {
                    platforms.Add(new Platform(ObjectManager.tileTexture, new Vector2(mapData[i][0], mapData[i][1]), mapData[i][2], mapData[i][3]));
                }
        }

        public void LoadMonsters(List<int[]> monsterData)
        {
            if (monsterData != null)
                for (int i = 0; i < monsterData.Count(); i++)
                {
                    monsters.Add(new Monster(ObjectManager.slimeTexture, new Vector2(monsterData[i][0], monsterData[i][1])));
                }
        }

        public void Update(GameTime gameTime)
        {

            player.Update(gameTime);
            CheckIfPlayerFellOff();

            foreach (var m in monsters)
            {
                m.Update(gameTime);
                if (player.sword.swingWeapon && !m.invulnerable && m.hitbox.Intersects(player.sword.hitbox))
                    if (m.PixelCollision(player.sword))
                    {
                        m.TakeDamage(player, player.sword.damage);
                        if (m.dead)
                            monsters.Remove(m);
                        break;
                    }
                if (!player.invulnerable && m.hitbox.Intersects(player.hitbox))
                    if (m.PixelCollision(player))
                        player.TakeDamage(m.damage);
                if (m.FellOff(MapHandler.worldSizeY))
                {
                    monsters.Remove(m);
                    break;
                }
            }

            if (monsters.Count() <= 0)
                win = true;
            if (player.health <= 0)
                lose = true;
        }

        private void CheckIfPlayerFellOff()
        {
            if (player.FellOff(MapHandler.worldSizeY))
            {
                player.health -= 1;
                player.pos = MapHandler.startingPos;
                player.invulnerable = true;
                player.invulnerableCount = -1000;
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Camera2D cam)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.get_transformation(graphicsDevice));
            foreach (var p in platforms)
                p.Draw(spriteBatch);
            foreach (var m in monsters)
                m.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }

    }
}
