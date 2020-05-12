using System;
using System.Collections.Generic;

using ReinCore;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class EffectBuilder
    {
        internal const String rootName = "Main";
        #region External facing
        internal struct EffectBaseParams
        {
            public String name;
            public Boolean useEffectComponent;
        }
        internal EffectBuilder( EffectBaseParams parameters )
        {
            if( materialLibrary == null )
            {
                BuildMaterialsLibrary();
            }
            this.name = parameters.name;

            this.effect = new GameObject( "effectTemp" );

            this.categories[rootName] = new EffectCategory( this.effect );
        }
        internal void CreateCategory( String name, String parent = "Main" )
        {
            if( this.categories.ContainsKey( name ) )
            {
                Main.LogE( name + " is already used in a category name." );
            }
        }



        internal GameObject Create( Boolean registerNetwork )
        {
            var clone = this.effect.ClonePrefab( this.name, registerNetwork);
            this.TransferCategories( clone );
            return clone;
        }
        #endregion

        private String name;
        private GameObject effect;
        private BuiltEffectController controller;


        #region Manager Component
        internal class BuiltEffectController : MonoBehaviour
        {
            internal void EnableCategory( String name )
            {
            }
            internal void DisableCategory( String name )
            {

            }

            internal Dictionary<String, EffectCategory> categories { get; private set; } = new Dictionary<String, EffectCategory>();
        }
        #endregion


        #region Categories
        private Dictionary<String, EffectCategory> categories = new Dictionary<String, EffectCategory>();
        private void TransferCategories( GameObject clone )
        {
            var control = clone.AddComponent<BuiltEffectController>();
            var newCats = control.categories;
            var newMain = clone.transform;

            var cats = this.categories;
            var main = cats[rootName].obj.transform;
            var mainCat = cats[rootName];

            var newMainCat = new EffectCategory( clone );
            newCats[rootName] = newMainCat;

            CopyCat( newMainCat, mainCat, newCats );
        }


        private static void CopyCat( EffectCategory copy, EffectCategory old, Dictionary<String, EffectCategory> catIndex )
        {
            var copyObj = copy.obj.transform;
            var copyRoot = catIndex[rootName].obj.transform;

            foreach( var child in old.childLoop )
            {
                var copyChild = copyObj.Find( child.name );
                if( copyChild == null )
                {
                    Main.LogE( "Couldn't find child named: " + child.name );
                    continue;
                }
                copy.AddChild( child.Duplicate( copyChild.gameObject, copy ) );
            }

            foreach( var childCat in old.childCatsLoop )
            {
                var path = childCat.path;
                var copyTemp = copyRoot.Find( path );
                if( copyTemp == null )
                {
                    Main.LogE( "Unable to find child at path: " + path );
                    continue;
                }
                var copyCat = new EffectCategory( childCat.name, copyTemp.gameObject, copy );
                catIndex[copyCat.name] = copyCat;
                CopyCat( copyCat, childCat, catIndex );
            }
        }




        #endregion
        #region Materials
        internal enum MaterialIndex
        {
            First = -1,
            Last = 0,
        }

        private static Dictionary<MaterialIndex,Material> materialLibrary;

        private static void BuildMaterialsLibrary()
        {

        }
        #endregion
    }
}
