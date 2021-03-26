namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MonoMod.Cil;

    using RoR2;

    using UnityEngine;

    using UnityObject = UnityEngine.Object;
    using Object = System.Object;

    using Bf = System.Reflection.BindingFlags;

    public static class BuffsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void Add(BuffDef def) => added.Add(def);

        public static event BuffAddDelegate getAdditionalEntries
        {
            add => value(added);
            remove { }
        }
        public delegate void BuffAddDelegate( List<BuffDef> buffList );

        private static readonly List<BuffDef> added = new();

        static BuffsCore()
        {
            HooksCore.RoR2.BuffCatalog.Init.Il += Init_Il;
            //HooksCore.RoR2.UI.BuffIcon.UpdateIcon.Il += UpdateIcon_Il;
            loaded = true;
        }

        //private static Sprite EmittedDelegate1(Sprite cur, BuffDef inDef) => inDef is CustomSpriteBuffDef def ? def.sprite : cur;

        //private static readonly MethodInfo image_set_sprite = typeof(UnityEngine.UI.Image)?.GetProperty(nameof(UnityEngine.UI.Image.sprite), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetSetMethod(true) ?? throw new MissingMemberException("No method set_sprite on image for bufficon");        
        //private static void UpdateIcon_Il(ILContext il) => new ILCursor(il)
        //    .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(image_set_sprite))
        //    .LdLoc_(0)
        //    .CallDel_<Func<Sprite, BuffDef, Sprite>>(EmittedDelegate1);

        private static void Init_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(typeof(BuffCatalog), nameof(BuffCatalog.SetBuffDefs)))
            .CallDel_(ArrayHelper.AppendDel(added))
            .Dup_()
            .CallDel_<Action<BuffDef[]>>(ShimDefs);



        private static void ShimDefs(BuffDef[] defs)
        {
            for(Int32 i = 0; i < defs.Length; i++)
            {
                var def = defs[i];
                //Check managed null first
                if(def is null)
                {
                    //Leave this as is if its managed null. This func is a Shim, not a solution for idiocy.
                    continue;
                }

                //If unity null and not managed null
                if(!def)
                {
                    //Do all this in a try catch for obvious reasons
                    try
                    {
                        var newDef = ScriptableObject.CreateInstance<BuffDef>();

                        newDef.buffColor = def.buffColor;
                        newDef.buffIndex = def.buffIndex;
                        newDef.canStack = def.canStack;
                        newDef.eliteDef = def.eliteDef;
                        newDef.iconPath = def.iconPath;
                        newDef.iconSprite = def.iconSprite;
                        newDef.isDebuff = def.isDebuff;
                        newDef.name = def.name;
                        newDef.startSfx = def.startSfx;

                        //Another nested try catch just in case unity implodes. 
                        try
                        {
                            //Setting the m_CachedPtr will make some of the functionality of the unityobject work
                            //Namely, this should fix dictionaries and hashsets
                            var field = typeof(UnityObject).GetField("m_CachedPtr", Bf.Instance | Bf.NonPublic | Bf.Public);
                            field.SetValue(def, field.GetValue(newDef));

                        } catch(Exception e)
                        {
                            Log.Error($"Exception thrown when setting ptr value on survivordef at index {i}, the old def will not behave properly but the new one is fine.\r\nException: {e}");
                        }


                        defs[i] = newDef;
                        Log.Warning($"Successfully shimmed survivordef {newDef.name} at index {i}, please contact the author and ask them to migrate to the new systems");
                    } catch(Exception e)
                    {
                        Log.Error($"Exception thrown shimming index {i} in survivordefs, this entry has been skipped\r\nException: {e}");
                    }
                }
            }
        }
    }

    public class CustomSpriteBuffDef : BuffDef
    {
        public CustomSpriteBuffDef(Sprite sprite) => base.iconSprite = this.sprite = sprite;
        public readonly Sprite sprite;
    }
}