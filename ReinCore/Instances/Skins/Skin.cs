namespace ReinCore
{
    using System;
    using System.Linq.Expressions;

    using RoR2;

    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Skin : IDisposable
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Static External
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Skin Create( CharacterModel character, params SkinDef[] baseSkins )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( character == null )
            {
                throw new ArgumentNullException( nameof( character ) );
            }

            GameObject root = character.gameObject;
            if( root == null )
            {
                throw new ArgumentNullException( nameof( character ) );
            }

            ModelSkinController controller = root.AddOrGetComponent<ModelSkinController>();
            if( controller == null )
            {
                throw new ArgumentNullException( nameof( character ) );
            }

            HooksCore.RoR2.SkinDef.Awake.On += DoNothing;
            SkinDef def = ScriptableObject.CreateInstance<SkinDef>();
            HooksCore.RoR2.SkinDef.Awake.On -= DoNothing;

            if( def == null )
            {
                throw new Exception( "Failed to create SkinDef" );
            }

            def.rootObject = root;
            def.baseSkins = baseSkins;

            if( baseSkins.Length == 0 )
            {
                def.rendererInfos = character.baseRendererInfos.Clone() as CharacterModel.RendererInfo[];
            }

            return new Skin( def, root, character, controller );
        }


        #endregion
        #region Static Internal
        private static void DoNothing( HooksCore.RoR2.SkinDef.Awake.Orig orig, SkinDef self ) { }

        private delegate void SkinDefAwake( SkinDef skinDef );
#pragma warning disable IDE1006 // Naming Styles
        private static readonly SkinDefAwake CallAwake;
#pragma warning restore IDE1006 // Naming Styles

        static Skin()
        {
            ParameterExpression instanceParam = Expression.Parameter( typeof(SkinDef), "instance" );
            MethodCallExpression body = Expression.Call( instanceParam, "Awake", null );
            CallAwake = Expression.Lambda<SkinDefAwake>( body, instanceParam ).Compile();
        }

        #endregion
        #region Instance External
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Sprite icon
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            set => this.wrappedSkin.icon = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public String nameToken
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            set => this.wrappedSkin.nameToken = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public String unlockableName
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            set => this.wrappedSkin.unlockableName = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public CharacterModel.RendererInfo[] rendererInfos
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => this.wrappedSkin.rendererInfos;
            set => this.wrappedSkin.rendererInfos = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public SkinDef.GameObjectActivation[] gameObjectActivations
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => this.wrappedSkin.gameObjectActivations;
            set => this.wrappedSkin.gameObjectActivations = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public SkinDef.MeshReplacement[] meshReplacements
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => this.wrappedSkin.meshReplacements;
            set => this.wrappedSkin.meshReplacements = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public SkinDef.ProjectileGhostReplacement[] projectileGhostReplacements
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => this.wrappedSkin.projectileGhostReplacements;
            set => this.wrappedSkin.projectileGhostReplacements = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public SkinDef.MinionSkinReplacement[] minionSkinReplacements
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            get => this.wrappedSkin.minionSkinReplacements;
            set => this.wrappedSkin.minionSkinReplacements = value;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Dispose()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            try
            {
                CallAwake( this.wrappedSkin );
            } catch( Exception e )
            {
                Log.Error( e );

                Log.Error( "Unable to create skin due to exception, make sure all fields are assigned" );
                return;
            }

            Int32 index = this.controller.skins.Length;
            Array.Resize( ref this.controller.skins, index + 1 );
            this.controller.skins[index] = this.wrappedSkin;
        }

        #endregion

        #region Instance Internal
        private readonly SkinDef wrappedSkin;
        private readonly ModelSkinController controller;
        private readonly CharacterModel model;







#pragma warning disable IDE0060 // Remove unused parameter
        private Skin( SkinDef skin, GameObject rootObject, CharacterModel model, ModelSkinController controller )
#pragma warning restore IDE0060 // Remove unused parameter
        {
            this.wrappedSkin = skin;
            this.controller = controller;
            this.model = model;
        }

        #endregion


    }
}
