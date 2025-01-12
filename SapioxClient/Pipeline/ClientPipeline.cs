﻿using System;
using System.Text;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Encoders;
using SapioxClient.API;
using SapioxClient.Events.Patches;

namespace SynapseClient.Pipeline
{
    public static class ClientPipeline
    {
        public static event DataEvent<PipelinePacket> DataReceivedEvent;
        
        public static void Receive(PipelinePacket data)
        {
            Log.Info($"=pipeline=>  {data}");
            DataReceivedEvent?.Invoke(data);
        }

        public static void Invoke(PipelinePacket packet)
        {
            var packed = DataUtils.Pack(packet);
            Log.Info($"<=pipeline=  {Base64.ToBase64String(packed)}");
           PipelinePatches.Transmission.CmdCommandToServer(packed, false);
        }
        
        public delegate void DataEvent<in TEvent>(TEvent ev);
    }
    
    public static class DataUtils
    {
        public static byte[] Pack(PipelinePacket packet)
        {
            var data = packet.Data;
            var buffer = new byte[data.Length + 7];
            buffer[0] = byte.MinValue;
            buffer[1] = byte.MaxValue;
            var idBytes = BitConverter.GetBytes(packet.PacketId);
            buffer[2] = idBytes[0];
            buffer[3] = idBytes[1];
            buffer[4] = idBytes[2];
            buffer[5] = idBytes[3];
            buffer[6] = packet.StreamStatus;
            for (var i = 0; i < data.Length; i++) buffer[i + 7] = data[i];
            return buffer;
        }
        
        public static PipelinePacket Unpack(byte[] encoded)
        {
            var packetId = BitConverter.ToUInt32(encoded, 2);
            var buffer = new byte[encoded.Length - 7];
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = encoded[i +  7];
            }
            return new PipelinePacket
            {
                PacketId = packetId,
                Data = buffer,
                StreamStatus = encoded[6]
            };
        }

        public static bool IsData(byte[] bytes)
        {
            if (bytes.Length < 2) return false;
            return bytes[0] == byte.MinValue && bytes[1] == byte.MaxValue;
        }

    }
    
    /*
     == PacketList ==
     
     BaseMisc:
     0: Welcome Packet
     1: Message Packet -PLANNED-
     
     Object Packets
     10: Object Spawn
     11: Object Destroy
     12: Object Location
     
     Client Packets
     20: Client Redirect
     21: Client PlaySound
     
     Round Packets
     30: Round Start
     
     Streaming Packets
     30: Streamed AssetBundle -PLANNED-
    
     */
    
    public class PipelinePacket
    {
        public uint PacketId { get; set; }
        public byte[] Data { get; set; }

        public byte StreamStatus { get; set; } = 0x00;
 
        public override string ToString()
        {
            return $"Packet_{PacketId} [ {Base64.ToBase64String(Data)} ] Stream: {StreamStatus}";
        }

        public string AsString()
        {
            return Encoding.UTF8.GetString(Data);
        }

        //Just to maintain a style
        public byte[] AsByteArray()
        {
            return Data;
        }

        public T As<T>()
        {
            var s = AsString();
            return JsonConvert.DeserializeObject<T>(s);
        }

        public static PipelinePacket From(uint id, byte[] payload)
        {
            return new PipelinePacket
            {
                PacketId = id,
                Data = payload
            };
        }
        
        public static PipelinePacket From(uint id, string payload)
        {
            return new PipelinePacket
            {
                PacketId = id,
                Data = Encoding.UTF8.GetBytes(payload)
            };
        }

        public static PipelinePacket From<T>(uint id, T payload)
        {
            var encoded = JsonConvert.SerializeObject(payload);
            return From(id, encoded);
        }
    }
}