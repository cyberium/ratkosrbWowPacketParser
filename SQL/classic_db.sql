-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.53 - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Version:             9.3.0.4998
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for table sniffs_new_test.characters
DROP TABLE IF EXISTS `characters`;
CREATE TABLE IF NOT EXISTS `characters` (
  `guid` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Global Unique Identifier',
  `account` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Account Identifier',
  `name` varchar(12) NOT NULL DEFAULT '',
  `race` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `class` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `gender` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `xp` int(10) unsigned NOT NULL DEFAULT '0',
  `money` int(10) unsigned NOT NULL DEFAULT '0',
  `playerBytes` int(10) unsigned NOT NULL DEFAULT '0',
  `playerBytes2` int(10) unsigned NOT NULL DEFAULT '0',
  `playerFlags` int(10) unsigned NOT NULL DEFAULT '0',
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `map` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Map Identifier',
  `orientation` float NOT NULL DEFAULT '0',
  `health` int(10) unsigned NOT NULL DEFAULT '0',
  `power1` int(10) unsigned NOT NULL DEFAULT '0',
  `equipmentCache` longtext,
  PRIMARY KEY (`guid`),
  KEY `idx_account` (`account`),
  KEY `idx_name` (`name`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='player data in format used by vmangos db';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_active_player
DROP TABLE IF EXISTS `character_active_player`;
CREATE TABLE IF NOT EXISTS `character_active_player` (
  `guid` int(10) unsigned NOT NULL,
  `unixtime` int(10) unsigned NOT NULL,
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='shows which character was controlled by the client at a given time';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_attack_start
DROP TABLE IF EXISTS `character_attack_start`;
CREATE TABLE IF NOT EXISTS `character_attack_start` (
  `guid` int(10) unsigned NOT NULL,
  `victim_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_id` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_ATTACK_START';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_attack_stop
DROP TABLE IF EXISTS `character_attack_stop`;
CREATE TABLE IF NOT EXISTS `character_attack_stop` (
  `guid` int(10) unsigned NOT NULL,
  `victim_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_id` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_ATTACK_STOP';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_inventory
DROP TABLE IF EXISTS `character_inventory`;
CREATE TABLE IF NOT EXISTS `character_inventory` (
  `guid` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Global Unique Identifier',
  `bag` int(11) unsigned NOT NULL DEFAULT '0',
  `slot` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `item` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Item Global Unique Identifier',
  `item_template` int(11) unsigned NOT NULL DEFAULT '0' COMMENT 'Item Identifier',
  PRIMARY KEY (`item`),
  KEY `idx_guid` (`guid`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='player item data in format used by vmangos db';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_movement
DROP TABLE IF EXISTS `character_movement`;
CREATE TABLE IF NOT EXISTS `character_movement` (
  `guid` int(10) unsigned NOT NULL,
  `opcode` varchar(64) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `move_time` int(10) unsigned NOT NULL DEFAULT '0',
  `move_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `map` smallint(5) unsigned NOT NULL DEFAULT '0',
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `orientation` float NOT NULL DEFAULT '0',
  `unixtimems` bigint(20) unsigned NOT NULL,
  PRIMARY KEY (`guid`,`opcode`,`unixtimems`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='shows all player movement packets';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_speed_update
DROP TABLE IF EXISTS `character_speed_update`;
CREATE TABLE IF NOT EXISTS `character_speed_update` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `speed_walk` float unsigned DEFAULT NULL,
  `speed_run` float unsigned DEFAULT NULL,
  `speed_run_back` float unsigned DEFAULT NULL,
  `speed_swim` float unsigned DEFAULT NULL,
  `speed_swim_back` float unsigned DEFAULT NULL,
  `speed_fly` float unsigned DEFAULT NULL,
  `speed_fly_back` float unsigned DEFAULT NULL,
  `rate_turn` float unsigned DEFAULT NULL,
  `rate_pitch` float unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='changes to movement speed';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_target_change
DROP TABLE IF EXISTS `character_target_change`;
CREATE TABLE IF NOT EXISTS `character_target_change` (
  `guid` int(10) unsigned NOT NULL,
  `victim_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_id` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='changes to UNIT_FIELD_TARGET';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.character_values_update
DROP TABLE IF EXISTS `character_values_update`;
CREATE TABLE IF NOT EXISTS `character_values_update` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `entry` int(10) unsigned DEFAULT NULL,
  `scale` float unsigned DEFAULT NULL,
  `display_id` int(10) unsigned DEFAULT NULL,
  `mount` int(10) unsigned DEFAULT NULL,
  `faction` int(10) unsigned DEFAULT NULL,
  `emote_state` int(10) unsigned DEFAULT NULL,
  `stand_state` int(10) unsigned DEFAULT NULL,
  `npc_flags` int(10) unsigned DEFAULT NULL,
  `unit_flags` int(10) unsigned DEFAULT NULL,
  `current_health` int(10) unsigned DEFAULT NULL,
  `max_health` int(10) unsigned DEFAULT NULL,
  `current_mana` int(10) unsigned DEFAULT NULL,
  `max_mana` int(10) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='values updates from SMSG_UPDATE_OBJECT';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.client_reclaim_corpse
DROP TABLE IF EXISTS `client_reclaim_corpse`;
CREATE TABLE IF NOT EXISTS `client_reclaim_corpse` (
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was sent',
  PRIMARY KEY (`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from CMSG_RECLAIM_CORPSE';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.client_release_spirit
DROP TABLE IF EXISTS `client_release_spirit`;
CREATE TABLE IF NOT EXISTS `client_release_spirit` (
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was sent',
  PRIMARY KEY (`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from CMSG_CLIENT_PORT_GRAVEYARD';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature
DROP TABLE IF EXISTS `creature`;
CREATE TABLE IF NOT EXISTS `creature` (
  `guid` bigint(20) unsigned NOT NULL DEFAULT '0',
  `id` mediumint(8) unsigned NOT NULL DEFAULT '0' COMMENT 'Creature Identifier',
  `map` smallint(5) unsigned DEFAULT '0' COMMENT 'Map Identifier',
  `zone_id` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Zone Identifier',
  `area_id` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Area Identifier',
  `phase_group` int(10) DEFAULT '0',
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `orientation` float NOT NULL DEFAULT '0',
  `wander_distance` float NOT NULL DEFAULT '0',
  `movement_type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `temp` tinyint(3) unsigned NOT NULL DEFAULT '120',
  `creator` int(10) unsigned NOT NULL DEFAULT '120',
  `summoner` int(10) unsigned NOT NULL DEFAULT '120',
  `summon_spell` int(10) unsigned NOT NULL DEFAULT '120',
  `display_id` int(10) unsigned NOT NULL DEFAULT '120',
  `faction` int(10) unsigned NOT NULL DEFAULT '120',
  `level` int(10) unsigned NOT NULL DEFAULT '120',
  `current_health` int(10) unsigned NOT NULL DEFAULT '120',
  `max_health` int(10) unsigned NOT NULL DEFAULT '120',
  `current_mana` int(10) unsigned NOT NULL DEFAULT '120',
  `max_mana` int(10) unsigned NOT NULL DEFAULT '120',
  `speed_walk` float NOT NULL DEFAULT '0',
  `speed_run` float NOT NULL DEFAULT '0',
  `scale` float NOT NULL DEFAULT '0',
  `base_attack_time` int(10) unsigned NOT NULL DEFAULT '0',
  `ranged_attack_time` int(10) unsigned NOT NULL DEFAULT '0',
  `npc_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `unit_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `SniffId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`guid`),
  KEY `idx_map` (`map`),
  KEY `idx_id` (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='Creature System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_addon
DROP TABLE IF EXISTS `creature_addon`;
CREATE TABLE IF NOT EXISTS `creature_addon` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `path_id` int(10) unsigned NOT NULL DEFAULT '0',
  `mount` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `bytes1` int(10) unsigned NOT NULL DEFAULT '0',
  `stand_state` int(10) unsigned NOT NULL DEFAULT '0',
  `pet_talent_points` int(10) unsigned NOT NULL DEFAULT '0',
  `vis_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `anim_tier` int(10) unsigned NOT NULL DEFAULT '0',
  `bytes2` int(10) unsigned NOT NULL DEFAULT '0',
  `sheath_state` int(10) unsigned NOT NULL DEFAULT '0',
  `pvp_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `pet_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `shapeshift_form` int(10) unsigned NOT NULL DEFAULT '0',
  `emote` int(10) unsigned NOT NULL DEFAULT '0',
  `auras` text,
  PRIMARY KEY (`guid`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_attack_start
DROP TABLE IF EXISTS `creature_attack_start`;
CREATE TABLE IF NOT EXISTS `creature_attack_start` (
  `guid` int(10) unsigned NOT NULL,
  `victim_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_id` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='from SMSG_ATTACK_START';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_attack_stop
DROP TABLE IF EXISTS `creature_attack_stop`;
CREATE TABLE IF NOT EXISTS `creature_attack_stop` (
  `guid` int(10) unsigned NOT NULL,
  `victim_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_id` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_ATTACK_STOP';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_client_interact
DROP TABLE IF EXISTS `creature_client_interact`;
CREATE TABLE IF NOT EXISTS `creature_client_interact` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was sent',
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='times when the client talked to the creature';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_create1_time
DROP TABLE IF EXISTS `creature_create1_time`;
CREATE TABLE IF NOT EXISTS `creature_create1_time` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `position_x` float NOT NULL,
  `position_y` float NOT NULL,
  `position_z` float NOT NULL,
  `orientation` float NOT NULL,
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='times when the object became visible to the client';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_create2_time
DROP TABLE IF EXISTS `creature_create2_time`;
CREATE TABLE IF NOT EXISTS `creature_create2_time` (
  `guid` int(10) unsigned NOT NULL COMMENT 'gameobject spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `position_x` float NOT NULL,
  `position_y` float NOT NULL,
  `position_z` float NOT NULL,
  `orientation` float NOT NULL,
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='the time at which the object spawned';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_default_trainer
DROP TABLE IF EXISTS `creature_default_trainer`;
CREATE TABLE IF NOT EXISTS `creature_default_trainer` (
  `CreatureId` int(11) unsigned NOT NULL,
  `TrainerId` int(11) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`CreatureId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_destroy_time
DROP TABLE IF EXISTS `creature_destroy_time`;
CREATE TABLE IF NOT EXISTS `creature_destroy_time` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='times when the object was destroyed from the client''s prespective due to despawning, becoming invisible, or going out of range';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_display_info_addon
DROP TABLE IF EXISTS `creature_display_info_addon`;
CREATE TABLE IF NOT EXISTS `creature_display_info_addon` (
  `display_id` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `bounding_radius` float NOT NULL DEFAULT '0',
  `combat_reach` float NOT NULL DEFAULT '0',
  `gender` tinyint(3) unsigned NOT NULL DEFAULT '2',
  PRIMARY KEY (`display_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Creature System (Model related info)';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_emote
DROP TABLE IF EXISTS `creature_emote`;
CREATE TABLE IF NOT EXISTS `creature_emote` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `emote_id` int(10) unsigned NOT NULL COMMENT 'references Emotes.dbc',
  `emote_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`guid`,`unixtime`,`emote_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='from SMSG_EMOTE';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_equip_template
DROP TABLE IF EXISTS `creature_equip_template`;
CREATE TABLE IF NOT EXISTS `creature_equip_template` (
  `CreatureID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `ID` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `ItemID1` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `ItemID2` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `ItemID3` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`CreatureID`,`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_gossip
DROP TABLE IF EXISTS `creature_gossip`;
CREATE TABLE IF NOT EXISTS `creature_gossip` (
  `CreatureId` int(10) unsigned NOT NULL DEFAULT '0',
  `GossipMenuId` int(10) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`CreatureId`,`GossipMenuId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_loot
DROP TABLE IF EXISTS `creature_loot`;
CREATE TABLE IF NOT EXISTS `creature_loot` (
  `entry` int(10) unsigned NOT NULL COMMENT 'creature template id',
  `loot_id` int(10) unsigned NOT NULL COMMENT 'counter',
  `money` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'copper',
  `items_count` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'number of items dropped',
  PRIMARY KEY (`entry`,`loot_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='each row represents a separate loot instance';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_loot_item
DROP TABLE IF EXISTS `creature_loot_item`;
CREATE TABLE IF NOT EXISTS `creature_loot_item` (
  `loot_id` int(10) unsigned NOT NULL COMMENT 'references creature_loot',
  `item_id` int(10) unsigned NOT NULL COMMENT 'item template id',
  `count` int(10) unsigned NOT NULL DEFAULT '1' COMMENT 'stack size'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='individual item that is part of a loot instance';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_movement
DROP TABLE IF EXISTS `creature_movement`;
CREATE TABLE IF NOT EXISTS `creature_movement` (
  `id` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `point` smallint(5) unsigned NOT NULL COMMENT 'counter of movements per guid',
  `move_time` int(10) unsigned NOT NULL COMMENT 'how long it will take to move between these points',
  `spline_flags` int(10) unsigned NOT NULL,
  `spline_count` smallint(5) unsigned NOT NULL COMMENT 'number of splines belonging to this point',
  `start_position_x` float NOT NULL COMMENT 'starting position',
  `start_position_y` float NOT NULL,
  `start_position_z` float NOT NULL,
  `end_position_x` float NOT NULL COMMENT 'final position',
  `end_position_y` float NOT NULL,
  `end_position_z` float NOT NULL,
  `orientation` float NOT NULL COMMENT 'final orientation',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`id`,`point`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='out of combat movement points from SMSG_ON_MONSTER_MOVE';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_movement_combat
DROP TABLE IF EXISTS `creature_movement_combat`;
CREATE TABLE IF NOT EXISTS `creature_movement_combat` (
  `id` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `point` smallint(5) unsigned NOT NULL COMMENT 'counter of movements per guid',
  `move_time` int(10) unsigned NOT NULL COMMENT 'how long it will take to move between these points',
  `spline_flags` int(10) unsigned NOT NULL,
  `spline_count` smallint(5) unsigned NOT NULL COMMENT 'number of splines belonging to this point',
  `start_position_x` float NOT NULL COMMENT 'starting position',
  `start_position_y` float NOT NULL,
  `start_position_z` float NOT NULL,
  `end_position_x` float NOT NULL COMMENT 'final position',
  `end_position_y` float NOT NULL,
  `end_position_z` float NOT NULL,
  `orientation` float NOT NULL COMMENT 'final orientation',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`id`,`point`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='in combat movement points from SMSG_ON_MONSTER_MOVE\r\nindividual splines are not saved';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_movement_spline
DROP TABLE IF EXISTS `creature_movement_spline`;
CREATE TABLE IF NOT EXISTS `creature_movement_spline` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `parent_point` smallint(5) unsigned NOT NULL COMMENT 'point from creature_movement to which the spline data belongs',
  `spline_point` smallint(5) unsigned NOT NULL COMMENT 'order of points within the spline',
  `position_x` float NOT NULL,
  `position_y` float NOT NULL,
  `position_z` float NOT NULL,
  PRIMARY KEY (`guid`,`parent_point`,`spline_point`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='individual spline points for out of combat movement';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_questitem
DROP TABLE IF EXISTS `creature_questitem`;
CREATE TABLE IF NOT EXISTS `creature_questitem` (
  `CreatureEntry` int(10) unsigned NOT NULL,
  `Idx` int(10) unsigned NOT NULL,
  `ItemId` int(10) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`CreatureEntry`,`Idx`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_speed_update
DROP TABLE IF EXISTS `creature_speed_update`;
CREATE TABLE IF NOT EXISTS `creature_speed_update` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `speed_walk` float unsigned DEFAULT NULL,
  `speed_run` float unsigned DEFAULT NULL,
  `speed_run_back` float unsigned DEFAULT NULL,
  `speed_swim` float unsigned DEFAULT NULL,
  `speed_swim_back` float unsigned DEFAULT NULL,
  `speed_fly` float unsigned DEFAULT NULL,
  `speed_fly_back` float unsigned DEFAULT NULL,
  `rate_turn` float unsigned DEFAULT NULL,
  `rate_pitch` float unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='changes to movement speed';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_stats
DROP TABLE IF EXISTS `creature_stats`;
CREATE TABLE IF NOT EXISTS `creature_stats` (
  `entry` int(10) unsigned NOT NULL COMMENT 'creature template id',
  `dmg_min` float DEFAULT NULL,
  `dmg_max` float DEFAULT NULL,
  `offhand_dmg_min` float DEFAULT NULL,
  `offhand_dmg_max` float DEFAULT NULL,
  `ranged_dmg_min` float DEFAULT NULL,
  `ranged_dmg_max` float DEFAULT NULL,
  `attack_power` int(10) unsigned DEFAULT NULL,
  `ranged_attack_power` int(10) unsigned DEFAULT NULL,
  `strength` int(10) unsigned DEFAULT NULL,
  `agility` int(10) unsigned DEFAULT NULL,
  `stamina` int(10) unsigned DEFAULT NULL,
  `intellect` int(10) unsigned DEFAULT NULL,
  `spirit` int(10) unsigned DEFAULT NULL,
  `armor` int(11) DEFAULT NULL,
  `holy_res` int(11) DEFAULT NULL,
  `fire_res` int(11) DEFAULT NULL,
  `nature_res` int(11) DEFAULT NULL,
  `frost_res` int(11) DEFAULT NULL,
  `shadow_res` int(11) DEFAULT NULL,
  `arcane_res` int(11) DEFAULT NULL,
  PRIMARY KEY (`entry`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='stats data from SMSG_UPDATE_OBJECT, server only sends it if the creature is mind controlled';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_target_change
DROP TABLE IF EXISTS `creature_target_change`;
CREATE TABLE IF NOT EXISTS `creature_target_change` (
  `guid` int(10) unsigned NOT NULL,
  `victim_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_id` int(10) unsigned NOT NULL DEFAULT '0',
  `victim_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='changes to UNIT_FIELD_TARGET';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_template
DROP TABLE IF EXISTS `creature_template`;
CREATE TABLE IF NOT EXISTS `creature_template` (
  `entry` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `gossip_menu_id` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `level_min` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `level_max` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `faction` smallint(5) unsigned NOT NULL DEFAULT '0',
  `npc_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `speed_walk` float NOT NULL DEFAULT '1' COMMENT 'Result of 2.5/2.5, most common value',
  `speed_run` float NOT NULL DEFAULT '1.14286' COMMENT 'Result of 8.0/7.0, most common value',
  `scale` float NOT NULL DEFAULT '1.14286',
  `base_attack_time` int(10) unsigned NOT NULL DEFAULT '0',
  `ranged_attack_time` int(10) unsigned NOT NULL DEFAULT '0',
  `unit_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `unit_flags2` int(10) unsigned NOT NULL DEFAULT '0',
  `vehicle_id` int(11) NOT NULL DEFAULT '0',
  `hover_height` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED COMMENT='Creature System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_template_addon
DROP TABLE IF EXISTS `creature_template_addon`;
CREATE TABLE IF NOT EXISTS `creature_template_addon` (
  `entry` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `path_id` int(10) unsigned NOT NULL DEFAULT '0',
  `mount` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `bytes1` int(10) unsigned NOT NULL DEFAULT '0',
  `bytes2` int(10) unsigned NOT NULL DEFAULT '0',
  `emote` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `visibilityDistanceType` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `auras` text,
  PRIMARY KEY (`entry`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_template_wdb
DROP TABLE IF EXISTS `creature_template_wdb`;
CREATE TABLE IF NOT EXISTS `creature_template_wdb` (
  `entry` int(10) unsigned NOT NULL,
  `kill_credit1` int(11) NOT NULL DEFAULT '0',
  `kill_credit2` int(11) NOT NULL DEFAULT '0',
  `display_total_count` int(11) NOT NULL DEFAULT '0',
  `display_total_probability` float NOT NULL DEFAULT '0',
  `display_id1` int(11) NOT NULL DEFAULT '0',
  `display_id2` int(11) NOT NULL DEFAULT '0',
  `display_id3` int(11) NOT NULL DEFAULT '0',
  `display_id4` int(11) NOT NULL DEFAULT '0',
  `display_scale1` float NOT NULL DEFAULT '0',
  `display_scale2` float NOT NULL DEFAULT '0',
  `display_scale3` float NOT NULL DEFAULT '0',
  `display_scale4` float NOT NULL DEFAULT '0',
  `display_probability1` float NOT NULL DEFAULT '0',
  `display_probability2` float NOT NULL DEFAULT '0',
  `display_probability3` float NOT NULL DEFAULT '0',
  `display_probability4` float NOT NULL DEFAULT '0',
  `name` varchar(256) NOT NULL DEFAULT '',
  `female_name` varchar(256) NOT NULL DEFAULT '',
  `subname` varchar(256) DEFAULT '',
  `title_alt` varchar(256) DEFAULT '',
  `icon_name` varchar(256) DEFAULT '',
  `health_scaling_expansion` int(11) NOT NULL DEFAULT '0',
  `required_expansion` int(11) NOT NULL DEFAULT '0',
  `vignette_id` int(11) NOT NULL DEFAULT '0',
  `unit_class` int(11) NOT NULL DEFAULT '0',
  `rank` int(11) NOT NULL DEFAULT '0',
  `beast_family` int(11) NOT NULL DEFAULT '0',
  `type` int(11) NOT NULL DEFAULT '0',
  `type_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `type_flags2` int(10) unsigned NOT NULL DEFAULT '0',
  `pet_spell_list_id` int(11) NOT NULL DEFAULT '0',
  `health_multiplier` float NOT NULL DEFAULT '0',
  `mana_multiplier` float NOT NULL DEFAULT '0',
  `civilian` int(11) NOT NULL DEFAULT '0',
  `racial_leader` int(11) NOT NULL DEFAULT '0',
  `movement_id` int(11) NOT NULL DEFAULT '0',
  `VerifiedBuild` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_text
DROP TABLE IF EXISTS `creature_text`;
CREATE TABLE IF NOT EXISTS `creature_text` (
  `CreatureGuid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `CreatureID` int(10) unsigned NOT NULL COMMENT 'creature template id',
  `GroupID` int(10) unsigned NOT NULL COMMENT 'counter of unique texts per creature id',
  `HealthPercent` float DEFAULT NULL COMMENT 'the creature''s current health percent at the time the text was sent',
  `UnixTime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`CreatureID`,`GroupID`,`UnixTime`,`CreatureGuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='individual instances of creatures sending a text message';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_text_template
DROP TABLE IF EXISTS `creature_text_template`;
CREATE TABLE IF NOT EXISTS `creature_text_template` (
  `CreatureID` mediumint(8) unsigned NOT NULL DEFAULT '0' COMMENT 'creature template id',
  `GroupID` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'counter of unique texts per creature id',
  `Text` longtext COMMENT 'the actual text that was sent',
  `Type` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'chat type',
  `Language` tinyint(3) NOT NULL DEFAULT '0' COMMENT 'references Languages.dbc',
  `Emote` mediumint(8) unsigned NOT NULL DEFAULT '0' COMMENT 'references Emotes.dbc',
  `Sound` mediumint(8) unsigned NOT NULL DEFAULT '0' COMMENT 'references SoundEntries.dbc',
  `BroadcastTextId` mediumint(6) NOT NULL DEFAULT '0' COMMENT 'must be manually set',
  `Comment` varchar(255) DEFAULT '',
  PRIMARY KEY (`CreatureID`,`GroupID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='unique texts per creature id';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.creature_values_update
DROP TABLE IF EXISTS `creature_values_update`;
CREATE TABLE IF NOT EXISTS `creature_values_update` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `entry` int(10) unsigned DEFAULT NULL,
  `scale` float unsigned DEFAULT NULL,
  `display_id` int(10) unsigned DEFAULT NULL,
  `mount` int(10) unsigned DEFAULT NULL,
  `faction` int(10) unsigned DEFAULT NULL,
  `emote_state` int(10) unsigned DEFAULT NULL,
  `stand_state` int(10) unsigned DEFAULT NULL,
  `npc_flags` int(10) unsigned DEFAULT NULL,
  `unit_flags` int(10) unsigned DEFAULT NULL,
  `current_health` int(10) unsigned DEFAULT NULL,
  `max_health` int(10) unsigned DEFAULT NULL,
  `current_mana` int(10) unsigned DEFAULT NULL,
  `max_mana` int(10) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='values updates from SMSG_UPDATE_OBJECT';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject
DROP TABLE IF EXISTS `gameobject`;
CREATE TABLE IF NOT EXISTS `gameobject` (
  `guid` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Global Unique Identifier',
  `id` mediumint(8) unsigned NOT NULL DEFAULT '0' COMMENT 'Gameobject Identifier',
  `map` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Map Identifier',
  `zone_id` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Zone Identifier',
  `area_id` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Area Identifier',
  `phase_group` int(10) unsigned NOT NULL DEFAULT '1',
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `orientation` float NOT NULL DEFAULT '0',
  `rotation0` float NOT NULL DEFAULT '0',
  `rotation1` float NOT NULL DEFAULT '0',
  `rotation2` float NOT NULL DEFAULT '0',
  `rotation3` float NOT NULL DEFAULT '0',
  `temp` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `creator` int(11) unsigned NOT NULL DEFAULT '0',
  `animprogress` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `state` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `flags` int(10) unsigned NOT NULL DEFAULT '0',
  `SniffId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`guid`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED COMMENT='Gameobject System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_addon
DROP TABLE IF EXISTS `gameobject_addon`;
CREATE TABLE IF NOT EXISTS `gameobject_addon` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `parent_rotation0` float NOT NULL DEFAULT '0',
  `parent_rotation1` float NOT NULL DEFAULT '0',
  `parent_rotation2` float NOT NULL DEFAULT '0',
  `parent_rotation3` float NOT NULL DEFAULT '1',
  `invisibilityType` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `invisibilityValue` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_client_use
DROP TABLE IF EXISTS `gameobject_client_use`;
CREATE TABLE IF NOT EXISTS `gameobject_client_use` (
  `guid` int(10) unsigned NOT NULL COMMENT 'gameobject spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was sent',
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='times when client used a gameobject\r\nfrom CMSG_GAME_OBJ_USE and CMSG_GAME_OBJ_REPORT_USE';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_create1_time
DROP TABLE IF EXISTS `gameobject_create1_time`;
CREATE TABLE IF NOT EXISTS `gameobject_create1_time` (
  `guid` int(10) unsigned NOT NULL COMMENT 'gameobject spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `position_x` float NOT NULL,
  `position_y` float NOT NULL,
  `position_z` float NOT NULL,
  `orientation` float NOT NULL,
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='times when the object became visible to the client';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_create2_time
DROP TABLE IF EXISTS `gameobject_create2_time`;
CREATE TABLE IF NOT EXISTS `gameobject_create2_time` (
  `guid` int(10) unsigned NOT NULL COMMENT 'creature spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `position_x` float NOT NULL,
  `position_y` float NOT NULL,
  `position_z` float NOT NULL,
  `orientation` float NOT NULL,
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='the time at which the object spawned';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_custom_anim
DROP TABLE IF EXISTS `gameobject_custom_anim`;
CREATE TABLE IF NOT EXISTS `gameobject_custom_anim` (
  `guid` int(10) unsigned NOT NULL COMMENT 'gameobject spawn guid',
  `anim_id` int(10) unsigned NOT NULL DEFAULT '0',
  `as_despawn` tinyint(3) unsigned DEFAULT NULL,
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_GAME_OBJECT_CUSTOM_ANIM';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_despawn_anim
DROP TABLE IF EXISTS `gameobject_despawn_anim`;
CREATE TABLE IF NOT EXISTS `gameobject_despawn_anim` (
  `guid` int(10) unsigned NOT NULL COMMENT 'gameobject spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_GAME_OBJECT_DESPAWN';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_destroy_time
DROP TABLE IF EXISTS `gameobject_destroy_time`;
CREATE TABLE IF NOT EXISTS `gameobject_destroy_time` (
  `guid` int(10) unsigned NOT NULL COMMENT 'gameobject spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`guid`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='times when the object was destroyed from the client''s prespective due to despawning, becoming invisible, or going out of range';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_loot
DROP TABLE IF EXISTS `gameobject_loot`;
CREATE TABLE IF NOT EXISTS `gameobject_loot` (
  `entry` int(10) unsigned NOT NULL COMMENT 'gameobject template id',
  `loot_id` int(10) unsigned NOT NULL COMMENT 'counter',
  `money` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'copper',
  `items_count` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'number of items dropped',
  PRIMARY KEY (`entry`,`loot_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='each row represents a separate loot instance';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_loot_item
DROP TABLE IF EXISTS `gameobject_loot_item`;
CREATE TABLE IF NOT EXISTS `gameobject_loot_item` (
  `loot_id` int(10) unsigned NOT NULL COMMENT 'references creature_loot',
  `item_id` int(10) unsigned NOT NULL COMMENT 'item template id',
  `count` int(10) unsigned NOT NULL DEFAULT '1' COMMENT 'stack size'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='individual item that is part of a loot instance';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_template
DROP TABLE IF EXISTS `gameobject_template`;
CREATE TABLE IF NOT EXISTS `gameobject_template` (
  `entry` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `displayId` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `name` varchar(100) NOT NULL DEFAULT '',
  `icon_name` varchar(100) NOT NULL DEFAULT '',
  `cast_bar_caption` varchar(100) NOT NULL DEFAULT '',
  `unk1` varchar(100) NOT NULL DEFAULT '',
  `size` float NOT NULL DEFAULT '1',
  `data0` int(10) unsigned NOT NULL DEFAULT '0',
  `data1` int(11) NOT NULL DEFAULT '0',
  `data2` int(10) unsigned NOT NULL DEFAULT '0',
  `data3` int(10) unsigned NOT NULL DEFAULT '0',
  `data4` int(10) unsigned NOT NULL DEFAULT '0',
  `data5` int(10) unsigned NOT NULL DEFAULT '0',
  `data6` int(11) NOT NULL DEFAULT '0',
  `data7` int(10) unsigned NOT NULL DEFAULT '0',
  `data8` int(10) unsigned NOT NULL DEFAULT '0',
  `data9` int(10) unsigned NOT NULL DEFAULT '0',
  `data10` int(10) unsigned NOT NULL DEFAULT '0',
  `data11` int(10) unsigned NOT NULL DEFAULT '0',
  `data12` int(10) unsigned NOT NULL DEFAULT '0',
  `data13` int(10) unsigned NOT NULL DEFAULT '0',
  `data14` int(10) unsigned NOT NULL DEFAULT '0',
  `data15` int(10) unsigned NOT NULL DEFAULT '0',
  `data16` int(10) unsigned NOT NULL DEFAULT '0',
  `data17` int(10) unsigned NOT NULL DEFAULT '0',
  `data18` int(10) unsigned NOT NULL DEFAULT '0',
  `data19` int(10) unsigned NOT NULL DEFAULT '0',
  `data20` int(10) unsigned NOT NULL DEFAULT '0',
  `data21` int(10) unsigned NOT NULL DEFAULT '0',
  `data22` int(10) unsigned NOT NULL DEFAULT '0',
  `data23` int(10) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`entry`),
  KEY `idx_name` (`name`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED COMMENT='Gameobject System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_template_addon
DROP TABLE IF EXISTS `gameobject_template_addon`;
CREATE TABLE IF NOT EXISTS `gameobject_template_addon` (
  `entry` int(10) unsigned NOT NULL,
  `faction` int(10) unsigned NOT NULL DEFAULT '0',
  `flags` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gameobject_values_update
DROP TABLE IF EXISTS `gameobject_values_update`;
CREATE TABLE IF NOT EXISTS `gameobject_values_update` (
  `guid` int(10) unsigned NOT NULL COMMENT 'gameobject spawn guid',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  `flags` int(10) unsigned DEFAULT NULL,
  `state` int(10) unsigned DEFAULT NULL,
  `animprogress` int(10) unsigned DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='values updates from SMSG_UPDATE_OBJECT';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gossip_menu
DROP TABLE IF EXISTS `gossip_menu`;
CREATE TABLE IF NOT EXISTS `gossip_menu` (
  `entry` smallint(5) unsigned NOT NULL DEFAULT '0',
  `text_id` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`,`text_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gossip_menu_option
DROP TABLE IF EXISTS `gossip_menu_option`;
CREATE TABLE IF NOT EXISTS `gossip_menu_option` (
  `menu_id` smallint(5) unsigned NOT NULL DEFAULT '0',
  `id` smallint(5) unsigned NOT NULL DEFAULT '0',
  `option_icon` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `option_text` text,
  `option_broadcast_text` mediumint(6) NOT NULL DEFAULT '0',
  `option_id` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `npc_option_npcflag` int(10) unsigned NOT NULL DEFAULT '0',
  `action_menu_id` int(10) unsigned NOT NULL DEFAULT '0',
  `action_poi_id` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `box_coded` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `box_money` int(10) unsigned NOT NULL DEFAULT '0',
  `box_text` text,
  `box_broadcast_text` mediumint(6) NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`menu_id`,`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gossip_menu_option_action
DROP TABLE IF EXISTS `gossip_menu_option_action`;
CREATE TABLE IF NOT EXISTS `gossip_menu_option_action` (
  `menu_id` int(11) DEFAULT NULL,
  `id` int(11) DEFAULT NULL,
  `action_menu_id` int(11) DEFAULT NULL,
  `action_poi_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.gossip_menu_option_trainer
DROP TABLE IF EXISTS `gossip_menu_option_trainer`;
CREATE TABLE IF NOT EXISTS `gossip_menu_option_trainer` (
  `menu_id` int(11) DEFAULT NULL,
  `id` int(11) DEFAULT NULL,
  `trainer_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.hotfix_blob
DROP TABLE IF EXISTS `hotfix_blob`;
CREATE TABLE IF NOT EXISTS `hotfix_blob` (
  `TableHash` int(11) DEFAULT NULL,
  `RecordId` int(11) DEFAULT NULL,
  `Blob` blob
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.hotfix_data
DROP TABLE IF EXISTS `hotfix_data`;
CREATE TABLE IF NOT EXISTS `hotfix_data` (
  `Id` int(10) unsigned NOT NULL,
  `TableHash` int(10) unsigned NOT NULL DEFAULT '0',
  `RecordId` int(10) unsigned NOT NULL DEFAULT '0',
  `Deleted` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.item_client_use
DROP TABLE IF EXISTS `item_client_use`;
CREATE TABLE IF NOT EXISTS `item_client_use` (
  `entry` int(10) unsigned NOT NULL COMMENT 'item template entry',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was sent',
  PRIMARY KEY (`entry`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='times when client used an item\r\nfrom CMSG_USE_ITEM';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.item_instance
DROP TABLE IF EXISTS `item_instance`;
CREATE TABLE IF NOT EXISTS `item_instance` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `itemEntry` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `owner_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `count` int(10) unsigned NOT NULL DEFAULT '1',
  `charges` tinytext,
  `enchantments` text NOT NULL,
  `durability` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`),
  KEY `idx_owner_guid` (`owner_guid`),
  KEY `idx_itemEntry` (`itemEntry`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='player item data in format used by vmangos db';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.npc_text
DROP TABLE IF EXISTS `npc_text`;
CREATE TABLE IF NOT EXISTS `npc_text` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Probability0` float NOT NULL DEFAULT '0',
  `Probability1` float NOT NULL DEFAULT '0',
  `Probability2` float NOT NULL DEFAULT '0',
  `Probability3` float NOT NULL DEFAULT '0',
  `Probability4` float NOT NULL DEFAULT '0',
  `Probability5` float NOT NULL DEFAULT '0',
  `Probability6` float NOT NULL DEFAULT '0',
  `Probability7` float NOT NULL DEFAULT '0',
  `BroadcastTextID0` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `BroadcastTextID1` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `BroadcastTextID2` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `BroadcastTextID3` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `BroadcastTextID4` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `BroadcastTextID5` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `BroadcastTextID6` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `BroadcastTextID7` mediumint(6) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.npc_vendor
DROP TABLE IF EXISTS `npc_vendor`;
CREATE TABLE IF NOT EXISTS `npc_vendor` (
  `entry` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `slot` smallint(6) NOT NULL DEFAULT '0',
  `item` mediumint(8) NOT NULL DEFAULT '0',
  `maxcount` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `incrtime` int(10) unsigned NOT NULL DEFAULT '0',
  `ExtendedCost` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `type` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `PlayerConditionID` int(10) unsigned NOT NULL DEFAULT '0',
  `IgnoreFiltering` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`entry`,`item`,`ExtendedCost`,`type`),
  KEY `slot` (`slot`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED COMMENT='Npc System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.object_names
DROP TABLE IF EXISTS `object_names`;
CREATE TABLE IF NOT EXISTS `object_names` (
  `ObjectType` int(11) DEFAULT NULL,
  `Id` int(11) DEFAULT NULL,
  `Name` varchar(256) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.page_text
DROP TABLE IF EXISTS `page_text`;
CREATE TABLE IF NOT EXISTS `page_text` (
  `entry` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `text` longtext NOT NULL,
  `next_page` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`entry`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Item System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.playercreateinfo
DROP TABLE IF EXISTS `playercreateinfo`;
CREATE TABLE IF NOT EXISTS `playercreateinfo` (
  `race` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `class` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `map` smallint(5) unsigned NOT NULL DEFAULT '0',
  `zone` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `orientation` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`race`,`class`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.play_music
DROP TABLE IF EXISTS `play_music`;
CREATE TABLE IF NOT EXISTS `play_music` (
  `music` int(10) unsigned NOT NULL COMMENT 'references SoundEntries.dbc',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`music`,`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='from SMSG_PLAY_MUSIC';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.play_sound
DROP TABLE IF EXISTS `play_sound`;
CREATE TABLE IF NOT EXISTS `play_sound` (
  `source_guid` int(10) unsigned NOT NULL COMMENT 'guid of the object which was the source of the sound',
  `source_id` int(10) unsigned NOT NULL COMMENT 'entry of the object which was the source of the sound',
  `source_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL COMMENT 'type of the object which was the source of the sound',
  `sound` int(10) unsigned NOT NULL COMMENT 'references SoundEntries.dbc',
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`source_id`,`source_type`,`sound`,`unixtime`,`source_guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='from SMSG_PLAY_SOUND';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.play_spell_visual_kit
DROP TABLE IF EXISTS `play_spell_visual_kit`;
CREATE TABLE IF NOT EXISTS `play_spell_visual_kit` (
  `caster_guid` int(10) unsigned NOT NULL,
  `caster_id` int(10) unsigned NOT NULL,
  `caster_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL,
  `kit_id` int(10) unsigned NOT NULL COMMENT 'references SpellVisualKit.dbc',
  `kit_type` int(10) unsigned DEFAULT NULL,
  `duration` int(10) unsigned DEFAULT NULL,
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`caster_id`,`caster_type`,`kit_id`,`unixtime`,`caster_guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_PLAY_SPELL_VISUAL_KIT';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.points_of_interest
DROP TABLE IF EXISTS `points_of_interest`;
CREATE TABLE IF NOT EXISTS `points_of_interest` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `PositionX` float NOT NULL DEFAULT '0',
  `PositionY` float NOT NULL DEFAULT '0',
  `Icon` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Flags` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Importance` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Name` text NOT NULL,
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_client_accept
DROP TABLE IF EXISTS `quest_client_accept`;
CREATE TABLE IF NOT EXISTS `quest_client_accept` (
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was sent',
  `object_guid` int(10) unsigned NOT NULL COMMENT 'guid of the quest ender object',
  `object_id` int(10) unsigned NOT NULL COMMENT 'entry of the quest ender object',
  `object_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL COMMENT 'type of the quest ender object',
  `quest_id` int(10) unsigned NOT NULL COMMENT 'references quest_template',
  PRIMARY KEY (`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='times when a quest was accepted by client';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_client_complete
DROP TABLE IF EXISTS `quest_client_complete`;
CREATE TABLE IF NOT EXISTS `quest_client_complete` (
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was sent',
  `object_guid` int(10) unsigned NOT NULL COMMENT 'guid of the quest ender object',
  `object_id` int(10) unsigned NOT NULL COMMENT 'entry of the quest ender object',
  `object_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL COMMENT 'type of the quest ender object',
  `quest_id` int(10) unsigned NOT NULL COMMENT 'references quest_template',
  PRIMARY KEY (`unixtime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='times when a quest was turned in by client';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_details
DROP TABLE IF EXISTS `quest_details`;
CREATE TABLE IF NOT EXISTS `quest_details` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Emote1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Emote2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Emote3` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Emote4` smallint(5) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay1` int(10) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay2` int(10) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay3` int(10) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay4` int(10) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_ender
DROP TABLE IF EXISTS `quest_ender`;
CREATE TABLE IF NOT EXISTS `quest_ender` (
  `object_id` int(10) unsigned NOT NULL COMMENT 'entry of the quest ender object',
  `object_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL COMMENT 'type of the quest ender object',
  `quest_id` int(10) unsigned NOT NULL COMMENT 'references quest_template',
  PRIMARY KEY (`object_id`,`object_type`,`quest_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='list of quests that can be turned in to a given creature or gameobject';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_greeting
DROP TABLE IF EXISTS `quest_greeting`;
CREATE TABLE IF NOT EXISTS `quest_greeting` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `GreetEmoteType` smallint(5) unsigned NOT NULL DEFAULT '0',
  `GreetEmoteDelay` int(10) unsigned NOT NULL DEFAULT '0',
  `Greeting` text,
  `VerifiedBuild` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`,`Type`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_objectives
DROP TABLE IF EXISTS `quest_objectives`;
CREATE TABLE IF NOT EXISTS `quest_objectives` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `QuestID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `StorageIndex` tinyint(3) NOT NULL DEFAULT '0',
  `ObjectID` int(10) NOT NULL DEFAULT '0',
  `Amount` int(10) NOT NULL DEFAULT '0',
  `Flags` int(10) unsigned NOT NULL DEFAULT '0',
  `Flags2` int(10) unsigned NOT NULL DEFAULT '0',
  `ProgressBarWeight` float NOT NULL DEFAULT '0',
  `Description` text,
  `VerifiedBuild` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_offer_reward
DROP TABLE IF EXISTS `quest_offer_reward`;
CREATE TABLE IF NOT EXISTS `quest_offer_reward` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Emote1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Emote2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Emote3` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Emote4` smallint(5) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay1` int(10) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay2` int(10) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay3` int(10) unsigned NOT NULL DEFAULT '0',
  `EmoteDelay4` int(10) unsigned NOT NULL DEFAULT '0',
  `RewardText` text,
  `VerifiedBuild` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_poi
DROP TABLE IF EXISTS `quest_poi`;
CREATE TABLE IF NOT EXISTS `quest_poi` (
  `QuestID` int(10) unsigned NOT NULL DEFAULT '0',
  `id` int(10) unsigned NOT NULL DEFAULT '0',
  `ObjectiveIndex` int(11) NOT NULL DEFAULT '0',
  `MapID` int(10) unsigned NOT NULL DEFAULT '0',
  `WorldMapAreaId` int(10) unsigned NOT NULL DEFAULT '0',
  `Floor` int(10) unsigned NOT NULL DEFAULT '0',
  `Priority` int(10) unsigned NOT NULL DEFAULT '0',
  `Flags` int(10) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) DEFAULT '0',
  PRIMARY KEY (`QuestID`,`id`),
  KEY `idx` (`QuestID`,`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_poi_points
DROP TABLE IF EXISTS `quest_poi_points`;
CREATE TABLE IF NOT EXISTS `quest_poi_points` (
  `QuestID` int(10) unsigned NOT NULL DEFAULT '0',
  `Idx1` int(10) unsigned NOT NULL DEFAULT '0',
  `Idx2` int(10) unsigned NOT NULL DEFAULT '0',
  `X` int(11) NOT NULL DEFAULT '0',
  `Y` int(11) NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`QuestID`,`Idx1`,`Idx2`),
  KEY `questId_id` (`QuestID`,`Idx1`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_request_items
DROP TABLE IF EXISTS `quest_request_items`;
CREATE TABLE IF NOT EXISTS `quest_request_items` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Emote` smallint(5) unsigned DEFAULT NULL,
  `CompletionText` text,
  `VerifiedBuild` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_starter
DROP TABLE IF EXISTS `quest_starter`;
CREATE TABLE IF NOT EXISTS `quest_starter` (
  `object_id` int(10) unsigned NOT NULL COMMENT 'entry of the quest giver object',
  `object_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL COMMENT 'type of the quest giver object',
  `quest_id` int(10) unsigned NOT NULL COMMENT 'references quest_template',
  PRIMARY KEY (`object_id`,`object_type`,`quest_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='list of quests that can be picked up from a given creature or gameobject';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.quest_template
DROP TABLE IF EXISTS `quest_template`;
CREATE TABLE IF NOT EXISTS `quest_template` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `QuestType` tinyint(3) unsigned NOT NULL DEFAULT '2',
  `QuestLevel` smallint(3) NOT NULL DEFAULT '1',
  `MinLevel` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `QuestSortID` smallint(6) NOT NULL DEFAULT '0',
  `QuestInfoID` smallint(5) unsigned NOT NULL DEFAULT '0',
  `SuggestedGroupNum` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `RequiredFactionId1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredFactionId2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredFactionValue1` mediumint(8) NOT NULL DEFAULT '0',
  `RequiredFactionValue2` mediumint(8) NOT NULL DEFAULT '0',
  `RewardNextQuest` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardXPDifficulty` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `RewardMoney` int(11) NOT NULL DEFAULT '0',
  `RewardBonusMoney` int(10) unsigned NOT NULL DEFAULT '0',
  `RewardDisplaySpell` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardSpell` int(11) NOT NULL DEFAULT '0',
  `RewardHonor` int(11) NOT NULL DEFAULT '0',
  `RewardKillHonor` float NOT NULL DEFAULT '0',
  `StartItem` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Flags` int(10) unsigned NOT NULL DEFAULT '0',
  `RequiredPlayerKills` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `RewardItem1` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardAmount1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardItem2` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardAmount2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardItem3` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardAmount3` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardItem4` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardAmount4` smallint(5) unsigned NOT NULL DEFAULT '0',
  `ItemDrop1` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `ItemDropQuantity1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `ItemDrop2` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `ItemDropQuantity2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `ItemDrop3` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `ItemDropQuantity3` smallint(5) unsigned NOT NULL DEFAULT '0',
  `ItemDrop4` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `ItemDropQuantity4` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemID1` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemQuantity1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemID2` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemQuantity2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemID3` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemQuantity3` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemID4` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemQuantity4` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemID5` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemQuantity5` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemID6` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardChoiceItemQuantity6` smallint(5) unsigned NOT NULL DEFAULT '0',
  `POIContinent` smallint(5) unsigned NOT NULL DEFAULT '0',
  `POIx` float NOT NULL DEFAULT '0',
  `POIy` float NOT NULL DEFAULT '0',
  `POIPriority` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RewardTitle` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `RewardTalents` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `RewardArenaPoints` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RewardFactionID1` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'faction id from Faction.dbc in this case',
  `RewardFactionValue1` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionOverride1` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionID2` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'faction id from Faction.dbc in this case',
  `RewardFactionValue2` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionOverride2` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionID3` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'faction id from Faction.dbc in this case',
  `RewardFactionValue3` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionOverride3` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionID4` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'faction id from Faction.dbc in this case',
  `RewardFactionValue4` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionOverride4` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionID5` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'faction id from Faction.dbc in this case',
  `RewardFactionValue5` mediumint(8) NOT NULL DEFAULT '0',
  `RewardFactionOverride5` mediumint(8) NOT NULL DEFAULT '0',
  `TimeAllowed` int(10) unsigned NOT NULL DEFAULT '0',
  `AllowableRaces` smallint(5) unsigned NOT NULL DEFAULT '0',
  `LogTitle` text,
  `LogDescription` text,
  `QuestDescription` text,
  `AreaDescription` text,
  `QuestCompletionLog` text,
  `RequiredNpcOrGo1` mediumint(8) NOT NULL DEFAULT '0',
  `RequiredNpcOrGo2` mediumint(8) NOT NULL DEFAULT '0',
  `RequiredNpcOrGo3` mediumint(8) NOT NULL DEFAULT '0',
  `RequiredNpcOrGo4` mediumint(8) NOT NULL DEFAULT '0',
  `RequiredNpcOrGoCount1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredNpcOrGoCount2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredNpcOrGoCount3` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredNpcOrGoCount4` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredItemId1` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RequiredItemId2` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RequiredItemId3` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RequiredItemId4` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RequiredItemId5` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RequiredItemId6` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `RequiredItemCount1` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredItemCount2` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredItemCount3` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredItemCount4` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredItemCount5` smallint(5) unsigned NOT NULL DEFAULT '0',
  `RequiredItemCount6` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Unknown0` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `ObjectiveText1` text,
  `ObjectiveText2` text,
  `ObjectiveText3` text,
  `ObjectiveText4` text,
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Quest System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.sniff_file
DROP TABLE IF EXISTS `sniff_file`;
CREATE TABLE IF NOT EXISTS `sniff_file` (
  `id` int(10) unsigned NOT NULL,
  `name` varchar(256) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_cast_failed
DROP TABLE IF EXISTS `spell_cast_failed`;
CREATE TABLE IF NOT EXISTS `spell_cast_failed` (
  `caster_guid` int(10) unsigned NOT NULL,
  `caster_id` int(10) unsigned NOT NULL,
  `caster_type` varchar(32) COLLATE utf8_unicode_ci NOT NULL,
  `spell_id` int(10) unsigned NOT NULL,
  `visual_id` int(10) unsigned DEFAULT NULL,
  `reason` int(10) unsigned DEFAULT NULL,
  `unixtime` int(10) unsigned NOT NULL COMMENT 'when the packet was received',
  PRIMARY KEY (`caster_id`,`caster_type`,`spell_id`,`unixtime`,`caster_guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_SPELL_FAILURE and SMSG_SPELL_FAILED_OTHER';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_cast_go
DROP TABLE IF EXISTS `spell_cast_go`;
CREATE TABLE IF NOT EXISTS `spell_cast_go` (
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'when the packet was received',
  `caster_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_id` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_type` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `spell_id` int(10) unsigned NOT NULL DEFAULT '0',
  `cast_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `cast_flags_ex` int(10) unsigned NOT NULL DEFAULT '0',
  `main_target_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `main_target_id` int(10) unsigned NOT NULL DEFAULT '0',
  `main_target_type` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `hit_targets_count` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `hit_targets_list_id` int(10) unsigned NOT NULL DEFAULT '0',
  `miss_targets_count` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `miss_targets_list_id` int(10) unsigned NOT NULL DEFAULT '0',
  `src_position_id` int(10) unsigned NOT NULL DEFAULT '0',
  `dst_position_id` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='from SMSG_SPELL_GO\r\nsent when a spell is successfully casted';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_cast_go_position
DROP TABLE IF EXISTS `spell_cast_go_position`;
CREATE TABLE IF NOT EXISTS `spell_cast_go_position` (
  `id` int(10) unsigned NOT NULL,
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='source and destination locations from SMSG_SPELL_GO';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_cast_go_target
DROP TABLE IF EXISTS `spell_cast_go_target`;
CREATE TABLE IF NOT EXISTS `spell_cast_go_target` (
  `list_id` int(10) unsigned NOT NULL DEFAULT '0',
  `target_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `target_id` int(10) unsigned NOT NULL DEFAULT '0',
  `target_type` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='hit and miss targets from SMSG_SPELL_GO';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_cast_start
DROP TABLE IF EXISTS `spell_cast_start`;
CREATE TABLE IF NOT EXISTS `spell_cast_start` (
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'when the packet was received',
  `caster_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_id` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_type` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `spell_id` int(10) unsigned NOT NULL DEFAULT '0',
  `cast_time` int(10) unsigned NOT NULL DEFAULT '0',
  `cast_flags` int(10) unsigned NOT NULL DEFAULT '0',
  `cast_flags_ex` int(10) unsigned NOT NULL DEFAULT '0',
  `target_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `target_id` int(10) unsigned NOT NULL DEFAULT '0',
  `target_type` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='from SMSG_SPELL_START\r\nsent when somebody starts casting a spell';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_channel_start
DROP TABLE IF EXISTS `spell_channel_start`;
CREATE TABLE IF NOT EXISTS `spell_channel_start` (
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'when the packet was received',
  `caster_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_id` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_type` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `spell_id` int(10) unsigned NOT NULL DEFAULT '0',
  `visual_id` int(10) unsigned DEFAULT NULL,
  `duration` int(10) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_SPELL_CHANNEL_START\r\nsent when somebody starts channeling a spell';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_channel_update
DROP TABLE IF EXISTS `spell_channel_update`;
CREATE TABLE IF NOT EXISTS `spell_channel_update` (
  `unixtime` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'when the packet was received',
  `caster_guid` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_id` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_type` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `duration` int(10) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='from SMSG_SPELL_CHANNEL_UPDATE';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_pet_actions
DROP TABLE IF EXISTS `spell_pet_actions`;
CREATE TABLE IF NOT EXISTS `spell_pet_actions` (
  `creature_id` int(10) unsigned NOT NULL DEFAULT '0',
  `slot1` int(10) unsigned NOT NULL DEFAULT '0',
  `slot2` int(10) unsigned NOT NULL DEFAULT '0',
  `slot3` int(10) unsigned NOT NULL DEFAULT '0',
  `slot4` int(10) unsigned NOT NULL DEFAULT '0',
  `slot5` int(10) unsigned NOT NULL DEFAULT '0',
  `slot6` int(10) unsigned NOT NULL DEFAULT '0',
  `slot7` int(10) unsigned NOT NULL DEFAULT '0',
  `slot8` int(10) unsigned NOT NULL DEFAULT '0',
  `slot9` int(10) unsigned NOT NULL DEFAULT '0',
  `slot10` int(10) unsigned NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_pet_cooldown
DROP TABLE IF EXISTS `spell_pet_cooldown`;
CREATE TABLE IF NOT EXISTS `spell_pet_cooldown` (
  `creature_id` int(10) unsigned NOT NULL DEFAULT '0',
  `flags` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `index` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `spell_id` int(10) unsigned NOT NULL DEFAULT '0',
  `cooldown` int(10) unsigned NOT NULL DEFAULT '0',
  `mod_rate` float unsigned NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.spell_target_position
DROP TABLE IF EXISTS `spell_target_position`;
CREATE TABLE IF NOT EXISTS `spell_target_position` (
  `ID` mediumint(8) unsigned NOT NULL DEFAULT '0' COMMENT 'Identifier',
  `EffectIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `MapID` smallint(5) unsigned NOT NULL DEFAULT '0',
  `PositionX` float NOT NULL DEFAULT '0',
  `PositionY` float NOT NULL DEFAULT '0',
  `PositionZ` float NOT NULL DEFAULT '0',
  `Orientation` float NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`ID`,`EffectIndex`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED COMMENT='Spell System';

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.trainer
DROP TABLE IF EXISTS `trainer`;
CREATE TABLE IF NOT EXISTS `trainer` (
  `Id` int(10) unsigned NOT NULL DEFAULT '0',
  `Type` tinyint(2) unsigned NOT NULL DEFAULT '2',
  `Requirement` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `Greeting` text,
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.trainer_spell
DROP TABLE IF EXISTS `trainer_spell`;
CREATE TABLE IF NOT EXISTS `trainer_spell` (
  `TrainerId` int(10) unsigned NOT NULL DEFAULT '0',
  `SpellId` int(10) unsigned NOT NULL DEFAULT '0',
  `MoneyCost` int(10) unsigned NOT NULL DEFAULT '0',
  `ReqSkillLine` int(10) unsigned NOT NULL DEFAULT '0',
  `ReqSkillRank` int(10) unsigned NOT NULL DEFAULT '0',
  `ReqAbility1` int(10) unsigned NOT NULL DEFAULT '0',
  `ReqAbility2` int(10) unsigned NOT NULL DEFAULT '0',
  `ReqAbility3` int(10) unsigned NOT NULL DEFAULT '0',
  `ReqLevel` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `VerifiedBuild` smallint(5) unsigned DEFAULT '0',
  PRIMARY KEY (`TrainerId`,`SpellId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.weather_update
DROP TABLE IF EXISTS `weather_update`;
CREATE TABLE IF NOT EXISTS `weather_update` (
  `map_id` int(11) NOT NULL DEFAULT '0',
  `zone_id` int(11) NOT NULL DEFAULT '0',
  `weather_state` int(11) NOT NULL DEFAULT '0',
  `grade` int(11) NOT NULL DEFAULT '0',
  `unk` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Data exporting was unselected.


-- Dumping structure for table sniffs_new_test.world_text
DROP TABLE IF EXISTS `world_text`;
CREATE TABLE IF NOT EXISTS `world_text` (
  `UnixTime` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'when the packet was received',
  `Text` longtext COMMENT 'the actual text that was sent',
  `Type` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'chat type',
  `Language` tinyint(3) NOT NULL DEFAULT '0' COMMENT 'references Languages.dbc',
  PRIMARY KEY (`UnixTime`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC COMMENT='unique texts per creature id';

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
