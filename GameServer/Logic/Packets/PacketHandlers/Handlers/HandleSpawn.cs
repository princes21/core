﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENet;
using static ENet.Native;
using LeagueSandbox.GameServer.Logic.Packets;
using LeagueSandbox.GameServer.Logic.Enet;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.Items;

namespace LeagueSandbox.GameServer.Core.Logic.PacketHandlers.Packets
{
    class HandleSpawn : IPacketHandler
    {
        public unsafe bool HandlePacket(ENetPeer* peer, byte[] data, Game game)
        {
            var start = new StatePacket2(PacketCmdS2C.PKT_S2C_StartSpawn);
            bool p1 = PacketHandlerManager.getInstace().sendPacket(peer, start, Channel.CHL_S2C);
            Logger.LogCoreInfo("Spawning map");

            int playerId = 0;
            ClientInfo playerInfo = null;
            foreach (var p in game.getPlayers())
            {
                if (p.Item2.getPeer() == peer)
                {
                    playerInfo = p.Item2;
                }
                var spawn = new HeroSpawn(p.Item2, playerId++);
                PacketHandlerManager.getInstace().sendPacket(peer, spawn, Channel.CHL_S2C);

                var info = new PlayerInfo(p.Item2);
                PacketHandlerManager.getInstace().sendPacket(peer, info, Channel.CHL_S2C);
            }

            var peerInfo = game.getPeerInfo(peer);
            var bluePill = ItemManager.getInstance().getItemTemplateById(game.getMap().getBluePillId());
            var itemInstance = new ItemInstance(bluePill, 7);
            var buyItem = new BuyItemAns(peerInfo.getChampion(), itemInstance);
            PacketHandlerManager.getInstace().sendPacket(peer, buyItem, Channel.CHL_S2C);

            // Not sure why both 7 and 14 skill slot, but it does not seem to work without it
            var skillUp = new SkillUpPacket(peerInfo.getChampion().getNetId(), 7, 1, (byte)peerInfo.getChampion().getSkillPoints());
            PacketHandlerManager.getInstace().sendPacket(peer, skillUp, Channel.CHL_GAMEPLAY);
            skillUp = new SkillUpPacket(peerInfo.getChampion().getNetId(), 14, 1, (byte)peerInfo.getChampion().getSkillPoints());
            PacketHandlerManager.getInstace().sendPacket(peer, skillUp, Channel.CHL_GAMEPLAY);

            peerInfo.getChampion().getStats().setSpellEnabled(7, true);
            peerInfo.getChampion().getStats().setSpellEnabled(14, true);
            peerInfo.getChampion().getStats().setSummonerSpellEnabled(0, true);
            peerInfo.getChampion().getStats().setSummonerSpellEnabled(1, true);

            var objects = game.getMap().getObjects();
            foreach (var kv in objects)
            {
                var t = kv.Value as Turret;
                if (t != null)
                {
                    var turretSpawn = new TurretSpawn(t);
                    PacketHandlerManager.getInstace().sendPacket(peer, turretSpawn, Channel.CHL_S2C);

                    // To suppress game HP-related errors for enemy turrets out of vision
                    var sh = new SetHealth(t);
                    PacketHandlerManager.getInstace().sendPacket(peer, sh, Channel.CHL_S2C);
                    continue;
                }

                var lp = kv.Value as LevelProp;
                if (lp != null)
                {
                    var lpsPacket = new LevelPropSpawn(lp);
                    PacketHandlerManager.getInstace().sendPacket(peer, lpsPacket, Channel.CHL_S2C);
                }
            }

            // Level props are just models, we need button-object minions to allow the client to interact with it
            if (playerInfo != null && playerInfo.getTeam() == TeamId.TEAM_BLUE)
            {
                // Shop (blue team)
                var ms1 = new MinionSpawn2(0xff10c6db);
                PacketHandlerManager.getInstace().sendPacket(peer, ms1, Channel.CHL_S2C);
                var sh1 = new SetHealth(0xff10c6db);
                PacketHandlerManager.getInstace().sendPacket(peer, sh1, Channel.CHL_S2C);

                // Vision for hardcoded objects
                // Top inhib
                var ms2 = new MinionSpawn2(0xffd23c3e);
                PacketHandlerManager.getInstace().sendPacket(peer, ms2, Channel.CHL_S2C);
                var sh2 = new SetHealth(0xffd23c3e);
                PacketHandlerManager.getInstace().sendPacket(peer, sh2, Channel.CHL_S2C);

                // Mid inhib
                var ms3 = new MinionSpawn2(0xff4a20f1);
                PacketHandlerManager.getInstace().sendPacket(peer, ms3, Channel.CHL_S2C);
                var sh3 = new SetHealth(0xff4a20f1);
                PacketHandlerManager.getInstace().sendPacket(peer, sh3, Channel.CHL_S2C);

                // Bottom inhib
                var ms4 = new MinionSpawn2(0xff9303e1);
                PacketHandlerManager.getInstace().sendPacket(peer, ms4, Channel.CHL_S2C);
                var sh4 = new SetHealth(0xff9303e1);
                PacketHandlerManager.getInstace().sendPacket(peer, sh4, Channel.CHL_S2C);

                // Nexus
                var ms5 = new MinionSpawn2(0xfff97db5);
                PacketHandlerManager.getInstace().sendPacket(peer, ms5, Channel.CHL_S2C);
                var sh5 = new SetHealth(0xfff97db5);
                PacketHandlerManager.getInstace().sendPacket(peer, sh5, Channel.CHL_S2C);

            }
            else if (playerInfo != null && playerInfo.getTeam() == TeamId.TEAM_PURPLE)
            {
                // Shop (purple team)
                var ms1 = new MinionSpawn2(0xffa6170e);
                PacketHandlerManager.getInstace().sendPacket(peer, ms1, Channel.CHL_S2C);
                var sh1 = new SetHealth(0xffa6170e);
                PacketHandlerManager.getInstace().sendPacket(peer, sh1, Channel.CHL_S2C);

                // Vision for hardcoded objects
                // Top inhib
                var ms2 = new MinionSpawn2(0xff6793d0);
                PacketHandlerManager.getInstace().sendPacket(peer, ms2, Channel.CHL_S2C);
                var sh2 = new SetHealth(0xff6793d0);
                PacketHandlerManager.getInstace().sendPacket(peer, sh2, Channel.CHL_S2C);

                // Mid inhib
                var ms3 = new MinionSpawn2(0xffff8f1f);
                PacketHandlerManager.getInstace().sendPacket(peer, ms3, Channel.CHL_S2C);
                var sh3 = new SetHealth(0xffff8f1f);
                PacketHandlerManager.getInstace().sendPacket(peer, sh3, Channel.CHL_S2C);

                // Bottom inhib
                var ms4 = new MinionSpawn2(0xff26ac0f);
                PacketHandlerManager.getInstace().sendPacket(peer, ms4, Channel.CHL_S2C);
                var sh4 = new SetHealth(0xff26ac0f);
                PacketHandlerManager.getInstace().sendPacket(peer, sh4, Channel.CHL_S2C);

                // Nexus
                var ms5 = new MinionSpawn2(0xfff02c0f);
                PacketHandlerManager.getInstace().sendPacket(peer, ms5, Channel.CHL_S2C);
                var sh5 = new SetHealth(0xfff02c0f);
                PacketHandlerManager.getInstace().sendPacket(peer, sh5, Channel.CHL_S2C);
            }

            var end = new StatePacket(PacketCmdS2C.PKT_S2C_EndSpawn);
            bool p2 = PacketHandlerManager.getInstace().sendPacket(peer, end, Channel.CHL_S2C);

            return p1 && p2;
        }
    }
}