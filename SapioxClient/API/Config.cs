using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace SapioxClient.API
{
    public static class Config
    {
        public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        public static ISerializer Serializer { get; } = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        
        public static Dictionary<string, IConfig> ConfigList = new Dictionary<string, IConfig>();
        public static string path { get; set; } = Path.Combine(SapioxManager.ConfigDirectory, $"Config-{ServerStatic.ServerPort}.yml");

        public static void Load()
        {
            foreach (IPlugin plugin in SapioxManager.Plugins)
            {
                Dictionary<string, object> rawConfigs = Deserializer.Deserialize<Dictionary<string, object>>(File.Exists(path)?File.ReadAllText(path): " ") ?? new Dictionary<string, object>();
                
                if (!rawConfigs.TryGetValue(plugin.Info.Name, out object rawConfig))
                {
                    ConfigList.Add(plugin.Info.Name, plugin.config);
                    //Log.Info($"Generated config for {plugin.Info.Name}!");
                }
                else
                {
                    if (!ConfigList.ContainsKey(plugin.Info.Name))
                    {
                        ConfigList.Add(plugin.Info.Name, (IConfig)Deserializer.Deserialize(Serializer.Serialize(rawConfig), plugin.config.GetType()));
                    }

                    plugin.config = ConfigList[plugin.Info.Name];
                }
            }

            var yaml = Serializer.Serialize(ConfigList);
            File.WriteAllText(path, yaml);
        }

        public static void Reload()
        {
            
        }
    }
}