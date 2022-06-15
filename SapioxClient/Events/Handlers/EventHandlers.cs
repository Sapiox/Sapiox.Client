using RemoteAdmin;
using SapioxClient.API;
using SapioxClient.Components;
using SynapseClient.Pipeline;
using SynapseClient.Pipeline.Packets;
using Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SapioxClient.Models;

namespace SapioxClient.Events.Handlers
{
    public class EventHandlers
    {
        public static void RegisterEvents()
        {
            ClientPipeline.DataReceivedEvent += MainReceivePipelineData;
            SapioxClient.Events.Handlers.Client.CreateCredits += OnCreateCredits;
        }

        public static void OnCreateCredits(CreditsHook ev)
        {
            ev.CreateCreditsCategory("Sapiox");
            ev.CreateCreditsEntry("Nakuliv", "Sapiox Developer", "Sapiox", CreditColors.Blue200);
        }

        public static void MainReceivePipelineData(PipelinePacket packet)
        {
            switch (packet.PacketId)
            {
                case ConnectionSuccessfulPacket.ID:
                    {
                        ConnectionSuccessfulPacket.Decode(packet, out var clientMods);
                        Log.Info("WelcomePacket: " + packet.AsString());
                        var salt = new byte[32];
                        for (var i = 0; i < 32; i++) salt[i] = 0x00;
                        var jwt = JsonWebToken.DecodeToObject<ClientConnectionData>("PrivateNakuId123", "", false);
                        var sessionBytes = Encoding.UTF8.GetBytes(jwt.Session);
                        var key = new byte[32];
                        for (var i = 0; i < 24; i++) key[i] = sessionBytes[i];
                        for (var i = 24; i < 32; i++) key[i] = 0x00;

                        QueryProcessor.Localplayer.Key = key;
                        QueryProcessor.Localplayer.CryptoManager.ExchangeRequested = true;
                        QueryProcessor.Localplayer.CryptoManager.EncryptionKey = key;
                        QueryProcessor.Localplayer.Salt = salt;
                        QueryProcessor.Localplayer.ClientSalt = salt;
                        ClientPipeline.Invoke(PipelinePacket.From(1, "Client connected successfully"));
                        //ClientBepInExPlugin.Get.ModLoader.ActivateForServer(clientMods); // Just activate for all for now
                        //SynapseEvents.InvokeConnectionSuccessful();
                        //SharedBundleManager.LogLoaded();
                        break;
                    }

                case RoundStartPacket.ID:
                    //API.Events.SynapseEvents.InvokeRoundStart();
                    break;

                case RedirectPacket.ID:
                    RedirectPacket.Decode(packet, out var target);
                    SapioxClient.API.Client.Redirect(target);
                    break;
            }
        }
    }
}
