using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;


namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class Utility : BaseState
    {
        ReinDataLibrary data;
        ReinElementTracker elements;
        ReinElementTracker.Element mainElem;
        ReinLightningBuffTracker lightning;

        int fireLevel = 0;
        int iceLevel = 0;
        int lightningLevel = 0;

        //public static GameObject goodCrosshairPrefab;
        //public static GameObject badCrosshairPrefab;
        private float duration;
        private float stopwatch;
        private bool goodPlacement;
        private GameObject areaIndicatorInstance;
        private GameObject cachedCrosshairPrefab;

        public override void OnEnter()
        {
            base.OnEnter();

            data = base.GetComponent<ReinDataLibrary>();
            elements = data.element;
            lightning = data.lightning;

            mainElem = elements.GetMainElement();

            duration = data.u_baseDuration / attackSpeedStat;

            base.characterBody.SetAimTimer(duration + 2f);

            cachedCrosshairPrefab = base.characterBody.crosshairPrefab;

            base.PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", duration);

            Util.PlaySound(data.u_prepSound, base.gameObject);

            areaIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(data.u_areaIndicator);

            UpdateAreaIndicator();
        }
        //good
        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }
        //good
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= duration && !base.inputBank.skill3.down && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (goodPlacement)
            {
                base.PlayAnimation("Gesture, Additive", "FireWall");
                Util.PlaySound(data.u_fireSound, base.gameObject);
                if (areaIndicatorInstance && base.isAuthority)
                {
                    GameObject tempMuzzleFlash;

                    fireLevel = elements.GetElementLevel(ReinElementTracker.Element.fire);
                    iceLevel = elements.GetElementLevel(ReinElementTracker.Element.ice);
                    lightningLevel = elements.GetElementLevel(ReinElementTracker.Element.lightning);

                    switch (mainElem)
                    {
                        case ReinElementTracker.Element.fire:
                            Chat.AddMessage("Fire wall");
                            elements.ResetElement(ReinElementTracker.Element.fire);
                            data.u_walkerController.firePillarPrefab = data.u_f_pillar;
                            tempMuzzleFlash = data.u_f_muzzle;
                            break;
                        case ReinElementTracker.Element.ice:
                            Chat.AddMessage("Ice wall");
                            elements.ResetElement(ReinElementTracker.Element.ice);
                            data.u_walkerController.firePillarPrefab = data.u_i_pillar;
                            tempMuzzleFlash = data.u_i_muzzle;
                            break;
                        case ReinElementTracker.Element.lightning:
                            Chat.AddMessage("Lightning wall");
                            elements.ResetElement(ReinElementTracker.Element.lightning);
                            data.u_walkerController.firePillarPrefab = data.u_l_pillar;
                            tempMuzzleFlash = data.u_l_muzzle;
                            break;
                        case ReinElementTracker.Element.none:
                            Chat.AddMessage("Base wall");

                            data.u_walkerController.firePillarPrefab = data.u_i_pillar;
                            tempMuzzleFlash = data.u_i_muzzle;

                            break;
                        default:
                            Chat.AddMessage("You fucking broke it... Good job moron");
                            data.u_walkerController.firePillarPrefab = data.u_i_pillar;
                            tempMuzzleFlash = data.u_i_muzzle;
                            break;
                    }
                    if (fireLevel > 0)
                    {

                    }
                    if (iceLevel > 0)
                    {

                    }
                    if (lightningLevel > 0)
                    {

                    }

                    elements.AddElement(ReinElementTracker.Element.ice, 2);


                    EffectManager.instance.SimpleMuzzleFlash(tempMuzzleFlash, base.gameObject, "MuzzleLeft", true);
                    EffectManager.instance.SimpleMuzzleFlash(tempMuzzleFlash, base.gameObject, "MuzzleRight", true);
                    //Vector3 forward = areaIndicatorInstance.transform.forward;
                    Vector3 forward = base.GetAimRay().direction;
                    forward.y = 0f;
                    forward.Normalize();
                    //Vector3 vector = Vector3.Cross(Vector3.up, forward);
                    bool crit = Util.CheckRoll(critStat, base.characterBody.master);
                    // TODO: Utility EntityState; FireProjectile is outdated
                    ProjectileManager.instance.FireProjectile(data.u_seedProjectile, areaIndicatorInstance.transform.position + Vector3.up, Util.QuaternionSafeLookRotation(forward), base.gameObject, damageStat * data.u_i_damageCoef, 0f, crit, DamageColorIndex.Default, null, -1f);

                    if( lightning.GetBuffed() )
                    {
                        LightningBlink blink = new LightningBlink();
                        blink.castValue = data.u_blinkCastValue;
                        data.bodyState.SetNextState(blink);
                    }
                }
            }
            else
            {
                base.skillLocator.utility.AddOneStock();
                base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            }
            EntityState.Destroy(areaIndicatorInstance.gameObject);
            base.characterBody.crosshairPrefab = cachedCrosshairPrefab;
            base.OnExit();
        }
        //good
        private void UpdateAreaIndicator()
        {
            goodPlacement = false;
            areaIndicatorInstance.SetActive(true);
            if (areaIndicatorInstance)
            {
                float num = data.u_maxRange;
                float num2 = 0f;
                Ray aimRay = base.GetAimRay();
                RaycastHit raycastHit;
                if (Physics.Raycast(CameraRigController.ModifyAimRayIfApplicable(aimRay, base.gameObject, out num2), out raycastHit, num + num2, LayerIndex.world.mask))
                {
                    areaIndicatorInstance.transform.position = raycastHit.point;
                    areaIndicatorInstance.transform.up = raycastHit.normal;
                    areaIndicatorInstance.transform.forward = -aimRay.direction;
                    goodPlacement = (Vector3.Angle(Vector3.up, raycastHit.normal) < data.u_maxSlope);
                }
                base.characterBody.crosshairPrefab = (goodPlacement ? data.u_goodCrosshair : data.u_badCrosshair);
            }
            areaIndicatorInstance.SetActive(goodPlacement);
        }
        //good
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}
