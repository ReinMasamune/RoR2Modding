using RoR2;
using UnityEngine;
using R2API.Utils;
using System.Reflection;

namespace WispSurvivor.Components
{
    public class WispAimAnimationController : MonoBehaviour
    {
        public bool cannonMode = false;

        private Vector3 finalDirection;
        private Vector3 baseCannonPos;
        private Vector3 baseHeadPos;
        private Vector3 offset1;
        private Quaternion baseHeadRot;
        private Quaternion baseCannonRot;

        private AimAnimator aa;
        private ModelLocator ml;
        private Transform headTransform;
        private Transform cannonTransform;
        private Transform modelTransform;
        private Transform refHeadTransform;
        private Transform refCannonTransform;

        public void Start()
        {
            ml = gameObject.GetComponent<ModelLocator>();
            modelTransform = ml.modelTransform;
            aa = modelTransform.GetComponent<AimAnimator>();
            refCannonTransform = modelTransform;
            cannonTransform = modelTransform.Find("CannonPivot");
            headTransform = cannonTransform.Find("AncientWispArmature").Find("Head");
            refHeadTransform = modelTransform.Find("Hurtbox");

            baseCannonRot = cannonTransform.localRotation;
            baseCannonPos = cannonTransform.localPosition;
            baseHeadRot = headTransform.localRotation;
            baseHeadPos = headTransform.localPosition;

            offset1 = headTransform.position;
            offset1 = modelTransform.InverseTransformPoint(offset1);
            
        }

        public void LateUpdate()
        {
            if (!headTransform) return;
            if (!cannonTransform) return;
            if (!refHeadTransform) return;
            if (!refCannonTransform) return;
            if (!modelTransform) return;
            if (!aa) return;

            DoAimAnimation(Time.deltaTime);

            Vector3 aimDirection = Vector3.Normalize(modelTransform.TransformDirection(finalDirection));
            headTransform.rotation = Quaternion.LookRotation(aimDirection, refHeadTransform.up);

            if ( cannonMode )
            {
                Quaternion rotation = Quaternion.LookRotation(aimDirection, modelTransform.up);
                cannonTransform.rotation = rotation;
            }
            else
            {
                cannonTransform.localRotation = baseCannonRot;
                cannonTransform.localPosition = baseCannonPos;
            }
        }

        private void DoAimAnimation(float t)
        {
            var angs = ReadAimAngles();

            float pitchInRad = angs[0] * Mathf.Deg2Rad;
            float yawInRad = angs[1] * Mathf.Deg2Rad;

            float sinPitch = Mathf.Sin(pitchInRad);
            float cosPitch = Mathf.Cos(pitchInRad);
            float sinYaw = Mathf.Sin(yawInRad);
            float cosYaw = Mathf.Cos(yawInRad);

            finalDirection = new Vector3(-cosPitch * sinYaw, sinPitch, -cosPitch * cosYaw);
            finalDirection *= -1f;
        }

        private float[] ReadAimAngles()
        {
            float[] ret = new float[2];

            FieldInfo f = typeof(AimAnimator).GetField("currentLocalAngles", BindingFlags.NonPublic | BindingFlags.Instance);
            var v = f.GetValue(aa);

            ret[0] = v.GetFieldValue<float>("pitch");
            ret[1] = v.GetFieldValue<float>("yaw");

            return ret;
        }
    }
}
