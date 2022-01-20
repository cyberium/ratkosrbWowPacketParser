ALTER TABLE `creature_movement_client`
	ADD COLUMN `swim_pitch` FLOAT NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `fall_time` INT NOT NULL DEFAULT '0' AFTER `swim_pitch`,
	ADD COLUMN `jump_horizontal_speed` FLOAT NOT NULL DEFAULT '0' AFTER `fall_time`,
	ADD COLUMN `jump_vertical_speed` FLOAT NOT NULL DEFAULT '0' AFTER `jump_horizontal_speed`,
	ADD COLUMN `jump_cos_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_vertical_speed`,
	ADD COLUMN `jump_sin_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_cos_angle`;

ALTER TABLE `player_movement_client`
	ADD COLUMN `swim_pitch` FLOAT NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `fall_time` INT NOT NULL DEFAULT '0' AFTER `swim_pitch`,
	ADD COLUMN `jump_horizontal_speed` FLOAT NOT NULL DEFAULT '0' AFTER `fall_time`,
	ADD COLUMN `jump_vertical_speed` FLOAT NOT NULL DEFAULT '0' AFTER `jump_horizontal_speed`,
	ADD COLUMN `jump_cos_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_vertical_speed`,
	ADD COLUMN `jump_sin_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_cos_angle`;

ALTER TABLE `creature_movement_client`
	ADD COLUMN `move_flags2` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `move_flags`,
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`,
	ADD COLUMN `spline_elevation` FLOAT NOT NULL DEFAULT '0' AFTER `jump_sin_angle`;

ALTER TABLE `player_movement_client`
	ADD COLUMN `move_flags2` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `move_flags`,
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`,
	ADD COLUMN `spline_elevation` FLOAT NOT NULL DEFAULT '0' AFTER `jump_sin_angle`;

ALTER TABLE `creature_movement_client`
	CHANGE COLUMN `fall_time` `fall_time` INT(11) UNSIGNED NOT NULL DEFAULT '0' AFTER `swim_pitch`;

ALTER TABLE `player_movement_client`
	CHANGE COLUMN `fall_time` `fall_time` INT(11) UNSIGNED NOT NULL DEFAULT '0' AFTER `swim_pitch`;

ALTER TABLE `creature`
	DROP COLUMN `move_flags`;

ALTER TABLE `player`
	DROP COLUMN `move_flags`;

ALTER TABLE `creature_create1_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`,
	ADD COLUMN `move_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `move_flags` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_time`,
	ADD COLUMN `move_flags2` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_flags`,
	ADD COLUMN `swim_pitch` FLOAT NOT NULL DEFAULT '0' AFTER `move_flags2`,
	ADD COLUMN `fall_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `swim_pitch`,
	ADD COLUMN `jump_horizontal_speed` FLOAT NOT NULL DEFAULT '0' AFTER `fall_time`,
	ADD COLUMN `jump_vertical_speed` FLOAT NOT NULL DEFAULT '0' AFTER `jump_horizontal_speed`,
	ADD COLUMN `jump_cos_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_vertical_speed`,
	ADD COLUMN `jump_sin_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_cos_angle`,
	ADD COLUMN `spline_elevation` FLOAT NOT NULL DEFAULT '0' AFTER `jump_sin_angle`;

ALTER TABLE `creature_create2_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`,
	ADD COLUMN `move_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `move_flags` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_time`,
	ADD COLUMN `move_flags2` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_flags`,
	ADD COLUMN `swim_pitch` FLOAT NOT NULL DEFAULT '0' AFTER `move_flags2`,
	ADD COLUMN `fall_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `swim_pitch`,
	ADD COLUMN `jump_horizontal_speed` FLOAT NOT NULL DEFAULT '0' AFTER `fall_time`,
	ADD COLUMN `jump_vertical_speed` FLOAT NOT NULL DEFAULT '0' AFTER `jump_horizontal_speed`,
	ADD COLUMN `jump_cos_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_vertical_speed`,
	ADD COLUMN `jump_sin_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_cos_angle`,
	ADD COLUMN `spline_elevation` FLOAT NOT NULL DEFAULT '0' AFTER `jump_sin_angle`;

ALTER TABLE `player_create1_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`,
	ADD COLUMN `move_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `move_flags` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_time`,
	ADD COLUMN `move_flags2` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_flags`,
	ADD COLUMN `swim_pitch` FLOAT NOT NULL DEFAULT '0' AFTER `move_flags2`,
	ADD COLUMN `fall_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `swim_pitch`,
	ADD COLUMN `jump_horizontal_speed` FLOAT NOT NULL DEFAULT '0' AFTER `fall_time`,
	ADD COLUMN `jump_vertical_speed` FLOAT NOT NULL DEFAULT '0' AFTER `jump_horizontal_speed`,
	ADD COLUMN `jump_cos_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_vertical_speed`,
	ADD COLUMN `jump_sin_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_cos_angle`,
	ADD COLUMN `spline_elevation` FLOAT NOT NULL DEFAULT '0' AFTER `jump_sin_angle`;

ALTER TABLE `player_create2_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`,
	ADD COLUMN `move_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `move_flags` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_time`,
	ADD COLUMN `move_flags2` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `move_flags`,
	ADD COLUMN `swim_pitch` FLOAT NOT NULL DEFAULT '0' AFTER `move_flags2`,
	ADD COLUMN `fall_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `swim_pitch`,
	ADD COLUMN `jump_horizontal_speed` FLOAT NOT NULL DEFAULT '0' AFTER `fall_time`,
	ADD COLUMN `jump_vertical_speed` FLOAT NOT NULL DEFAULT '0' AFTER `jump_horizontal_speed`,
	ADD COLUMN `jump_cos_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_vertical_speed`,
	ADD COLUMN `jump_sin_angle` FLOAT NOT NULL DEFAULT '0' AFTER `jump_cos_angle`,
	ADD COLUMN `spline_elevation` FLOAT NOT NULL DEFAULT '0' AFTER `jump_sin_angle`;
  
ALTER TABLE `dynamicobject_create1_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `dynamicobject_create2_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `gameobject_create1_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `gameobject_create2_time`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`,
	ADD COLUMN `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	ADD COLUMN `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	ADD COLUMN `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `gameobject_create1_time`
	ADD COLUMN `transport_path_timer` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`;

ALTER TABLE `gameobject_create2_time`
	ADD COLUMN `transport_path_timer` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`;

ALTER TABLE `creature_movement_server`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`;

ALTER TABLE `creature_movement_server_combat`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`;

ALTER TABLE `player_movement_server`
	ADD COLUMN `transport_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `orientation`;

ALTER TABLE `creature_text`
	ADD COLUMN `target_guid` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `health_percent`,
	ADD COLUMN `target_id` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `target_guid`,
	ADD COLUMN `target_type` VARCHAR(16) NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `target_id`;

DROP TABLE `gameobject_text`;
DROP TABLE `gameobject_text_template`;

ALTER TABLE `creature_movement_client`
	ADD COLUMN `packet_id` INT(10) UNSIGNED NOT NULL AFTER `guid`;

ALTER TABLE `creature_movement_client`
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`packet_id`);

ALTER TABLE `player_movement_client`
	ADD COLUMN `packet_id` INT(10) UNSIGNED NOT NULL AFTER `guid`;

ALTER TABLE `player_movement_client`
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`packet_id`);

CREATE TABLE `creature_faction` (
	`entry` MEDIUMINT UNSIGNED NOT NULL,
	`faction` INT UNSIGNED NOT NULL,
	PRIMARY KEY (`entry`, `faction`)
)
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_faction`
	COMMENT='all unique faction template ids used by a given creature id';


CREATE TABLE `creature_damage_school` (
	`entry` MEDIUMINT UNSIGNED NOT NULL,
	`total_school_mask` INT UNSIGNED NOT NULL,
	PRIMARY KEY (`entry`, `total_school_mask`)
)
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_damage_school`
	COMMENT='all schools of damage dealt with melee attacks by a given creature id';

ALTER TABLE `player`
	CHANGE COLUMN `map` `map` SMALLINT UNSIGNED NULL DEFAULT '0' COMMENT 'Map Identifier' AFTER `guid`,
	ADD COLUMN `zone_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'Zone Identifier' AFTER `map`,
	ADD COLUMN `area_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'Area Identifier' AFTER `zone_id`;

ALTER TABLE `creature_stats`
	ADD COLUMN `is_pet` TINYINT UNSIGNED NOT NULL AFTER `level`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`entry`, `level`, `is_pet`);

CREATE TABLE `logout_time` (
	`unixtimems` BIGINT(20) UNSIGNED NOT NULL COMMENT 'when the packet was received',
	PRIMARY KEY (`unixtimems`)
)
COMMENT='from SMSG_LOGOUT_COMPLETE'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE `creature_pet_name` (
	`guid` INT UNSIGNED NOT NULL,
	`name` VARCHAR(16) NOT NULL DEFAULT '',
	PRIMARY KEY (`guid`)
)
COMMENT='from SMSG_QUERY_PET_NAME_RESPONSE'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_threat_update`
	COMMENT='from SMSG_THREAT_UPDATE and SMSG_HIGHEST_THREAT_UPDATE';

ALTER TABLE `creature_threat_update_target`
	COMMENT='individual targets and their threat from SMSG_THREAT_UPDATE and SMSG_HIGHEST_THREAT_UPDATE';

ALTER TABLE `client_release_spirit`
	COMMENT='from CMSG_REPOP_REQUEST';

ALTER TABLE `client_creature_interact`
	COMMENT='times when the client talked to a creature';

ALTER TABLE `creature`
	COMMENT='the initial state of all individual creature spawns seen in the sniff';

ALTER TABLE `creature_display_info_addon`
	COMMENT='data for display ids used by creatures';

ALTER TABLE `creature_equip_template`
	COMMENT='all weapons used by a given creature id, assigned in the virtual item slots';

ALTER TABLE `creature_gossip`
	COMMENT='all unique gossip menu ids used for given creature id';

ALTER TABLE `creature_loot`
	COMMENT='each row represents a separate loot instance\r\nmight not contain all the items or gold that dropped if somebody else looted them first';

ALTER TABLE `creature_pet_actions`
	COMMENT='from SMSG_PET_SPELLS_MESSAGE';

ALTER TABLE `creature_pet_cooldown`
	COMMENT='from SMSG_SPELL_COOLDOWN';

ALTER TABLE `creature_questitem`
	COMMENT='quest items that drop from a given creature id\r\nfrom SMSG_QUERY_CREATURE_RESPONSE';

ALTER TABLE `creature_stats`
	COMMENT='stats data from SMSG_UPDATE_OBJECT\r\nserver only sends it to the creature\'s charmer, or player who casts beast lore on it';

ALTER TABLE `creature_template`
	COMMENT='most commonly seen values per given creature id\r\ndata here is not guaranteed to be the true default',
	CHANGE COLUMN `gossip_menu_id` `gossip_menu_id` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'will only be set if the client talked to this npc' AFTER `entry`,
	CHANGE COLUMN `scale` `scale` FLOAT NOT NULL DEFAULT '1' AFTER `speed_run`,
	CHANGE COLUMN `auras` `auras` TEXT NULL COMMENT 'only includes auras with the NO_CASTER flag' AFTER `hover_height`;

ALTER TABLE `creature_template_wdb`
	COMMENT='static creature data from SMSG_QUERY_CREATURE_RESPONSE which gets saved to wdb cache';

ALTER TABLE `dynamicobject`
	COMMENT='the initial state of all individual dynamicobject spawns seen in the sniff\r\nthese are the objects used for ground targeted spell animations';

ALTER TABLE `faction_standing_update`
	COMMENT='from SMSG_SET_FACTION_STANDING';

ALTER TABLE `gameobject`
	COMMENT='the initial state of all individual gameobject spawns seen in the sniff';

ALTER TABLE `gameobject_loot`
	COMMENT='each row represents a separate loot instance\r\nmight not contain all the items or gold that dropped if somebody else looted them first';

ALTER TABLE `gameobject_questitem`
	COMMENT='quest items that drop from a given gameobject id\r\nfrom SMSG_QUERY_GAME_OBJECT_RESPONSE';

ALTER TABLE `gameobject_template`
	COMMENT='static gameobject data from SMSG_QUERY_GAME_OBJECT_RESPONSE which gets saved to wdb cache';

ALTER TABLE `player`
	COMMENT='the initial state of all players seen in the sniff';

ALTER TABLE `sniff_data`
	COMMENT='information about the contents of each sniff included in the database\r\ncan be used to figure out where specific data can be found';

ALTER TABLE `sniff_file`
	COMMENT='the names of all sniffs included in the database';
  
ALTER TABLE `spell_target_position`
	COMMENT='target coordinates for spells which need them defined in the database';
  
ALTER TABLE `weather_update`
	COMMENT='from SMSG_WEATHER';

ALTER TABLE `world_text`
	COMMENT='texts sent by the server which did not originate from a creature or player';

ALTER TABLE `gameobject`
	ADD COLUMN `is_spawn` TINYINT(1) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'create object type 2' AFTER `rotation3`;

CREATE TABLE `creature_threat_clear` (
	`unixtimems` BIGINT UNSIGNED NOT NULL DEFAULT '0',
	`guid` INT UNSIGNED NOT NULL,
	PRIMARY KEY (`unixtimems`, `guid`)
)
COMMENT='from SMSG_THREAT_CLEAR'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE `creature_threat_remove` (
	`unixtimems` BIGINT UNSIGNED NOT NULL DEFAULT '0',
	`guid` INT UNSIGNED NOT NULL,
	`target_guid` INT UNSIGNED NOT NULL DEFAULT '0',
	`target_id` INT UNSIGNED NOT NULL DEFAULT '0',
	`target_type` VARCHAR(16) NOT NULL DEFAULT '',
	PRIMARY KEY (`unixtimems`, `guid`)
)
COMMENT='from SMSG_THREAT_REMOVE'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_guid_values_update`
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`guid`, `unixtimems`, `field_name`, `object_guid`, `object_id`, `object_type`);

ALTER TABLE `player_guid_values_update`
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`guid`, `unixtimems`, `field_name`, `object_guid`, `object_id`, `object_type`);

CREATE TABLE `cinematic_begin` (
	`unixtimems` BIGINT UNSIGNED NOT NULL COMMENT 'when the packet was received',
	`cinematic_id` INT UNSIGNED NOT NULL,
	PRIMARY KEY (`unixtimems`)
)
COMMENT='from SMSG_TRIGGER_CINEMATIC'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE `cinematic_end` (
	`unixtimems` BIGINT UNSIGNED NOT NULL COMMENT 'when the packet was sent',
	PRIMARY KEY (`unixtimems`)
)
COMMENT='from CMSG_COMPLETE_CINEMATIC'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_threat_remove`
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`unixtimems`, `guid`, `target_guid`, `target_id`, `target_type`);

CREATE TABLE `creature_pet_remaining_cooldown` (
	`entry` INT UNSIGNED NOT NULL DEFAULT '0',
	`spell_id` INT UNSIGNED NOT NULL DEFAULT '0',
	`cooldown` INT UNSIGNED NOT NULL DEFAULT '0',
	`category` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`category_cooldown` INT UNSIGNED NOT NULL DEFAULT '0',
	`mod_rate` FLOAT UNSIGNED NOT NULL DEFAULT '1'
)
COMMENT='cooldowns that were already present when the creature became charmed\r\nfrom SMSG_PET_SPELLS_MESSAGE'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `sniff_data`
	CHANGE COLUMN `object_type` `object_type` ENUM('None','Spell','Map','LFGDungeon','Battleground','Unit','GameObject','CreatureDifficulty','Item','Quest','Opcode','PageText','NpcText','BroadcastText','Gossip','Zone','Area','AreaTrigger','Phase','Player','Achievement','CreatureFamily','Criteria','Currency','Difficulty','Faction','QuestGreeting','QuestObjective','Sound','Taxi') NOT NULL DEFAULT 'None' AFTER `sniff_id`;

ALTER TABLE `creature_pet_remaining_cooldown`
	ADD COLUMN `time_since_cast` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'milliseconds since last SMSG_SPELL_GO for this spell' AFTER `mod_rate`;

CREATE TABLE `creature_spell_timers` (
	`entry` INT UNSIGNED NOT NULL DEFAULT '0',
	`spell_id` INT UNSIGNED NOT NULL DEFAULT '0',
	`initial_casts_count` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`initial_delay_min` INT UNSIGNED NOT NULL DEFAULT '0',
	`initial_delay_average` INT UNSIGNED NOT NULL DEFAULT '0',
	`initial_delay_max` INT UNSIGNED NOT NULL DEFAULT '0',
	`repeat_casts_count` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`repeat_delay_min` INT UNSIGNED NOT NULL DEFAULT '0',
	`repeat_delay_average` INT UNSIGNED NOT NULL DEFAULT '0',
	`repeat_delay_max` INT UNSIGNED NOT NULL DEFAULT '0',
	`sniff_build` SMALLINT UNSIGNED NOT NULL DEFAULT '0'
)
COMMENT='calculated time between casts for creatures'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_damage_school`
	COMMENT='statistics for melee damage per creature id';
  
RENAME TABLE `creature_damage_school` TO `creature_melee_damage`;

ALTER TABLE `creature_melee_damage`
	CHANGE COLUMN `entry` `entry` INT UNSIGNED NOT NULL DEFAULT '0' FIRST,
	ADD COLUMN `hits_count` SMALLINT UNSIGNED NOT NULL DEFAULT '0' AFTER `entry`,
	ADD COLUMN `damage_min` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `hits_count`,
	ADD COLUMN `damage_average` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `damage_min`,
	ADD COLUMN `damage_max` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `damage_average`,
	CHANGE COLUMN `total_school_mask` `total_school_mask` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `damage_max`,
	ADD COLUMN `sniff_build` SMALLINT UNSIGNED NOT NULL DEFAULT '0' AFTER `total_school_mask`,
	DROP PRIMARY KEY;

ALTER TABLE `creature_melee_damage`
	ADD COLUMN `is_dirty` TINYINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'mob had auras that affect damage or there were no normal hits' AFTER `entry`;

ALTER TABLE `creature_stats`
	ADD COLUMN `is_dirty` TINYINT(3) UNSIGNED NOT NULL COMMENT 'mob had auras that affect stats' AFTER `is_pet`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`entry`, `level`, `is_pet`, `is_dirty`);

ALTER TABLE `creature_melee_damage`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `total_school_mask`;

ALTER TABLE `creature_spell_timers`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `repeat_delay_max`;
  
ALTER TABLE `creature_template_locale`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `TitleAlt`;

ALTER TABLE `gameobject`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `sniff_id`;

ALTER TABLE `gameobject`
	CHANGE COLUMN `sniff_id` `sniff_id` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table' AFTER `custom_param`;

ALTER TABLE `gameobject_questitem`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `item_id`;

ALTER TABLE `quest_details`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `EmoteDelay4`;

ALTER TABLE `quest_greeting`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `Greeting`;

ALTER TABLE `quest_greeting_locale`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `Greeting`;

ALTER TABLE `quest_objectives`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `Description`;

ALTER TABLE `quest_objectives_locale`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `Description`;

ALTER TABLE `quest_offer_reward`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `RewardText`;

ALTER TABLE `quest_offer_reward_locale`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `RewardText`;

ALTER TABLE `quest_request_items`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `CompletionText`;

ALTER TABLE `quest_request_items_locale`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `CompletionText`;

ALTER TABLE `quest_template`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NULL DEFAULT '0' AFTER `CompleteSoundKitID`;

ALTER TABLE `quest_template_locale`
	CHANGE COLUMN `VerifiedBuild` `VerifiedBuild` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `QuestCompletionLog`;

ALTER TABLE `trainer`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `greeting`;

ALTER TABLE `trainer_spell`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `required_level`;

ALTER TABLE `creature_pet_actions`
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `slot10`;

ALTER TABLE `creature_pet_cooldown`
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `mod_rate`;

ALTER TABLE `creature_pet_remaining_cooldown`
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `time_since_cast`;

ALTER TABLE `creature_stats`
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `negative_arcane_res`;

ALTER TABLE `gameobject`
	CHANGE COLUMN `map` `map` SMALLINT(5) UNSIGNED NULL DEFAULT '0' COMMENT 'Map Identifier' AFTER `id`;

ALTER TABLE `dynamicobject`
	CHANGE COLUMN `map` `map` SMALLINT(5) UNSIGNED NULL DEFAULT '0' COMMENT 'Map Identifier' AFTER `guid`;

ALTER TABLE `creature`
	ADD COLUMN `waypoint_count` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'number of out of combat movement packets seen' AFTER `wander_distance`;

ALTER TABLE `creature`
	ADD COLUMN `power_type` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `max_health`;

ALTER TABLE `creature`
	CHANGE COLUMN `current_mana` `current_power` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `power_type`,
	CHANGE COLUMN `max_mana` `max_power` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `current_power`;

ALTER TABLE `player`
	ADD COLUMN `power_type` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `max_health`;

ALTER TABLE `player`
	CHANGE COLUMN `current_mana` `current_power` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `power_type`,
	CHANGE COLUMN `max_mana` `max_power` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `current_power`;

CREATE TABLE `creature_power_values` (
	`guid` INT UNSIGNED NOT NULL,
	`power_type` TINYINT UNSIGNED NOT NULL,
	`current_power` INT UNSIGNED NOT NULL DEFAULT '0',
	`max_power` INT UNSIGNED NOT NULL DEFAULT '0',
	PRIMARY KEY (`guid`, `power_type`)
)
COMMENT='initial value of power update fields'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE `player_power_values` (
	`guid` INT UNSIGNED NOT NULL,
	`power_type` TINYINT UNSIGNED NOT NULL,
	`current_power` INT UNSIGNED NOT NULL DEFAULT '0',
	`max_power` INT UNSIGNED NOT NULL DEFAULT '0',
	PRIMARY KEY (`guid`, `power_type`)
)
COMMENT='initial value of power update fields'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE `creature_power_values_update` (
	`unixtimems` BIGINT UNSIGNED NOT NULL,
	`guid` INT UNSIGNED NOT NULL,
	`power_type` TINYINT UNSIGNED NOT NULL,
	`current_power` INT UNSIGNED NULL,
	`max_power` INT UNSIGNED NULL
)
COMMENT='changes to power update fields'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE `player_power_values_update` (
	`unixtimems` BIGINT UNSIGNED NOT NULL,
	`guid` INT UNSIGNED NOT NULL,
	`power_type` TINYINT UNSIGNED NOT NULL,
	`current_power` INT UNSIGNED NULL,
	`max_power` INT UNSIGNED NULL
)
COMMENT='changes to power update fields'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_values_update`
	CHANGE COLUMN `current_mana` `power_type` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `max_health`,
	DROP COLUMN `max_mana`;

ALTER TABLE `player_values_update`
	CHANGE COLUMN `current_mana` `power_type` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `max_health`,
	DROP COLUMN `max_mana`;

ALTER TABLE `creature`
	ADD COLUMN `is_vehicle` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' AFTER `is_pet`;

ALTER TABLE `creature_create1_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `creature_create2_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `player_create1_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `player_create2_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `gameobject_create1_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `gameobject_create2_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `dynamicobject_create1_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `dynamicobject_create2_time`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `creature_movement_client`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `player_movement_client`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `creature_movement_server`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `creature_movement_server_combat`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `player_movement_server`
	ADD COLUMN `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	ADD COLUMN `transport_type` VARCHAR(16) NOT NULL DEFAULT '' AFTER `transport_id`;

ALTER TABLE `player_create1_time`
	CHANGE COLUMN `transport_guid` `transport_guid` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	CHANGE COLUMN `transport_id` `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	CHANGE COLUMN `transport_type` `transport_type` VARCHAR(16) NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `transport_id`,
	CHANGE COLUMN `transport_x` `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_type`,
	CHANGE COLUMN `transport_y` `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	CHANGE COLUMN `transport_z` `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	CHANGE COLUMN `transport_o` `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `player_create1_time`
	ADD COLUMN `vehicle_id` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	ADD COLUMN `vehicle_orientation` FLOAT NOT NULL DEFAULT '0' AFTER `vehicle_id`;

ALTER TABLE `player_create2_time`
	CHANGE COLUMN `transport_guid` `transport_guid` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	CHANGE COLUMN `transport_id` `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	CHANGE COLUMN `transport_type` `transport_type` VARCHAR(16) NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `transport_id`,
	CHANGE COLUMN `transport_x` `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_type`,
	CHANGE COLUMN `transport_y` `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	CHANGE COLUMN `transport_z` `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	CHANGE COLUMN `transport_o` `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `player_create2_time`
	ADD COLUMN `vehicle_id` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	ADD COLUMN `vehicle_orientation` FLOAT NOT NULL DEFAULT '0' AFTER `vehicle_id`;

ALTER TABLE `creature_create1_time`
	CHANGE COLUMN `transport_guid` `transport_guid` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	CHANGE COLUMN `transport_id` `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	CHANGE COLUMN `transport_type` `transport_type` VARCHAR(16) NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `transport_id`,
	CHANGE COLUMN `transport_x` `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_type`,
	CHANGE COLUMN `transport_y` `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	CHANGE COLUMN `transport_z` `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	CHANGE COLUMN `transport_o` `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `creature_create1_time`
	ADD COLUMN `vehicle_id` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	ADD COLUMN `vehicle_orientation` FLOAT NOT NULL DEFAULT '0' AFTER `vehicle_id`;

ALTER TABLE `creature_create2_time`
	CHANGE COLUMN `transport_guid` `transport_guid` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	CHANGE COLUMN `transport_id` `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	CHANGE COLUMN `transport_type` `transport_type` VARCHAR(16) NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `transport_id`,
	CHANGE COLUMN `transport_x` `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_type`,
	CHANGE COLUMN `transport_y` `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	CHANGE COLUMN `transport_z` `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	CHANGE COLUMN `transport_o` `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `creature_create2_time`
	ADD COLUMN `vehicle_id` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	ADD COLUMN `vehicle_orientation` FLOAT NOT NULL DEFAULT '0' AFTER `vehicle_id`;

ALTER TABLE `gameobject`
	ADD COLUMN `is_transport` TINYINT(1) UNSIGNED NOT NULL DEFAULT '0' AFTER `is_spawn`;

ALTER TABLE `creature`
	CHANGE COLUMN `is_pet` `is_pet` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' AFTER `is_hovering`,
	CHANGE COLUMN `is_vehicle` `is_vehicle` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' AFTER `is_pet`;

ALTER TABLE `creature`
	ADD COLUMN `original_id` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'entry from guid' AFTER `guid`,
	CHANGE COLUMN `id` `id` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'entry from update fields' AFTER `original_id`,
	DROP INDEX `idx_map`,
	DROP INDEX `idx_id`;

ALTER TABLE `gameobject`
	ADD COLUMN `original_id` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'entry from guid' AFTER `guid`,
	CHANGE COLUMN `id` `id` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'entry from update fields' AFTER `original_id`;

ALTER TABLE `creature_create1_time`
	ADD COLUMN `transport_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_time`;

ALTER TABLE `creature_create2_time`
	ADD COLUMN `transport_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_time`;

ALTER TABLE `player_create1_time`
	ADD COLUMN `transport_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_time`;

ALTER TABLE `player_create2_time`
	ADD COLUMN `transport_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_time`;

ALTER TABLE `creature_movement_server`
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_type`;

ALTER TABLE `creature_movement_server_combat`
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_type`;

ALTER TABLE `player_movement_server`
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_type`;

ALTER TABLE `creature_movement_client`
	CHANGE COLUMN `transport_guid` `transport_guid` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	CHANGE COLUMN `transport_id` `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	CHANGE COLUMN `transport_type` `transport_type` VARCHAR(16) NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `transport_id`,
	CHANGE COLUMN `transport_x` `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_type`,
	CHANGE COLUMN `transport_y` `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	CHANGE COLUMN `transport_z` `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	CHANGE COLUMN `transport_o` `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `creature_movement_client`
	ADD COLUMN `transport_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_time`;

ALTER TABLE `player_movement_client`
	CHANGE COLUMN `transport_guid` `transport_guid` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `spline_elevation`,
	CHANGE COLUMN `transport_id` `transport_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_guid`,
	CHANGE COLUMN `transport_type` `transport_type` VARCHAR(16) NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `transport_id`,
	CHANGE COLUMN `transport_x` `transport_x` FLOAT NOT NULL DEFAULT '0' AFTER `transport_type`,
	CHANGE COLUMN `transport_y` `transport_y` FLOAT NOT NULL DEFAULT '0' AFTER `transport_x`,
	CHANGE COLUMN `transport_z` `transport_z` FLOAT NOT NULL DEFAULT '0' AFTER `transport_y`,
	CHANGE COLUMN `transport_o` `transport_o` FLOAT NOT NULL DEFAULT '0' AFTER `transport_z`;

ALTER TABLE `player_movement_client`
	ADD COLUMN `transport_time` INT UNSIGNED NOT NULL DEFAULT '0' AFTER `transport_o`,
	ADD COLUMN `transport_seat` TINYINT NOT NULL DEFAULT '0' AFTER `transport_time`;

ALTER TABLE `gameobject`
	ADD COLUMN `anim_progress` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' AFTER `artkit`;

ALTER TABLE `gameobject_values_update`
	ADD COLUMN `anim_progress` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `artkit`;

ALTER TABLE `gameobject`
	CHANGE COLUMN `artkit` `art_kit` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' AFTER `type`;

ALTER TABLE `gameobject_values_update`
	CHANGE COLUMN `artkit` `art_kit` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `state`;

ALTER TABLE `creature_melee_damage`
	ADD COLUMN `level` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `entry`;

ALTER TABLE `creature`
	ADD COLUMN `anim_tier` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'from UNIT_FIELD_BYTES_1' AFTER `vis_flags`;

ALTER TABLE `creature_values_update`
	ADD COLUMN `anim_tier` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `vis_flags`;

ALTER TABLE `player`
	ADD COLUMN `anim_tier` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'from UNIT_FIELD_BYTES_1' AFTER `vis_flags`;

ALTER TABLE `player_values_update`
	ADD COLUMN `anim_tier` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `vis_flags`;
  
ALTER TABLE `creature_stats`
	ADD COLUMN `sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table' AFTER `negative_arcane_res`;

ALTER TABLE `creature_pet_actions`
	ADD COLUMN `sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table' AFTER `slot10`;

ALTER TABLE `creature_pet_cooldown`
	ADD COLUMN `sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table' AFTER `mod_rate`;

ALTER TABLE `creature_pet_remaining_cooldown`
	ADD COLUMN `sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table' AFTER `time_since_cast`;

ALTER TABLE `creature_loot`
	ADD COLUMN `sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table' AFTER `items_count`,
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `sniff_id`;

ALTER TABLE `gameobject_loot`
	ADD COLUMN `sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table' AFTER `items_count`,
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `sniff_id`;

CREATE TABLE `creature_armor` (
	`entry` INT UNSIGNED NOT NULL DEFAULT '0',
	`level` INT UNSIGNED NOT NULL DEFAULT '0',
	`hits_count` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`armor` INT UNSIGNED NOT NULL DEFAULT '0',
	`damage_reduction` FLOAT NOT NULL DEFAULT '0',
	`sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0'
)
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_armor`
	COMMENT='estimated armor of creatures from damage taken';

ALTER TABLE `spell_target_position`
	DROP COLUMN `effect_index`;

CREATE TABLE `spell_script_target` (
  `spell_id` int(10) unsigned NOT NULL,
  `target_type` varchar(16) COLLATE utf8_unicode_ci NOT NULL,
  `target_id` int(10) unsigned NOT NULL,
  `sniff_build` mediumint(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`spell_id`,`target_type`,`target_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;


ALTER TABLE `player`
	ADD COLUMN `skin` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `money`,
	ADD COLUMN `face` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `skin`,
	ADD COLUMN `hair_style` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `face`,
	ADD COLUMN `hair_color` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `hair_style`,
	ADD COLUMN `facial_hair` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `hair_color`,
	DROP COLUMN `player_bytes1`,
	DROP COLUMN `player_bytes2`;

CREATE TABLE `creature_spell_immunity` (
	`entry` INT UNSIGNED NOT NULL,
	`spell_id` INT UNSIGNED NOT NULL,
	`sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0',
	PRIMARY KEY (`entry`, `spell_id`, `sniff_id`, `sniff_build`)
)
COMMENT='spells that creatures were immune to\r\nfrom SMSG_SPELL_GO'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `spell_cast_go_target`
	ADD COLUMN `miss_reason` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `target_type`;

CREATE TABLE `creature_respawn_time` (
	`old_guid` INT UNSIGNED NOT NULL,
	`new_guid` INT UNSIGNED NOT NULL,
	`respawn_time` INT UNSIGNED NOT NULL COMMENT 'time in seconds',
	PRIMARY KEY (`old_guid`, `new_guid`)
)
COMMENT='stores the time in seconds between the death of one creature, and the spawn of another on the same position\r\nrespawn time is reduced dynamically if there are too many players in the same area, so beware of abnormally low values'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

CREATE TABLE `creature_kill_reputation` (
	`entry` INT UNSIGNED NOT NULL,
	`reputation_list_id` INT UNSIGNED NOT NULL,
	`old_standing` INT NOT NULL,
	`new_standing` INT NOT NULL,
	`player_level` TINYINT UNSIGNED NOT NULL,
	`player_race` TINYINT UNSIGNED NOT NULL,
	`sniff_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table',
	`sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0'
)
COMMENT='faction standing changes after a creature died\r\ncan be wrong if multiple creatures died at the same time'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `spell_unique_caster`
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `spell_id`;

CREATE TABLE `spell_aura_flags` (
	`spell_id` INT UNSIGNED NOT NULL,
	`flags` INT UNSIGNED NOT NULL,
	`sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0',
	PRIMARY KEY (`spell_id`, `flags`)
)
COMMENT='all aura flags seen for a given spell id'
COLLATE='utf8_unicode_ci'
ENGINE=InnoDB
;

ALTER TABLE `spell_target_position`
	CHANGE COLUMN `entry` `spell_id` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' FIRST,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`spell_id`, `map`);

CREATE TABLE `mail_template` (
  `entry` int(10) unsigned NOT NULL DEFAULT '0',
  `sniff_build` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `stationery_id` int(10) unsigned NOT NULL DEFAULT '0',
  `sender_id` int(10) unsigned NOT NULL DEFAULT '0',
  `sender_type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `money` bigint(20) unsigned NOT NULL DEFAULT '0',
  `items_count` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `subject` varchar(64) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `body` varchar(1024) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`entry`,`sniff_build`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='data for mail sent by creatures\r\nfrom SMSG_MAIL_LIST_RESULT';

CREATE TABLE `mail_template_item` (
  `entry` int(10) unsigned NOT NULL DEFAULT '0',
  `slot` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `item_id` int(10) unsigned NOT NULL DEFAULT '0',
  `count` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `sniff_build` mediumint(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`,`slot`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci COMMENT='items attached to mail templates\r\nfrom SMSG_MAIL_LIST_RESULT';

ALTER TABLE `mail_template`
	CHANGE COLUMN `subject` `subject` TEXT NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `items_count`,
	CHANGE COLUMN `body` `body` TEXT NOT NULL DEFAULT '' COLLATE 'utf8_unicode_ci' AFTER `subject`;

ALTER TABLE `creature_template_wdb`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' AFTER `entry`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`entry`, `sniff_build`);

ALTER TABLE `gameobject_template`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' AFTER `entry`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`entry`, `sniff_build`);

ALTER TABLE `npc_text`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' AFTER `entry`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`entry`, `sniff_build`);

ALTER TABLE `page_text`
	CHANGE COLUMN `sniff_build` `sniff_build` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' AFTER `entry`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`entry`, `sniff_build`);

ALTER TABLE `sniff_data`
	CHANGE COLUMN `object_type` `object_type` ENUM('None','Spell','Map','LFGDungeon','Battleground','Unit','GameObject','CreatureDifficulty','Item','Quest','Opcode','PageText','NpcText','BroadcastText','Gossip','Zone','Area','AreaTrigger','Phase','Player','Achievement','CreatureFamily','Criteria','Currency','Difficulty','Faction','MailTemplate','WorldState','Sound','Taxi') NOT NULL DEFAULT 'None' AFTER `sniff_id`;

CREATE TABLE `client_areatrigger_enter` (
	`unixtimems` BIGINT UNSIGNED NOT NULL COMMENT 'when the packet was sent',
	`areatrigger_id` INT UNSIGNED NOT NULL,
	PRIMARY KEY (`unixtimems`, `areatrigger_id`)
)
COMMENT='times when the client notified the server it has entered an areatrigger\'s confines\r\nfrom CMSG_AREA_TRIGGER'
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
;

CREATE TABLE `client_areatrigger_leave` (
	`unixtimems` BIGINT UNSIGNED NOT NULL COMMENT 'when the packet was sent',
	`areatrigger_id` INT UNSIGNED NOT NULL,
	PRIMARY KEY (`unixtimems`, `areatrigger_id`)
)
COMMENT='times when the client notified the server it has left an areatrigger\'s confines\r\nfrom CMSG_AREA_TRIGGER'
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
;

ALTER TABLE `creature_template`
	DROP COLUMN `ranged_attack_time`;

ALTER TABLE `creature`
	CHANGE COLUMN `is_hovering` `is_hovering` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'hover bit from create object' AFTER `is_spawn`,
	CHANGE COLUMN `is_pet` `is_pet` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'highguid is pet' AFTER `is_hovering`,
	CHANGE COLUMN `is_vehicle` `is_vehicle` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'highguid is vehicle' AFTER `is_pet`;

CREATE TABLE `player_crit_chance` (
	`race` TINYINT UNSIGNED NOT NULL DEFAULT '0',
	`class` TINYINT UNSIGNED NOT NULL DEFAULT '0',
	`level` TINYINT UNSIGNED NOT NULL DEFAULT '0',
	`agility` INT UNSIGNED NOT NULL DEFAULT '0',
	`crit_chance` FLOAT NOT NULL DEFAULT '0',
	`weapon_item_id` INT UNSIGNED NOT NULL DEFAULT '0',
	`weapon_skill_id` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`skill_current_value` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`skill_max_value` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`relevant_auras` VARCHAR(128) NOT NULL DEFAULT '' COMMENT 'auras of type 52'
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
;

ALTER TABLE `player_crit_chance`
	COMMENT='data about player melee crit chance, so that we can calculate the correct bonus to crit chance per agility in vanilla\r\nother expansions have this data in a dbc, because its shown on the user interface when you mouse over the agility stat';

CREATE TABLE `player_dodge_chance` (
	`race` TINYINT UNSIGNED NOT NULL DEFAULT '0',
	`class` TINYINT UNSIGNED NOT NULL DEFAULT '0',
	`level` TINYINT UNSIGNED NOT NULL DEFAULT '0',
	`agility` INT UNSIGNED NOT NULL DEFAULT '0',
	`dodge_chance` FLOAT NOT NULL DEFAULT '0',
	`defense_current_value` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`defense_max_value` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
	`relevant_auras` VARCHAR(128) NOT NULL DEFAULT '' COMMENT 'auras of type 49'
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
;

ALTER TABLE `player_dodge_chance`
	COMMENT='data about player dodge chance, so that we can calculate the correct bonus to dodge chance per agility in vanilla\r\nother expansions have this data in a dbc, because its shown on the user interface when you mouse over the agility stat';

ALTER TABLE `creature_questitem`
	ALTER `id` DROP DEFAULT;
ALTER TABLE `creature_questitem`
	CHANGE COLUMN `id` `idx` INT(10) UNSIGNED NOT NULL AFTER `entry`;

ALTER TABLE `creature_questitem`
	ALTER `idx` DROP DEFAULT;
ALTER TABLE `creature_questitem`
	CHANGE COLUMN `idx` `idx` TINYINT UNSIGNED NOT NULL AFTER `entry`;

ALTER TABLE `gameobject_questitem`
	CHANGE COLUMN `id` `idx` TINYINT UNSIGNED NOT NULL DEFAULT '0' AFTER `entry`;
  
ALTER TABLE `gameobject_questitem`
	ALTER `entry` DROP DEFAULT,
	ALTER `idx` DROP DEFAULT;
ALTER TABLE `gameobject_questitem`
	CHANGE COLUMN `entry` `entry` INT(10) UNSIGNED NOT NULL FIRST,
	CHANGE COLUMN `idx` `idx` TINYINT(3) UNSIGNED NOT NULL AFTER `entry`;

RENAME TABLE `creature_questitem` TO `creature_quest_item`;

RENAME TABLE `gameobject_questitem` TO `gameobject_quest_item`;

ALTER TABLE `creature_text_template`
	CHANGE COLUMN `group_id` `idx` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'counter of unique texts per creature id' AFTER `entry`,
	CHANGE COLUMN `chat_type` `chat_type` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'version specific chat type enum, not the same as values used in mangos' AFTER `text`,
	CHANGE COLUMN `language` `language` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'part of the packet, references Languages.dbc' AFTER `chat_type`,
	CHANGE COLUMN `emote` `emote` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'not part of the packet, emote seen close to when the chat packet was received, references Emotes.dbc' AFTER `language`,
	CHANGE COLUMN `sound` `sound` MEDIUMINT(8) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'not part of the packet, sound heard close to when the chat packet was received, references SoundEntries.dbc' AFTER `emote`,
	CHANGE COLUMN `broadcast_text_id` `broadcast_text_id` MEDIUMINT(6) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'not part of the packet, must be manually set' AFTER `sound`;

ALTER TABLE `creature_text`
	ALTER `group_id` DROP DEFAULT;
ALTER TABLE `creature_text`
	CHANGE COLUMN `group_id` `idx` INT(10) UNSIGNED NOT NULL COMMENT 'counter of unique texts per creature id' AFTER `entry`;

ALTER TABLE `creature_text`
	DROP COLUMN `health_percent`;

ALTER TABLE `creature_text_template`
	ADD COLUMN `health_percent` FLOAT NULL DEFAULT NULL COMMENT 'not part of the packet, the current health of the creature at the time the text was said' AFTER `broadcast_text_id`;

ALTER TABLE `gameobject`
	ADD COLUMN `is_on_transport` TINYINT(1) UNSIGNED NOT NULL DEFAULT '0' AFTER `is_transport`;

ALTER TABLE `creature`
	ADD COLUMN `is_on_transport` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' AFTER `is_vehicle`;

CREATE TABLE `gameobject_unique_anim` (
  `entry` int(10) unsigned NOT NULL,
  `anim_id` int(10) unsigned NOT NULL,
  `as_despawn` tinyint(3) unsigned DEFAULT NULL,
  `sniff_build` mediumint(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`,`anim_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='all unique custom anim ids used by a given gameobject id\r\nfrom SMSG_GAME_OBJECT_CUSTOM_ANIM';

ALTER TABLE `creature_faction`
	ADD COLUMN `sniff_build` MEDIUMINT UNSIGNED NOT NULL DEFAULT '0' AFTER `faction`;

RENAME TABLE `creature_faction` TO `creature_unique_faction`;

ALTER TABLE `creature_gossip`
	ALTER `entry` DROP DEFAULT,
	ALTER `gossip_menu_id` DROP DEFAULT,
	ALTER `is_default` DROP DEFAULT;

ALTER TABLE `creature_gossip`
	CHANGE COLUMN `entry` `entry` MEDIUMINT(8) UNSIGNED NOT NULL FIRST,
	CHANGE COLUMN `gossip_menu_id` `gossip_menu_id` MEDIUMINT(8) UNSIGNED NOT NULL AFTER `entry`,
	CHANGE COLUMN `is_default` `is_default` TINYINT(3) UNSIGNED NOT NULL AFTER `gossip_menu_id`;

RENAME TABLE `creature_gossip` TO `creature_unique_gossip`;

CREATE TABLE `creature_unique_emote` (
  `entry` int(10) unsigned NOT NULL,
  `emote_id` int(10) unsigned NOT NULL,
  `emote_name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `sniff_build` mediumint(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`,`emote_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT COMMENT='all unique emote ids played by a given creature id\r\nfrom SMSG_EMOTE';

ALTER TABLE `creature_text_template`
	COLLATE='latin1_general_ci';
  
ALTER TABLE `creature_text_template`
	CHANGE COLUMN `text` `text` LONGTEXT NULL COMMENT 'the actual text that was sent' COLLATE 'latin1_general_ci' AFTER `idx`;

ALTER TABLE `creature_text_template`
	CHANGE COLUMN `text` `text` VARCHAR(512) NOT NULL COMMENT 'the actual text that was sent' COLLATE 'latin1_general_ci' AFTER `idx`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`entry`, `text`);

ALTER TABLE `creature_text_template`
	CHANGE COLUMN `idx` `idx` TINYINT(3) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'counter of unique texts per creature id' AFTER `entry`,
	ADD UNIQUE INDEX `entry` (`entry`, `idx`);

ALTER TABLE `creature_text_template`
	ADD COLUMN `sniff_id_list` TEXT NOT NULL AFTER `comment`;

ALTER TABLE `creature_unique_emote`
	ADD COLUMN `sniff_id_list` TEXT NOT NULL COLLATE 'latin1_general_ci' AFTER `sniff_build`;

ALTER TABLE `creature_unique_emote`
	DROP COLUMN `sniff_build`;
  
ALTER TABLE `creature_unique_emote`
	COLLATE='latin1_general_ci',
	CHANGE COLUMN `emote_name` `emote_name` VARCHAR(50) NULL DEFAULT NULL COLLATE 'latin1_general_ci' AFTER `emote_id`;

ALTER TABLE `creature_unique_faction`
	ADD COLUMN `sniff_id_list` TEXT NOT NULL AFTER `sniff_build`;

ALTER TABLE `creature_unique_faction`
	DROP COLUMN `sniff_build`;

ALTER TABLE `creature_unique_faction`
	COLLATE='latin1_general_ci',
	CHANGE COLUMN `sniff_id_list` `sniff_id_list` TEXT NOT NULL COLLATE 'latin1_general_ci' AFTER `faction`;

ALTER TABLE `creature_unique_gossip`
	DROP COLUMN `sniff_build`;

ALTER TABLE `creature_unique_gossip`
	ADD COLUMN `sniff_id_list` TEXT NOT NULL AFTER `is_default`;

ALTER TABLE `creature_unique_gossip`
	COLLATE='latin1_general_ci',
	CHANGE COLUMN `sniff_id_list` `sniff_id_list` TEXT NOT NULL COLLATE 'latin1_general_ci' AFTER `is_default`;

ALTER TABLE `gameobject_unique_anim`
	COLLATE='utf8_unicode_ci',
	ADD COLUMN `sniff_id_list` TEXT NOT NULL COLLATE 'latin1_general_ci' AFTER `as_despawn`,
	DROP COLUMN `sniff_build`;

ALTER TABLE `gameobject_unique_anim`
	COLLATE='latin1_general_ci';

ALTER TABLE `broadcast_text`
	COLLATE='latin1_general_ci';

ALTER TABLE `broadcast_text`
	CHANGE COLUMN `male_text` `male_text` VARCHAR(1024) NOT NULL DEFAULT '' COLLATE 'latin1_general_ci' AFTER `entry`,
	CHANGE COLUMN `female_text` `female_text` VARCHAR(1024) NOT NULL DEFAULT '' COLLATE 'latin1_general_ci' AFTER `male_text`;

ALTER TABLE `creature_spell_immunity`
	DROP COLUMN `sniff_id`,
	DROP COLUMN `sniff_build`;

ALTER TABLE `creature_spell_immunity`
	ADD COLUMN `sniff_id_list` TEXT NOT NULL AFTER `spell_id`;

ALTER TABLE `spell_unique_caster`
	ADD COLUMN `sniff_id_list` TEXT NOT NULL AFTER `spell_id`,
	DROP COLUMN `sniff_build`;

ALTER TABLE `gameobject_loot_item`
	CHANGE COLUMN `loot_id` `loot_id` INT(10) UNSIGNED NOT NULL COMMENT 'references gameobject_loot' FIRST;

CREATE TABLE `item_loot` (
  `entry` int(10) unsigned NOT NULL COMMENT 'item template id',
  `loot_id` int(10) unsigned NOT NULL COMMENT 'counter',
  `money` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'copper',
  `items_count` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'number of items dropped',
  `sniff_id` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'points to sniff_file table',
  `sniff_build` mediumint(8) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`,`loot_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci ROW_FORMAT=COMPACT COMMENT='each row represents a separate loot instance';


CREATE TABLE `item_loot_item` (
  `loot_id` int(10) unsigned NOT NULL COMMENT 'references item_loot',
  `item_id` int(10) unsigned NOT NULL COMMENT 'item template id',
  `count` int(10) unsigned NOT NULL DEFAULT '1' COMMENT 'stack size'
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci ROW_FORMAT=COMPACT COMMENT='individual item that is part of a loot instance';

RENAME TABLE `player_crit_chance` TO `player_melee_crit_chance`;

CREATE TABLE `player_ranged_crit_chance` (
  `race` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `class` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `agility` int(10) unsigned NOT NULL DEFAULT '0',
  `crit_chance` float NOT NULL DEFAULT '0',
  `weapon_item_id` int(10) unsigned NOT NULL DEFAULT '0',
  `weapon_skill_id` smallint(5) unsigned NOT NULL DEFAULT '0',
  `skill_current_value` smallint(5) unsigned NOT NULL DEFAULT '0',
  `skill_max_value` smallint(5) unsigned NOT NULL DEFAULT '0',
  `relevant_auras` varchar(128) COLLATE latin1_general_ci NOT NULL DEFAULT '' COMMENT 'auras of type 52'
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci ROW_FORMAT=COMPACT COMMENT='data about player ranged crit chance, so that we can calculate the correct bonus to crit chance per agility in vanilla\r\nother expansions have this data in a dbc, because its shown on the user interface when you mouse over the agility stat';

CREATE TABLE `player_spell_crit_chance` (
  `race` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `class` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `intellect` int(10) unsigned NOT NULL DEFAULT '0',
  `crit_chance` float NOT NULL DEFAULT '0',
  `relevant_auras` varchar(128) COLLATE latin1_general_ci NOT NULL DEFAULT '' COMMENT 'auras of type 57 and 71'
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci ROW_FORMAT=COMPACT COMMENT='data about player spell crit chance, so that we can calculate the correct bonus to crit chance per intellect in vanilla\r\nother expansions have this data in a dbc, because its shown on the user interface when you mouse over the intellect stat';
