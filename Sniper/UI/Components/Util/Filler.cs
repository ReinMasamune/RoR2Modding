namespace Sniper.UI.Components.Util
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent( typeof( Image ) )]
    public class Filler : MonoBehaviour
    {
        [SerializeField]
        private Image img;
        [SerializeField]
        [Range(0f, 1f)]
        private Single currentFill;
        [SerializeField]
        private Single minFill = 0f;
        [SerializeField]
        private Single maxFill = 1f;

        public Single fill
        {
            set
            {
                if( value != this.currentFill )
                {
                    this.img.fillAmount = Mathf.Lerp( this.minFill, this.maxFill, value );
                    this.currentFill = value;
                }
            }
        }


        //public void OnValidate()
        //{
        //    this.img = base.GetComponent<Image>();
        //    this.img.fillAmount = this.img.fillAmount = Mathf.Lerp( this.minFill, this.maxFill, this.currentFill );
        //}

        //public void Awake()
        //{

        //}

    }
}
