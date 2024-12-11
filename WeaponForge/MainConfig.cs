using BepInEx.Configuration;
using WeaponForge;

namespace WeaponForge
{
    public static class MainConfig
    {
        // Config entries
        private static ConfigEntry<bool> EnableCommand;
        private static ConfigEntry<bool> AdminOnlyCommand;
        private static ConfigEntry<int> RequiredItemGUID;
        private static ConfigEntry<int> RequiredItemAmount;
        private static ConfigEntry<string> ItemName;

        private static ConfigFile _configFile;

        /// <summary>
        /// Initialize the configuration settings for WeaponForge.
        /// </summary>
        /// <param name="config">The ConfigFile instance from BepInEx.</param>
        public static void SettingsInit(ConfigFile config)
        {
            _configFile = config; // Store a reference to the config file for reloading
            _configFile.SaveOnConfigSet = true; // Automatically save changes when settings are updated

            // Bind settings to the configuration file
            EnableCommand = _configFile.Bind(
                "WeaponForge",
                "EnableCommand",
                true,
                "Enable or disable the Weapon Forge command."
            );

            AdminOnlyCommand = _configFile.Bind(
                "WeaponForge",
                "AdminOnlyCommand",
                false,
                "Restrict the Weapon Forge command to admins only."
            );

            RequiredItemGUID = _configFile.Bind(
                "WeaponForge",
                "RequiredItemGUID",
                -598100816, // Example default GUID
                "The PrefabGUID of the item required to use the Weapon Forge command."
            );

            RequiredItemAmount = _configFile.Bind(
                "WeaponForge",
                "RequiredItemAmount",
                10,
                "The amount of the required item needed to forge a weapon."
            );

            ItemName = _configFile.Bind(
                "WeaponForge",
                "ItemName",
                "Thistle",
                "The name of the item required to forge a weapon."
            );

            // Apply the settings to the DB class
            ApplySettingsToDB();

            // Log initialization status
            Plugin.Logger?.LogInfo("[WF] Configuration initialized successfully.");
        }

        /// <summary>
        /// Updates the database with the current configuration settings.
        /// </summary>
        private static void ApplySettingsToDB()
        {
            DB.EnabledCommand = EnableCommand.Value;
            DB.AdminOnlyCommand = AdminOnlyCommand.Value;
            DB.RequiredItemGUID = RequiredItemGUID.Value;
            DB.RequiredItemAmount = RequiredItemAmount.Value;
            DB.ItemName = ItemName.Value;

            // Log the current settings for debugging
            Plugin.Logger?.LogInfo($"[WF] Current Settings: EnableCommand = {DB.EnabledCommand}, AdminOnlyCommand = {DB.AdminOnlyCommand}, RequiredItemGUID = {DB.RequiredItemGUID}, RequiredItemAmount = {DB.RequiredItemAmount}, ItemName = {DB.ItemName}");
        }

        /// <summary>
        /// Reloads the configuration file and rebinds the settings.
        /// </summary>
        public static void ReloadConfig()
        {
            if (_configFile == null)
            {
                Plugin.Logger?.LogError("[WF] Configuration file not found. Reload failed.");
                return;
            }

            try
            {
                Plugin.Logger?.LogInfo("[WF] Reloading configuration...");

                _configFile.Reload(); // Reload the configuration file
                _configFile.Save(); // Save any unsaved changes
                ApplySettingsToDB(); // Reapply the settings to the database

                Plugin.Logger?.LogInfo("[WF] Configuration reloaded successfully.");
            }
            catch (System.Exception ex)
            {
                Plugin.Logger?.LogError($"[WF] Failed to reload configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the command is enabled.
        /// </summary>
        /// <returns>True if the command is enabled, otherwise false.</returns>
        public static bool IsCommandEnabled() => EnableCommand.Value;

        /// <summary>
        /// Checks if the command is restricted to admins only.
        /// </summary>
        /// <returns>True if the command is admin-only, otherwise false.</returns>
        public static bool IsAdminOnly() => AdminOnlyCommand.Value;
    }
}
