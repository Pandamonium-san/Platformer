using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Platformer
{
    static class MapHandler
    {
        public static StreamReader sr;
        public static List<int[]> mapData;
        public static String test = @"Content\test.txt";
        public static String lvl1 = @"Content\lvl1.txt";
        public static String lvl2 = @"Content\lvl2.txt";
        public static String lvl3 = @"Content\lvl3.txt";

        enum Read { platforms, startPoint }
        static Read read;
        static string[] split;
        static int[] split_int;
        static string line;

        public static List<int[]> GetMapFromText(String mapPath)
        {
            if (!File.Exists(mapPath))
                return null;
            mapData = new List<int[]>();

            sr = new StreamReader(mapPath);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line == "[platforms]")
                {
                    read = Read.platforms;
                    continue;
                }
                else if (line == "[start]")
                {
                    read = Read.startPoint;
                    continue;
                }
                if (read == Read.platforms)
                    ReadPlatforms();
            }
            sr.Close();

            return mapData;
        }

        private static void ReadPlatforms()
        {
            split = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            split_int = new int[split.Count()];

            for (int i = 0; i < split.Count(); i++)
            {
                split_int[i] = int.Parse(split[i]);
            }

            mapData.Add(split_int);
        }

    }
}
