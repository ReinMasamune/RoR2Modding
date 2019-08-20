using RoR2;
using UnityEngine;
using RoR2.UI;
using RoR2.Projectile;
using R2API.Utils;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using EntityStates;
using System;

namespace ReinArtificerer
{
    public class ReinElementTracker : MonoBehaviour
    {
        public ReinDataLibrary data;
        public CharacterBody body;

        private Dictionary<Element, int> levels = new Dictionary<Element, int>();

        private Element lastUsed = Element.none;

        public enum Element
        {
            none = 0,
            fire = 1,
            ice = 2,
            lightning = 3
        }

        public Element GetMainElement()
        {
            int fLevel = levels[Element.fire];
            int iLevel = levels[Element.ice];
            int lLevel = levels[Element.lightning];

            if( fLevel > iLevel && fLevel > lLevel )
            {
                return Element.fire;
            }
            if( iLevel > lLevel && iLevel > fLevel )
            {
                return Element.ice;
            }
            if( lLevel > fLevel && lLevel > iLevel )
            {
                return Element.lightning;
            }

            return lastUsed;
        }

        public int GetElementLevel( Element el )
        {
            int mainLevel = levels[el];
            int bonusLevel = GetElemBonus(el);
            return mainLevel + bonusLevel;
        }

        public void AddElement(Element el, int level)
        {
            lastUsed = el;
            int tempLev = levels[el];
            tempLev += level;
            tempLev = Mathf.Clamp(tempLev, 0, data.el_maxElementLevel);
            levels[el] = tempLev;
        }

        public void ResetElement(Element el )
        {
            levels[el] = 0;
        }

        private int GetElemBonus(Element el )
        {
            int i = 0;
            Inventory inv = body.inventory;
            switch( el )
            {
                case Element.fire:
                    if( inv.currentEquipmentIndex == EquipmentIndex.AffixRed )
                    {
                        i += data.el_bonusFromAffixItem;
                    }
                    if( body.HasBuff(BuffIndex.AffixRed))
                    {
                        i += data.el_bonusFromAffixBuff;
                    }
                    return i;
                case Element.ice:
                    if( inv.currentEquipmentIndex == EquipmentIndex.AffixWhite )
                    {
                        i += data.el_bonusFromAffixItem;
                    }
                    if( body.HasBuff(BuffIndex.AffixWhite))
                    {
                        i += data.el_bonusFromAffixBuff;
                    }
                    return i;
                case Element.lightning:
                    if( inv.currentEquipmentIndex == EquipmentIndex.AffixBlue )
                    {
                        i += data.el_bonusFromAffixItem;
                    }
                    if( body.HasBuff(BuffIndex.AffixBlue ) )
                    {
                        i += data.el_bonusFromAffixBuff;
                    }
                    return i;
                default:
                    return i;
            }
        }
    }
}