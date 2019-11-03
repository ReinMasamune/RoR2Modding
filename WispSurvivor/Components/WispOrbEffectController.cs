using RoR2;
using UnityEngine;

namespace WispSurvivor.Components
{
    [RequireComponent(typeof( EffectComponent ) )]
    public class WispOrbEffectController : MonoBehaviour
    {
        public string startSound = "";
        public string endSound = "";

        private float duration;
        private float timeLeft;
        private float startDist;
        private float deathDelay = 3f;

        private bool useTarget;
        private bool dead = false;

        private Vector3 start;
        private Vector3 end;
        private Vector3 prevPos;

        private Transform target;


        public void Start()
        {
            EffectComponent effectComp = GetComponent<EffectComponent>();
            EffectData data = effectComp.effectData;

            useTarget = data.genericBool;
            duration = data.genericFloat;
            start = data.origin;
            end = data.start;

            if( useTarget )
            {
                target = data.ResolveHurtBoxReference().transform;
                startDist = Vector3.Distance(start, target.position);
            }else
            {
                startDist = Vector3.Distance(start, end);
            }

            timeLeft = duration;
            prevPos = start;

            transform.position = start;
            RoR2.Util.PlaySound(startSound, gameObject);
        }

        public void Update()
        {
            if (!dead)
            {
                if (useTarget)
                {
                    if (!target)
                    {
                        useTarget = false;
                    }
                }

                Vector3 dest = end;
                if (useTarget)
                {
                    dest = target.position;
                }

                //This can be modified to make arcing effects
                timeLeft -= Time.deltaTime;
                float frac = 1f - timeLeft / duration;
                Vector3 desiredPos = Vector3.Lerp(start, dest, frac);

                transform.rotation = Quaternion.FromToRotation(Vector3.Normalize(desiredPos - prevPos), transform.forward);
                transform.position = desiredPos;
                prevPos = desiredPos;

                if (timeLeft < 0f)
                {
                    RoR2.Util.PlaySound(endSound, gameObject);
                    dead = true;
                }
            }
            else
            {
                deathDelay -= Time.deltaTime;
                if( deathDelay < 0 )
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
