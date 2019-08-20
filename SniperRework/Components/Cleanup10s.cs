using UnityEngine;


namespace ReinSniperRework
{
    class CleanMeUpLater : MonoBehaviour
    {
        private float timer = 0f;
        public float lifetime = 10f;
        public bool debug = false;

        private void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;

            if( timer >= lifetime )
            {
                if( debug )
                {
                    Debug.Log("Time for suicide: " + gameObject.name);
                }
                Destroy(gameObject);
            }
        }
    }
}
