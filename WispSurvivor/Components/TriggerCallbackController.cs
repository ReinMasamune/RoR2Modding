#if ANCIENTWISP
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        internal class TriggerCallbackController : MonoBehaviour
        {
            internal Rigidbody rb;
            private LineRenderer line;
            private Collider col;
            private Vector3[] corners;


            private static TriggerCallbackController Create( GameObject obj, Collider collider, Boolean continuous, LayerIndex layer, Boolean debugVisible )
            {
                obj.layer = layer.intVal;
                var call = obj.AddComponent<TriggerCallbackController>();
                if( continuous )
                {
                    var rb = obj.AddComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    call.rb = rb;
                }
                if( debugVisible )
                {
                    call.line = obj.AddComponent<LineRenderer>();
                    var bounds = collider.bounds;
                    call.line.positionCount = 8;
                    var extX = bounds.extents.x;
                    var extY = bounds.extents.y;
                    var extZ = bounds.extents.z;
                    var corners = new[]
                    {
                        bounds.center + new Vector3( -extX, extY, -extZ ),
                        bounds.center + new Vector3( -extX, extY, extZ ),
                        bounds.center + new Vector3( -extX, -extY, extZ ),
                        bounds.center + new Vector3( -extX, -extY, -extZ ),
                        bounds.center + new Vector3( extX, -extY, -extZ ),
                        bounds.center + new Vector3( extX, -extY, extZ ),
                        bounds.center + new Vector3( extX, extY, extZ ),
                        bounds.center + new Vector3( extX, extY, -extZ ),
                    };
                    
                    call.corners = corners;
                    call.line.SetPositions( corners );
                    call.line.loop = true;
                    call.line.startWidth = 0.1f;
                    call.line.endWidth = 0.1f;
                    call.line.startColor = Color.white;
                    call.line.endColor = Color.white;
                }

                call.col = collider;
                return call;
            }
            internal static TriggerCallbackController CreateSphere( Single radius, Vector3 position, Boolean continuous, LayerIndex layer, Boolean debugVisible = false )
            {
                var obj = new GameObject("SphereCallback");
                if( debugVisible )
                {
                    var deb = GameObject.CreatePrimitive( PrimitiveType.Sphere );
                    deb.transform.parent = obj.transform;
                    deb.transform.localPosition = Vector3.zero;
                    deb.transform.localRotation = Quaternion.identity;
                    deb.transform.localScale = new Vector3( radius, radius, radius ) * 2f;
                    var rb = deb.GetComponent<Rigidbody>();
                    if( rb ) UnityEngine.Object.Destroy( rb );
                    var col = deb.GetComponent<SphereCollider>();
                    if( col ) UnityEngine.Object.Destroy( col );
                }


                obj.transform.position = position;
                var sphere = obj.AddComponent<SphereCollider>();
                sphere.radius = radius;
                sphere.isTrigger = true;
                return Create( obj, sphere, continuous, layer, debugVisible );
            }
            internal static TriggerCallbackController CreateBox( Vector3 center, Vector3 size, Quaternion rotation, Boolean continuous, LayerIndex layer, Boolean debugVisible = false )
            {
                var obj = new GameObject( "BoxCallback" );
                obj.transform.position = center;
                obj.transform.rotation = rotation;
                var box = obj.AddComponent<BoxCollider>();
                box.size = size;
                box.isTrigger = true;
                return Create( obj, box, continuous, layer, debugVisible );
            }
            internal enum CapsuleDirection { X = 0, Y = 1, Z = 2 }
            internal static TriggerCallbackController CreateCapsule( Vector3 center, CapsuleDirection direction, Single radius, Single height, Quaternion rotation, Boolean continuous, LayerIndex layer, Boolean debugVisible = false )
            {
                var obj = new GameObject( "CapsuleCallback" );
                obj.transform.position = center;
                obj.transform.rotation = rotation;
                var cap = obj.AddComponent<CapsuleCollider>();
                cap.direction = (Int32)direction;
                cap.radius = radius;
                cap.height = height;
                cap.isTrigger = true;
                return Create( obj, cap, continuous, layer, debugVisible );
            }
            internal void SetDebugColor( Color color )
            {

            }
            internal void Cleanup()
            {
                if( this != null && base.gameObject != null )
                {
                    UnityEngine.Object.Destroy( base.gameObject );
                }
            }
            internal event Action<Collider> onTriggerEnter;
            internal event Action<Collider> onTriggerExit;
            internal event Action<Collider> onTriggerStay;

            private void Update()
            {
                if( this.line != null )
                {
                    var bounds = this.col.bounds;
                    var extX = bounds.extents.x;
                    var extY = bounds.extents.y;
                    var extZ = bounds.extents.z;
                    this.corners[0] = bounds.center + new Vector3( -extX, extY, -extZ );
                    this.corners[1] = bounds.center + new Vector3( -extX, extY, extZ );
                    this.corners[2] = bounds.center + new Vector3( -extX, -extY, extZ );
                    this.corners[3] = bounds.center + new Vector3( -extX, -extY, -extZ );
                    this.corners[4] = bounds.center + new Vector3( extX, -extY, -extZ );
                    this.corners[5] = bounds.center + new Vector3( extX, -extY, extZ );
                    this.corners[6] = bounds.center + new Vector3( extX, extY, extZ );
                    this.corners[7] = bounds.center + new Vector3( extX, extY, -extZ );
                    this.line.SetPositions( this.corners );
                }
            }
            private void OnTriggerEnter( Collider col )
            {
                this.onTriggerEnter?.Invoke( col );
            }
            private void OnTriggerExit( Collider col )
            {
                this.onTriggerExit?.Invoke( col );
            }
            private void OnTriggerStay( Collider col )
            {
                this.onTriggerStay?.Invoke( col );
            }
        }
    }
}
#endif