namespace Sniper.UI.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using UnityEngine;

    public class NibController : MonoBehaviour
    {
        private static readonly Int32 nibTypeCount = Enum.GetNames(typeof(NibType)).Length;

        //[HideInInspector]
        [SerializeField]
        private GameObject[] nibs;

        public enum NibType
        {
            ArrowShort,
            ArrowLong,
            Bar,
            None,
        }

        internal NibType type
        {
            set
            {
                for(Int32 i = 0; i < this.nibs.Length; ++i) this.nibs[i].SetActive(value == (NibType)i);
            }
        }

        protected void OnValidate()
        {
            this.nibs = Enum.GetNames(typeof(NibType)).Select(base.transform.Find).Select((t) => t.gameObject).ToArray();
        }
    }
}