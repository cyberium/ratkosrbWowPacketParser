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

