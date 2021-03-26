namespace ReinCore
{
    using System;

    using RoR2;
    using RoR2.Achievements;

    using UnityEngine;

    public interface IModdedUnlockableDataProvider
    {
        String achievementIdentifier { get; }
        String unlockableIdentifier { get; }
        String prerequisiteUnlockableIdentifier { get; }
        String achievementNameToken { get; }
        String achievementDescToken { get; }
        String unlockableNameToken { get; }
        String spritePath { get; }
    }
    public interface IAchievementSpriteProvider
    {
        String pathString { get; }
        Sprite sprite { get; }
    }

    public readonly struct VanillaSpriteProvider : IAchievementSpriteProvider
    {
        public VanillaSpriteProvider( String path )
        {
            this.pathString = path;
            this.sprite = UnityEngine.Resources.Load<Sprite>( path );
        }

        public String pathString { get; }
        public Sprite sprite { get; }
    }

    internal static class ModdedUnlockableDataProviderExtensions
    {
        internal static String GetHowToUnlock<TDataProvider>( this TDataProvider self )
            where TDataProvider : class, IModdedUnlockableDataProvider
        {
            var name = Language.GetString( self.achievementNameToken );
            var desc = Language.GetString( self.achievementDescToken );
            return Language.GetStringFormatted( "UNLOCK_VIA_ACHIEVEMENT_FORMAT", name, desc );
        }

        internal static String GetUnlocked<TDataProvider>( this TDataProvider self )
            where TDataProvider : class, IModdedUnlockableDataProvider
        {
            var name = Language.GetString( self.achievementNameToken );
            var desc = Language.GetString( self.achievementDescToken );
            return Language.GetStringFormatted( "UNLOCKED_FORMAT", name, desc );
        }
    }

    public abstract class ModdedUnlockable<TSpriteProvider> : BaseAchievement, IModdedUnlockableDataProvider
        where TSpriteProvider : IAchievementSpriteProvider
    {
        #region Implementation
        public String spritePath { get => this.spriteProvider.pathString; }
        #endregion

        #region Contract
        protected abstract TSpriteProvider spriteProvider { get; }
        public abstract String achievementIdentifier { get; }
        public abstract String unlockableIdentifier { get; }
        public abstract String prerequisiteUnlockableIdentifier { get; }
        public abstract String achievementNameToken { get; }
        public abstract String achievementDescToken { get; }
        public abstract String unlockableNameToken { get; }
        #endregion

        #region Virtuals
        public override void OnGranted() => base.OnGranted();
        public override void OnInstall() => base.OnInstall();
        public override void OnUninstall() => base.OnUninstall();
        public override Single ProgressForAchievement() => base.ProgressForAchievement();
        public override BodyIndex LookUpRequiredBodyIndex() => base.LookUpRequiredBodyIndex();
        public override void OnBodyRequirementBroken() => base.OnBodyRequirementBroken();
        public override void OnBodyRequirementMet() => base.OnBodyRequirementMet();
        public override Boolean wantsBodyCallbacks { get => base.wantsBodyCallbacks; }
        #endregion
    }
}
