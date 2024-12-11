using Stunlock.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace WeaponForge
{
    public static class DB
    {
        // File locations for dynamic data
        private static readonly string FileDirectory = Path.Combine("BepInEx", "config", "WeaponForge");
        private static readonly string ModifiersFileName = "WeaponModifiers.json";
        private static readonly string ModifiersFilePath = Path.Combine(FileDirectory, ModifiersFileName);

        // Command settings (populated dynamically from MainConfig)
        public static bool EnabledCommand { get; set; } = true;
        public static bool AdminOnlyCommand { get; set; } = false;

        // Required item for crafting weapons
        public static int RequiredItemGUID { get; set; } = -598100816;
        public static string ItemName { get; set; } = "Thistle";
        public static int RequiredItemAmount { get; set; } = 10;

        // Infusion types mapped to their respective PrefabGUIDs
        public static readonly Dictionary<string, PrefabGUID> InfusionMap = new()
        {
            { "blood", new PrefabGUID(-634479113) },
            { "chaos", new PrefabGUID(-1102157891) },
            { "frost", new PrefabGUID(-1538516012) },
            { "illusion", new PrefabGUID(-1957977808) },
            { "storm", new PrefabGUID(-1099263242) },
            { "unholy", new PrefabGUID(-766734228) }
        };

        // Weapon types mapped to their respective PrefabGUIDs
        private static readonly Dictionary<WeaponType, PrefabGUID> WeaponMap = new()
        {
            { WeaponType.Spear, new PrefabGUID(-1931117134) },
            { WeaponType.Axe, new PrefabGUID(-102830349) },
            { WeaponType.Crossbow, new PrefabGUID(935392085) },
            { WeaponType.GreatSword, new PrefabGUID(-1173681254) },
            { WeaponType.Mace, new PrefabGUID(1994084762) },
            { WeaponType.Pistols, new PrefabGUID(-944318126) },
            { WeaponType.Reaper, new PrefabGUID(-105026635) },
            { WeaponType.Slashers, new PrefabGUID(821410795) },
            { WeaponType.Whip, new PrefabGUID(429323760) },
            { WeaponType.Sword, new PrefabGUID(195858450) },
            { WeaponType.Longbow, new PrefabGUID(1177453385) }
        };

        /// <summary>
        /// Retrieves the PrefabGUID for a given WeaponType.
        /// </summary>
        public static PrefabGUID GetWeaponGUID(WeaponType type)
        {
            return WeaponMap.TryGetValue(type, out var guid) ? guid : default;
        }

        // Stat modifiers mapped to their respective PrefabGUIDs, default powers, and descriptions
        private static readonly Dictionary<char, (PrefabGUID Guid, float Power, string Description)> DefaultModifiers = new()
        {
            { '1', (new PrefabGUID(-542568600), 10.0f, "Attack Speed") },
            { '2', (new PrefabGUID(303731846), 5.0f, "Damage Reduction") },
            { '3', (new PrefabGUID(1915954443), 50.0f, "Max Health") },
            { '4', (new PrefabGUID(-285192213), 5.0f, "Movement Speed") },
            { '5', (new PrefabGUID(-184681371), 15.0f, "Phys Crit Chance") },
            { '6', (new PrefabGUID(-1480767601), 20.0f, "Phys Crit Damage") },
            { '7', (new PrefabGUID(-1122907647), 10.0f, "Weapon Skill Cooldown Recovery Rate") },
            { '8', (new PrefabGUID(-1157374165), 25.0f, "Spell Crit Chance") },
            { '9', (new PrefabGUID(193642528), 30.0f, "Spell Crit Damage") },
            { 'A', (new PrefabGUID(-1639076208), 10.0f, "Spell Skill Cooldown Recovery Rate") },
            { 'B', (new PrefabGUID(1705753146), 25.0f, "Spell Power") },
            { 'C', (new PrefabGUID(-427223401), 5.0f, "Spell Leech") },
            { 'D', (new PrefabGUID(-1276596814), 20.0f, "Resource Yield") }
        };

        // Current stat modifiers
        public static Dictionary<char, (PrefabGUID Guid, float Power, string Description)> StatModifiers = new(DefaultModifiers);

        /// <summary>
        /// Parses a string of stat modifiers into a list of PrefabGUIDs and power values.
        /// </summary>
        public static List<(PrefabGUID Guid, float Power)> ParseStatModifiers(string input)
        {
            var result = new List<(PrefabGUID Guid, float Power)>();
            foreach (var ch in input)
            {
                if (StatModifiers.TryGetValue(ch, out var stat))
                {
                    result.Add((stat.Guid, stat.Power));
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieves a formatted list of available stat modifiers.
        /// </summary>
        public static string GetModifiersList()
        {
            var sb = new StringBuilder();
            foreach (var mod in StatModifiers)
            {
                sb.AppendLine($"{mod.Key}: {mod.Value.Description}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Loads modifiers from a JSON file or initializes defaults if the file is missing.
        /// </summary>
        public static void LoadModifiers()
        {
            if (!Directory.Exists(FileDirectory))
                Directory.CreateDirectory(FileDirectory);

            if (!File.Exists(ModifiersFilePath))
            {
                StatModifiers = new Dictionary<char, (PrefabGUID, float, string)>(DefaultModifiers);
                SaveModifiers();
            }
            else
            {
                var json = File.ReadAllText(ModifiersFilePath);
                StatModifiers = JsonSerializer.Deserialize<Dictionary<char, (PrefabGUID, float, string)>>(json);
            }
        }

        /// <summary>
        /// Saves the current modifiers to a JSON file.
        /// </summary>
        public static void SaveModifiers()
        {
            var json = JsonSerializer.Serialize(StatModifiers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ModifiersFilePath, json);
        }

        /// <summary>
        /// Reloads the configuration and modifiers dynamically.
        /// </summary>
        public static void ReloadData()
        {
            Plugin.Logger.LogInfo("[WF] Reloading configuration and modifiers...");
            MainConfig.ReloadConfig();
            LoadModifiers();
            Plugin.Logger.LogInfo("[WF] Reload complete.");
        }
    }

    /// <summary>
    /// Enum for weapon types.
    /// </summary>
    public enum WeaponType
    {
        Spear,
        Axe,
        Crossbow,
        GreatSword,
        Mace,
        Pistols,
        Reaper,
        Slashers,
        Whip,
        Sword,
        Longbow
    }
}
