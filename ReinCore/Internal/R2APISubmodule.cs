namespace ReinCore
{
    using System;

    [Flags]
    internal enum R2APISubmodule : UInt64
    {
        None = 0,
        AssetAPI = 1,
        DifficultyAPI = 2,
        DirectorAPI = 4,
        EffectAPI = 8,
        InventoryAPI = 16,
        ItemAPI = 32,
        ItemDropAPI = 64,
        LoadoutAPI = 128,
        LobbyConfigAPI = 256,
        ModListAPI = 512,
        OrbAPI = 1024,
        PlayerAPI = 2048,
        PrefabAPI = 4096,
        ResourcesAPI = 8192,
        SkillAPI = 16384,
        SkinAPI = 32768,
        SurvivorAPI = 65536,
        AssetPlus = 131072,
        EntityAPI = 262144,
    }
}
