namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using KinematicCharacterController;
    using RoR2;
    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
    public static class _KinematicCharacterMotor
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Accessor<KinematicCharacterMotor,Single> _capsuleRadius = new Accessor<KinematicCharacterMotor, Single>( "CapsuleRadius" );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Accessor<KinematicCharacterMotor,Single> _capsuleHeight = new Accessor<KinematicCharacterMotor, Single>( "CapsuleHeight" );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Accessor<KinematicCharacterMotor,Single> _capsuleYOffset = new Accessor<KinematicCharacterMotor, Single>( "CapsuleYOffset" );
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static Single _GetCapsuleRadius( this KinematicCharacterMotor inst ) => _capsuleRadius.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetCapsuleRadius( this KinematicCharacterMotor inst, Single value ) => _capsuleRadius.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static Single _GetCapsuleHeight( this KinematicCharacterMotor inst ) => _capsuleHeight.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetCapsuleHeight( this KinematicCharacterMotor inst, Single value ) => _capsuleHeight.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static Single _GetCapsuleYOffset( this KinematicCharacterMotor inst ) => _capsuleYOffset.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetCapsuleYOffset( this KinematicCharacterMotor inst, Single value ) => _capsuleYOffset.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
