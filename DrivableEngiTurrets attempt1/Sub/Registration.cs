using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using R2API;
using R2API.Utils;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        private void RegisterBody()
        {
            // TODO: Implement
            RoR2.BodyCatalog.getAdditionalEntries += ( list ) =>
            {
                list.Add( body );
            };
        }

        private void RegisterMaster()
        {
            // TODO: Implement
            RoR2.MasterCatalog.getAdditionalEntries += ( list ) =>
            {
                list.Add( master );
            };
        }

        private void RegisterSkills()
        {
            RoR2Plugin.SkillsAPI.AddSkill( typeof( Skills.Engi.Special.PlaceOmegaTurret ) );
            RoR2Plugin.SkillsAPI.AddSkill( typeof( Skills.Other.DriverState ) );
        }
    }
}
