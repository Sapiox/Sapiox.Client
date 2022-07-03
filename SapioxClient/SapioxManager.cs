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
using SapioxClient.Events.Patches;
using SapioxClient.Components;
using SapioxClient.Events.Handlers;
using MelonLoader;
using UnityEngine;
using UnityEngine.Video;

namespace SapioxClient
{
    public class SapioxManager : MelonMod
    {
        public const int ClientMajor = 1;
        public const int ClientMinor = 0;
        public const int ClientPatch = 0;
        public static string ClientDescription = "Modded scpsl client";

        public static bool IsLoaded = false;
        private static string _sapioxDirectory;
        private static string _pluginDirectory;
        private static string _configDirectory;
        private static string _sapioxVersion = "1.0.0-Beta";

        public static List<IPlugin> Plugins = new List<IPlugin>();
        public static MelonLogger.Instance log;

        public static string SapioxVersion
        {
            get => _sapioxVersion;
            private set => _sapioxVersion = value;
        }

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
            typeof(Events.Patches.Credits),
            //typeof(Events.Patches.ServerList),
            typeof(GlobalPermissions),
            //typeof(PipelinePatches),
            typeof(SmallPatches),
            //typeof(CommandLine)
        };

        private void PatchMethods()
        {
            try
            {
                var instance = new HarmonyLib.Harmony("Sapiox.patches");

                foreach (var type in TypesToPatch)
                instance.PatchAll(type);
                LoggerInstance.Msg("Harmony Patching was sucessfully!");
            }
            catch (Exception e)
            {
                LoggerInstance.Error($"Harmony Patching threw an error:\n\n {e}");
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = LocalPlayer.Singleton.Raycast().point;

                var videoply = cube.AddComponent<VideoPlayer>();
                videoply.url = "https://drive.google.com/uc?id=1AaFL5p4IfdrbopVdbSV0yViI8yc1Ovrm&export=download";
                videoply.Play();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = LocalPlayer.Singleton.Raycast().point;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                LocalPlayer.Singleton.GetComponent<CharacterClassManager>().SetPlayersClass(RoleType.ClassD, LocalPlayer.Singleton.gameObject, CharacterClassManager.SpawnReason.ForceClass);
                LocalPlayer.Singleton.GetComponent<CharacterClassManager>().CurClass = RoleType.ClassD;
            }
        }

        public override void OnApplicationStart()
        {
            if (IsLoaded) return;

            log = LoggerInstance;

            CustomNetworkManager.Modded = true;
            BuildInfoCommand.ModDescription = string.Join(
                "\n",
                AppDomain.CurrentDomain.GetAssemblies().Select(a => $"{a.GetName().Name} - Version {a.GetName().Version.ToString(3)}"));

            var localpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sapiox", "Client");
            SapioxDirectory = Directory.Exists(localpath) ? localpath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sapiox", "Client");
            ConfigDirectory = Path.Combine(SapioxDirectory, "configs");
            PluginDirectory = Path.Combine(SapioxDirectory, "plugins");


            try
            {
                ComponentHandler.RegisterTypes();
                PatchMethods();
                EventHandlers.RegisterEvents();
                ActivatePlugins();
                IsLoaded = true;
            }
            catch (Exception e)
            {
                LoggerInstance.Error($"Sapiox.Loader: Error:\n{e}");
                return;
            }

            LoggerInstance.Msg("Sapiox.Loader: Sapiox is now Loaded!");

            IsLoaded = true;
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
                    LoggerInstance.Error($"Sapiox.Loader: Loading Assembly of {pluginpath} failed!\n{e}");
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
                    LoggerInstance.Error($"Sapiox.Loader: {infoTypePair.Value.Key.Assembly.GetName().Name} could not be activated!\n{e}");
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
                    LoggerInstance.Error($"Sapiox.Loader: {plugin.Info.Name} Loading failed!\n{e}");
                }
            }
        }
    }
}
