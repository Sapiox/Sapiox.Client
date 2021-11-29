using System;
using System.Reflection;

namespace SapioxClient.API
{
    public static class Log
    {
        public static void Info(string message) => Info((object)message);

        public static void Info(object message)
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Send($"{name}: {message}", UnityEngine.Color.green);
        }

        public static void Warn(string message) => Warn((object)message);

        public static void Warn(object message)
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Send($"{name}: {message}", UnityEngine.Color.yellow);
        }

        public static void Error(string message) => Error((object)message);

        public static void Error(object message)
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Send($"{name}: {message}", UnityEngine.Color.red);
        }

        public static void Send(string message, UnityEngine.Color color) => GameCore.Console.AddLog("<color=grey>[SAPIOX]</color>" + message, color);
    }
}