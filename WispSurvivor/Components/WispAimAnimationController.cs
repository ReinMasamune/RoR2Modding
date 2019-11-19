using R2API.Utils;
using RoR2;
using System.Reflection;
using UnityEngine;

namespace WispSurvivor.Components
{
    public class WispAimAnimationController : MonoBehaviour
    {
        private System.Boolean cannonMode = false;
        private System.Boolean transition = false;
        private System.Single timer = 0f;
        private System.Single maxTime = 0f;
        private System.Single cannonAimSpeed = 0f;

        private Vector3 finalDirection;
        private Vector3 baseCannonPos;
        private Vector3 baseHeadPos;
        private Vector3 offset1;
        private Quaternion baseHeadRot;
        private Quaternion baseCannonRot;

        private AimAnimator aa;
        private ModelLocator ml;
        private InputBankTest input;
        private Transform headTransform;
        private Transform cannonTransform;
        private Transform modelTransform;
        private Transform refHeadTransform;
        private Transform refCannonTransform;

        public void Start()
        {
            this.input = this.GetComponent<InputBankTest>();
            this.ml = this.gameObject.GetComponent<ModelLocator>();
            this.modelTransform = this.ml.modelTransform;
            this.aa = this.modelTransform.GetComponent<AimAnimator>();
            this.refCannonTransform = this.modelTransform;
            this.cannonTransform = this.modelTransform.Find( "CannonPivot" );
            this.headTransform = this.cannonTransform.Find( "AncientWispArmature" ).Find( "Head" );
            this.refHeadTransform = this.modelTransform.Find( "Hurtbox" );

            this.baseCannonRot = this.cannonTransform.localRotation;
            this.baseCannonPos = this.cannonTransform.localPosition;
            this.baseHeadRot = this.headTransform.localRotation;
            this.baseHeadPos = this.headTransform.localPosition;

            this.offset1 = this.headTransform.position;
            this.offset1 = this.modelTransform.InverseTransformPoint( this.offset1 );

        }

        public void LateUpdate()
        {
            if( !this.headTransform ) return;
            if( !this.cannonTransform ) return;
            if( !this.refHeadTransform ) return;
            if( !this.refCannonTransform ) return;
            if( !this.modelTransform ) return;
            if( !this.aa ) return;

            this.DoAimAnimation( Time.deltaTime );

            Vector3 aimDirection = Vector3.Normalize(this.modelTransform.TransformDirection(this.finalDirection));


            if( this.transition )
            {
                this.timer += Time.deltaTime;
                if( this.timer >= this.maxTime ) this.transition = false;
            }

            if( this.cannonMode )
            {
                Vector3 vec;
                RaycastHit rh;
                Ray r = new Ray(this.input.aimOrigin, this.input.aimDirection);

                if( Physics.Raycast( r, out rh, Skills.Special.Incineration.baseMaxRange, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    vec = rh.point;
                } else
                {
                    vec = r.GetPoint( Skills.Special.Incineration.baseMaxRange );
                }

                Quaternion rotation = Quaternion.Slerp( Quaternion.LookRotation(Vector3.Normalize(vec - this.headTransform.position), this.modelTransform.up), Quaternion.LookRotation(Vector3.Normalize(input.aimDirection) , this.modelTransform.up ), 0.25f );
                //Quaternion headRot = Quaternion.LookRotation(input.aimDirection, modelTransform.forward);

                if( this.transition )
                {
                    this.cannonTransform.rotation = Quaternion.RotateTowards( this.cannonTransform.rotation, rotation, 90f * (Time.deltaTime / this.maxTime) );
                    //headTransform.rotation = Quaternion.RotateTowards(headTransform.rotation, headRot, 90f * (Time.deltaTime / maxTime));
                } else
                {

                    this.cannonTransform.rotation = Quaternion.RotateTowards( this.cannonTransform.rotation, rotation, this.cannonAimSpeed * Time.deltaTime );
                    //headTransform.rotation = headRot;
                }
            } else
            {
                if( this.transition )
                {
                    this.cannonTransform.localRotation = Quaternion.RotateTowards( this.cannonTransform.localRotation, this.baseCannonRot, 90f * (Time.deltaTime / this.maxTime) );
                } else
                {
                    this.cannonTransform.localRotation = this.baseCannonRot;
                }
                this.cannonTransform.localPosition = this.baseCannonPos;
                this.headTransform.rotation = Quaternion.LookRotation( aimDirection, this.refHeadTransform.up );
            }
        }

        public void StartCannonMode( System.Single transTime, System.Single cannonAimSpeed )
        {
            this.timer = 0f;
            this.maxTime = transTime;
            this.cannonAimSpeed = cannonAimSpeed;
            this.cannonMode = true;
            this.transition = true;
            this.headTransform.localEulerAngles = new Vector3( 0f, -90f, 0f );
            this.aa.fullYaw = true;
        }

        public void EndCannonMode( System.Single transTime )
        {
            this.timer = 0f;
            this.maxTime = transTime;
            this.cannonMode = false;
            this.transition = true;
            this.aa.fullYaw = false;
        }


        private void DoAimAnimation( System.Single t )
        {
            System.Single[] angs = this.ReadAimAngles();

            System.Single pitchInRad = angs[0] * Mathf.Deg2Rad;
            System.Single yawInRad = angs[1] * Mathf.Deg2Rad;

            System.Single sinPitch = Mathf.Sin( pitchInRad );
            System.Single cosPitch = Mathf.Cos( pitchInRad );
            System.Single sinYaw = Mathf.Sin( yawInRad );
            System.Single cosYaw = Mathf.Cos( yawInRad );

            this.finalDirection = new Vector3( -cosPitch * sinYaw, sinPitch, -cosPitch * cosYaw );
            this.finalDirection *= -1f;
        }

        private System.Single[] ReadAimAngles()
        {
            System.Single[] ret = new System.Single[2];

            FieldInfo f = typeof(AimAnimator).GetField("currentLocalAngles", BindingFlags.NonPublic | BindingFlags.Instance);
            System.Object v = f.GetValue( this.aa );

            ret[0] = v.GetFieldValue<System.Single>( "pitch" );
            ret[1] = v.GetFieldValue<System.Single>( "yaw" );

            return ret;
        }
    }
}
