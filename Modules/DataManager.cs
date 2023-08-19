using BepInEx;
using System.IO;
using System;

namespace TownOfThem.Modules
{
    public static class DataManager
    {
        public static string DATA_FOLDER_NAME = "TOT_DATA";
        public static string AMONG_US_PATH = Directory.GetCurrentDirectory();
        public static string TOT_DATA_PATH = AMONG_US_PATH + @"\" + DATA_FOLDER_NAME;
        public static void Init()
        {
            try
            {
                if (!Directory.Exists(DATA_FOLDER_NAME))
                {
                    Directory.CreateDirectory(DATA_FOLDER_NAME);
                    Main.Log.LogInfo("Data dir created successfully!");
                }

            }
            catch (Exception e)
            {
                Main.Log.LogFatal($"Error creating data dir: {e.Message}");
            }

        }
    }
}
