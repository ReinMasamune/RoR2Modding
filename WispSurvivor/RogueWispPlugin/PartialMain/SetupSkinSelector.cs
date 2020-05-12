using System;
using System.Linq.Expressions;

using Mono.Cecil.Cil;

using MonoMod.Cil;

using Rein.RogueWispPlugin.Helpers;

using ReinCore;

using RoR2;
using RoR2.UI;

using UnityEngine;
using UnityEngine.UI;

namespace Rein.RogueWispPlugin
{
	internal partial class Main
	{
		private static Func<CharacterSelectController,LocalUser> getLocalUser;

		partial void SetupSkinSelector()
		{
			this.Enable += this.Main_Enable1;
			this.Disable += this.Main_Disable1;

			var instanceParam = Expression.Parameter(typeof(CharacterSelectController), "instance" );
			var fieldAccess = Expression.Field( instanceParam, "localUser" );
			getLocalUser = Expression.Lambda<Func<CharacterSelectController, LocalUser>>( fieldAccess, instanceParam ).Compile();
		}

		private void Main_Disable1()
		{
			HooksCore.RoR2.UI.LoadoutPanelController.Rebuild.Il -= this.Rebuild_Il;
			HooksCore.RoR2.UI.CharacterSelectController.RebuildLocal.Il -= this.RebuildLocal_Il;
			HooksCore.RoR2.UI.CharacterSelectController.Awake.On -= this.Awake_On1;
			HooksCore.RoR2.UI.CharacterSelectController.OnNetworkUserLoadoutChanged.Il -= this.OnNetworkUserLoadoutChanged_Il;
		}
		private void Main_Enable1()
		{
			HooksCore.RoR2.UI.LoadoutPanelController.Rebuild.Il += this.Rebuild_Il;
			HooksCore.RoR2.UI.CharacterSelectController.RebuildLocal.Il += this.RebuildLocal_Il;
			HooksCore.RoR2.UI.CharacterSelectController.Awake.On += this.Awake_On1;
			HooksCore.RoR2.UI.CharacterSelectController.OnNetworkUserLoadoutChanged.Il += this.OnNetworkUserLoadoutChanged_Il;
		}

		private void OnNetworkUserLoadoutChanged_Il( ILContext il )
		{
			ILCursor c = new ILCursor( il );

			c.GotoNext( MoveType.After, x => x.MatchCallvirt<RoR2.Loadout.BodyLoadoutManager>( "GetSkinIndex" ) );
			c.Emit( OpCodes.Ldloc_3 );
			c.Emit( OpCodes.Ldloc_1 );
			c.EmitDelegate<Func<UInt32, Int32, CharacterSelectController.CharacterPad, UInt32>>( ( skinIndex, bodyIndex, pad ) =>
			{
				if( bodyIndex == BodyCatalog.FindBodyIndex( this.RW_body ) )
				{
					var controller = pad.displayInstance?.GetComponent<BitSkinController>();
					if( !controller ) controller = pad.displayInstance?.GetComponentInChildren<BitSkinController>();
					controller?.Apply( WispBitSkin.GetWispSkin( skinIndex ) );
				}

				return skinIndex;
			} );
		}
		private void Awake_On1( HooksCore.RoR2.UI.CharacterSelectController.Awake.Orig orig, CharacterSelectController self )
		{
			menuEditsInEffect = false;
			wispySkinHeader = default;
			defaultImagesLength = self.primaryColorImages.Length;
			defaultTextLength = self.primaryColorTexts.Length;
			orig( self );
		}
		private void RebuildLocal_Il( ILContext il )
		{
			ILCursor c = new ILCursor( il );

			var label = default(ILLabel);

			c.GotoNext( MoveType.Before,
				x => x.MatchStloc( 7 ),
				x => x.MatchLdloc( 2 ),
				x => x.MatchLdcI4( -1 ),
				x => x.MatchBeq( out label )
			);

			c.GotoLabel( label );
			c.Emit( OpCodes.Ldloc_2 );
			c.Emit( OpCodes.Ldarg_0 );

			c.EmitDelegate<Action<Int32, CharacterSelectController>>( ( bodyIndex, controller ) =>
			{
				if( bodyIndex == BodyCatalog.FindBodyIndex( this.RW_body ) )
				{
					ApplyMenuEdits( bodyIndex, controller );
				} else
				{
					RemoveMenuEdits( controller );
				}
			} );
		}
		private void Rebuild_Il( ILContext il )
		{
			ILCursor c = new ILCursor( il );

			c.GotoNext( MoveType.Before, x => x.MatchCall( typeof( RoR2.BodyCatalog ), nameof( RoR2.BodyCatalog.GetBodySkins ) ) );
			c.Remove();
			c.EmitDelegate<Func<Int32, Boolean>>( ( index ) =>
			{
				return index != BodyCatalog.FindBodyIndex( this.RW_body );
			} );
			c.RemoveRange( 4 );
		}

		private static Int32 defaultImagesLength;
		private static Int32 defaultTextLength;
		private static Boolean menuEditsInEffect = false;
		private static HGHeaderNavigationController.Header wispySkinHeader;
		private static void ApplyMenuEdits( Int32 bodyIndex, CharacterSelectController controller )
		{

			if( menuEditsInEffect ) return;
			menuEditsInEffect = true;



			var headerPanelObj = controller.loadoutHeaderButton.transform.parent;

			var headerNavController = headerPanelObj.GetComponent<HGHeaderNavigationController>();



			if( headerNavController.headers.Length != 4 )
			{


				if( String.IsNullOrEmpty( wispySkinHeader.headerName ) )
				{

					HGHeaderNavigationController.Header refHeader = default;
					Int32 counter = 0;

					while( counter < 3 && String.IsNullOrEmpty( refHeader.headerName ) )
					{
						var temp = headerNavController.headers[counter++];
						if( temp.headerName == "Overview" )
						{
							refHeader = temp;
						}
					}


					if( !String.IsNullOrEmpty( refHeader.headerName ) )
					{
						var newButtonObj = UnityEngine.Object.Instantiate<GameObject>(refHeader.headerButton.gameObject, refHeader.headerButton.transform.parent );
						var newButton = newButtonObj.GetComponent<HGButton>();


						var buttonTrans = newButtonObj.transform.Find( "ButtonText" );

						var newButtonText = buttonTrans.GetComponent<HGTextMeshProUGUI>();

						var newButtonLang = newButtonObj.GetComponent<LanguageTextMeshController>();

						newButtonLang.token = "LOADOUT_SKIN";


						var newPanel = UnityEngine.Object.Instantiate<GameObject>(refHeader.headerRoot, refHeader.headerRoot.transform.parent );

						var skinUI = newPanel.AddComponent<WispSkinSelectionUI>();

						skinUI.SetData( bodyIndex, getLocalUser( controller ).userProfile );

						var panelTextObj = newPanel.transform.Find("TextMeshPro Text" );

						panelTextObj.GetComponent<HGTextMeshProUGUI>().text = "";


						wispySkinHeader = new HGHeaderNavigationController.Header
						{
							headerName = "Skins",
							headerButton = newButton,
							tmpHeaderText = newButtonText,
							headerRoot = newPanel,
						};

						Array.Resize<Image>( ref controller.primaryColorImages, defaultImagesLength + 1 );

						controller.primaryColorImages[defaultImagesLength] = newButtonObj.GetComponent<Image>();

					}

				}
				Array.Resize<HGHeaderNavigationController.Header>( ref headerNavController.headers, 4 );
				headerNavController.headers[3] = wispySkinHeader;
				wispySkinHeader.headerButton.gameObject.SetActive( true );
			}

		}

		private static void RemoveMenuEdits( CharacterSelectController controller )
		{
			if( !menuEditsInEffect ) return;
			menuEditsInEffect = false;

			var headerPanelObj = controller.loadoutHeaderButton.transform.parent;
			var headerNavController = headerPanelObj.GetComponent<HGHeaderNavigationController>();

			if( headerNavController.headers.Length != 3 )
			{
				Array.Resize<HGHeaderNavigationController.Header>( ref headerNavController.headers, 3 );
				wispySkinHeader.headerButton.gameObject.SetActive( false );
				wispySkinHeader.headerRoot.gameObject.SetActive( false );

				Array.Resize<Image>( ref controller.primaryColorImages, defaultImagesLength );
				if( headerNavController.currentHeaderIndex == 3 ) headerNavController.ChooseHeader( "Skills" );
			}
		}

	}
}
