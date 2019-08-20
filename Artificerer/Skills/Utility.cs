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

        //public static GameObject goodCrosshairPrefab;
        //public static GameObject badCrosshairPrefab;
        private float duration;
        private float stopwatch;
        private bool goodPlacement;
        private GameObject areaIndicatorInstance;
        private GameObject cachedCrosshairPrefab;

        private GameObject tempProjectile;
        private GameObject tempMuzzleFlash;

        public override void OnEnter()
        {
            base.OnEnter();

            data = base.GetComponent<ReinDataLibrary>();

            int elem = Mathf.RoundToInt(Random.Range(1f, 3f));

            switch( elem )
            {
                case 1:
                    Chat.AddMessage("Fire");
                    data.u_walkerController.firePillarPrefab = data.u_f_pillar;
                    tempMuzzleFlash = data.u_f_muzzle;
                    break;

                case 2:
                    Chat.AddMessage("Ice");
                    data.u_walkerController.firePillarPrefab = data.u_i_pillar;
                    tempMuzzleFlash = data.u_i_muzzle;
                    break;

                case 3:
                    Chat.AddMessage("Lightning");
                    data.u_walkerController.firePillarPrefab = data.u_l_pillar;
                    tempMuzzleFlash = data.u_l_muzzle;
                    break;

                default:
                    Chat.AddMessage("Wtf");
                    data.u_walkerController.firePillarPrefab = data.u_i_pillar;
                    tempMuzzleFlash = data.u_i_muzzle;
                    break;
            }
            if( elem == 1 )
            {

            }
            if( elem == 2 )
            {

            }
            if( elem == 3 )
            {

            }

            duration = data.u_baseDuration / attackSpeedStat;
            base.characterBody.SetAimTimer(duration + 2f);
            cachedCrosshairPrefab = base.characterBody.crosshairPrefab;
            base.PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", duration);
            Util.PlaySound(data.u_prepSound, base.gameObject);
            areaIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(data.u_areaIndicator);
            MageLastElementTracker component = base.GetComponent<MageLastElementTracker>();
            if (component)
            {
                component.ApplyElement(MageElement.Ice);
            }
            UpdateAreaIndicator();
        }

        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }

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
                if (/*areaIndicatorInstance &&*/ base.isAuthority)
                {
                    EffectManager.instance.SimpleMuzzleFlash(tempMuzzleFlash, base.gameObject, "MuzzleLeft", true);
                    EffectManager.instance.SimpleMuzzleFlash(tempMuzzleFlash, base.gameObject, "MuzzleRight", true);
                    //Vector3 forward = areaIndicatorInstance.transform.forward;
                    Vector3 forward = base.GetAimRay().direction;
                    forward.y = 0f;
                    forward.Normalize();
                    Vector3 vector = Vector3.Cross(Vector3.up, forward);
                    bool crit = Util.CheckRoll(critStat, base.characterBody.master);
                    ProjectileManager.instance.FireProjectile(data.u_seedProjectile, areaIndicatorInstance.transform.position + Vector3.up, Util.QuaternionSafeLookRotation(forward), base.gameObject, damageStat * data.u_i_damageCoef, 0f, crit, DamageColorIndex.Default, null, -1f);
                    //ProjectileManager.instance.FireProjectile(tempProjectile, areaIndicatorInstance.transform.position + Vector3.up, Util.QuaternionSafeLookRotation(-vector), base.gameObject, damageStat * data.u_i_damageCoef, 0f, crit, DamageColorIndex.Default, null, -1f);
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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}
