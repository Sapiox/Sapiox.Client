using Il2CppSystem.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Console = GameCore.Console;

namespace SapioxClient.API
{
    public static class Client
    {
        public static bool CredentialsValid { get; set; } = false;
        private static NewMainMenu MainMenu => UnityEngine.Object.FindObjectOfType<NewMainMenu>();
        public static string CurrentSceneName => SceneManager.GetActiveScene().name;
        public static bool IsConnected => CurrentSceneName == "Facility";

        public static void QuitGame() => Application.Quit();

        public static void Connect(string address)
        {
            if (!IsConnected)
                MainMenu.Connect(address);
        }

        public static void Disconnect()
        {
            if (IsConnected)
            {
                Console.singleton.TypeCommand("disconnect");
                Console.singleton._clientCommandLogs.RemoveAt(Console.singleton._clientCommandLogs.Count - 1);
            }
        }

        public static void Redirect(string address)
        {
            Disconnect();
            _redirectCallback = delegate
            {
                CallbackQueue.Enqueue(
                    delegate
                    {
                        Log.Info("Trying to connect again");
                        Thread.Sleep(500);
                        Connect(address);
                    });
            };

        }

        internal static Action _redirectCallback;
        public static System.Collections.Generic.Queue<Action> CallbackQueue { get; } = new System.Collections.Generic.Queue<Action>();
        internal static void DoQueueTick()
        {
            for (int i = 0; i < CallbackQueue.Count; i++)
            {
                CallbackQueue.Dequeue().Invoke();
            }
        }
    }
}
