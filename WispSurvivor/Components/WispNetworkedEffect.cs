using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Components
{
    [RequireComponent(typeof( EffectComponent ) )]
    public class WispNetworkedEffect : NetworkBehaviour
    {
        [SyncVar]
        public float syncedFloat = 0f;
    }
}
