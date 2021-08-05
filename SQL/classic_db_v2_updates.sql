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
