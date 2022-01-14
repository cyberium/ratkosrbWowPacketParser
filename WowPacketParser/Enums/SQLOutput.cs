﻿namespace WowPacketParser.Enums
{
    public enum SQLOutput
    {
        // ReSharper disable InconsistentNaming
        ObjectNames,
        SniffData,
        SniffDataOpcodes,

        cinematic_begin,
        cinematic_end,
        client_areatrigger_enter,
        client_areatrigger_leave,
        client_creature_interact,
        client_gameobject_use,
        client_item_use,
        client_quest_accept,
        client_quest_complete,
        client_reclaim_corpse,
        client_release_spirit,
        creature,
        creature_attack_log,
        creature_attack_start,
        creature_attack_stop,
        creature_auras_update,
        creature_create1_time,
        creature_create2_time,
        creature_destroy_time,
        creature_emote,
        creature_equipment_values_update,
        creature_guid_values,
        creature_guid_values_update,
        creature_movement_client,
        creature_movement_server,
        creature_movement_server_combat,
        creature_pet_name,
        creature_power_values,
        creature_power_values_update,
        creature_speed_update,
        creature_text,
        creature_text_template,
        creature_threat_clear,
        creature_threat_remove,
        creature_threat_update,
        creature_values_update,
        dynamicobject,
        dynamicobject_create1_time,
        dynamicobject_create2_time,
        dynamicobject_destroy_time,
        faction_standing_update,
        gameobject,
        gameobject_create1_time,
        gameobject_create2_time,
        gameobject_custom_anim,
        gameobject_despawn_anim,
        gameobject_destroy_time,
        gameobject_values_update,
        logout_time,
        play_music,
        play_sound,
        play_spell_visual_kit,
        player,
        player_active_player,
        player_attack_log,
        player_attack_start,
        player_attack_stop,
        player_auras_update,
        player_chat,
        player_create1_time,
        player_create2_time,
        player_destroy_time,
        player_emote,
        player_equipment_values_update,
        player_guid_values,
        player_guid_values_update,
        player_movement_client,
        player_movement_server,
        player_power_values,
        player_power_values_update,
        player_speed_update,
        player_values_update,
        quest_update_complete,
        quest_update_failed,
        spell_cast_failed,
        spell_cast_go,
        spell_cast_start,
        spell_channel_start,
        spell_channel_update,
        weather_updates,
        world_state_init,
        world_state_update,
        world_text,
        xp_gain_aborted,
        xp_gain_log,

        characters,
        character_inventory,
        character_reputation,
        character_skills,
        character_spell,
        guild,
        guild_rank,

        areatrigger_template,
        broadcast_text,
        broadcast_text_locale,
        conversation_actor_template,
        conversation_actors,
        conversation_line_template,
        conversation_template,
        creature_addon,
        creature_armor,
        creature_display_info_addon,
        creature_equip_template,
        creature_faction,
        creature_gossip,
        creature_kill_reputation,
        creature_loot,
        creature_melee_damage,
        creature_pet_actions,
        creature_pet_cooldown,
        creature_pet_remaining_cooldown,
        creature_respawn_time,
        creature_spell_immunity,
        creature_spell_timers,
        creature_stats,
        creature_template,
        creature_template_addon,
        creature_template_locale,
        creature_template_scaling,
        creature_template_wdb,
        gameobject_addon,
        gameobject_loot,
        gameobject_template,
        gameobject_template_addon,
        gossip_menu,
        gossip_menu_option,
        item_template,
        locales_quest,
        locales_quest_objectives,
        mail_template,
        npc_spellclick_spells,
        npc_text,
        npc_trainer,
        npc_vendor,
        page_text,
        page_text_locale,
        playerchoice,
        playerchoice_locale,
        playercreateinfo,
        playercreateinfo_action,
        player_classlevelstats,
        player_crit_chance,
        player_dodge_chance,
        player_levelstats,
        player_levelup_info,
        points_of_interest,
        quest_ender,
        quest_poi,
        quest_poi_points,
        quest_starter,
        quest_template,
        scenario_poi,
        scene_template,
        spell_areatrigger,
        spell_areatrigger_splines,
        spell_areatrigger_vertices,
        spell_aura_flags,
        spell_script_target,
        spell_target_position,
        spell_unique_caster,
        trainer,
        vehicle_template_accessory,

        hotfix_data,
        hotfix_blob
        // ReSharper restore InconsistentNaming
    }
}
