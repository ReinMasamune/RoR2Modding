using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        public class WispAimLineController : MonoBehaviour
        {
            private LineRenderer lr;

            private Transform lrEnd;

            public void Awake()
            {
                this.lr = this.GetComponent<LineRenderer>();
                this.lrEnd = this.transform.Find( "lineEnd" );
            }

            public void Update()
            {
                this.lr.SetPosition( 0, this.transform.position );
                if( this.lrEnd )
                {
                    this.lr.SetPosition( 1, this.lrEnd.position );
                }
            }
        }
    }
}
