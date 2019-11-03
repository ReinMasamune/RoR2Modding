using UnityEngine;
using System;

namespace WispSurvivor.Components
{
    public class WispAimLineController : MonoBehaviour
    {
        private LineRenderer lr;

        private Transform lrEnd;

        public void Awake()
        {
            lr = GetComponent<LineRenderer>();
            lrEnd = transform.Find("lineEnd");
        }

        public void Update()
        {
            lr.SetPosition(0, transform.position);
            if( lrEnd )
            {
                lr.SetPosition(1, lrEnd.position);
            }
        }
    }
}
