using RoR2;
using UnityEngine;
using System.Collections;

namespace WispSurvivor.Components
{
    [RequireComponent(typeof(EffectComponent))]
    public class WispBlazeEffectController : MonoBehaviour
    {
        private float timeLeft;
        public void Start()
        {
            EffectComponent effectComp = GetComponent<EffectComponent>();
            EffectData data = effectComp.effectData;

            timeLeft = data.genericFloat;

            StartCoroutine(DestroyOnTimer(timeLeft));
        }

        IEnumerator DestroyOnTimer(float timer)
        {
            yield return new WaitForSeconds(timer);

            MonoBehaviour.Destroy(gameObject);
        }
    }


}
