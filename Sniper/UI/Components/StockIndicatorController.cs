//#define TESTING
namespace Rein.Sniper.UI.Components
{

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Rein.Sniper.UI.Components.Util;

    using UnityEngine;
    using UnityEngine.UI;

    public class StockIndicatorController : MonoBehaviour
    {
#if TESTING
        [Range( 0f,1f)]
        public Single testFill;
#endif
        public Single fill
        {
            set
            {
                var displayed = Mathf.Clamp01(value);
                if( displayed != this.currentFill )
                {
                    this.bulletFiller.fill = displayed;
                    this.currentFill = displayed;
                }
            }
        }
#if TESTING
        public Boolean testHolder;
#endif
        public Boolean holder
        {
            set
            {
                if( value != this.holderVisible )
                {
                    this.holderVisible = value;
                    this.holderObj.SetActive( value );
                }
            }
        }
#if TESTING
        public Boolean testBullet;
#endif
        public Boolean bullet
        {
            set
            {
                if( value != this.bulletVisible )
                {
                    this.bulletVisible = value;
                    this.bulletObj.SetActive( value );
                }
            }
        }

        [HideInInspector]
        [SerializeField]
        private GameObject holderObj;
        [HideInInspector]
        [SerializeField]
        private GameObject bulletObj;
        [HideInInspector]
        [Range(0f, 1f)]
        [SerializeField]
        private Single currentFill;
        [HideInInspector]
        [SerializeField]
        private Boolean holderVisible;
        [HideInInspector]
        [SerializeField]
        private Boolean bulletVisible;
        [HideInInspector]
        [SerializeField]
        private Image holderImg;
        [HideInInspector]
        [SerializeField]
        private Image bulletBgImg;
        [HideInInspector]
        [SerializeField]
        private Image bulletFillImg;
        [HideInInspector]
        [SerializeField]
        private Filler bulletFiller;
        [HideInInspector]
        [SerializeField]
        private Boolean init = false;

        private void OnValidate()
        {
            if( !this.init )
            {
                var holder = base.transform.Find("Holder");
                var bullet = base.transform.Find("Bullet");
                var bulletBG = bullet.Find("Background");
                var bulletFill = bullet.Find("Fill");

                this.holderObj = holder.gameObject;
                this.bulletObj = bullet.gameObject;
                this.holderImg = holder.GetComponent<Image>();
                this.bulletBgImg = bulletBG.GetComponent<Image>();
                this.bulletFillImg = bulletFill.GetComponent<Image>();
                this.bulletFiller = bulletFill.GetComponent<Filler>();
                this.init = true;
            }
#if TESTING
            this.fill = this.testFill;
            this.bullet = this.testBullet;
            this.holder = this.testHolder;
#endif
        }
    }
}