namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RoR2;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public delegate Int32 DeployableSlotLimitDelegate( CharacterMaster master );

    /// <summary>
    /// 
    /// </summary>
    public class DeployableSlotDef
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitDelegate"></param>
        public DeployableSlotDef( DeployableSlotLimitDelegate limitDelegate )
        {
            this.GetLimit = limitDelegate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        public DeployableSlotDef( Int32 limit )
        {
            this.GetLimit = new DeployableSlotLimitDelegate( (_) => limit );
        }

        /// <summary>
        /// 
        /// </summary>
        public DeployableSlot slot { get; internal set; } = (DeployableSlot)(-1);

#pragma warning disable IDE1006 // Naming Styles
        internal DeployableSlotLimitDelegate GetLimit { get; private set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
