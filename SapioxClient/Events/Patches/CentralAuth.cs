using System;
using System.IO;
using System.Threading;
using GameCore;
using HarmonyLib;
using LiteNetLib.Utils;
using Mirror;
using Org.BouncyCastle.Utilities.Encoders;
using SapioxClient.API;
using SapioxClient.Components;
using UnhollowerBaseLib;
using Byte = System.Byte;
using DateTimeOffset = System.DateTimeOffset;
using Encoding = Il2CppSystem.Text.Encoding;
using Log = SapioxClient.API.Log;
using Random = System.Random;
using String = Il2CppSystem.String;

namespace SapioxClient.Events.Patches
{
    public class CentralAuth
    {
        private static int injectionStep = 0;
        private static bool hasInjectedByte = false;
        private static string targetAddress = "";
        public static string sapioxSessionToken = "";

        /*[HarmonyPatch(typeof(NetDataWriter), nameof(NetDataWriter.Put), typeof(byte))]
        [HarmonyPrefix]
        public static bool OnNetDataWriterByte(ref byte value)
        {
            if (!hasInjectedByte)
            {
                hasInjectedByte = true;
                value = 5;
            }
            else
            {
                return false;
            }
            Log.Info("NetWriter Write: Byte " + value);
            injectionStep = 0;
            return true;
        }

        [HarmonyPatch(typeof(NetDataWriter), nameof(NetDataWriter.Put), typeof(string))]
        [HarmonyPrefix]
        public static bool OnNetDataWriterString(NetDataWriter __instance, String value)
        {
            injectionStep += 1;

            if (injectionStep == 1)
            {
                SapioxManager.log.Msg("Beginning own Body");

                while (!Client.CredentialsValid)
                {
                    Thread.Sleep(5); //Not pretty but works
                }

                SapioxManager.log.Msg("Starting session");
                //synapseSessionToken = "Private.Naku.Id";

                //var str = File.ReadAllText(Path.Combine(Computer.ApplicationDataDir, "user.dat"));
                var random = new Random();
                byte[] bytes = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    bytes[i] = (byte)random.Next(0x41, 0x4b);
                }

                var nonce = "Private.test.Id" + "#" + Encoding.UTF8.GetString(bytes);

                hasInjectedByte = false;
                __instance.Put(15);
                __instance.Put(Encoding.UTF8.GetBytes("Private.test.Id"));
                __instance.Put(15);
                __instance.Put(Encoding.UTF8.GetBytes("Private.test.Id"));
                __instance.Put(nonce.Length);
                __instance.Put(Encoding.UTF8.GetBytes(nonce));
                SapioxManager.log.Msg("==> Body complete");
                PlayerPrefsSl.Set("nickname", nonce);
                SapioxManager.log.Msg("==> Updated NickName to include nonce");
                return false;
            }

            SapioxManager.log.Msg("NetWriter Write: String " + value);

            return true;
        }

        [HarmonyPatch(typeof(NetDataWriter), nameof(NetDataWriter.Put), typeof(Il2CppStructArray<byte>))]
        [HarmonyPrefix]
        public static bool OnNetDataWriterByteArray(Il2CppStructArray<byte> data)
        {
            if (hasInjectedByte) return false;
            Log.Info("NetWriter Write: ByteArray " + (data).ToString());
            hasInjectedByte = false;
            return true;
        }

        [HarmonyPatch(typeof(NetDataWriter), nameof(NetDataWriter.Put), typeof(int))]
        [HarmonyPrefix]
        public static bool OnNetDataWriterInt(int value)
        {
            if (hasInjectedByte) return false;
            Log.Info("NetWriter Write: Int " + value);
            return true;
        }

        [HarmonyPatch(typeof(NetworkClient), nameof(NetworkClient.Connect), typeof(string))]
        [HarmonyPrefix]
        public static bool OnClientConnect(string address)
        {
            SapioxManager.log.Msg($"Connecting via Network Client with address {address}");
            targetAddress = address;
            return true;
        }

        [HarmonyPatch(typeof(NetworkClient), nameof(NetworkClient.OnConnected))]
        [HarmonyPrefix]
        public static bool OnClientConnected()
        {
            SapioxManager.log.Msg("Finished Connecting");
            return true;
        }*/


        /*[HarmonyPatch(typeof(CentralAuthManager), nameof(CentralAuthManager.Sign))]
        [HarmonyPrefix]
        public static bool OnSing(ref string __result, string ticket)
        {
            try
            {
                SapioxManager.log.Msg($"Trying to sign: {ticket}");
                CentralAuthManager.Authenticated = true;
                __result = "TICKET";
                return true;
            }
            catch (Exception e)
            {
                SapioxManager.log.Msg($"{typeof(CentralAuth).FullName}.{nameof(OnSing)}:\n{e}");
                return false;
            }
        }*/


        [HarmonyPatch(typeof(CentralAuthManager), nameof(CentralAuthManager.Authentication))]
        [HarmonyPrefix]
        public static bool OnAuth()
        {
            SapioxManager.log.Msg("Faking Central Server Data...");
            /*CentralAuthManager.Abort();
            CentralAuthManager.InitAuth();
            CentralAuthManager.ApiToken = "";*/
            CentralAuthManager.Authenticated = true;
            CentralAuthManager.AuthStatusType = AuthStatusType.Success;
            CentralAuthManager.Nonce = "Northwood";
            CentralAuthManager.PreauthToken = new CentralAuthPreauthToken
            {
                Country = "PL",
                Expiration = DateTimeOffset.Now.ToUnixTimeSeconds() + 1000,
                Flags = Byte.MaxValue,
                Signature = "test",
                UserId = "Zabszk@Northwood"
            };

            Handlers.Client.OnCentralAuth();

            return true;
        }
    }
}
