namespace ReinCore
{
    using System;
    using System.Linq.Expressions;

    using RoR2;

    using UnityEngine;
    public class Skin : IDisposable
    {
        #region Static External
        public static Skin Create( CharacterModel character, params SkinDef[] baseSkins )
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
            controller.skins ??= Array.Empty<SkinDef>();

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

        #endregion
        #region Instance External

        public Sprite icon

        {
            set => this.wrappedSkin.icon = value;
        }


        public String nameToken

        {
            set => this.wrappedSkin.nameToken = value;
        }

        public String unlockableName

        {
            set => this.wrappedSkin.unlockableName = value;
        }

        public CharacterModel.RendererInfo[] rendererInfos

        {
            get => this.wrappedSkin.rendererInfos;
            set => this.wrappedSkin.rendererInfos = value;
        }


        public SkinDef.GameObjectActivation[] gameObjectActivations

        {
            get => this.wrappedSkin.gameObjectActivations;
            set => this.wrappedSkin.gameObjectActivations = value;
        }


        public SkinDef.MeshReplacement[] meshReplacements

        {
            get => this.wrappedSkin.meshReplacements;
            set => this.wrappedSkin.meshReplacements = value;
        }


        public SkinDef.ProjectileGhostReplacement[] projectileGhostReplacements

        {
            get => this.wrappedSkin.projectileGhostReplacements;
            set => this.wrappedSkin.projectileGhostReplacements = value;
        }

        public SkinDef.MinionSkinReplacement[] minionSkinReplacements

        {
            get => this.wrappedSkin.minionSkinReplacements;
            set => this.wrappedSkin.minionSkinReplacements = value;
        }


        public void Dispose()
        {
            try
            {
                this.wrappedSkin.Awake();
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


        private Skin( SkinDef skin, GameObject rootObject, CharacterModel model, ModelSkinController controller )
        {
            this.wrappedSkin = skin;
            this.controller = controller;
            this.model = model;
        }
        #endregion
    }
}
