using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("characters")]
    public sealed class CharacterTemplate : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("account", false, true)]
        public string Account;

        [DBFieldName("name")]
        public string Name;

        [DBFieldName("race")]
        public uint? Race;

        [DBFieldName("class")]
        public uint Class;

        [DBFieldName("gender")]
        public uint Gender;

        [DBFieldName("level")]
        public uint Level;

        [DBFieldName("xp")]
        public uint XP = 0;

        [DBFieldName("money")]
        public uint Money = 0;

        [DBFieldName("skin")]
        public byte Skin;

        [DBFieldName("face")]
        public byte Face;

        [DBFieldName("hair_style")]
        public byte HairStyle;

        [DBFieldName("hair_color")]
        public byte HairColor;

        [DBFieldName("facial_hair")]
        public byte FacialHair;

        [DBFieldName("player_flags")]
        public uint PlayerFlags;

        [DBFieldName("position_x")]
        public float PositionX;

        [DBFieldName("position_y")]
        public float PositionY;

        [DBFieldName("position_z")]
        public float PositionZ;

        [DBFieldName("map")]
        public uint Map;

        [DBFieldName("orientation")]
        public float Orientation;

        [DBFieldName("health")]
        public uint Health = 1;

        [DBFieldName("power1")]
        public uint Power1 = 0;

        [DBFieldName("equipment_cache")]
        public string EquipmentCache = "";
    }
    [DBTableName("player")]
    public sealed class PlayerTemplate : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("map")]
        public uint Map;

        [DBFieldName("zone_id")]
        public uint? ZoneID;

        [DBFieldName("area_id")]
        public uint? AreaID;

        [DBFieldName("position_x")]
        public float PositionX;

        [DBFieldName("position_y")]
        public float PositionY;

        [DBFieldName("position_z")]
        public float PositionZ;

        [DBFieldName("orientation")]
        public float Orientation;

        [DBFieldName("name")]
        public string Name;

        [DBFieldName("race")]
        public uint? Race;

        [DBFieldName("class")]
        public uint Class;

        [DBFieldName("gender")]
        public uint Gender;

        [DBFieldName("level")]
        public uint Level;

        [DBFieldName("skin")]
        public byte Skin;

        [DBFieldName("face")]
        public byte Face;

        [DBFieldName("hair_style")]
        public byte HairStyle;

        [DBFieldName("hair_color")]
        public byte HairColor;

        [DBFieldName("facial_hair")]
        public byte FacialHair;

        [DBFieldName("player_flags")]
        public uint PlayerFlags;

        [DBFieldName("pvp_rank")]
        public byte PvPRank;

        [DBFieldName("scale")]
        public float? Scale;

        [DBFieldName("display_id")]
        public uint? DisplayID;

        [DBFieldName("native_display_id")]
        public uint? NativeDisplayID;

        [DBFieldName("mount_display_id")]
        public uint? MountDisplayID;

        [DBFieldName("faction")]
        public uint? FactionTemplate;

        [DBFieldName("unit_flags")]
        public uint? UnitFlags;

        [DBFieldName("unit_flags2")]
        public uint? UnitFlags2;

        [DBFieldName("current_health")]
        public uint? CurHealth;

        [DBFieldName("max_health")]
        public uint? MaxHealth;

        [DBFieldName("power_type")]
        public uint? PowerType;

        [DBFieldName("current_power")]
        public uint? CurrentPower;

        [DBFieldName("max_power")]
        public uint? MaxPower;

        [DBFieldName("aura_state")]
        public uint? AuraState;

        [DBFieldName("emote_state")]
        public uint? EmoteState;

        [DBFieldName("stand_state")]
        public uint? StandState;

        //[DBFieldName("pet_talent_points")]
        //public uint? PetTalentPoints;

        [DBFieldName("vis_flags")]
        public uint? VisFlags;

        [DBFieldName("anim_tier", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, DbType = (TargetedDbType.WPP))]
        [DBFieldName("anim_tier", TargetedDbExpansion.WrathOfTheLichKing, DbType = (TargetedDbType.WPP))]
        public uint? AnimTier;

        [DBFieldName("sheath_state")]
        public uint? SheatheState;

        [DBFieldName("pvp_flags")]
        public uint? PvpFlags;

        //[DBFieldName("pet_flags")]
        //public uint? PetFlags;

        [DBFieldName("shapeshift_form")]
        public uint? ShapeshiftForm;

        [DBFieldName("speed_walk")]
        public float? SpeedWalk;

        [DBFieldName("speed_run")]
        public float? SpeedRun;

        [DBFieldName("speed_run_back", DbType = (TargetedDbType.WPP))]
        public float? SpeedRunBack;

        [DBFieldName("speed_swim", DbType = (TargetedDbType.WPP))]
        public float? SpeedSwim;

        [DBFieldName("speed_swim_back", DbType = (TargetedDbType.WPP))]
        public float? SpeedSwimBack;

        [DBFieldName("speed_fly", DbType = (TargetedDbType.WPP))]
        public float? SpeedFly;

        [DBFieldName("speed_fly_back", DbType = (TargetedDbType.WPP))]
        public float? SpeedFlyBack;

        [DBFieldName("bounding_radius")]
        public float? BoundingRadius;

        [DBFieldName("combat_reach")]
        public float? CombatReach;

        [DBFieldName("mod_melee_haste")]
        public float? ModMeleeHaste;

        [DBFieldName("main_hand_attack_time")]
        public uint? MainHandAttackTime;

        [DBFieldName("off_hand_attack_time")]
        public uint? OffHandAttackTime;

        [DBFieldName("ranged_attack_time")]
        public uint? RangedAttackTime;

        [DBFieldName("channel_spell_id")]
        public uint? ChannelSpellId;

        [DBFieldName("channel_visual_id")]
        public uint? ChannelVisualId;

        [DBFieldName("equipment_cache")]
        public string EquipmentCache = "";

        [DBFieldName("auras")]
        public string Auras;
    }

    [DBTableName("player_create1_time")]
    public sealed class PlayerCreate1 : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("map")]
        public uint? Map;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("move_time")]
        public uint MoveTime;

        [DBFieldName("move_flags")]
        public uint MoveFlags;

        [DBFieldName("move_flags2")]
        public uint MoveFlags2;

        [DBFieldName("swim_pitch")]
        public float SwimPitch;

        [DBFieldName("fall_time")]
        public uint FallTime;

        [DBFieldName("jump_horizontal_speed")]
        public float JumpHorizontalSpeed;

        [DBFieldName("jump_vertical_speed")]
        public float JumpVerticalSpeed;

        [DBFieldName("jump_cos_angle")]
        public float JumpCosAngle;

        [DBFieldName("jump_sin_angle")]
        public float JumpSinAngle;

        [DBFieldName("spline_elevation")]
        public float SplineElevation;

        [DBFieldName("vehicle_id", OnlyWhenSavingTransports = true)]
        public uint VehicleId;

        [DBFieldName("vehicle_orientation", OnlyWhenSavingTransports = true)]
        public float VehicleOrientation;

        [DBFieldName("transport_guid", false, true, OnlyWhenSavingTransports = true)]
        public string TransportGuid = "0";

        [DBFieldName("transport_id", OnlyWhenSavingTransports = true)]
        public uint TransportId;

        [DBFieldName("transport_type", OnlyWhenSavingTransports = true)]
        public string TransportType = "";

        [DBFieldName("transport_x", OnlyWhenSavingTransports = true)]
        public float TransportPositionX;

        [DBFieldName("transport_y", OnlyWhenSavingTransports = true)]
        public float TransportPositionY;

        [DBFieldName("transport_z", OnlyWhenSavingTransports = true)]
        public float TransportPositionZ;

        [DBFieldName("transport_o", OnlyWhenSavingTransports = true)]
        public float TransportOrientation;

        [DBFieldName("transport_time", OnlyWhenSavingTransports = true)]
        public uint TransportTime;

        [DBFieldName("transport_seat", OnlyWhenSavingTransports = true)]
        public sbyte TransportSeat;
    }

    [DBTableName("player_create2_time")]
    public sealed class PlayerCreate2 : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("map")]
        public uint? Map;

        [DBFieldName("position_x")]
        public float? PositionX;

        [DBFieldName("position_y")]
        public float? PositionY;

        [DBFieldName("position_z")]
        public float? PositionZ;

        [DBFieldName("orientation")]
        public float? Orientation;

        [DBFieldName("move_time")]
        public uint MoveTime;

        [DBFieldName("move_flags")]
        public uint MoveFlags;

        [DBFieldName("move_flags2")]
        public uint MoveFlags2;

        [DBFieldName("swim_pitch")]
        public float SwimPitch;

        [DBFieldName("fall_time")]
        public uint FallTime;

        [DBFieldName("jump_horizontal_speed")]
        public float JumpHorizontalSpeed;

        [DBFieldName("jump_vertical_speed")]
        public float JumpVerticalSpeed;

        [DBFieldName("jump_cos_angle")]
        public float JumpCosAngle;

        [DBFieldName("jump_sin_angle")]
        public float JumpSinAngle;

        [DBFieldName("spline_elevation")]
        public float SplineElevation;

        [DBFieldName("vehicle_id", OnlyWhenSavingTransports = true)]
        public uint VehicleId;

        [DBFieldName("vehicle_orientation", OnlyWhenSavingTransports = true)]
        public float VehicleOrientation;

        [DBFieldName("transport_guid", false, true, OnlyWhenSavingTransports = true)]
        public string TransportGuid = "0";

        [DBFieldName("transport_id", OnlyWhenSavingTransports = true)]
        public uint TransportId;

        [DBFieldName("transport_type", OnlyWhenSavingTransports = true)]
        public string TransportType = "";

        [DBFieldName("transport_x", OnlyWhenSavingTransports = true)]
        public float TransportPositionX;

        [DBFieldName("transport_y", OnlyWhenSavingTransports = true)]
        public float TransportPositionY;

        [DBFieldName("transport_z", OnlyWhenSavingTransports = true)]
        public float TransportPositionZ;

        [DBFieldName("transport_o", OnlyWhenSavingTransports = true)]
        public float TransportOrientation;

        [DBFieldName("transport_time", OnlyWhenSavingTransports = true)]
        public uint TransportTime;

        [DBFieldName("transport_seat", OnlyWhenSavingTransports = true)]
        public sbyte TransportSeat;
    }

    [DBTableName("player_destroy_time")]
    public sealed class PlayerDestroy : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;
    }

    [DBTableName("player_minimap_ping")]
    public sealed class PlayerMinimapPing : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("guid", true, true)]
        public string GUID;

        [DBFieldName("position_x")]
        public float PositionX;

        [DBFieldName("position_y")]
        public float PositionY;
    }

    [DBTableName("raid_target_icon_update")]
    public sealed class RaidTargetIconUpdate : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;

        [DBFieldName("is_full_update")]
        public bool IsFullUpdate;

        [DBFieldName("icon", true)]
        public sbyte Icon;

        [DBFieldName("target_guid", true, true)]
        public string TargetGuid = "0";

        [DBFieldName("target_id")]
        public uint TargetId;

        [DBFieldName("target_type", true)]
        public string TargetType = "";

        public WowGuid TargetGUID;
    }

    [DBTableName("player_melee_crit_chance")]
    public sealed class PlayerWeaponCritChance : IDataModel
    {
        [DBFieldName("race", true)]
        public uint RaceId;

        [DBFieldName("class", true)]
        public uint ClassId;

        [DBFieldName("level", true)]
        public int Level;

        [DBFieldName("agility", true)]
        public int Agility;

        [DBFieldName("crit_chance", true)]
        public float CritChance;

        [DBFieldName("weapon_item_id")]
        public uint WeaponItemId;

        [DBFieldName("weapon_skill_id", true)]
        public ushort WeaponSkillId;

        [DBFieldName("skill_current_value", true)]
        public ushort SkillCurrentValue;

        [DBFieldName("skill_max_value", true)]
        public ushort SkillMaxValue;

        [DBFieldName("relevant_auras", true)]
        public string RelevantAuras;

        [DBFieldName("sniff_id", false, true, false, true)]
        public string SniffId;

        [DBFieldName("sniff_build")]
        public int SniffBuild = ClientVersion.BuildInt;
    }

    [DBTableName("player_spell_crit_chance")]
    public sealed class PlayerSpellCritChance : IDataModel
    {
        [DBFieldName("race", true)]
        public uint RaceId;

        [DBFieldName("class", true)]
        public uint ClassId;

        [DBFieldName("level", true)]
        public int Level;

        [DBFieldName("intellect", true)]
        public int Intellect;

        [DBFieldName("crit_chance", true)]
        public float CritChance;

        [DBFieldName("relevant_auras", true)]
        public string RelevantAuras;

        [DBFieldName("sniff_id", false, true, false, true)]
        public string SniffId;

        [DBFieldName("sniff_build")]
        public int SniffBuild = ClientVersion.BuildInt;
    }

    [DBTableName("player_dodge_chance")]
    public sealed class PlayerDodgeChance : IDataModel
    {
        [DBFieldName("race", true)]
        public uint RaceId;

        [DBFieldName("class", true)]
        public uint ClassId;

        [DBFieldName("level", true)]
        public int Level;

        [DBFieldName("agility", true)]
        public int Agility;

        [DBFieldName("dodge_chance", true)]
        public float DodgeChance;

        [DBFieldName("defense_current_value", true)]
        public ushort DefenseCurrentValue;

        [DBFieldName("defense_max_value", true)]
        public ushort DefenseMaxValue;

        [DBFieldName("relevant_auras", true)]
        public string RelevantAuras;

        [DBFieldName("sniff_id", false, true, false, true)]
        public string SniffId;

        [DBFieldName("sniff_build")]
        public int SniffBuild = ClientVersion.BuildInt;
    }

    [DBTableName("player_classlevelstats")]
    public sealed class PlayerClassLevelStats : ITableWithSniffIdList
    {
        [DBFieldName("class", true)]
        public uint ClassId;

        [DBFieldName("level", true)]
        public int Level;

        [DBFieldName("base_health", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("basehp", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public int BaseHP;

        [DBFieldName("base_mana", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("basemana", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public int BaseMana;
    }

    [DBTableName("player_levelstats")]
    public sealed class PlayerLevelStats : ITableWithSniffIdList
    {
        [DBFieldName("race", true)]
        public uint RaceId;

        [DBFieldName("class", true)]
        public uint ClassId;

        [DBFieldName("level", true)]
        public int Level;

        [DBFieldName("strength", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("str", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public int Strength;

        [DBFieldName("agility", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("agi", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public int Agility;

        [DBFieldName("stamina", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("sta", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public int Stamina;

        [DBFieldName("intellect", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("inte", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public int Intellect;

        [DBFieldName("spirit", true, DbType = (TargetedDbType.WPP))]
        [DBFieldName("spi", DbType = (TargetedDbType.TRINITY | TargetedDbType.VMANGOS | TargetedDbType.CMANGOS))]
        public int Spirit;
    }

    [DBTableName("player_levelup_info")]
    public sealed class PlayerLevelupInfo : IDataModel
    {
        [DBFieldName("race", true)]
        public uint RaceId;

        [DBFieldName("class", true)]
        public uint ClassId;

        [DBFieldName("level", true)]
        public int Level;

        [DBFieldName("health")]
        public int Health;

        [DBFieldName("power", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Zero, 6, true)]
        [DBFieldName("power", TargetedDbExpansion.Zero, TargetedDbExpansion.WrathOfTheLichKing, 5, true)]
        [DBFieldName("power", TargetedDbExpansion.WrathOfTheLichKing, TargetedDbExpansion.Cataclysm, 7, true)]
        [DBFieldName("power", TargetedDbExpansion.Cataclysm, TargetedDbExpansion.WarlordsOfDraenor, 5, true)]
        [DBFieldName("power", TargetedDbExpansion.WarlordsOfDraenor, 6, true)]
        public int?[] Power;

        [DBFieldName("stat", TargetedDbExpansion.BurningCrusadeClassic, TargetedDbExpansion.Legion, 5, true)]
        [DBFieldName("stat", TargetedDbExpansion.Legion, 4, true)]
        public int?[] Stat;

        [DBFieldName("sniff_id", false, true, false, true)]
        public string SniffId;

        [DBFieldName("sniff_build")]
        public int SniffBuild = ClientVersion.BuildInt;

        public WowGuid GUID;
    }

    [DBTableName("character_inventory")]
    public sealed class CharacterInventory : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("bag")]
        public uint Bag;

        [DBFieldName("slot")]
        public uint Slot;

        [DBFieldName("item", true, true)]
        public string ItemGuid;

        [DBFieldName("item_template")]
        public uint ItemTemplate;
    }

    [DBTableName("item_instance")]
    public sealed class CharacterItemInstance : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("itemEntry")]
        public uint ItemEntry;

        [DBFieldName("owner_guid", false, true)]
        public string OwnerGuid;

        [DBFieldName("count")]
        public uint Count = 1;

        [DBFieldName("charges")]
        public string Charges = "0 0 0 0 0 ";

        [DBFieldName("enchantments")]
        public string Enchantments = "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ";

        [DBFieldName("durability")]
        public uint Durability = 1;
    }

    [DBTableName("character_reputation")]
    public sealed class CharacterReputation : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("faction", true)]
        public uint Faction;

        [DBFieldName("standing")]
        public int Standing;

        [DBFieldName("flags")]
        public uint Flags;
    }
    public class CharacterReputationData
    {
        public uint Faction;
        public int Standing;
        public uint? Flags;
    }

    [DBTableName("character_skills")]
    public sealed class CharacterSkill : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("skill", true)]
        public uint Skill;

        [DBFieldName("value")]
        public uint Value;

        [DBFieldName("max")]
        public uint Max;
    }

    [DBTableName("character_spell")]
    public sealed class CharacterSpell : IDataModel
    {
        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("spell", true)]
        public uint Spell;

        [DBFieldName("active")]
        public uint Active = 1;

        [DBFieldName("disabled")]
        public uint Disabled = 0;
    }

    [DBTableName("player_movement_client")]
    public sealed class ClientSideMovement : IDataModel
    {
        [DBFieldName("unixtimems")]
        public ulong UnixTimeMs;

        [DBFieldName("guid", false, true)]
        public string Guid;

        [DBFieldName("packet_id", true)]
        public uint PacketId;

        [DBFieldName("opcode")]
        public string Opcode;

        [DBFieldName("move_time")]
        public uint MoveTime;

        [DBFieldName("move_flags")]
        public uint MoveFlags;

        [DBFieldName("move_flags2")]
        public uint MoveFlags2;

        [DBFieldName("map")]
        public uint Map;

        [DBFieldName("position_x")]
        public float PositionX;

        [DBFieldName("position_y")]
        public float PositionY;

        [DBFieldName("position_z")]
        public float PositionZ;

        [DBFieldName("orientation")]
        public float Orientation;

        [DBFieldName("swim_pitch")]
        public float SwimPitch;

        [DBFieldName("fall_time")]
        public uint FallTime;

        [DBFieldName("jump_horizontal_speed")]
        public float JumpHorizontalSpeed;

        [DBFieldName("jump_vertical_speed")]
        public float JumpVerticalSpeed;

        [DBFieldName("jump_cos_angle")]
        public float JumpCosAngle;

        [DBFieldName("jump_sin_angle")]
        public float JumpSinAngle;

        [DBFieldName("spline_elevation")]
        public float SplineElevation;

        [DBFieldName("transport_guid", false, true, OnlyWhenSavingTransports = true)]
        public string TransportGuid = "0";

        [DBFieldName("transport_id", OnlyWhenSavingTransports = true)]
        public uint TransportId;

        [DBFieldName("transport_type", OnlyWhenSavingTransports = true)]
        public string TransportType = "";

        [DBFieldName("transport_x", OnlyWhenSavingTransports = true)]
        public float TransportPositionX;

        [DBFieldName("transport_y", OnlyWhenSavingTransports = true)]
        public float TransportPositionY;

        [DBFieldName("transport_z", OnlyWhenSavingTransports = true)]
        public float TransportPositionZ;

        [DBFieldName("transport_o", OnlyWhenSavingTransports = true)]
        public float TransportOrientation;

        [DBFieldName("transport_time", OnlyWhenSavingTransports = true)]
        public uint TransportTime;

        [DBFieldName("transport_seat", OnlyWhenSavingTransports = true)]
        public sbyte TransportSeat;
    }
    public sealed class PlayerMovement
    {
        public WowGuid Guid;
        public uint Map;
        public MovementInfo MoveInfo;
        public int Opcode;
        public Direction OpcodeDirection;
        public DateTime Time;
    }

    [DBTableName("player_active_player")]
    public sealed class CharacterActivePlayer : IDataModel
    {
        [DBFieldName("unixtime", true)]
        public uint UnixTime;

        [DBFieldName("guid", true, true)]
        public string Guid;
    }
    public sealed class ActivePlayerCreateTime
    {
        public WowGuid Guid;
        public DateTime Time;
    }

    [DBTableName("guild_member")]
    public sealed class GuildMember : IDataModel
    {
        [DBFieldName("guildid")]
        public ulong GuildGUID = 0;

        [DBFieldName("guid", true, true)]
        public string Guid;

        [DBFieldName("rank")]
        public uint GuildRank = 0;

        [DBFieldName("pnote")]
        public string pnote = "";

        [DBFieldName("offnote")]
        public string offnote = "";
    }

    [DBTableName("logout_time")]
    public sealed class LogoutTime : IDataModel
    {
        [DBFieldName("unixtimems", true)]
        public ulong UnixTimeMs;
    }
}
