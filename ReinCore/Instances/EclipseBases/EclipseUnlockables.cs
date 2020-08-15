namespace ReinCore
{
    using System;

    using RoR2;
    using RoR2.Achievements;

    using UnityEngine;

    public enum EclipseNum
    {
        Invalid = -1,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight
    }

    public static class EclipseHelpers
    {
        public static Boolean serverTracked { get => false; }


        public static String eclipse1IconPath { get => ""; }
        public static String eclipse2IconPath { get => ""; }
        public static String eclipse3IconPath { get => ""; }
        public static String eclipse4IconPath { get => ""; }
        public static String eclipse5IconPath { get => ""; }
        public static String eclipse6IconPath { get => ""; }
        public static String eclipse7IconPath { get => ""; }
        public static String eclipse8IconPath { get => ""; }


        public static String eclipse1NameToken { get => ""; }
        public static String eclipse2NameToken { get => ""; }
        public static String eclipse3NameToken { get => ""; }
        public static String eclipse4NameToken { get => ""; }
        public static String eclipse5NameToken { get => ""; }
        public static String eclipse6NameToken { get => ""; }
        public static String eclipse7NameToken { get => ""; }
        public static String eclipse8NameToken { get => ""; }


        public static String eclipse1UnlockableNameToken { get => ""; }
        public static String eclipse2UnlockableNameToken { get => ""; }
        public static String eclipse3UnlockableNameToken { get => ""; }
        public static String eclipse4UnlockableNameToken { get => ""; }
        public static String eclipse5UnlockableNameToken { get => ""; }
        public static String eclipse6UnlockableNameToken { get => ""; }
        public static String eclipse7UnlockableNameToken { get => ""; }
        public static String eclipse8UnlockableNameToken { get => ""; }


        public static String eclipse1DescToken { get => ""; }
        public static String eclipse2DescToken { get => ""; }
        public static String eclipse3DescToken { get => ""; }
        public static String eclipse4DescToken { get => ""; }
        public static String eclipse5DescToken { get => ""; }
        public static String eclipse6DescToken { get => ""; }
        public static String eclipse7DescToken { get => ""; }
        public static String eclipse8DescToken { get => ""; }



        public static String PathForIcon(EclipseNum num)
        {
            switch(num)
            {
                case EclipseNum.One:
                    return eclipse1IconPath;
                case EclipseNum.Two:
                    return eclipse2IconPath;
                case EclipseNum.Three:
                    return eclipse3IconPath;
                case EclipseNum.Four:
                    return eclipse4IconPath;
                case EclipseNum.Five:
                    return eclipse5IconPath;
                case EclipseNum.Six:
                    return eclipse6IconPath;
                case EclipseNum.Seven:
                    return eclipse7IconPath;
                case EclipseNum.Eight:
                    return eclipse8IconPath;
                default:
                    throw new ArgumentOutOfRangeException(nameof(num));
            }
        }

        public static String AchNameToken(EclipseNum num)
        {
            switch(num)
            {
                case EclipseNum.One:
                    return eclipse1NameToken;
                case EclipseNum.Two:
                    return eclipse2NameToken;
                case EclipseNum.Three:
                    return eclipse3NameToken;
                case EclipseNum.Four:
                    return eclipse4NameToken;
                case EclipseNum.Five:
                    return eclipse5NameToken;
                case EclipseNum.Six:
                    return eclipse6NameToken;
                case EclipseNum.Seven:
                    return eclipse7NameToken;
                case EclipseNum.Eight:
                    return eclipse8NameToken;
                default:
                    throw new ArgumentOutOfRangeException(nameof(num));
            }
        }

        public static String UnlockableNameToken(EclipseNum num)
        {
            switch(num)
            {
                case EclipseNum.One:
                    return eclipse1UnlockableNameToken;
                case EclipseNum.Two:
                    return eclipse2UnlockableNameToken;
                case EclipseNum.Three:
                    return eclipse3UnlockableNameToken;
                case EclipseNum.Four:
                    return eclipse4UnlockableNameToken;
                case EclipseNum.Five:
                    return eclipse5UnlockableNameToken;
                case EclipseNum.Six:
                    return eclipse6UnlockableNameToken;
                case EclipseNum.Seven:
                    return eclipse7UnlockableNameToken;
                case EclipseNum.Eight:
                    return eclipse8UnlockableNameToken;
                default:
                    throw new ArgumentOutOfRangeException(nameof(num));
            }
        }

        public static String AchDescToken(EclipseNum num)
        {
            switch(num)
            {
                case EclipseNum.One:
                    return eclipse1DescToken;
                case EclipseNum.Two:
                    return eclipse2DescToken;
                case EclipseNum.Three:
                    return eclipse3DescToken;
                case EclipseNum.Four:
                    return eclipse4DescToken;
                case EclipseNum.Five:
                    return eclipse5DescToken;
                case EclipseNum.Six:
                    return eclipse6DescToken;
                case EclipseNum.Seven:
                    return eclipse7DescToken;
                case EclipseNum.Eight:
                    return eclipse8DescToken;
                default:
                    throw new ArgumentOutOfRangeException(nameof(num));
            }
        }

        public static String GenerateUnlockableIdentifier(Int32 index, String identity)
        {
            return $"Eclipse.{identity}.{index + 1}";
        }
        public static String GenerateAchievementIdentifier(Int32 index, String identity)
        {
            return $"Eclipse.{identity}.{index + 1}.AchievementIdentifier";
        }
    }

    public abstract class EclipseBase : BaseAchievement, IModdedUnlockableDataProvider
    {
        public EclipseBase()
        {
            this.spritePath = EclipseHelpers.PathForIcon(this.index);
            this.achievementIdentifier = EclipseHelpers.GenerateAchievementIdentifier((Int32)this.index, this.identity);
            this.unlockableIdentifier = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index, this.identity);
            this.achievementNameToken = EclipseHelpers.AchNameToken(this.index);
            this.achievementDescToken = EclipseHelpers.AchDescToken(this.index);
            this.unlockableNameToken = EclipseHelpers.UnlockableNameToken(this.index);
        }

        #region Implementation
        public String spritePath { get; }
        public String achievementIdentifier { get; }
        public String unlockableIdentifier { get; }
        public String prerequisiteUnlockableIdentifier { get => this.prereq; }
        public String achievementNameToken { get; }
        public String achievementDescToken { get; }
        public String unlockableNameToken { get; }
        #endregion

        #region Contract
        public abstract SurvivorIndex targetSurvivor { get; }
        public abstract EclipseNum index { get; }
        public abstract String prereq { get; }
        public abstract String identity { get; }
        #endregion

        #region Virtuals
        public sealed override void OnGranted() => base.OnGranted();
        public sealed override void OnInstall() => base.OnInstall();
        public sealed override void OnUninstall() => base.OnUninstall();
        public sealed override Single ProgressForAchievement() => base.ProgressForAchievement();
        public sealed override Int32 LookUpRequiredBodyIndex() => base.LookUpRequiredBodyIndex();
        public sealed override void OnBodyRequirementBroken() => base.OnBodyRequirementBroken();
        public sealed override void OnBodyRequirementMet() => base.OnBodyRequirementMet();
        public sealed override Boolean wantsBodyCallbacks { get => base.wantsBodyCallbacks; }
        #endregion
    }

    public sealed class Test : Eclipse1Base
    {
        public sealed override String prereq => "";

        public sealed override String identity => "";

        public override SurvivorIndex targetSurvivor => throw new NotImplementedException();
    }
    public sealed class Test2 : Eclipse2Base
    {
        public sealed override String identity => "";

        public override SurvivorIndex targetSurvivor => throw new NotImplementedException();
    }

    public abstract class Eclipse1Base : EclipseBase
    {
        public Eclipse1Base() : base()
        {

        }
        public sealed override EclipseNum index => EclipseNum.One;
        public abstract override String prereq { get; }
    }

    public abstract class Eclipse2Base : EclipseBase
    {
        public Eclipse2Base() : base()
        {
            this.prereq = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index - 1, this.identity);
        }
        public sealed override EclipseNum index => EclipseNum.Two;
        public sealed override String prereq { get; }
    }

    public abstract class Eclipse3Base : EclipseBase
    {
        public Eclipse3Base() : base()
        {
            this.prereq = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index - 1, this.identity);
        }
        public sealed override EclipseNum index => EclipseNum.Three;
        public sealed override String prereq { get; }
    }

    public abstract class Eclipse4Base : EclipseBase
    {
        public Eclipse4Base() : base()
        {
            this.prereq = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index - 1, this.identity);
        }
        public sealed override EclipseNum index => EclipseNum.Four;
        public sealed override String prereq { get; }
    }

    public abstract class Eclipse5Base : EclipseBase
    {
        public Eclipse5Base() : base()
        {
            this.prereq = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index - 1, this.identity);
        }
        public sealed override EclipseNum index => EclipseNum.Five;
        public sealed override String prereq { get; }
    }

    public abstract class Eclipse6Base : EclipseBase
    {
        public Eclipse6Base() : base()
        {
            this.prereq = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index - 1, this.identity);
        }
        public sealed override EclipseNum index => EclipseNum.Six;
        public sealed override String prereq { get; }
    }

    public abstract class Eclipse7Base : EclipseBase
    {
        public Eclipse7Base() : base()
        {
            this.prereq = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index - 1, this.identity);
        }
        public sealed override EclipseNum index => EclipseNum.Seven;
        public sealed override String prereq { get; }
    }

    public abstract class Eclipse8Base : EclipseBase
    {
        public Eclipse8Base() : base()
        {
            this.prereq = EclipseHelpers.GenerateUnlockableIdentifier((Int32)this.index - 1, this.identity);
        }
        public sealed override EclipseNum index => EclipseNum.Eight;
        public sealed override String prereq { get; }
    }
}
