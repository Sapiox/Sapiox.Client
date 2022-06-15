using SapioxClient.Components;
using System;

namespace SapioxClient.Events.Handlers
{
    public class Client
    {
        public static event Action CentralAuth;
        public static event Action MainMenuStart;
        public static event Action<CreditsHook> CreateCredits;

        public static void OnCentralAuth() => CentralAuth?.Invoke();
        public static void OnMainMenuStart() => MainMenuStart?.Invoke();
        internal static void OnCreateCredits(CreditsHook ev) => CreateCredits?.Invoke(ev);
    }
}