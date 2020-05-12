using System;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    public class CollisionFixer : MonoBehaviour
    {
        private void Awake()
        {
            var cols = base.GetComponentsInChildren<Collider>();

            for( Int32 i = 0; i < cols.Length; ++i )
            {
                for( Int32 j = i + 1; j < cols.Length; ++j )
                {
                    Physics.IgnoreCollision( cols[i], cols[j], true );
                }
            }
        }
    }

}