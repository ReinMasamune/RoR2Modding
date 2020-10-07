namespace Rein.Sniper.Components
{
    using UnityEngine;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    internal class DisplayAnimationPlayer : MonoBehaviour
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        private void Awake()
        {
            Animator animator = base.GetComponentInChildren<Animator>();
            animator.Play( "DisplayEnter" );
        }
    }
}
