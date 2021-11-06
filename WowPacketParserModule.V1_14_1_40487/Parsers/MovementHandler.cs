using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using WowPacketParserModule.V7_0_3_22248.Enums;

namespace WowPacketParserModule.V1_14_1_40487.Parsers
{
    public static class MovementHandler
    {
        public static WowGuid ReadMovementStats(Packet packet, params object[] idx)
        {
            PlayerMovement moveData = new PlayerMovement();
            moveData.MoveInfo = new MovementInfo();
            moveData.Guid = packet.ReadPackedGuid128("MoverGUID", idx);
            moveData.MoveInfo.Flags = (uint)packet.ReadUInt32E<MovementFlag>("MovementFlags", idx);

            packet.ResetBitReader();
            moveData.MoveInfo.FlagsExtra = (uint)packet.ReadBitsE<MovementFlags2>("ExtraMovementFlags", 26, idx);
            packet.ReadBits(6);
            packet.ReadUInt32("Unk1", idx);

            moveData.MoveInfo.MoveTime = packet.ReadUInt32("MoveTime", idx);
            moveData.MoveInfo.Position = packet.ReadVector3("Position", idx);
            moveData.MoveInfo.Orientation = packet.ReadSingle("Orientation", idx);

            moveData.MoveInfo.SwimPitch = packet.ReadSingle("Pitch", idx);
            moveData.MoveInfo.SplineElevation = packet.ReadSingle("SplineElevation", idx);

            var int152 = packet.ReadInt32("RemoveForcesCount", idx);
            packet.ReadInt32("MoveIndex", idx);

            for (var i = 0; i < int152; i++)
                packet.ReadPackedGuid128("RemoveForcesIDs", idx, i);

            packet.ResetBitReader();

            var hasTransport = packet.ReadBit("HasTransportData", idx);
            var hasFall = packet.ReadBit("HasFallData", idx);
            packet.ReadBit("HasSpline", idx);
            packet.ReadBit("HeightChangeFailed", idx);
            packet.ReadBit("RemoteTimeValid", idx);

            if (hasTransport)
                V6_0_2_19033.Parsers.MovementHandler.ReadTransportData(moveData.MoveInfo, packet, idx, "TransportData");

            if (hasFall)
                V6_0_2_19033.Parsers.MovementHandler.ReadFallData(moveData.MoveInfo, packet, idx, "FallData");

            if (Settings.SqlTables.player_movement_client || Settings.SqlTables.creature_movement_client)
            {
                moveData.Map = WowPacketParser.Parsing.Parsers.MovementHandler.CurrentMapId;
                moveData.Opcode = packet.Opcode;
                moveData.OpcodeDirection = packet.Direction;
                moveData.Time = packet.Time;
                Storage.PlayerMovements.Add(moveData);
            }
            return moveData.Guid;
        }

        [Parser(Opcode.SMSG_LOGIN_SET_TIME_SPEED)]
        public static void HandleLoginSetTimeSpeed(Packet packet)
        {
            WowPacketParserModule.V6_0_2_19033.Parsers.MovementHandler.HandleLoginSetTimeSpeed(packet);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_NEW_WORLD)]
        public static void HandleNewWorld(Packet packet)
        {
            V7_0_3_22248.Parsers.MovementHandler.HandleNewWorld(packet);
        }

        [Parser(Opcode.CMSG_MOVE_CHANGE_TRANSPORT)]
        [Parser(Opcode.CMSG_MOVE_DISMISS_VEHICLE)]
        [Parser(Opcode.CMSG_MOVE_FALL_LAND)]
        [Parser(Opcode.CMSG_MOVE_FALL_RESET)]
        [Parser(Opcode.CMSG_MOVE_HEARTBEAT)]
        [Parser(Opcode.CMSG_MOVE_JUMP)]
        [Parser(Opcode.CMSG_MOVE_REMOVE_MOVEMENT_FORCES)]
        [Parser(Opcode.CMSG_MOVE_SET_FACING)]
        [Parser(Opcode.CMSG_MOVE_SET_FLY)]
        [Parser(Opcode.CMSG_MOVE_SET_PITCH)]
        [Parser(Opcode.CMSG_MOVE_SET_RUN_MODE)]
        [Parser(Opcode.CMSG_MOVE_SET_WALK_MODE)]
        [Parser(Opcode.CMSG_MOVE_START_ASCEND)]
        [Parser(Opcode.CMSG_MOVE_START_BACKWARD)]
        [Parser(Opcode.CMSG_MOVE_START_DESCEND)]
        [Parser(Opcode.CMSG_MOVE_START_FORWARD)]
        [Parser(Opcode.CMSG_MOVE_START_PITCH_DOWN)]
        [Parser(Opcode.CMSG_MOVE_START_PITCH_UP)]
        [Parser(Opcode.CMSG_MOVE_START_SWIM)]
        [Parser(Opcode.CMSG_MOVE_START_TURN_LEFT)]
        [Parser(Opcode.CMSG_MOVE_START_TURN_RIGHT)]
        [Parser(Opcode.CMSG_MOVE_START_STRAFE_LEFT)]
        [Parser(Opcode.CMSG_MOVE_START_STRAFE_RIGHT)]
        [Parser(Opcode.CMSG_MOVE_STOP)]
        [Parser(Opcode.CMSG_MOVE_STOP_ASCEND)]
        [Parser(Opcode.CMSG_MOVE_STOP_PITCH)]
        [Parser(Opcode.CMSG_MOVE_STOP_STRAFE)]
        [Parser(Opcode.CMSG_MOVE_STOP_SWIM)]
        [Parser(Opcode.CMSG_MOVE_STOP_TURN)]
        [Parser(Opcode.SMSG_MOVE_UPDATE_KNOCK_BACK)]
        [Parser(Opcode.SMSG_MOVE_UPDATE)]
        [Parser(Opcode.CMSG_MOVE_DOUBLE_JUMP)]
        public static void HandlePlayerMove(Packet packet)
        {
            ReadMovementStats(packet, "MovementStats");
        }
    }
}
