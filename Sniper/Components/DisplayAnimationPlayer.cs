namespace Sniper.Components
{
    using UnityEngine;

    internal class DisplayAnimationPlayer : MonoBehaviour
    {
        private void Awake()
        {
            Animator animator = base.GetComponentInChildren<Animator>();
            animator.Play( "DisplayEnter" );
        }
    }
}
