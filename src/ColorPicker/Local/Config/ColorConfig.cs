﻿using ColorPicker.Local.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ColorPicker.Local.Config
{
    public class ColorConfig
    {
        public static string WIN_PATH { get; }
        public static string SYS_PATH { get; }
        public static string CFG_PATH { get; }
        public static ConfigModel Config { get; private set; }

        static ColorConfig()
        {
            WIN_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            SYS_PATH = string.Format(@"{0}\ColorPicker\System", WIN_PATH);
            CFG_PATH = string.Format(@"{0}\Config.yaml", SYS_PATH);

            LoadConfigFile();
        }

        private static void LoadConfigFile()
        {
            if (!Directory.Exists(SYS_PATH))
            {
                _ = Directory.CreateDirectory(SYS_PATH);
            }

            if (!File.Exists(CFG_PATH))
            {
                SaveConfig(new ConfigModel());
            }

            IDeserializer deserializer = new DeserializerBuilder()
              .WithNamingConvention(CamelCaseNamingConvention.Instance)
              .Build();

            Config = deserializer.Deserialize<ConfigModel>(File.ReadAllText(CFG_PATH));
        }

        public static void SaveSpoidColor(string color)
        {
            Config.SpoidColor = color;
            SaveConfig(Config);
        }

        public static ConfigModel LoadConfig()
        {
            return Config;
        }

        public static void SaveLocation(int x, int y, int width, int height)
        {
            if (Config.ViewOptions.FirstOrDefault() is ViewOptionModel view)
            {
                view.LocX = x;
                view.LocY = y;
                view.Width = width;
                view.Height = height;
            }
            else
            {
                Config.ViewOptions.Add(new ViewOptionModel { LocX = x, LocY = y, Width = width, Height = height });
            }

            SaveConfig(Config);
        }

        private static void SaveConfig(ConfigModel config)
        {
            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            string yaml = serializer.Serialize(config);

            File.WriteAllText(CFG_PATH, yaml);
        }
    }
}
