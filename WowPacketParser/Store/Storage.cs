using System;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Store.Objects;
using System.Linq;

namespace WowPacketParser.Store
{
    public static class Storage
    {
        // Stores opcodes read, npc/GOs/spell/item/etc IDs found in sniffs
        // and other miscellaneous stuff
        public static readonly DataBag<SniffData> SniffData = new DataBag<SniffData>(Settings.SqlTables.SniffData || Settings.SqlTables.SniffDataOpcodes);

        /* Key: Guid */

        // Units, GameObjects, Players, Items
        public static readonly StoreDictionary<WowGuid, WoWObject> Objects = new StoreDictionary<WowGuid, WoWObject>(new List<SQLOutput>());
        public static void StoreNewObject(WowGuid guid, WoWObject obj, Packet packet)
        {
            obj.OriginalMovement = obj.Movement != null ? obj.Movement.CopyFromMe() : null;
            obj.OriginalUpdateFields = obj.UpdateFields != null ? new Dictionary<int, UpdateField>(obj.UpdateFields) : null;
            if (!string.IsNullOrWhiteSpace(Settings.SQLFileName) && Settings.DumpFormatWithSQL())
                obj.SourceSniffId = Program.sniffFileNames.IndexOf(packet.FileName);
            Storage.Objects.Add(guid, obj, packet.TimeSpan);
        }

        public static readonly Dictionary<WowGuid, List<DateTime>> ObjectDestroyTimes = new Dictionary<WowGuid, List<DateTime>>();
        public static void StoreObjectDestroyTime(WowGuid guid, DateTime time)
        {
            if (guid.GetObjectType() != ObjectType.Unit &&
                guid.GetObjectType() != ObjectType.GameObject)
                return;

            if (guid.GetObjectType() == ObjectType.Unit && !Settings.SqlTables.creature_destroy_time)
                return;

            if (guid.GetObjectType() == ObjectType.GameObject && !Settings.SqlTables.gameobject_destroy_time)
                return;

            if (Storage.ObjectDestroyTimes.ContainsKey(guid))
            {
                Storage.ObjectDestroyTimes[guid].Add(time);
            }
            else
            {
                List<DateTime> timeList = new List<DateTime>();
                timeList.Add(time);
                Storage.ObjectDestroyTimes.Add(guid, timeList);
            }
        }
        public static readonly Dictionary<WowGuid, List<DateTime>> ObjectCreate1Times = new Dictionary<WowGuid, List<DateTime>>();
        public static void StoreObjectCreate1Time(WowGuid guid, DateTime time)
        {
            if (guid.GetObjectType() != ObjectType.Unit &&
                guid.GetObjectType() != ObjectType.GameObject)
                return;

            if (guid.GetObjectType() == ObjectType.Unit && !Settings.SqlTables.creature_create1_time)
                return;

            if (guid.GetObjectType() == ObjectType.GameObject && !Settings.SqlTables.gameobject_create1_time)
                return;

            if (Storage.ObjectCreate1Times.ContainsKey(guid))
            {
                Storage.ObjectCreate1Times[guid].Add(time);
            }
            else
            {
                List<DateTime> timeList = new List<DateTime>();
                timeList.Add(time);
                Storage.ObjectCreate1Times.Add(guid, timeList);
            }
        }
        public static readonly Dictionary<WowGuid, List<DateTime>> ObjectCreate2Times = new Dictionary<WowGuid, List<DateTime>>();
        public static void StoreObjectCreate2Time(WowGuid guid, DateTime time)
        {
            if (guid.GetObjectType() != ObjectType.Unit &&
                guid.GetObjectType() != ObjectType.GameObject)
                return;

            if (guid.GetObjectType() == ObjectType.Unit && !Settings.SqlTables.creature_create2_time)
                return;

            if (guid.GetObjectType() == ObjectType.GameObject && !Settings.SqlTables.gameobject_create2_time)
                return;

            if (Storage.ObjectCreate2Times.ContainsKey(guid))
            {
                Storage.ObjectCreate2Times[guid].Add(time);
            }
            else
            {
                List<DateTime> timeList = new List<DateTime>();
                timeList.Add(time);
                Storage.ObjectCreate2Times.Add(guid, timeList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureUpdate>> CreatureUpdates = new Dictionary<WowGuid, List<CreatureUpdate>>();
        public static void StoreCreatureUpdate(WowGuid guid, CreatureUpdate update)
        {
            if (!Settings.SqlTables.creature_update)
                return;

            if (guid.GetObjectType() != ObjectType.Unit ||
                guid.GetHighType() == HighGuidType.Pet)
                return;

            if (Storage.CreatureUpdates.ContainsKey(guid))
            {
                Storage.CreatureUpdates[guid].Add(update);
            }
            else
            {
                List<CreatureUpdate> updateList = new List<CreatureUpdate>();
                updateList.Add(update);
                Storage.CreatureUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<GameObjectUpdate>> GameObjectUpdates = new Dictionary<WowGuid, List<GameObjectUpdate>>();
        public static void StoreGameObjectUpdate(WowGuid guid, GameObjectUpdate update)
        {
            if (!Settings.SqlTables.gameobject_update)
                return;

            if (guid.GetObjectType() != ObjectType.GameObject)
                return;

            if (Storage.GameObjectUpdates.ContainsKey(guid))
            {
                Storage.GameObjectUpdates[guid].Add(update);
            }
            else
            {
                List<GameObjectUpdate> updateList = new List<GameObjectUpdate>();
                updateList.Add(update);
                Storage.GameObjectUpdates.Add(guid, updateList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureEmote>> Emotes = new Dictionary<WowGuid, List<CreatureEmote>>();
        public static void StoreCreatureEmote(WowGuid guid, EmoteType emote, DateTime time)
        {
            if (!Settings.SqlTables.creature_emote)
                return;

            if (Storage.Emotes.ContainsKey(guid))
            {
                Storage.Emotes[guid].Add(new CreatureEmote(emote, time));
            }
            else
            {
                List<CreatureEmote> emotesList = new List<CreatureEmote>();
                emotesList.Add(new CreatureEmote(emote, time));
                Storage.Emotes.Add(guid, emotesList);
            }
        }
        public static readonly Dictionary<WowGuid, List<CreatureAttackData>> CreatureAttackStartTimes = new Dictionary<WowGuid, List<CreatureAttackData>>();
        public static readonly Dictionary<WowGuid, List<CreatureAttackData>> CreatureAttackStopTimes = new Dictionary<WowGuid, List<CreatureAttackData>>();
        public static void StoreCreatureAttack(WowGuid attackerGuid, WowGuid victimGuid, DateTime time, bool start)
        {
            Dictionary<WowGuid, List<CreatureAttackData>> store = null;
            if (start)
            {
                if (!Settings.SqlTables.creature_attack_start)
                    return;

                store = CreatureAttackStartTimes;
            }
            else
            {
                if (!Settings.SqlTables.creature_attack_stop)
                    return;

                store = CreatureAttackStopTimes;
            }

            if (store.ContainsKey(attackerGuid))
            {
                store[attackerGuid].Add(new CreatureAttackData(victimGuid, time));
            }
            else
            {
                List<CreatureAttackData> attackList = new List<CreatureAttackData>();
                attackList.Add(new CreatureAttackData(victimGuid, time));
                store.Add(attackerGuid, attackList);
            }
        }
        /* Key: Entry */

        // Templates
        public static readonly DataBag<AreaTriggerTemplate> AreaTriggerTemplates = new DataBag<AreaTriggerTemplate>(Settings.SqlTables.areatrigger_template );
        public static readonly DataBag<AreaTriggerTemplateVertices> AreaTriggerTemplatesVertices = new DataBag<AreaTriggerTemplateVertices>(Settings.SqlTables.areatrigger_template_polygon_vertices);
        public static readonly DataBag<ConversationActor> ConversationActors = new DataBag<ConversationActor>(Settings.SqlTables.conversation_actors);
        public static readonly DataBag<ConversationActorTemplate> ConversationActorTemplates = new DataBag<ConversationActorTemplate>(Settings.SqlTables.conversation_actor_template);
        public static readonly DataBag<ConversationLineTemplate> ConversationLineTemplates = new DataBag<ConversationLineTemplate>(Settings.SqlTables.conversation_line_template);
        public static readonly DataBag<ConversationTemplate> ConversationTemplates = new DataBag<ConversationTemplate>(Settings.SqlTables.conversation_template);
        public static readonly DataBag<GameObjectTemplate> GameObjectTemplates = new DataBag<GameObjectTemplate>(Settings.SqlTables.gameobject_template);
        public static readonly DataBag<GameObjectTemplateQuestItem> GameObjectTemplateQuestItems = new DataBag<GameObjectTemplateQuestItem>(Settings.SqlTables.gameobject_template);
        public static readonly DataBag<ItemTemplate> ItemTemplates = new DataBag<ItemTemplate>(Settings.SqlTables.item_template);
        public static readonly DataBag<QuestTemplate> QuestTemplates = new DataBag<QuestTemplate>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestObjective> QuestObjectives = new DataBag<QuestObjective>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestVisualEffect> QuestVisualEffects = new DataBag<QuestVisualEffect>(Settings.SqlTables.quest_template);
        public static readonly DataBag<CreatureTemplate> CreatureTemplates = new DataBag<CreatureTemplate>(Settings.SqlTables.creature_template_wdb);
        public static readonly DataBag<CreatureTemplateClassic> CreatureTemplatesClassic = new DataBag<CreatureTemplateClassic>(Settings.SqlTables.creature_template_wdb);
        public static readonly DataBag<CreatureTemplateNonWDB> CreatureTemplatesNonWDB = new DataBag<CreatureTemplateNonWDB>(Settings.SqlTables.creature_template);
        public static readonly DataBag<CreatureTemplateQuestItem> CreatureTemplateQuestItems = new DataBag<CreatureTemplateQuestItem>(Settings.SqlTables.creature_template_wdb);
        public static readonly DataBag<CreatureTemplateScaling> CreatureTemplateScalings = new DataBag<CreatureTemplateScaling>(Settings.SqlTables.creature_template_scaling);
        public static readonly DataBag<CreatureTemplateModel> CreatureTemplateModels = new DataBag<CreatureTemplateModel>(Settings.SqlTables.creature_template);
        public static readonly DataBag<CreatureStats> CreatureStats = new DataBag<CreatureStats>(Settings.SqlTables.creature_stats);

        // Vendor & trainer
        public static readonly DataBag<NpcTrainer> NpcTrainers = new DataBag<NpcTrainer>(Settings.SqlTables.npc_trainer); // legacy 3.3.5 support
        public static readonly DataBag<NpcVendor> NpcVendors = new DataBag<NpcVendor>(Settings.SqlTables.npc_vendor);
        public static readonly DataBag<Trainer> Trainers = new DataBag<Trainer>(Settings.SqlTables.trainer);
        public static readonly DataBag<TrainerSpell> TrainerSpells = new DataBag<TrainerSpell>(Settings.SqlTables.trainer);
        public static readonly DataBag<CreatureDefaultTrainer> CreatureDefaultTrainers = new DataBag<CreatureDefaultTrainer>(Settings.SqlTables.trainer);

        // Loot
        public static readonly Dictionary<uint, Dictionary<WowGuid, LootEntry>> CreatureLoot = new Dictionary<uint, Dictionary<WowGuid, LootEntry>>();
        public static readonly Dictionary<uint, Dictionary<WowGuid, LootEntry>> GameObjectLoot = new Dictionary<uint, Dictionary<WowGuid, LootEntry>>();
        public static void StoreLoot(LootEntry loot, WowGuid objectGuid, WowGuid lootGuid)
        {
            Dictionary<uint, Dictionary<WowGuid, LootEntry>> lootStorage = null;
            if (objectGuid.GetObjectType() == ObjectType.Unit)
            {
                if (!Settings.SqlTables.creature_loot)
                    return;

                lootStorage = CreatureLoot;
            }
            else if (objectGuid.GetObjectType() == ObjectType.GameObject)
            {
                if (!Settings.SqlTables.gameobject_loot)
                    return;

                lootStorage = GameObjectLoot;
            }
            if (lootStorage == null)
                return;

            if (lootStorage.ContainsKey(loot.Entry))
            {
                if (lootStorage[loot.Entry].ContainsKey(lootGuid))
                    return;

                loot.LootId = LootEntry.LootIdCounter++;
                foreach (LootItem item in loot.ItemsList)
                    item.LootId = loot.LootId;

                lootStorage[loot.Entry].Add(lootGuid, loot);
            }
            else
            {
                loot.LootId = LootEntry.LootIdCounter++;
                foreach (LootItem item in loot.ItemsList)
                    item.LootId = loot.LootId;

                Dictionary<WowGuid, LootEntry> dict = new Dictionary<WowGuid, LootEntry>();
                dict.Add(lootGuid, loot);
                lootStorage.Add(loot.Entry, dict);
            }
        }

        // Page & npc text
        public static readonly DataBag<PageText> PageTexts = new DataBag<PageText>(Settings.SqlTables.page_text);
        public static readonly DataBag<NpcText> NpcTexts = new DataBag<NpcText>(Settings.SqlTables.npc_text);
        public static readonly DataBag<NpcTextMop> NpcTextsMop = new DataBag<NpcTextMop>(Settings.SqlTables.npc_text);

        // Creature text (says, yells, etc.)
        public static readonly DataBag<CreatureText> CreatureTexts = new DataBag<CreatureText>(Settings.SqlTables.creature_text);
        public static readonly StoreMulti<uint, CreatureTextTemplate> CreatureTextTemplates = new StoreMulti<uint, CreatureTextTemplate>(Settings.SqlTables.creature_text_template);

        // Points of Interest
        public static readonly DataBag<PointsOfInterest> GossipPOIs = new DataBag<PointsOfInterest>(Settings.SqlTables.points_of_interest);

        // "Helper" stores, do not match a specific table
        public static readonly List<ObjectSound> Sounds = new List<ObjectSound>();
        public static readonly DataBag<PlayMusic> Music = new DataBag<PlayMusic>(Settings.SqlTables.play_music);
        public static readonly StoreDictionary<uint, List<uint?>> SpellsX = new StoreDictionary<uint, List<uint?>>(Settings.SqlTables.creature_template); // `creature_template`.`spellsX`
        public static readonly DataBag<QuestOfferReward> QuestOfferRewards = new DataBag<QuestOfferReward>(Settings.SqlTables.quest_template);
        public static readonly StoreDictionary<Tuple<uint, uint>, object> GossipSelects = new StoreDictionary<Tuple<uint, uint>, object>(Settings.SqlTables.points_of_interest || Settings.SqlTables.gossip_menu || Settings.SqlTables.gossip_menu_option);

        /* Key: Misc */

        // Start info (Race, Class)
        public static readonly DataBag<PlayerCreateInfoAction> StartActions = new DataBag<PlayerCreateInfoAction>(Settings.SqlTables.playercreateinfo_action);
        public static readonly DataBag<PlayerCreateInfo> StartPositions = new DataBag<PlayerCreateInfo>(Settings.SqlTables.playercreateinfo);

        // Gossips (MenuId, TextId)
        public static readonly Dictionary<uint, uint> CreatureDefaultGossips = new Dictionary<uint, uint>();
        public static readonly DataBag<CreatureGossip> CreatureGossips = new DataBag<CreatureGossip>(Settings.SqlTables.creature_gossip);
        public static readonly DataBag<GossipMenu> Gossips = new DataBag<GossipMenu>(Settings.SqlTables.gossip_menu);
        public static readonly DataBag<GossipMenuOption> GossipMenuOptions = new DataBag<GossipMenuOption>(Settings.SqlTables.gossip_menu_option);
        public static readonly DataBag<GossipMenuOptionAction> GossipMenuOptionActions = new DataBag<GossipMenuOptionAction>(Settings.SqlTables.gossip_menu_option);
        public static readonly DataBag<GossipMenuOptionBox> GossipMenuOptionBoxes = new DataBag<GossipMenuOptionBox>(Settings.SqlTables.gossip_menu_option);
        public static readonly DataBag<GossipMenuOptionTrainer> GossipMenuOptionTrainers = new DataBag<GossipMenuOptionTrainer>(Settings.SqlTables.gossip_menu_option);

        // Quest POI (QuestId, Id)
        public static readonly DataBag<QuestPOI> QuestPOIs = new DataBag<QuestPOI>(Settings.SqlTables.quest_poi_points);
        public static readonly DataBag<QuestPOIPoint> QuestPOIPoints = new DataBag<QuestPOIPoint>(Settings.SqlTables.quest_poi_points); // WoD

        // Quest Misc
        public static readonly DataBag<QuestStarter> QuestStarters = new DataBag<QuestStarter>(Settings.SqlTables.quest_starter);
        public static readonly DataBag<QuestEnder> QuestEnders = new DataBag<QuestEnder>(Settings.SqlTables.quest_ender);
        public static readonly DataBag<QuestClientAccept> QuestClientAcceptTimes = new DataBag<QuestClientAccept>(Settings.SqlTables.quest_client_accept);
        public static readonly DataBag<QuestClientComplete> QuestClientCompleteTimes = new DataBag<QuestClientComplete>(Settings.SqlTables.quest_client_complete);
        public static readonly DataBag<QuestGreeting> QuestGreetings = new DataBag<QuestGreeting>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestDetails> QuestDetails = new DataBag<QuestDetails>(Settings.SqlTables.quest_template);
        public static readonly DataBag<QuestRequestItems> QuestRequestItems = new DataBag<QuestRequestItems>(Settings.SqlTables.quest_template);

        // Names
        public static readonly DataBag<ObjectName> ObjectNames = new DataBag<ObjectName>();

        // Vehicle Template Accessory
        public static readonly DataBag<VehicleTemplateAccessory> VehicleTemplateAccessories = new DataBag<VehicleTemplateAccessory>(Settings.SqlTables.vehicle_template_accessory);

        // Weather updates
        public static readonly DataBag<WeatherUpdate> WeatherUpdates = new DataBag<WeatherUpdate>(Settings.SqlTables.weather_updates);

        // Npc Spell Click
        public static readonly StoreBag<WowGuid> NpcSpellClicks = new StoreBag<WowGuid>(Settings.SqlTables.npc_spellclick_spells);
        public static readonly DataBag<NpcSpellClick> SpellClicks = new DataBag<NpcSpellClick>(Settings.SqlTables.npc_spellclick_spells);

        // Locales
        public static readonly DataBag<CreatureTemplateLocale> LocalesCreatures = new DataBag<CreatureTemplateLocale>(Settings.SqlTables.creature_template_locale);
        public static readonly DataBag<LocalesQuest> LocalesQuests = new DataBag<LocalesQuest>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<QuestObjectivesLocale> LocalesQuestObjectives = new DataBag<QuestObjectivesLocale>(Settings.SqlTables.locales_quest_objectives);
        public static readonly DataBag<QuestOfferRewardLocale> LocalesQuestOfferRewards = new DataBag<QuestOfferRewardLocale>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<QuestGreetingLocale> LocalesQuestGreeting = new DataBag<QuestGreetingLocale>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<QuestRequestItemsLocale> LocalesQuestRequestItems = new DataBag<QuestRequestItemsLocale>(Settings.SqlTables.locales_quest);
        public static readonly DataBag<PageTextLocale> LocalesPageText = new DataBag<PageTextLocale>(Settings.SqlTables.page_text_locale);

        // Spell Casts
        public static readonly DataBag<SpellCastData> SpellCastStart = new DataBag<SpellCastData>(Settings.SqlTables.spell_cast_start);
        public static readonly DataBag<SpellCastData> SpellCastGo = new DataBag<SpellCastData>(Settings.SqlTables.spell_cast_go);
        public static void AddSpellCastDataIfShould(SpellCastData castData, DataBag<SpellCastData> storage, Packet packet)
        {
            if (!Settings.SqlTables.spell_cast_start &&
                !Settings.SqlTables.spell_cast_go)
                return;

            if (!castData.CasterType.Contains("Unit") &&
                !castData.CasterType.Contains("Creature") &&
                !castData.CasterType.Contains("GameObject"))
                return;

            if (castData.MainTargetID != 0 &&
                castData.MainTargetType.Contains("Player"))
                castData.MainTargetID = 0;

            for (uint i = 0; i < SpellCastData.MAX_SPELL_HIT_TARGETS_DB; i++)
            {
                if (castData.HitTargetID[i] != 0 &&
                    castData.HitTargetType[i].Contains("Player"))
                    castData.HitTargetID[i] = 0;
            }

            castData.UnixTime = (uint)Utilities.GetUnixTimeFromDateTime(packet.Time);

            /*
            uncomment for unique casts only
            foreach (var cast_pair in storage)
            {
                if (cast_pair.Item1.CasterID == castData.CasterID &&
                    cast_pair.Item1.CasterType == castData.CasterType &&
                    cast_pair.Item1.CastFlags == castData.CastFlags &&
                    cast_pair.Item1.CastFlagsEx == castData.CastFlagsEx &&
                    cast_pair.Item1.SpellID == castData.SpellID &&
                    cast_pair.Item1.MainTargetID == castData.MainTargetID &&
                    cast_pair.Item1.MainTargetType == castData.MainTargetType &&
                    cast_pair.Item1.HitTargetID.SequenceEqual(castData.HitTargetID) &&
                    cast_pair.Item1.HitTargetType.SequenceEqual(castData.HitTargetType))
                    return;
            }
            */

            storage.Add(castData, packet.TimeSpan);
        }
        public static readonly DataBag<SpellPetCooldown> SpellPetCooldown = new DataBag<SpellPetCooldown>(Settings.SqlTables.spell_pet_cooldown);
        public static readonly DataBag<SpellPetActions> SpellPetActions = new DataBag<SpellPetActions>(Settings.SqlTables.spell_pet_action);
        public static readonly DataBag<SpellTargetPosition> SpellTargetPositions = new DataBag<SpellTargetPosition>(Settings.SqlTables.spell_target_position);

        public static readonly DataBag<HotfixData> HotfixDatas = new DataBag<HotfixData>(Settings.SqlTables.hotfix_data);
        public static readonly DataBag<HotfixBlob> HotfixBlobs = new DataBag<HotfixBlob>(Settings.SqlTables.hotfix_blob);
        // Scenes
        public static readonly DataBag<SceneTemplate> Scenes = new DataBag<SceneTemplate>(Settings.SqlTables.scene_template);

        // Scenario
        public static readonly DataBag<ScenarioPOI> ScenarioPOIs = new DataBag<ScenarioPOI>(Settings.SqlTables.scenario_poi);
        public static readonly DataBag<ScenarioPOIPoint> ScenarioPOIPoints = new DataBag<ScenarioPOIPoint>(Settings.SqlTables.scenario_poi);

        public static readonly DataBag<BroadcastText> BroadcastTexts = new DataBag<BroadcastText>(Settings.SqlTables.broadcast_text);
        public static readonly DataBag<BroadcastTextLocale> BroadcastTextLocales = new DataBag<BroadcastTextLocale>(Settings.SqlTables.broadcast_text_locale);

        //Player Choice
        public static readonly DataBag<PlayerChoiceTemplate> PlayerChoices = new DataBag<PlayerChoiceTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceLocaleTemplate> PlayerChoiceLocales = new DataBag<PlayerChoiceLocaleTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseTemplate> PlayerChoiceResponses = new DataBag<PlayerChoiceResponseTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseLocaleTemplate> PlayerChoiceResponseLocales = new DataBag<PlayerChoiceResponseLocaleTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardTemplate> PlayerChoiceResponseRewards = new DataBag<PlayerChoiceResponseRewardTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardCurrencyTemplate> PlayerChoiceResponseRewardCurrencies = new DataBag<PlayerChoiceResponseRewardCurrencyTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardFactionTemplate> PlayerChoiceResponseRewardFactions = new DataBag<PlayerChoiceResponseRewardFactionTemplate>(Settings.SqlTables.playerchoice);
        public static readonly DataBag<PlayerChoiceResponseRewardItemTemplate> PlayerChoiceResponseRewardItems = new DataBag<PlayerChoiceResponseRewardItemTemplate>(Settings.SqlTables.playerchoice);

        public static void ClearContainers()
        {
            SniffData.Clear();

            Objects.Clear();
            ObjectDestroyTimes.Clear();
            ObjectCreate1Times.Clear();
            ObjectCreate2Times.Clear();

            AreaTriggerTemplates.Clear();
            AreaTriggerTemplatesVertices.Clear();

            ConversationActors.Clear();
            ConversationActorTemplates.Clear();
            ConversationLineTemplates.Clear();
            ConversationTemplates.Clear();

            GameObjectLoot.Clear();
            GameObjectTemplates.Clear();
            GameObjectTemplateQuestItems.Clear();
            GameObjectUpdates.Clear();

            ItemTemplates.Clear();

            QuestTemplates.Clear();
            QuestObjectives.Clear();
            QuestVisualEffects.Clear();

            CreatureLoot.Clear();
            CreatureStats.Clear();
            CreatureTemplates.Clear();
            CreatureTemplatesClassic.Clear();
            CreatureTemplatesNonWDB.Clear();
            CreatureTemplateQuestItems.Clear();
            CreatureTemplateScalings.Clear();
            CreatureTemplateModels.Clear();
            CreatureUpdates.Clear();

            NpcTrainers.Clear();
            NpcVendors.Clear();
            Trainers.Clear();
            TrainerSpells.Clear();
            CreatureDefaultTrainers.Clear();

            PageTexts.Clear();
            NpcTexts.Clear();
            NpcTextsMop.Clear();

            CreatureTexts.Clear();
            CreatureTextTemplates.Clear();

            GossipPOIs.Clear();

            Emotes.Clear();
            Sounds.Clear();
            SpellsX.Clear();
            QuestOfferRewards.Clear();
            GossipSelects.Clear();

            StartActions.Clear();
            StartPositions.Clear();

            CreatureDefaultGossips.Clear();
            CreatureGossips.Clear();
            Gossips.Clear();
            GossipMenuOptions.Clear();
            GossipMenuOptionActions.Clear();
            GossipMenuOptionBoxes.Clear();
            GossipMenuOptionTrainers.Clear();

            QuestPOIs.Clear();
            QuestPOIPoints.Clear();

            QuestStarters.Clear();
            QuestEnders.Clear();
            QuestClientAcceptTimes.Clear();
            QuestClientCompleteTimes.Clear();
            QuestGreetings.Clear();
            QuestDetails.Clear();
            QuestRequestItems.Clear();

            ObjectNames.Clear();

            VehicleTemplateAccessories.Clear();

            WeatherUpdates.Clear();

            NpcSpellClicks.Clear();
            SpellClicks.Clear();

            SpellCastStart.Clear();
            SpellCastGo.Clear();
            SpellPetActions.Clear();
            SpellPetCooldown.Clear();
            SpellTargetPositions.Clear();

            LocalesCreatures.Clear();
            LocalesQuests.Clear();
            LocalesQuestObjectives.Clear();
            LocalesQuestOfferRewards.Clear();
            LocalesQuestGreeting.Clear();
            LocalesQuestRequestItems.Clear();
            LocalesPageText.Clear();

            HotfixDatas.Clear();

            Scenes.Clear();

            ScenarioPOIs.Clear();
            ScenarioPOIPoints.Clear();

            BroadcastTexts.Clear();
            BroadcastTextLocales.Clear();
        }
    }
}
