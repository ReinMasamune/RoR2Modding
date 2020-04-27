using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace ReinCore
{
    /// <summary>
    /// Zeroes out a tracer's length over its duration
    /// </summary>
    public class ZeroTracerLengthOverDuration : MonoBehaviour
    {
        /// <summary>
        /// The tracer to edit
        /// </summary>
        public Tracer tracer;


        private Single startLength;
        private void Awake() => this.startLength = tracer.length;

        private void Update() => this.tracer.length = Mathf.Lerp( this.startLength, 0f,  this.tracer._GetDistanceTraveled() / this.tracer._GetTotalDistance()  );
    }
}
