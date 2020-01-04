using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin.Components
{
    [RequireComponent( typeof( EffectComponent ) )]
    public class WispNetworkedEffect : NetworkBehaviour
    {
        [SyncVar]
        public System.Single syncedFloat = 0f;
    }
}
