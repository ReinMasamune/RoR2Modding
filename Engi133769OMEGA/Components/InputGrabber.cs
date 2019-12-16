using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA.Components
{
    public class InputGrabber : MonoBehaviour
    {
        public Boolean enabled;
        public Boolean skill1;
        public Boolean skill2;
        public Boolean skill3;
        public Boolean skill4;
        public Boolean sprint;
        public Boolean jump;

        public Int32 emote;

        public Vector3 moveVector;
        public Vector3 aimDirection;
    }
}
