using System;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using KinematicCharacterController;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    public static class _KinematicCharacterMotor
    {
        private static Accessor<KinematicCharacterMotor,Single> _capsuleRadius = new Accessor<KinematicCharacterMotor, Single>( "CapsuleRadius" );
        private static Accessor<KinematicCharacterMotor,Single> _capsuleHeight = new Accessor<KinematicCharacterMotor, Single>( "CapsuleHeight" );
        private static Accessor<KinematicCharacterMotor,Single> _capsuleYOffset = new Accessor<KinematicCharacterMotor, Single>( "CapsuleYOffset" );

        public static Single _GetCapsuleRadius( this KinematicCharacterMotor inst )
        {
            return _capsuleRadius.Get( inst );
        }
        public static void _SetCapsuleRadius( this KinematicCharacterMotor inst, Single value )
        {
            _capsuleRadius.Set( inst, value );
        }

        public static Single _GetCapsuleHeight( this KinematicCharacterMotor inst )
        {
            return _capsuleHeight.Get( inst );
        }
        public static void _SetCapsuleHeight( this KinematicCharacterMotor inst, Single value )
        {
            _capsuleHeight.Set( inst, value );
        }

        public static Single _GetCapsuleYOffset( this KinematicCharacterMotor inst )
        {
            return _capsuleYOffset.Get( inst );
        }
        public static void _SetCapsuleYOffset( this KinematicCharacterMotor inst, Single value )
        {
            _capsuleYOffset.Set( inst, value );
        }
    }
}
