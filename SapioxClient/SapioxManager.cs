using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandSystem.Commands;
using CommandSystem.Commands.Shared;
using SapioxClient.API;
using HarmonyLib;
using CommandSystem;
using RemoteAdmin;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using BepInEx;
using SapioxClient.Events.Patches;

namespace SapioxClient
{
    [BepInPlugin("Sapiox", "SapioxClient", "1.0.0")]
    public class SapioxManager : BasePlugin
    {
        public const int ClientMajor = 1;
        public const int ClientMinor = 0;
        public const int ClientPatch = 0;
        public const string ClientVersion = "1.0.0-Beta";
        public const string ClientDescription = "Modded scpsl client";


        public static ManualLogSource log;

        public static bool IsLoaded = false;
        private static string _sapioxDirectory;
        private static string _pluginDirectory;
        private static string _configDirectory;
        public static List<IPlugin> Plugins = new List<IPlugin>();

        public static string SapioxDirectory
        {
            get
            {
                if (!Directory.Exists(_sapioxDirectory))
                    Directory.CreateDirectory(_sapioxDirectory);

                return _sapioxDirectory;
            }
            private set => _sapioxDirectory = value;
        }

        public static string PluginDirectory
        {
            get
            {
                if (!Directory.Exists(_pluginDirectory))
                    Directory.CreateDirectory(_pluginDirectory);

                return _pluginDirectory;
            }
            private set => _pluginDirectory = value;
        }

        public static string ConfigDirectory
        {
            get
            {
                if (!Directory.Exists(_configDirectory))
                    Directory.CreateDirectory(_configDirectory);

                return _configDirectory;
            }
            private set => _configDirectory = value;
        }

        public List<Type> TypesToPatch { get; } = new List<Type>
        {
            typeof(Events.Patches.CentralAuth),
            typeof(MainMenu),
            typeof(News),
            typeof(Events.Patches.ServerList)
        };

        public void PatchMethods()
        {
            try
            {
                var instance = new Harmony("Sapiox.patches");

                foreach (var type in TypesToPatch)
                instance.PatchAll(type);

                Log.LogInfo("Harmony Patching was sucessfully!");
            }
            catch (Exception e)
            {
                Log.LogError($"Harmony Patching threw an error:\n\n {e}");
            }
        }

        public override void Load()
        {
            if (IsLoaded) return;

            CustomNetworkManager.Modded = true;
            BuildInfoCommand.ModDescription = string.Join(
                "\n",
                AppDomain.CurrentDomain.GetAssemblies().Select(a => $"{a.GetName().Name} - Version {a.GetName().Version.ToString(3)}"));

            log = Log;

            var localpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sapiox", "Client");
            SapioxDirectory = Directory.Exists(localpath) ? localpath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sapiox", "Client");
            ConfigDirectory = Path.Combine(SapioxDirectory, "configs");
            PluginDirectory = Path.Combine(SapioxDirectory, "plugins");

            try
            {
                PatchMethods();
                ActivatePlugins();
                IsLoaded = true;
            }
            catch (Exception e)
            {
                Log.LogError($"Sapiox.Loader: Error:\n{e}");
                return;
            }

            Log.LogInfo("Sapiox.Loader: Sapiox is now Loaded!");
        }

        public void ActivatePlugins()
        {
            var paths = Directory.GetFiles(PluginDirectory, "*.dll").ToList();

            var dictionary = new Dictionary<API.PluginInfo, KeyValuePair<Type, List<Type>>>();

            foreach (var pluginpath in paths)
            {
                try
                {
                    var assembly = Assembly.Load(File.ReadAllBytes(pluginpath));
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!typeof(IPlugin).IsAssignableFrom(type))
                            continue;

                        var infos = type.GetCustomAttribute<API.PluginInfo>();

                        if (infos == null)
                        {
                            infos = new API.PluginInfo();
                        }

                        var allTypes = assembly.GetTypes().ToList();
                        allTypes.Remove(type);
                        dictionary.Add(infos, new KeyValuePair<Type, List<Type>>(type, allTypes));
                        break;
                    }
                }
                catch (Exception e)
                {
                    Log.LogError($"Sapiox.Loader: Loading Assembly of {pluginpath} failed!\n{e}");
                }
            }

            foreach (var infoTypePair in dictionary)
            {
                try
                {
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(infoTypePair.Value.Key);
                    plugin.Info = infoTypePair.Key;
                    plugin.PluginDirectory = GetPluginDirectory(plugin.Info);
                    Plugins.Add(plugin);
                }
                catch (Exception e)
                {
                    Log.LogError($"Sapiox.Loader: {infoTypePair.Value.Key.Assembly.GetName().Name} could not be activated!\n{e}");
                }
            }
            LoadPlugins();
        }

        public string GetPluginDirectory(API.PluginInfo infos)
        {
            return Path.Combine(PluginDirectory, infos.Name);
        }

        public void LoadPlugins()
        {
            foreach (var plugin in Plugins)
            {
                try
                {
                    plugin.Load();
                }
                catch (Exception e)
                {
                    Log.LogError($"Sapiox.Loader: {plugin.Info.Name} Loading failed!\n{e}");
                }
            }
        }
    }
}
