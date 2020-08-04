namespace ReinCore
{
    using System;

    using KinematicCharacterController;

    public static class _KinematicCharacterMotor
    {
        //private static readonly Accessor<KinematicCharacterMotor,Single> _capsuleRadius = new Accessor<KinematicCharacterMotor, Single>( "CapsuleRadius" );

        //private static readonly Accessor<KinematicCharacterMotor,Single> _capsuleHeight = new Accessor<KinematicCharacterMotor, Single>( "CapsuleHeight" );

        //private static readonly Accessor<KinematicCharacterMotor,Single> _capsuleYOffset = new Accessor<KinematicCharacterMotor, Single>( "CapsuleYOffset" );

        [Obsolete( "unneeded", true )]
        public static Single _GetCapsuleRadius( this KinematicCharacterMotor inst ) => inst.CapsuleRadius;// _capsuleRadius.Get( inst );
        [Obsolete( "unneeded", true )]
        public static void _SetCapsuleRadius( this KinematicCharacterMotor inst, Single value ) => inst.CapsuleRadius = value;// => _capsuleRadius.Set( inst, value );
        [Obsolete( "unneeded", true )]
        public static Single _GetCapsuleHeight( this KinematicCharacterMotor inst ) => inst.CapsuleHeight;// _capsuleHeight.Get( inst );
        [Obsolete( "unneeded", true )]
        public static void _SetCapsuleHeight( this KinematicCharacterMotor inst, Single value ) => inst.CapsuleHeight = value;// => _capsuleHeight.Set( inst, value );
        [Obsolete( "unneeded", true )]
        public static Single _GetCapsuleYOffset( this KinematicCharacterMotor inst ) => inst.CapsuleYOffset;// _capsuleYOffset.Get( inst );
        [Obsolete( "unneeded", true )]
        public static void _SetCapsuleYOffset( this KinematicCharacterMotor inst, Single value ) => inst.CapsuleYOffset = value;// => _capsuleYOffset.Set( inst, value );

    }
}
