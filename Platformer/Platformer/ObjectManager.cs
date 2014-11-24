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
        public static Texture2D tileTexture, actorTexture, slimeTexture, redSlimeTexture, monsterTexture, gargoyleTexture, 
            smokeTexture, 
            daggerTexture, axeTexture, bowTexture,
            arrowTexture, ballTexture;
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
            daggerTexture = Content.Load<Texture2D>("swordSheet");
            actorTexture = Content.Load<Texture2D>("Actor1");
            monsterTexture = Content.Load<Texture2D>("Monster1");
            slimeTexture = Content.Load<Texture2D>("slime");
            redSlimeTexture = Content.Load<Texture2D>("red_slime");
            smokeTexture = Content.Load<Texture2D>("smoke_particle");
            axeTexture = Content.Load<Texture2D>("RubyAxe1");
            bowTexture = Content.Load<Texture2D>("bow");
            arrowTexture = Content.Load<Texture2D>("arrow");
            ballTexture = Content.Load<Texture2D>("ball");
            gargoyleTexture = Content.Load<Texture2D>("gargoyle");
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
                LoadWeapons(MapHandler.GetWeaponData(path));
            }
            player = new Player(actorTexture, MapHandler.startingPos);
        }

        public void LoadWorld(List<int[]> mapData)
        {
            if (mapData != null)
                for (int i = 0; i < mapData.Count(); i++)
                {
                    platforms.Add(new Platform(ObjectManager.tileTexture, new Vector2(mapData[i][0], mapData[i][1]), mapData[i][2]));
                }
        }

        public void LoadMonsters(List<int[]> monsterData)
        {
            if (monsterData != null)
            {
                for (int i = 0; i < monsterData.Count(); i++)
                {
                    Vector2 position = new Vector2 (monsterData[i][0], monsterData[i][1]);
                    if (monsterData[i][2] == 1)
                        monsters.Add(new Slime(ObjectManager.slimeTexture, position));
                    else if (monsterData[i][2] == 2)
                        monsters.Add(new RedSlime(ObjectManager.redSlimeTexture, position));
                    else if (monsterData[i][2] == 3)
                        monsters.Add(new Skeleton(ObjectManager.monsterTexture, position));
                    else if (monsterData[i][2] == 4)
                        monsters.Add(new Gargoyle(ObjectManager.gargoyleTexture, position));
                }
            }
        }

        public void LoadWeapons(List<int[]> weaponData)
        {
            if (weaponData != null)
            {
                for (int i = 0; i < weaponData.Count(); i++)
                {
                    Vector2 position = new Vector2(weaponData[i][0], weaponData[i][1]);
                    if (weaponData[i][2] == 1)
                        weapons.Add(new Dagger(ObjectManager.daggerTexture, position));
                    else if (weaponData[i][2] == 2)
                        weapons.Add(new RubyAxe(ObjectManager.axeTexture, position));
                    else if (weaponData[i][2] == 3)
                        weapons.Add(new Bow(ObjectManager.bowTexture, position));
                }
            }
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
                if(player.equippedWeapon is ProjectileWeapon)
                    ProjectileCollidesWithMonster((ProjectileWeapon)player.equippedWeapon, m);
                if (m.ranged)
                    PlayerCollidesWithMonsterProjectile(m.projectiles);
                PlayerCollidesWithMonster(m);
                if (m.dead)
                {
                    if(m is Skeleton)
                    MonsterDropWeapon((Skeleton)m);
                    monsters.Remove(m);
                    break;
                }
                if(m is Gargoyle)
                UpdateGargoyle((Gargoyle)m);
            }

            if (WinCondition() || LoseCondition())
            {
                gameOverCounter -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private void UpdateGargoyle(Gargoyle g)
        {
            g.TrackPlayer(player);
        }

        private void PlayerCollidesWithMonsterProjectile(List<Projectile> projectiles)
        {
            if (projectiles == null)
                return;

            for (int i = 0; i < projectiles.Count(); i++)
            {
                if (player.hitbox.Intersects(projectiles[i].hitbox))
                    if (player.PixelCollision(projectiles[i]))
                    {
                        player.TakeDamage(projectiles[i].dir, projectiles[i].damage);
                        projectiles.Remove(projectiles[i]);
                        i--;
                    }
            }
        }

        private void PlayerCollidesWithMonster(Monster m)
        {
            if (!player.invulnerable && !player.dying && m.hitbox.Intersects(player.hitbox))
                if (m.PixelCollision(player))
                    player.TakeDamage(m.dir, m.damage);
        }

        private void WeaponCollidesWithMonster(Weapon w, Monster m)
        {
            if (w == null || !(w is MeleeWeapon) || m.invulnerable)
                return;

            if (w.attacking  && m.hitbox.Intersects(w.hitbox))
                if (m.PixelCollision(w))
                    m.TakeDamage(player.dir, w.damage);
        }

        private void ProjectileCollidesWithMonster(ProjectileWeapon w, Monster m)
        {
            if (w == null || !(w is ProjectileWeapon) || m.invulnerable)
                return;

            for (int i = 0; i < w.projectiles.Count(); i++)
            {
                if (m.hitbox.Intersects(w.projectiles[i].hitbox))
                    if (m.PixelCollision(w.projectiles[i]))
                    {
                        m.TakeDamage(w.projectiles[i].dir, w.damage);
                        w.projectiles.Remove(w.projectiles[i]);
                        i--;
                    }
            }
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

        private void MonsterDropWeapon(Skeleton s)
        {
            s.bow.PrepareToDropWeapon();
            weapons.Add(s.bow);
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
            foreach(Monster m in monsters)
                if(m is Gargoyle)
                return false;
            return true;
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
