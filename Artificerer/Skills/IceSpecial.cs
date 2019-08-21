using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;

namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class IceSpecial : BaseState
    {
        ReinDataLibrary data;
        ReinElementTracker elements;
        ReinLightningBuffTracker lightning;

        public int fireLevel = 0;
        public int iceLevel = 0;
        public int lightningLevel = 0;

        private GameObject indicator;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            elements = data.element;
            lightning = data.lightning;
            Chat.AddMessage("Pretend this is the ice special");

            elements.ResetElement(ReinElementTracker.Element.ice);
            elements.AddElement(ReinElementTracker.Element.fire, 2);

            //Animation
            //Sound
            indicator = UnityEngine.Object.Instantiate<GameObject>(data.r_i_areaIndicator);
            indicator.transform.localScale = Vector3.zero; //Set var from data
        }

        public override void Update()
        {
            base.Update();
            UpdateIndicator();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && !base.inputBank.skill4.down)
            {
                if( lightning.GetBuffed() )
                {
                    LightningBlink blink = new LightningBlink();
                    blink.castValue = data.r_blinkCastValue;
                    data.bodyState.SetNextState(blink);
                }

                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            //Animation
            //Sound
            //Sound
            //Muzzle Effect
            if( base.cameraTargetParams )
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            }
            if( indicator )
            {
                //Fire projectile and element stuff

                Destroy(indicator);
            }
            base.OnExit();
        }

        private void UpdateIndicator()
        {
            if( indicator )
            {
                RaycastHit hit;
                if( Physics.Raycast(base.GetAimRay() , out hit , 1000f , LayerIndex.water.mask ) )
                {
                    indicator.transform.position = hit.point;
                    indicator.transform.up = hit.normal;
                }
            }
        }
    }
}
