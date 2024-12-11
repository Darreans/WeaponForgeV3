using Stunlock.Core;

namespace WeaponForge
{
    public struct CreateLegendaryWeaponDebugEvent : IComponentData
    {
        public PrefabGUID WeaponPrefabGuid;
        public int Tier;
        public PrefabGUID InfuseSpellMod;
        public PrefabGUID StatMod1;
        public float StatMod1Power;
        public PrefabGUID StatMod2;
        public float StatMod2Power;
        public PrefabGUID StatMod3;
        public float StatMod3Power;
    }

    // Placeholder interface to prevent errors
    public interface IComponentData { }
}
