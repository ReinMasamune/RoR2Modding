namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.IO;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using Sniper.Data;
    using Sniper.ScriptableObjects.Custom;
    using UnityEngine;
    using Unity.Jobs;

    internal static class TextureModule
	{
		static TextureModule()
		{
			standardRamp = TexturesCore.GenerateRampTextureBatch
			(
				aKeys: new[]
				{
					new GradientAlphaKey(0f, 0f),
					new GradientAlphaKey(1f, 1f),
				},
				cKeys: new[]
				{
					new GradientColorKey(Color.black,0f),
					new GradientColorKey(new Color( 0.3f, 0.5f, 0.3f), 0.4f ),
					new GradientColorKey(new Color( 0.6f, 0.8f, 0.6f ), 0.95f ),
					new GradientColorKey(Color.white,1f),
				}
			);


			explosiveRamp = TexturesCore.GenerateRampTextureBatch
			(
				aKeys: new[]
				{
					new GradientAlphaKey(0f, 0f),
					new GradientAlphaKey(1f, 1f),
				},
				cKeys: new[]
				{
					new GradientColorKey(Color.black,0f),
					new GradientColorKey(new Color( 0.5f, 0.4f, 0.3f), 0.4f ),
					new GradientColorKey(new Color( 0.8f, 0.65f, 0.4f ), 0.95f ),
					new GradientColorKey(Color.white,1f),
				}
			);


			scatterRamp = TexturesCore.GenerateRampTextureBatch
			(
				aKeys: new[]
				{
					new GradientAlphaKey(0f, 0f),
					new GradientAlphaKey(1f, 1f),
				},
				cKeys: new[]
				{
					new GradientColorKey(Color.black,0f),
					new GradientColorKey(new Color( 0.4f, 0.4f, 0.4f), 0.4f ),
					new GradientColorKey(new Color( 0.8f, 0.8f, 0.8f ), 0.95f ),
					new GradientColorKey(Color.white,1f),
				}
			);


			plasmaRamp = TexturesCore.GenerateRampTextureBatch
			(
				aKeys: new[]
				{
					new GradientAlphaKey(0f, 0f),
					new GradientAlphaKey(1f, 1f),
				},
				cKeys: new[]
				{
					new GradientColorKey(Color.black,0f),
					new GradientColorKey(new Color( 0.5f, 0.3f, 0.5f), 0.4f ),
					new GradientColorKey(new Color( 0.8f, 0.6f, 0.8f ), 0.95f ),
					new GradientColorKey(Color.white,1f),
				}
			);


			shockRamp = TexturesCore.GenerateRampTextureBatch
			(
				aKeys: new[]
				{
					new GradientAlphaKey(0f, 0f),
					new GradientAlphaKey(1f, 1f),
				},
				cKeys: new[]
				{
					new GradientColorKey(Color.black,0f),
					new GradientColorKey(new Color( 0.2f, 0.2f, 0.5f), 0.4f ),
					new GradientColorKey(new Color( 0.5f, 0.5f, 0.8f ), 0.95f ),
					new GradientColorKey(Color.white,1f),
				}
			);

            JobHandle.ScheduleBatchedJobs();
		}
		private static ITextureJob standardRamp;
		private static ITextureJob explosiveRamp;
		private static ITextureJob scatterRamp;
		private static ITextureJob plasmaRamp;
		private static ITextureJob shockRamp;

		internal static Texture2D GetStandardAmmoRamp()
		{
			if( standardAmmoRamp == null )
			{
				standardAmmoRamp = standardRamp.OutputTextureAndDispose();
				standardRamp = null;
			}

			return standardAmmoRamp;
		}
		private static Texture2D standardAmmoRamp;

		internal static Texture2D GetExplosiveAmmoRamp()
		{
			if( explosiveAmmoRamp == null )
			{
				explosiveAmmoRamp = explosiveRamp.OutputTextureAndDispose();
				explosiveRamp = null;
			}

			return explosiveAmmoRamp;
		}
		private static Texture2D explosiveAmmoRamp;

		internal static Texture2D GetScatterAmmoRamp()
		{
			if( scatterAmmoRamp == null )
			{
				scatterAmmoRamp = scatterRamp.OutputTextureAndDispose();
				scatterRamp = null;
			}

			return scatterAmmoRamp;
		}
		private static Texture2D scatterAmmoRamp;

		internal static Texture2D GetPlasmaAmmoRamp()
		{
			if( plasmaAmmoRamp == null )
			{
				plasmaAmmoRamp = plasmaRamp.OutputTextureAndDispose();
				plasmaRamp = null;
			}

			return plasmaAmmoRamp;
		}
		private static Texture2D plasmaAmmoRamp;

		internal static Texture2D GetShockAmmoRamp()
		{
			if( shockAmmoRamp == null )
			{
				shockAmmoRamp = shockRamp.OutputTextureAndDispose();
				shockRamp = null;
			}

			return shockAmmoRamp;
		}
		private static Texture2D shockAmmoRamp;



		private static SniperTextureSet masterSet;
		
		private static void ScanAndLoadPacks()
		{
			masterSet = AssetModule.GetSniperAssetBundle().LoadAllAssets<SniperTextureSet>()[0];

			DirectoryInfo dir = new FileInfo( Assembly.GetExecutingAssembly().Location ).Directory;

			foreach( FileInfo file in dir.EnumerateFiles("*-tex") )
			{
				try
				{
					var bundle = AssetBundle.LoadFromFile(file.FullName);
					foreach( SniperTextureSet set in bundle.LoadAllAssets<SniperTextureSet>() )
					{
						masterSet.MergeAndReplace( set );
					}
				} catch( Exception e ) 
				{
					Log.Error( e );
				}
			}

		}


		private static SniperTextureSet GetMasterSet()
		{
			if( masterSet == null )
            {
                ScanAndLoadPacks();
            }

            return masterSet;
		}


		internal static TextureSet GetSniperDefaultTextures()
		{
			if( _sniperDefaultTextures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.SniperDefault_Diffuse];
				Texture2D tex2 = set[Properties.Resources.SniperDefault_Normal];
				Texture2D tex3 = set[Properties.Resources.SniperDefault_Emissive];

				_sniperDefaultTextures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperDefaultTextures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _sniperDefaultTextures;
#pragma warning restore IDE1006 // Naming Styles

		internal static TextureSet GetSniperSkin1Textures()
		{
			if( _sniperSkin1Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.SniperSkin1_Diffuse];
				Texture2D tex2 = set[Properties.Resources.SniperSkin1_Normal];
				Texture2D tex3 = set[Properties.Resources.SniperSkin1_Emissive];

				_sniperSkin1Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperSkin1Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _sniperSkin1Textures;
#pragma warning restore IDE1006 // Naming Styles

		internal static TextureSet GetSniperSkin2Textures()
		{
			if( _sniperSkin2Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.SniperSkin2_Diffuse];
				Texture2D tex2 = set[Properties.Resources.SniperSkin2_Normal];
				Texture2D tex3 = set[Properties.Resources.SniperSkin2_Emissive];

				_sniperSkin2Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperSkin2Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _sniperSkin2Textures;
#pragma warning restore IDE1006 // Naming Styles

		internal static TextureSet GetSniperSkin3Textures()
		{
			if( _sniperSkin3Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.SniperSkin3_Diffuse];
				Texture2D tex2 = set[Properties.Resources.SniperSkin3_Normal];
				Texture2D tex3 = set[Properties.Resources.SniperSkin3_Emissive];

				_sniperSkin3Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperSkin3Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _sniperSkin3Textures;
#pragma warning restore IDE1006 // Naming Styles


		internal static TextureSet GetSniperSkin4Textures()
		{
			if( _sniperSkin4Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.SniperSkin4_Diffuse];
				Texture2D tex2 = set[Properties.Resources.SniperSkin4_Normal];
				Texture2D tex3 = set[Properties.Resources.SniperSkin4_Emissive];

				_sniperSkin4Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperSkin4Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _sniperSkin4Textures;
#pragma warning restore IDE1006 // Naming Styles

		internal static TextureSet GetSniperSkin5Textures()
		{
			if( _sniperSkin5Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.SniperSkin5_Diffuse];
				Texture2D tex2 = set[Properties.Resources.SniperSkin5_Normal];
				Texture2D tex3 = set[Properties.Resources.SniperSkin5_Emissive];

				_sniperSkin5Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperSkin5Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _sniperSkin5Textures;
#pragma warning restore IDE1006 // Naming Styles




		internal static TextureSet GetRailDefaultTextures()
		{
			if( _railDefaultTextures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.RailgunDefault_Diffuse];
				Texture2D tex2 = set[Properties.Resources.RailgunDefault_Normal];
				Texture2D tex3 = set[Properties.Resources.RailgunDefault_Emissive];

				_railDefaultTextures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railDefaultTextures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _railDefaultTextures;
#pragma warning restore IDE1006 // Naming Styles

		internal static TextureSet GetRailAlt1Textures()
		{
			if( _railAlt1Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.RailgunAlt1_Diffuse];
				Texture2D tex2 = set[Properties.Resources.RailgunAlt1_Normal];
				Texture2D tex3 = set[Properties.Resources.RailgunAlt1_Emissive];

				_railAlt1Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt1Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _railAlt1Textures;
#pragma warning restore IDE1006 // Naming Styles

		internal static TextureSet GetRailAlt2Textures()
		{
			if( _railAlt2Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.RailgunAlt2_Diffuse];
				Texture2D tex2 = set[Properties.Resources.RailgunAlt2_Normal];
				Texture2D tex3 = set[Properties.Resources.RailgunAlt2_Emissive];

				_railAlt2Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt2Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _railAlt2Textures;
#pragma warning restore IDE1006 // Naming Styles

		internal static TextureSet GetRailAlt3Textures()
		{
			if( _railAlt3Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.RailgunAlt3_Diffuse];
				Texture2D tex2 = set[Properties.Resources.RailgunAlt3_Normal];
				Texture2D tex3 = set[Properties.Resources.RailgunAlt3_Emissive];

				_railAlt3Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt3Textures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _railAlt3Textures;
#pragma warning restore IDE1006 // Naming Styles




		internal static TextureSet GetThrowKnifeDefaultTextures()
		{
			if( _throwKnifeDefaultTextures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.ThrowKnifeDefault_Diffuse];
				Texture2D tex2 = set[Properties.Resources.ThrowKnifeDefault_Normal];
				Texture2D tex3 = set[Properties.Resources.ThrowKnifeDefault_Emissive];

				_throwKnifeDefaultTextures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeDefaultTextures;
		}
#pragma warning disable IDE1006 // Naming Styles
		private static TextureSet _throwKnifeDefaultTextures;
#pragma warning restore IDE1006 // Naming Styles
	}

}
