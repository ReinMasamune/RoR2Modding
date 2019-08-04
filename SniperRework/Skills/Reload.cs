using RoR2;
using UnityEngine;
using ReinSniperRework;

namespace EntityStates.ReinSniperRework.SniperWeapon
{
    public class SniperReload : LockSkill
    {
        ReinDataLibrary data;

        private float reloadTimer1;
        private float reloadTimer2;
        private float reloadTimer3;
        private float reloadTimer4;

        private bool reloading = false;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();

            reloadTimer1 = 0f;
            reloadTimer2 = 0f;

            data.g_ui.showReloadBar = true;
            reloading = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            float speed = AdjustedAttackSpeed(base.attackSpeedStat);
            reloadTimer1 += Time.fixedDeltaTime * speed;

            UpdateUI(reloadTimer1, reloadTimer2);

            base.characterBody.isSprinting = false;

            if (base.inputBank.skill1.justPressed)
            {
                DoReload();
            }
            reloadTimer2 += Time.fixedDeltaTime * speed;
        }

        public override void Update()
        {
            base.Update();

            base.characterBody.isSprinting = false;

            float speed = AdjustedAttackSpeed(base.attackSpeedStat);
            reloadTimer3 += Time.deltaTime * speed;

            UpdateUI(reloadTimer3, reloadTimer4);

            reloadTimer4 += Time.deltaTime * speed;
        }

        public override void OnExit()
        {
            base.OnExit();
            data.g_ui.showReloadBar = false;
        }

        private void DoReload()
        {
            if (!reloading)
            {
                reloading = true;
                float timer1 = reloadTimer1;
                float timer2 = reloadTimer2;
                float timer3 = reloadTimer3;
                float timer4 = reloadTimer4;

                if (timer1 > data.p_loadTime)
                {
                    timer1 = timer1 % data.p_loadTime;
                }
                if (timer2 > data.p_loadTime)
                {
                    timer2 = timer2 % data.p_loadTime;
                }
                if (timer3 > data.p_loadTime)
                {
                    timer3 = timer3 % data.p_loadTime;
                }
                if (timer4 > data.p_loadTime)
                {
                    timer4 = timer4 % data.p_loadTime;
                }

                timer1 /= data.p_loadTime;
                timer2 /= data.p_loadTime;
                timer3 /= data.p_loadTime;
                timer4 /= data.p_loadTime;

                if (timer2 > timer1)
                {
                    timer2 = 0f;
                }
                if (timer4 > timer3)
                {
                    timer4 = 0f;
                }

                bool t1InSweet = timer1 >= data.p_sweetLoadStart && timer1 <= data.p_sweetLoadEnd;
                bool t2InSweet = timer2 >= data.p_sweetLoadStart && timer2 <= data.p_sweetLoadEnd;
                bool t3InSweet = timer3 >= data.p_sweetLoadStart && timer3 <= data.p_sweetLoadEnd;
                bool t4InSweet = timer4 >= data.p_sweetLoadStart && timer4 <= data.p_sweetLoadEnd;

                bool t1InSoft = timer1 >= data.p_softLoadStart && timer1 <= data.p_softLoadEnd;
                bool t2InSoft = timer2 >= data.p_softLoadStart && timer2 <= data.p_softLoadEnd;
                bool t3InSoft = timer3 >= data.p_softLoadStart && timer3 <= data.p_softLoadEnd;
                bool t4InSoft = timer4 >= data.p_softLoadStart && timer4 <= data.p_softLoadEnd;

                bool t12AcrossSweet = timer1 >= data.p_sweetLoadEnd && timer2 <= data.p_sweetLoadStart;
                bool t12AcrossSoft = timer1 >= data.p_softLoadEnd && timer2 <= data.p_softLoadStart;
                bool t34AcrossSweet = timer3 >= data.p_sweetLoadEnd && timer4 <= data.p_sweetLoadStart;
                bool t34AcrossSoft = timer3 >= data.p_softLoadEnd && timer4 <= data.p_softLoadStart;

                string feedbackSound = "";
                int reloadTier = 0;

                if (t12AcrossSoft || t34AcrossSoft || t1InSoft || t2InSoft || t3InSoft || t4InSoft)
                {
                    feedbackSound = data.p_softLoadSound;
                    reloadTier = 1;
                }
                if (t12AcrossSweet || t34AcrossSweet || t1InSweet || t2InSweet || t3InSweet || t4InSweet)
                {
                    feedbackSound = data.p_sweetLoadSound;
                    reloadTier = 2;
                }

                data.g_reloadTier = reloadTier;

                Util.PlaySound(data.p_baseLoadSound, base.gameObject);
                Util.PlaySound(feedbackSound, base.gameObject);

                base.outer.SetNextStateToMain();
            }
        }

        private void UpdateUI(float t1, float t2)
        {
            float timer1 = t1;
            float timer2 = t2;
            
            if (timer1 > data.p_loadTime)
            {
                timer1 = timer1 % data.p_loadTime;
            }
            if (timer2 > data.p_loadTime)
            {
                timer2 = timer2 % data.p_loadTime;
            }

            timer1 /= data.p_loadTime;
            timer2 /= data.p_loadTime;

            if( timer2 > timer1 )
            {
                timer2 = 0f;
            }

            data.g_ui.reloadFrac = (timer1 + timer2) / 2f;
        }

        private float AdjustedAttackSpeed( float attackSpeed )
        {
            return Mathf.Min(attackSpeed, 1f + data.p_attackSpeedSoft * (1f - 1f / attackSpeed));
        }
    }
}