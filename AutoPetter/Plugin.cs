using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Wish;

namespace AutoPetter;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("CustomItems", "0.2.2")]
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);
    private static ManualLogSource _logger;
        
    private void Awake()
    {
        _logger = Logger;
        _harmony.PatchAll();
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} v{PluginInfo.PLUGIN_VERSION} is loaded!");
    }
    
    [HarmonyPatch]
    private class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuController), "PlayGame", new Type[] { })]
        public static void MainMenuControllerAwake()
        {
            try
            {
                _logger.LogDebug("Patch MainMenuController.PlayGame");
            }
            catch (Exception e)
            {
                _logger.LogError(e);
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Placeable), "OnDisable")]
        public static bool PlaceableOnDisable(ref Placeable __instance)
        {
            try
            {
                if (!__instance.Player)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
            }
            
            return true;
        }
    }
}
