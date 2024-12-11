using Stunlock.Core;
using ProjectM.Network;
using Unity.Entities;
using UnityEngine;
using ProjectM;

namespace WeaponForge
{
    public static class Helper
    {
        public static void CreateWeapon(
            User user,
            PrefabGUID weaponGUID,
            PrefabGUID infusionGUID = default,
            PrefabGUID statMod1Guid = default,
            float statMod1Power = 0f,
            PrefabGUID statMod2Guid = default,
            float statMod2Power = 0f,
            PrefabGUID statMod3Guid = default,
            float statMod3Power = 0f)
        {
            try
            {
                // Retrieve the EntityManager
                var entityManager = Server.EntityManager;

                // Retrieve the DebugEventsSystem
                var debugEventsSystem = Server.GetExistingSystemManaged<DebugEventsSystem>();
                if (debugEventsSystem == null)
                {
                    Debug.LogError("[WeaponForge] DebugEventsSystem not found.");
                    return;
                }
                Debug.Log("[WeaponForge] DebugEventsSystem retrieved.");

                // Prepare the CreateLegendaryWeaponDebugEvent
                var createWeaponEvent = new ProjectM.Network.CreateLegendaryWeaponDebugEvent
                {
                    WeaponPrefabGuid = weaponGUID,
                    Tier = 1, // Default tier (can be customized later)
                    InfuseSpellMod = infusionGUID, // Infusion modifier
                    StatMod1 = statMod1Guid,
                    StatMod1Power = statMod1Power,
                    StatMod2 = statMod2Guid,
                    StatMod2Power = statMod2Power,
                    StatMod3 = statMod3Guid,
                    StatMod3Power = statMod3Power
                };

                // Trigger the event via DebugEventsSystem
                debugEventsSystem.CreateLegendaryWeaponEvent(user.Index, ref createWeaponEvent);

                // Confirm the event
                Debug.Log($"[WeaponForge] Weapon creation event sent successfully for GUID: {weaponGUID} with modifiers.");
            }
            catch (System.Exception ex)
            {
                // Log errors for debugging
                Debug.LogError($"[WeaponForge] Failed to create weapon: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // Server World Accessor
        private static World? _serverWorld;
        public static World Server
        {
            get
            {
                if (_serverWorld == null)
                {
                    foreach (var world in World.s_AllWorlds)
                    {
                        if (world.Name == "Server")
                        {
                            _serverWorld = world;
                            break;
                        }
                    }

                    if (_serverWorld == null)
                    {
                        throw new System.Exception("[WeaponForge] Server world not found.");
                    }
                }
                return _serverWorld;
            }
        }
    }
}
