namespace AlternateArtificer.SelectablePassive
{

    /*
    public class PassiveComponent : MonoBehaviour
    {
        public enum PassiveMode
        {
            ENVSuit = 0,
            ElementalIntensity = 1
        };

        public PassiveMode mode;

        private EntityStateMachine passiveStateMachine;
        private GenericSkill passiveSkill;

        public void Start()
        {
            this.passiveStateMachine = EntityStateMachine.FindByCustomName( base.gameObject, "Jet" );

            var loc = base.GetComponent<SkillLocator>();

            foreach( GenericSkill skill in base.gameObject.GetComponents<GenericSkill>() )
            {
                if( skill != loc.primary && skill != loc.secondary && skill != loc.utility && skill != loc.special ) this.passiveSkill = skill;
            }

            if( this.passiveSkill != null && this.passiveSkill.skillDef == AltArtiMain.elementalIntensity )
            {
                Debug.Log( "StuffGoinDown" );
                this.passiveStateMachine.SetNextState( new AltArtiPassive() );
                this.passiveStateMachine.initialStateType = new SerializableEntityStateType( typeof( AltArtiPassive ) );
                this.passiveStateMachine.mainStateType = new SerializableEntityStateType( typeof( AltArtiPassive ) );
                this.mode = PassiveMode.ElementalIntensity;

                var charModel = base.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>();
                var meshRender = charModel.transform.Find("MageMesh").GetComponent<SkinnedMeshRenderer>();
                if( meshRender.sharedMesh != AltArtiMain.elementalIntensityMesh ) meshRender.sharedMesh = AltArtiMain.elementalIntensityMesh;

                var jetOn = charModel.GetComponent<ChildLocator>().FindChild( "JetOn" );
                var jetPar = jetOn.parent;
                jetPar.Find( "Jets, Right" ).gameObject.SetActive( false );
                jetPar.Find( "Jets, Left" ).gameObject.SetActive( false );
                jetOn.gameObject.SetActive( false );
            } else
            {
                Debug.Log( "StuffGoinUp" );
                this.passiveStateMachine.SetNextState( new Idle() );
                this.passiveStateMachine.initialStateType = new SerializableEntityStateType( typeof( Idle ) );
                this.passiveStateMachine.mainStateType = new SerializableEntityStateType( typeof( Idle ) );
                this.mode = PassiveMode.ENVSuit;

                var charModel = base.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>();
                var meshRender = charModel.transform.Find("MageMesh").GetComponent<SkinnedMeshRenderer>();
                if( meshRender.sharedMesh != AltArtiMain.envSuitMesh ) meshRender.sharedMesh = AltArtiMain.envSuitMesh;

                var jetOn = charModel.GetComponent<ChildLocator>().FindChild( "JetOn" );
                var jetPar = jetOn.parent;
                jetPar.Find( "Jets, Right" ).gameObject.SetActive( true );
                jetPar.Find( "Jets, Left" ).gameObject.SetActive( true );
                //jetOn.gameObject.SetActive( false );
            }
        }
    }
    */
}
