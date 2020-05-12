namespace AlternativeArtificer.States.Main
{

    /*
    public class AltArtiMain : GenericCharacterMain
    {
        public static Mesh envSuitMesh;
        public static Mesh elementalIntensityMesh;

        public static PassiveSkillDef envSuit;
        public static PassiveSkillDef elementalIntensity;

        public PassiveComponent.PassiveMode mode;

        private EntityStateMachine passiveStateMachine;
        private GenericSkill passiveSkill;

        public override void OnEnter()
        {
            base.OnEnter();
            this.mode = base.gameObject.GetComponent<PassiveComponent>().mode;
            this.passiveStateMachine = EntityStateMachine.FindByCustomName( base.gameObject, "Jet" );
        }

        public override void ProcessJump()
        {
            base.ProcessJump();

            if( this.mode == PassiveComponent.PassiveMode.ENVSuit )
            {
                if( this.hasCharacterMotor && this.hasInputBank && base.isAuthority )
                {
                    Boolean inJetpack = this.passiveStateMachine.state.GetType() == typeof( JetpackOn );
                    Boolean jetpackOk = base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded;

                    if( inJetpack )
                    {
                        if( !jetpackOk || !base.inputBank.jump.down )
                        {
                            passiveStateMachine.SetNextState( new Idle() );
                        }
                    } else
                    {
                        if( jetpackOk && base.inputBank.jump.down )
                        {
                            passiveStateMachine.SetNextState( new JetpackOn() );
                        }
                    }
                    
                }
            }
        }
    }
    */
}
