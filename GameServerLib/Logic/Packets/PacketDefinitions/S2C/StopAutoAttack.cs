using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Packets.PacketHandlers;

namespace LeagueSandbox.GameServer.Logic.Packets.PacketDefinitions.S2C
{
    public class StopAutoAttack : BasePacket
    {
        public StopAutoAttack(AttackableUnit attacker)
            : base(PacketCmd.PKT_S2C_STOP_AUTO_ATTACK, attacker.NetId)
        {
            _buffer.Write((byte)0); // Flag
            _buffer.Write(0); // A netId
        }
    }
}