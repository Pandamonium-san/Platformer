using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    class ObjectManager
    {
        public static Texture2D tileTexture, swordTexture, actorTexture, slimeTexture, redSlimeTexture, smokeTexture, axeTexture;
        public static List<Platform> platforms;
        public static List<Monster> monsters;
        public static List<Weapon> weapons;
        public static ParticleEngine2D particleEngine;

        public Player player;
        public double gameOverCounter;
        public bool lose, win;

        public void LoadContent(ContentManager Content)
        {
            tileTexture = Content.Load<Texture2D>("tile");
            swordTexture = Content.Load<Texture2D>("swordSheet");
            actorTexture = Content.Load<Texture2D>("Actor1");
            slimeTexture = Content.Load<Texture2D>("slime");
            redSlimeTexture = Content.Load<Texture2D>("red_slime");
            smokeTexture = Content.Load<Texture2D>("smoke_particle");
            axeTexture = Content.Load<Texture2D>("RubyAxe1");
            Start(null);
        }

        public void Start(string path)
        {
            particleEngine = new ParticleEngine2D();

            win = false;
            lose = false;
            gameOverCounter = 2000;

            platforms = new List<Platform>();
            monsters = new List<Monster>();
            weapons = new List<Weapon>();
            if (path != null)
            {
                LoadWorld(MapHandler.GetMapData(path));
                LoadMonsters(MapHandler.GetMonsterData(path));
            }
            player = new Player(actorTexture, MapHandler.startingPos);
            weapons.Add(new MeleeWeapon(ObjectManager.axeTexture, player.pos));
            //weapons.Add(new MeleeWeapon(ObjectManager.swordTexture, player.pos + Vector2.UnitX*50));
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
            {
                for (int i = 0; i < monsterData.Count(); i++)
                {
                    if (monsterData[i][2] == 1)
                        monsters.Add(new Slime(ObjectManager.slimeTexture, new Vector2(monsterData[i][0], monsterData[i][1])));
                    else if (monsterData[i][2] == 2)
                        monsters.Add(new RedSlime(ObjectManager.redSlimeTexture, new Vector2(monsterData[i][0], monsterData[i][1])));
                }
            }
        }

        public void LoadWeapons(List<int[]> weaponData)
        {
        }

        public void Update(GameTime gameTime)
        {
            particleEngine.Update(gameTime);
            player.Update(gameTime);
            UpdateWeapons(gameTime);

            foreach (var m in monsters)
            {
                m.Update(gameTime);
                WeaponCollidesWithMonster(player.equippedWeapon, m);
                PlayerCollidesWithMonster(m);
                if (m.dead)
                {
                    monsters.Remove(m);
                    break;
                }
            }

            if (WinCondition() || LoseCondition())
            {
                gameOverCounter -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private void PlayerCollidesWithMonster(Monster m)
        {
            if (!player.invulnerable && !player.dying && m.hitbox.Intersects(player.hitbox))
                if (m.PixelCollision(player))
                    player.TakeDamage(m.damage);
        }

        private void WeaponCollidesWithMonster(Weapon w, Monster m)
        {
            if (w != null)
                if (w.attacking && !m.invulnerable && m.hitbox.Intersects(w.hitbox))
                    if (m.PixelCollision(w))
                        m.TakeDamage(player, w.damage);
        }

        private void UpdateWeapons(GameTime gameTime)
        {
            if (player.weaponIsEquipped && KeyMouseReader.KeyPressed(Keys.X))
            {
                DropWeapon(player);
                return;
            }

            foreach(Weapon w in weapons)
            {
                w.Update(gameTime);
                if (!player.weaponIsEquipped && w.hitbox.Intersects(player.hitbox) && KeyMouseReader.KeyPressed(Keys.X))
                {
                    EquipWeaponToPlayer(w, player);
                    break;
                }
            }
        }

        private void DropWeapon(Player player)
        {
            player.equippedWeapon.PrepareToDropWeapon();
            weapons.Add(player.equippedWeapon);
            player.equippedWeapon = null;
            player.weaponIsEquipped = false;
        }

        private void EquipWeaponToPlayer(Weapon weapon, Player player)
        {
            player.EquipWeapon(weapon);
            weapons.Remove(weapon);
        }

        public bool WinCondition()
        {
            if (monsters.Count() <= 0)
                return true;
            return false;
        }

        public bool LoseCondition()
        {
            if (player.dead)
                return true;
            return false;
        }

        
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Camera2D cam)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.GetViewMatrix(new Vector2(1,1)));
            foreach (var p in platforms)
                p.Draw(spriteBatch);
            foreach (var m in monsters)
                m.Draw(spriteBatch);
            foreach (var w in weapons)
                w.Draw(spriteBatch);
            player.Draw(spriteBatch);
            particleEngine.Draw(spriteBatch);
            spriteBatch.End();
        }

    }
}
