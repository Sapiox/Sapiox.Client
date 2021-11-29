using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SapioxClient.API
{    
    public interface IPlugin
    {
        PluginInfo Info { get; set; }
        IConfig config { get; set; }
        
        string PluginDirectory { get; set; }

        void Load();
    }

    public abstract class Plugin : IPlugin
    {
        public PluginInfo Info { get; set; }

        public virtual IConfig config { get; set; }

        private string _pluginDirectory;

        public virtual void Load()
        {
            if(!config.Load) return;
            Config.Load();
            SapioxManager.log.LogInfo($"{Info.Name} by {Info.Author} has been enabled!");
        }

        public string PluginDirectory
        {
            get
            {
                if (_pluginDirectory == null)
                    return null;

                if (!Directory.Exists(_pluginDirectory))
                    Directory.CreateDirectory(_pluginDirectory);

                return _pluginDirectory;
            }
            set => _pluginDirectory = value;
        }
    }
}