using SapioxClient.API;

namespace SapioxClient.Example
{
    [PluginInfo(
        Author = "Naku",
        Name = "Sapiox-ExampleMod",
        Description = "Example client plugin",
        Version = "1.0.0"
        )]
    public class Plugin : API.Plugin
    {
        public override IConfig config { get; set; } = new Config();
        public override void Load()
        {
            base.Load();
            Events.Handlers.Round.Start += OnRoundStart;
        }

        public void OnRoundStart()
        {
            Log.Info($"Round has been started.");
        }
    }
}
