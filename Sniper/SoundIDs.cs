


namespace Sniper.Properties
{
	using System;
	using System.Runtime.CompilerServices;

	internal static class SoundIDConsts
	{
		#pragma warning disable IDE1006 // Naming Styles
		internal const UInt32 Bolt_New_Bullet_Trash = 339887885u;
		internal const UInt32 Bolt_Normal_Shot = 763788813u;
		internal const UInt32 Bolt_Quickscope = 800730984u;
		internal const UInt32 Knife_Projectile_Organic_hit = 1183426810u;
		internal const UInt32 Bolt_Scatter_shot = 1314071446u;
		internal const UInt32 Bolt_Open_Chamber = 1389001356u;
		internal const UInt32 Bolt_New_Bullet_Best = 1939097407u;
		internal const UInt32 Bolt_Plasma_Shot = 2491373792u;
		internal const UInt32 Bolt_New_Bullet_Good = 2811185582u;
		internal const UInt32 Knife_Projectile_Metallic_hit = 2965791104u;
		internal const UInt32 Bolt_Ricochet = 3672228492u;
		internal const UInt32 Sniper_Charge_Amount = 135031646u;
		internal const UInt32 Volume_SFX = 3673881719u;
		#pragma warning restore IDE1006 // Naming Styles
	}

	internal enum Sounds : UInt32
	{
		Bolt_New_Bullet_Trash = 339887885u,
		Bolt_Normal_Shot = 763788813u,
		Bolt_Quickscope = 800730984u,
		Knife_Projectile_Organic_hit = 1183426810u,
		Bolt_Scatter_shot = 1314071446u,
		Bolt_Open_Chamber = 1389001356u,
		Bolt_New_Bullet_Best = 1939097407u,
		Bolt_Plasma_Shot = 2491373792u,
		Bolt_New_Bullet_Good = 2811185582u,
		Knife_Projectile_Metallic_hit = 2965791104u,
		Bolt_Ricochet = 3672228492u,
		Sniper_Charge_Amount = 135031646u,
		Volume_SFX = 3673881719u,
	}

	internal static class SoundsExtensions
	{
		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		internal static UInt32 ID(this Sounds sound )
		{
			return (UInt32)sound;
		}
	}
}