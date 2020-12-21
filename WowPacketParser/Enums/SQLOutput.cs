﻿namespace WowPacketParser.Enums
{
    public enum SQLOutput
    {
        // ReSharper disable InconsistentNaming
        ObjectNames,
        SniffData,
        SniffDataOpcodes,

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
        creature_speed_update,
        creature_text,
        creature_text_template,
        creature_values_update,
        dynamicobject,
        dynamicobject_create1_time,
        dynamicobject_create2_time,
        dynamicobject_destroy_time,
        gameobject,
        gameobject_create1_time,
        gameobject_create2_time,
        gameobject_custom_anim,
        gameobject_despawn_anim,
        gameobject_destroy_time,
        gameobject_text,
        gameobject_text_template,
        gameobject_values_update,
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
        player_speed_update,
        player_values_update,
        quest_complete_time,
        quest_fail_time,
        spell_cast_failed,
        spell_cast_go,
        spell_cast_start,
        spell_channel_start,
        spell_channel_update,
        weather_updates,
        world_state_init,
        world_state_update,
        world_text,

        characters,
        character_inventory,

        areatrigger_template,
        areatrigger_template_polygon_vertices,
        broadcast_text,
        broadcast_text_locale,
        conversation_actor_template,
        conversation_actors,
        conversation_line_template,
        conversation_template,
        creature_addon,
        creature_display_info_addon,
        creature_equip_template,
        creature_gossip,
        creature_loot,
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
        points_of_interest,
        quest_ender,
        quest_poi,
        quest_poi_points,
        quest_starter,
        quest_template,
        scenario_poi,
        scene_template,
        spell_areatrigger,
        spell_pet_action,
        spell_pet_cooldown,
        spell_target_position,
        trainer,
        vehicle_template_accessory,

        hotfix_data,
        hotfix_blob
        // ReSharper restore InconsistentNaming
    }
}
