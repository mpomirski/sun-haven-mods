using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Wish;
using ZeroFormatter;

namespace AutoPetter;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("CustomItems", "0.2.2")]
public class Plugin : BaseUnityPlugin {
    public static ManualLogSource _logger;
    private readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);

    private void Awake() {
        _logger = Logger;
        _harmony.PatchAll();
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} v{PluginInfo.PLUGIN_VERSION} is loaded!");
    }

    [HarmonyPatch]
    private class Patches {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuController), "PlayGame", new Type[] { })]
        public static void MainMenuControllerAwake() {
            try {
                _logger.LogInfo("Creating AutoPetters");
                ItemHandler.CreateAutoPetters();
            }
            catch (Exception e) {
                _logger.LogError(e);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(NPCManager), "UpdateAnimalDrops")]
        public static void NPCManagerUpdateAnimalDrops(NPCManager __instance) {
            var animals = __instance.animals;
            var autoPettersSceneIds = new HashSet<int>();
            try {
                foreach (KeyValuePair<short, Dictionary<KeyTuple<ushort, ushort, sbyte>, DecorationPositionData>>
                             decoration in SingletonBehaviour<GameSave>.Instance.CurrentWorld.Decorations) {
                    foreach (KeyValuePair<KeyTuple<ushort, ushort, sbyte>, DecorationPositionData> keyValuePair in
                             decoration.Value) {
                        DecorationPositionData decorationPositionData = keyValuePair.Value;
                        int id = keyValuePair.Value.id;

                        if (id is not (ItemHandler.AutoPetterId)) continue;
                        autoPettersSceneIds.Add(decorationPositionData.sceneID);
                    }
                }

                foreach (KeyValuePair<AnimalPositionData, Animal> animal in animals) {
                    if (autoPettersSceneIds.Contains(animal.Key.scene) && !animal.Key.hasPetted) {
                        AutoPetter.PetAnimal(animal.Value);
                    }
                }
            }
            catch (Exception e) {
                _logger.LogError(e);
            }
        }
    }
}