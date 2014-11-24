using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platformer
{
    static class MapHandler
    {
        public static StreamReader sr;
        public static List<int[]> mapData, monsterData, weaponData;
        public static String test = @"Content\test.txt";
        public static String lvl1 = @"Content\lvl1.txt";
        public static String lvl2 = @"Content\lvl2.txt";
        public static String lvl3 = @"Content\lvl3.txt";
        public static Vector2 startingPos;
        public static Rectangle worldSize;

        enum Read { platforms, monsters, weapons, playerPos, worldSize }
        static Read read;

        static string line;

        public static List<int[]> GetMapData(String mapPath)
        {
            if (!File.Exists(mapPath))
                return null;
            mapData = new List<int[]>();

            sr = new StreamReader(mapPath);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                ReadHeaders();

                if (line == "" || line[0] == '[')
                    continue;

                switch (read)
                {
                    case Read.platforms:
                        mapData.Add(SplitLineAndConvertToInt());
                        break;
                    case Read.playerPos:
                        ReadStartPos();
                        break;
                    case Read.worldSize:
                        ReadWorldSize();
                        break;
                }
            }
            sr.Close();

            return mapData;
        }

        public static List<int[]> GetMonsterData(String mapPath)
        {
            if (!File.Exists(mapPath))
                return null;
            monsterData = new List<int[]>();

            sr = new StreamReader(mapPath);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                ReadHeaders();

                if (line == "" || line[0] == '[')
                    continue;
                
                if (read == Read.monsters)
                {
                    monsterData.Add(SplitLineAndConvertToInt());
                }
            }
            sr.Close();

            return monsterData;
        }

        public static List<int[]> GetWeaponData(String mapPath)
        {
            if (!File.Exists(mapPath))
                return null;
            weaponData = new List<int[]>();

            sr = new StreamReader(mapPath);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                ReadHeaders();

                if (line == "" || line[0] == '[')
                    continue;

                if (read == Read.weapons)
                {
                    weaponData.Add(SplitLineAndConvertToInt());
                }
            }
            sr.Close();

            return weaponData;
        }

        private static void ReadHeaders()
        {
            if (line == "[platforms]")
            {
                read = Read.platforms;
            }
            else if (line == "[startPos]")
            {
                read = Read.playerPos;
            }
            else if (line == "[worldSize]")
            {
                read = Read.worldSize;
            }
            else if (line == "[monsters]")
            {
                read = Read.monsters;
            }
            else if (line == "[weapons]")
            {
                read = Read.weapons;
            }
        }

        private static void ReadStartPos()
        {
            int[] playerPos = SplitLineAndConvertToInt();
            startingPos = new Vector2(playerPos[0], playerPos[1]);
        }

        private static void ReadWorldSize()
        {
            int[] worldSizeData = SplitLineAndConvertToInt();
            worldSize = new Rectangle(0, 0, worldSizeData[0], worldSizeData[1]);
        }

        private static int[] SplitLineAndConvertToInt()
        {
            string[] split = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int[] split_int = new int[split.Count()];

            for (int i = 0; i < split.Count(); i++)
            {
                split_int[i] = int.Parse(split[i]);
            }
            return split_int;
        }

        public static void SaveWorldToText(ObjectManager obj, string path)
        {
            List<String> mapText = new List<String>();

            mapText.Add("[platforms]");
            for (int i = 0; i < ObjectManager.platforms.Count(); i++)
            {
                line = ObjectManager.platforms[i].hitbox.X.ToString() + ";"
                    + ObjectManager.platforms[i].hitbox.Y.ToString() + ";"
                    + ObjectManager.platforms[i].platformID.ToString();
                mapText.Add(line);
            }

            mapText.Add("[monsters]");
            for (int i = 0; i < ObjectManager.monsters.Count(); i++)
            {
                line = ((int)ObjectManager.monsters[i].pos.X).ToString() + ";" + ((int)ObjectManager.monsters[i].pos.Y).ToString() + ";" + ObjectManager.monsters[i].monsterID.ToString();
                mapText.Add(line);
            }

            mapText.Add("[weapons]");
            for (int i = 0; i < ObjectManager.weapons.Count(); i++)
            {
                line = ((int)ObjectManager.weapons[i].pos.X).ToString() + ";" + ((int)ObjectManager.weapons[i].pos.Y).ToString() + ";" + ObjectManager.weapons[i].weaponID.ToString();
                mapText.Add(line);
            }

            mapText.Add("[startPos]");
            line = ((int)obj.player.pos.X).ToString() + ";" + ((int)obj.player.pos.Y).ToString();
            mapText.Add(line);

            mapText.Add("[worldSize]");
            line = "6016;1450";
            mapText.Add(line);

            File.WriteAllLines(path, mapText);
        }


    }
}
