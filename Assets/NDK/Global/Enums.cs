public class Enums
{
    public enum SPEC_DATA_TYPE
    {
        NONE = 0, STAGE_DATA, MONSTER_DATA, CHARACTER_DATA, PLAYER_SKILL_DATA, FEDERATION_SKILL_DATA, FEDERATION_DATA, PLAYER_DATA, ITEM_DATA,
        // None=0,CharacterSpecData, IngameLevelData, OutGameGradeData, StageInfo, AchievementInfo,EquipmentLevelData,
        // Skill,UpgradeSpecDataSO, SkinData,  
        
        EMax
    }
    
    public enum SCENE_TYPE
    {
        Unknown = -1,
        MainScene, LobbyScene, BattleScene, LoadingScene
    }
    
    public enum AUDIO_TYPE
    {
        NONE = -1,
        
        MASTER, SFX, BGM
    }

    public enum ELEMENT
    {
        NONE=0,Light,Water,Fire,Earth,Dark,
        EMax
    }
}
