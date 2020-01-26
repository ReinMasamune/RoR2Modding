#if ANCIENTWISP
using R2API;
using RoR2;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void AW_CreateEffects()
        {
            this.Load += this.AW_SecondaryPrediction;
            this.Load += this.AW_SecondaryExplosion;
            this.Load += this.AW_PrimaryCharge;
            this.Load += this.AW_SpecialCharge;
            this.Load += this.AW_PrimaryGhost;
            this.Load += this.AW_SpecialGhost;
            this.Load += this.AW_SpecialZoneGhost;
        }

        private void AW_SpecialZoneGhost() => throw new System.NotImplementedException();
        private void AW_SpecialGhost() => throw new System.NotImplementedException();
        private void AW_PrimaryGhost() => throw new System.NotImplementedException();
        private void AW_SpecialCharge() => throw new System.NotImplementedException();
        private void AW_PrimaryCharge() => throw new System.NotImplementedException();

        private void AW_SecondaryExplosion()
        {
            var obj = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/MeteorStrikeImpact").InstantiateClone("AncientWispPillar", false);
            EffectAPI.AddEffect( obj );
            this.AW_secExplodeEffect = obj;
        }

        private void AW_SecondaryPrediction()
        {
            var obj = Resources.Load<GameObject>("Prefabs/Effects/MeteorStrikePredictionEffect").InstantiateClone("AncientWispPillarPrediction", false);
            Destroy( obj.GetComponent<DestroyOnTimer>() );
            Destroy( obj.GetComponent<EffectComponent>() );


            this.AW_secDelayEffect = obj;
        }
    }
}
#endif
