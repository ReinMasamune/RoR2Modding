using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using static WispSurvivor.Helpers.APIInterface;
using static WispSurvivor.Helpers.ComponentHelpers;

namespace WispSurvivor
{
    using static WispSurvivor.ComponentExtensions.ComponentExtensions;
    public partial class WispSurvivorMain
    {
        private static class WispComponentModule
        {
            public static Dictionary<Type, Component> DoModule( GameObject body )
            {
                Dictionary<Type, Component> components = new Dictionary<Type, Component>();

                RemoveComponents( body );
                AddComponents( body, components );
                GetComponents( body, components );

                return components;
            }

            private static void RemoveComponents( GameObject body )
            {
                MonoBehaviour.Destroy( body.GetComponent<RigidbodyDirection>() );
                MonoBehaviour.Destroy( body.GetComponent<RigidbodyMotor>() );
                MonoBehaviour.Destroy( body.GetComponent<QuaternionPID>() );
                MonoBehaviour.Destroy( body.GetComponent<DeathRewards>() );
                foreach( VectorPID c in body.GetComponents<VectorPID>() )
                {
                    MonoBehaviour.Destroy( c );
                }
                MonoBehaviour.DestroyImmediate( body.GetComponent<CapsuleCollider>() );

            }

            private static void AddComponents( GameObject body, Dictionary<Type, Component> dic )
            {
                body.AddOrGetComponentToDictionary<CharacterMotor>( dic );
                body.AddOrGetComponentToDictionary<CharacterDirection>( dic );
                body.AddOrGetComponentToDictionary<KinematicCharacterController.KinematicCharacterMotor>( dic );
                body.AddOrGetComponentToDictionary<Components.WispPassiveController>( dic );
                body.AddOrGetComponentToDictionary<Components.WispFlamesController>( dic );
                body.AddOrGetComponentToDictionary<Components.WispUIController>( dic ).passive = dic.C<Components.WispPassiveController>();
                body.AddOrGetComponentToDictionary<NetworkStateMachine>( dic );
                body.AddComponentToDictionary<EntityStateMachine>( dic ).customName = "Gaze";
                body.AddOrGetComponentToDictionary<CapsuleCollider>( dic );
                body.AddOrGetComponentToDictionary<SetStateOnHurt>( dic );
                body.AddOrGetComponentToDictionary<Components.WispFlareController>( dic );
                body.AddOrGetComponentToDictionary<Misc.ClientOrbController>( dic );
            }

            private static void GetComponents( GameObject body, Dictionary<Type, Component> dic )
            {
                body.AddOrGetComponentToDictionary<ModelLocator>( dic );
                body.AddOrGetComponentToDictionary<Rigidbody>( dic );
                body.AddOrGetComponentToDictionary<CharacterBody>( dic );
                body.AddOrGetComponentToDictionary<CameraTargetParams>( dic );
                body.AddOrGetComponentToDictionary<CharacterDeathBehavior>( dic );
            }
        }
    }
    namespace WispSurvivor.ComponentExtensions
    {
        internal static class ComponentExtensions
        {
            internal static T AddOrGetComponentToDictionary<T>( this GameObject g, Dictionary<Type, Component> dic ) where T : Component
            {
                T comp = g.AddOrGetComponent<T>();
                dic.Add( typeof( T ), comp );
                return comp;
            }

            internal static T AddComponentToDictionary<T>( this GameObject g, Dictionary<Type, Component> dic ) where T : Component
            {
                T comp = g.AddComponent<T>();
                dic.Add( typeof( T ), comp );
                return comp;
            }

            internal static T C<T>( this Dictionary<Type, Component> dic ) where T : Component => dic[typeof( T )] as T;
        }
    }
}
