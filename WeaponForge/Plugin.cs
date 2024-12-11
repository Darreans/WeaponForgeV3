using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using VampireCommandFramework;
using Bloodstone.API; // Required for listening to chat commands
using Bloodstone.Hooks;

namespace WeaponForge
{
    [BepInPlugin("weaponforge", "Weapon Forge", "1.0.0")]
    [BepInDependency("gg.deca.VampireCommandFramework")]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Logger;
        private Harmony _harmony;

        public override void Load()
        {
            Logger = Log;
            Logger.LogInfo("Weapon Forge Loaded!");

            // Initialize Harmony
            _harmony = new Harmony("weaponforge");
            _harmony.PatchAll();

            // Initialize configuration
            MainConfig.SettingsInit(Config);

            // Register all commands
            CommandRegistry.RegisterAll();

            // Hook into chat system to listen for !reload
            Chat.OnChatMessage += HandleReloadCommand;

            Logger.LogInfo("Commands registered successfully.");
        }

        public override bool Unload()
        {
            // Use UnpatchSelf instead of UnpatchAll
            _harmony?.UnpatchSelf();

            // Unregister commands and hooks when the plugin is unloaded
            CommandRegistry.UnregisterAssembly();
            Chat.OnChatMessage -= HandleReloadCommand;

            Logger.LogInfo("Weapon Forge unloaded successfully.");
            return true;
        }

        private void HandleReloadCommand(VChatEvent ev)
        {
            // Check if the message is "!reload" and the user is an admin
            if (ev.Message == "!reload" && ev.User.IsAdmin)
            {
                Logger.LogInfo("[WF] Reload command received.");

                try
                {
                    // Reload the configuration
                    MainConfig.ReloadConfig();

                    // Notify the admin that the reload was successful
                    ev.User.SendSystemMessage("<color=#00FF00>WeaponForge configuration reloaded successfully.</color>");
                    Logger.LogInfo("WeaponForge configuration reloaded via !reload command.");
                }
                catch (System.Exception ex)
                {
                    // Notify the admin about the error
                    ev.User.SendSystemMessage($"<color=#FF0000>Failed to reload configuration:</color> {ex.Message}");
                    Logger.LogError($"Error reloading configuration: {ex}");
                }
            }
        }
    }
}
