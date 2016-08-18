﻿using System;
using System.Collections.Generic;
using ENet;
using static LeagueSandbox.GameServer.Logic.Chatbox.ChatboxManager;
using LeagueSandbox.GameServer.Core.Logic.PacketHandlers;

namespace LeagueSandbox.GameServer.Logic.Chatbox.Commands
{
    class PacketCommand : ChatCommand
    {
        public PacketCommand(string command, string syntax, ChatboxManager owner) : base(command, syntax, owner) { }

        public override void Execute(Peer peer, bool hasReceivedArguments, string arguments = "")
        {
            try
            {
                var s = arguments.Split(' ');
                if (s.Length < 2)
                {
                    _owner.SendDebugMsgFormatted(DebugMsgType.SYNTAXERROR);
                    ShowSyntax();
                    return;
                }

                var opcode = Convert.ToByte(s[1], 16);
                var packet = new Packets.Packet((PacketCmdS2C)opcode);
                var buffer = packet.getBuffer();

                for (int i = 2; i < s.Length; i++)
                {
                    if (s[i] == "netid")
                    {
                        buffer.Write(_owner.GetGame().GetPeerInfo(peer).GetChampion().getNetId());
                    }
                    else
                    {
                        buffer.Write(Convert.ToByte(s[i], 16));
                    }
                }

                _owner.GetGame().PacketHandlerManager.sendPacket(peer, packet, Channel.CHL_S2C);
            }
            catch { }
        }
    }
}