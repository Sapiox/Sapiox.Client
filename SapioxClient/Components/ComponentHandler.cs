using MelonLoader;
using SapioxClient.API;
using UnhollowerRuntimeLib;

namespace SapioxClient.Components
{
    public static class ComponentHandler
    {
        public static void RegisterTypes()
        {
            Log.Info("Registering Types for Il2Cpp use...");
            //UnhollowerSupport.Initialize();
            ClassInjector.RegisterTypeInIl2Cpp<SapioxMenuWorker>();
            ClassInjector.RegisterTypeInIl2Cpp<SapioxSpawned>();
            //ClassInjector.RegisterTypeInIl2Cpp<LookReceiver>();
            //ClassInjector.RegisterTypeInIl2Cpp<CreditsHook>();
            ClassInjector.RegisterTypeInIl2Cpp<LocalPlayer>();
        }
    }
}
