namespace Rein.Sniper.Modules
{
	using System;
	using System.IO;
	using System.Reflection;

	using ReinCore;

	using Rein.Sniper.Data;
	using Rein.Sniper.ScriptableObjects.Custom;

	using Unity.Jobs;

	using UnityEngine;
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

		internal static Texture2D GetCrosshairTexture()
		{
			if( crosshairTexture == null )
			{
				crosshairTexture = LoadCrosshairTexture();
                if( crosshairTexture == null )
                {
                    Log.Error( "Could not load crosshair texture" );
                }
			}


			return crosshairTexture;
		}
		private static Texture2D crosshairTexture;
        private static Texture2D LoadCrosshairTexture()
        {
            return AssetModule.GetSniperAssetBundle().LoadAsset<Texture2D>( Properties.Resources.icon__CrosshairCenter );
        }

        internal static Sprite GetCrosshairSprite()
        {
            if( crosshairSprite is null )
            {
                crosshairSprite = LoadCrosshairSprite();
                if( crosshairSprite is null )
                {
                    Log.Error( "Could not load crosshair sprite" );
                }
            }

            return crosshairSprite;
        }
        private static Sprite crosshairSprite;
        private static Sprite LoadCrosshairSprite()
        {
            return AssetModule.GetSniperAssetBundle().LoadAsset<Sprite>( Properties.Resources.icon__CrosshairCenter );
        }


		private static SniperTextureSet masterSet;

		private static void ScanAndLoadPacks()
		{
			masterSet = AssetModule.GetSniperAssetBundle().LoadAllAssets<SniperTextureSet>()[0];

			DirectoryInfo dir = new FileInfo( Assembly.GetExecutingAssembly().Location ).Directory;

			foreach( FileInfo file in dir.EnumerateFiles( "*-tex" ) )
			{
				var bundle = AssetBundle.LoadFromFile(file.FullName);
				foreach( SniperTextureSet set in bundle.LoadAllAssets<SniperTextureSet>() )
				{
					masterSet.MergeAndReplace( set );
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


		internal static TextureSet GetSniperTextures()
		{
			if( _sniperTextures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__Sniper_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__Sniper_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__Sniper_Emissive];

				_sniperTextures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperTextures;
		}

		private static TextureSet _sniperTextures;


		internal static TextureSet GetSniperAlt1Textures()
		{
			if( _sniperAlt1Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__SniperAlt1_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__SniperAlt1_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__SniperAlt1_Emissive];

				_sniperAlt1Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperAlt1Textures;
		}

		private static TextureSet _sniperAlt1Textures;


		internal static TextureSet GetSniperAlt2Textures()
		{
			if( _sniperAlt2Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__SniperAlt2_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__SniperAlt2_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__SniperAlt2_Emissive];

				_sniperAlt2Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperAlt2Textures;
		}

		private static TextureSet _sniperAlt2Textures;


		internal static TextureSet GetSniperAlt3Textures()
		{
			if( _sniperAlt3Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__SniperAlt3_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__SniperAlt3_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__SniperAlt3_Emissive];

				_sniperAlt3Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperAlt3Textures;
		}

		private static TextureSet _sniperAlt3Textures;



		internal static TextureSet GetSniperAlt4Textures()
		{
			if( _sniperAlt4Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__SniperAlt4_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__SniperAlt4_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__SniperAlt4_Emissive];

				_sniperAlt4Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperAlt4Textures;
		}

		private static TextureSet _sniperAlt4Textures;


		internal static TextureSet GetSniperAlt5Textures()
		{
			if( _sniperAlt5Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__SniperAlt5_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__SniperAlt5_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__SniperAlt5_Emissive];

				_sniperAlt5Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperAlt5Textures;
		}

		private static TextureSet _sniperAlt5Textures;


		internal static TextureSet GetSniperAlt6Textures()
		{
			if( _sniperAlt6Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__SniperAlt6_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__SniperAlt6_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__SniperAlt6_Emissive];

				_sniperAlt6Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _sniperAlt6Textures;
		}

		private static TextureSet _sniperAlt6Textures;





		internal static TextureSet GetRailTextures()
		{
			if( _railTextures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__Railgun_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__Railgun_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__Railgun_Emissive];

				_railTextures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railTextures;
		}

		private static TextureSet _railTextures;


		internal static TextureSet GetRailAlt1Textures()
		{
			if( _railAlt1Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__RailgunAlt1_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__RailgunAlt1_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__RailgunAlt1_Emissive];

				_railAlt1Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt1Textures;
		}

		private static TextureSet _railAlt1Textures;


		internal static TextureSet GetRailAlt2Textures()
		{
			if( _railAlt2Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__RailgunAlt2_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__RailgunAlt2_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__RailgunAlt2_Emissive];

				_railAlt2Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt2Textures;
		}

		private static TextureSet _railAlt2Textures;


		internal static TextureSet GetRailAlt3Textures()
		{
			if( _railAlt3Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__RailgunAlt3_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__RailgunAlt3_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__RailgunAlt3_Emissive];

				_railAlt3Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt3Textures;
		}

		private static TextureSet _railAlt3Textures;


		internal static TextureSet GetRailAlt4Textures()
		{
			if( _railAlt4Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__RailgunAlt4_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__RailgunAlt4_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__RailgunAlt4_Emissive];

				_railAlt4Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt4Textures;
		}

		private static TextureSet _railAlt4Textures;


		internal static TextureSet GetRailAlt5Textures()
		{
			if( _railAlt5Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__RailgunAlt5_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__RailgunAlt5_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__RailgunAlt5_Emissive];

				_railAlt5Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt5Textures;
		}

		private static TextureSet _railAlt5Textures;


		internal static TextureSet GetRailAlt6Textures()
		{
			if( _railAlt6Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__RailgunAlt6_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__RailgunAlt6_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__RailgunAlt6_Emissive];

				_railAlt6Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _railAlt6Textures;
		}

		private static TextureSet _railAlt6Textures;





		internal static TextureSet GetThrowKnifeTextures()
		{
			if( _throwKnifeTextures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__ThrowKnife_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__ThrowKnife_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__ThrowKnife_Emissive];

				_throwKnifeTextures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeTextures;
		}
		private static TextureSet _throwKnifeTextures;

		internal static TextureSet GetThrowKnifeAlt1Textures()
		{
			if( _throwKnifeAlt1Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__ThrowKnifeAlt1_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__ThrowKnifeAlt1_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__ThrowKnifeAlt1_Emissive];

				_throwKnifeAlt1Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeAlt1Textures;
		}
		private static TextureSet _throwKnifeAlt1Textures;


		internal static TextureSet GetThrowKnifeAlt2Textures()
		{
			if( _throwKnifeAlt2Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__ThrowKnifeAlt2_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__ThrowKnifeAlt2_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__ThrowKnifeAlt2_Emissive];

				_throwKnifeAlt2Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeAlt2Textures;
		}
		private static TextureSet _throwKnifeAlt2Textures;


		internal static TextureSet GetThrowKnifeAlt3Textures()
		{
			if( _throwKnifeAlt3Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__ThrowKnifeAlt3_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__ThrowKnifeAlt3_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__ThrowKnifeAlt3_Emissive];

				_throwKnifeAlt3Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeAlt3Textures;
		}
		private static TextureSet _throwKnifeAlt3Textures;


		internal static TextureSet GetThrowKnifeAlt4Textures()
		{
			if( _throwKnifeAlt4Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__ThrowKnifeAlt4_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__ThrowKnifeAlt4_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__ThrowKnifeAlt4_Emissive];

				_throwKnifeAlt4Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeAlt4Textures;
		}
		private static TextureSet _throwKnifeAlt4Textures;


		internal static TextureSet GetThrowKnifeAlt5Textures()
		{
			if( _throwKnifeAlt5Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__ThrowKnifeAlt5_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__ThrowKnifeAlt5_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__ThrowKnifeAlt5_Emissive];

				_throwKnifeAlt5Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeAlt5Textures;
		}
		private static TextureSet _throwKnifeAlt5Textures;


		internal static TextureSet GetThrowKnifeAlt6Textures()
		{
			if( _throwKnifeAlt6Textures == null )
			{
				SniperTextureSet set = GetMasterSet();
				Texture2D tex1 = set[Properties.Resources.skin__ThrowKnifeAlt6_Diffuse];
				Texture2D tex2 = set[Properties.Resources.skin__ThrowKnifeAlt6_Normal];
				Texture2D tex3 = set[Properties.Resources.skin__ThrowKnifeAlt6_Emissive];

				_throwKnifeAlt6Textures = new TextureSet( tex1, tex2, tex3 );
			}

			return _throwKnifeAlt6Textures;
		}
		private static TextureSet _throwKnifeAlt6Textures;

	}

}
