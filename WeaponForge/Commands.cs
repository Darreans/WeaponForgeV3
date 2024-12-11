using System;
using WeaponForge; // Ensure namespace matches where WeaponType is defined
using VampireCommandFramework;
using Stunlock.Core;
using UnityEngine;
using BloodyShop.Server.Core; // For inventory management
using ProjectM.Network;
using System.Collections.Generic;


namespace WeaponForge.Commands
{
    internal class ForgeCommands
    {
        [Command("weaponforge", shortHand: "wf", usage: ".wf spear blood 123", description: "Forge a weapon with infusion, modifiers, and a purchase system.", adminOnly: false)]
        public static void WeaponForgeCommand(ChatCommandContext ctx, string weaponType = "", string infusionType = "", string modifiers = "")
        {
            // Check if the command is enabled
            if (!MainConfig.IsCommandEnabled())
            {
                ctx.Reply("<color=green>[WF]</color> This command is currently disabled by the server.");
                return;
            }

            // Check if the command is restricted to admins and the user is not an admin
            if (MainConfig.IsAdminOnly() && !ctx.Event.User.IsAdmin)
            {
                ctx.Reply("<color=green>[WF]</color> This command is restricted to admins only.");
                return;
            }

            // If the user inputs "?" as the weapon type, show the list of modifiers
            if (weaponType == "?")
            {
                ShowStatModifiers(ctx);
                return;
            }

            // Validate the weapon type input (case-insensitive)
            if (string.IsNullOrWhiteSpace(weaponType) || !Enum.TryParse<WeaponType>(weaponType, true, out var weaponEnum))
            {
                ctx.Reply($"<color=green>[WF] Invalid weapon type: {Markup.Highlight(weaponType)}.</color>\nAvailable weapons: {Markup.Highlight(string.Join(", ", Enum.GetNames(typeof(WeaponType))))}");
                return;
            }

            // Capitalize weapon type for consistent output
            string formattedWeaponType = CapitalizeFirstLetter(weaponType);

            // Validate the infusion type (case-insensitive)
            if (string.IsNullOrWhiteSpace(infusionType) || !DB.InfusionMap.TryGetValue(infusionType.ToLower(), out var infusionGUID))
            {
                ctx.Reply($"<color=green>[WF] Invalid infusion type: {Markup.Highlight(infusionType)}.</color>\nAvailable infusions: {Markup.Highlight(string.Join(", ", DB.InfusionMap.Keys))}");
                return;
            }

            // Normalize modifiers to uppercase and parse
            modifiers = modifiers.ToUpper();
            var statModifiers = DB.ParseStatModifiers(modifiers);

            // Ensure there are exactly 3 modifiers
            if (statModifiers.Count != 3)
            {
                ctx.Reply($"<color=green>[WF] Invalid modifiers.</color> Provide exactly 3 valid modifiers as a string of characters (e.g., {Markup.Highlight("123")}).");
                return;
            }

            // Check for duplicate modifiers
            if (HasDuplicateModifiers(modifiers))
            {
                ctx.Reply("<color=green>[WF]</color> Duplicate modifiers are not allowed. Please use unique modifiers (e.g., '123').");
                return;
            }

            // Payment details fetched from DB
            var requiredItemGUID = new PrefabGUID(DB.RequiredItemGUID); // Use the GUID from the DB
            int requiredAmount = DB.RequiredItemAmount; // Fetch the required amount from the DB
            string itemName = DB.ItemName; // Fetch the item name from the DB

            // Get the character name
            var characterName = ctx.Event.User.CharacterName.ToString();

            // Check if the user has sufficient items in their inventory
            if (!InventorySystem.verifyHaveSuficientPrefabsInInventory(characterName, requiredItemGUID, requiredAmount))
            {
                ctx.Reply($"<color=green>[WF]</color> The forge requires {Markup.Highlight(requiredAmount)} {Markup.Highlight(itemName)} to create this weapon.");
                return;
            }

            // Attempt to remove the required items from the user's inventory
            if (!InventorySystem.getPrefabFromInventory(characterName, requiredItemGUID, requiredAmount))
            {
                ctx.Reply("<color=green>[WF]</color> Failed to remove the required items from your inventory.");
                return;
            }

            // Notify the user that the payment was successful
            ctx.Reply($"<color=green>[WF]</color> Payment successful. Creating your {Markup.Highlight(formattedWeaponType)}...");

            try
            {
                // Retrieve the weapon GUID
                var weaponGUID = DB.GetWeaponGUID(weaponEnum);
                if (weaponGUID == default)
                {
                    ctx.Reply($"<color=green>[WF]</color> Invalid weapon configuration for {Markup.Highlight(formattedWeaponType)}.");
                    return;
                }

                // Ensure we always have 3 stat modifiers
                var stat1 = statModifiers[0];
                var stat2 = statModifiers[1];
                var stat3 = statModifiers[2];

                // Call the Helper to create the weapon with modifiers and infusion
                Helper.CreateWeapon(
                    ctx.User,
                    weaponGUID,
                    infusionGUID,
                    stat1.Item1, stat1.Item2,
                    stat2.Item1, stat2.Item2,
                    stat3.Item1, stat3.Item2
                );

                // Notify the user of successful creation
                ctx.Reply($"<color=green>[WF]</color> {Markup.Highlight(formattedWeaponType)} successfully created with infusion: {Markup.Highlight(infusionType)} and modifiers: {Markup.Highlight(modifiers)}.");
            }
            catch (Exception ex)
            {
                // Notify the user of any errors
                ctx.Reply("<color=green>[WF]</color> An error occurred while attempting to create the weapon. Please check the server logs for details.");
                Debug.LogError($"<color=green>[WF] Error in WeaponForgeCommand: {ex.Message}\n{ex.StackTrace}</color>");
            }
        }

        private static void ShowStatModifiers(ChatCommandContext ctx)
        {
            var modifiersList = DB.GetModifiersList();
            ctx.Reply($"<color=green>[WF]</color> Available Stat Modifiers:\n{modifiersList}");
        }

        /// <summary>
        /// Checks if a string of modifiers contains duplicates.
        /// </summary>
        private static bool HasDuplicateModifiers(string modifiers)
        {
            var seen = new HashSet<char>();
            foreach (var ch in modifiers)
            {
                if (seen.Contains(ch)) return true;
                seen.Add(ch);
            }
            return false;
        }

        /// <summary>
        /// Capitalizes the first letter of a given string.
        /// </summary>
        private static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
