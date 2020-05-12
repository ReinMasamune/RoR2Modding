//using System;
//using BepInEx;
//using RoR2;
//using RoR2.Skills;
//using UnityEngine;

//namespace ReinCore
//{
//    public sealed class PassiveSkillDef<TInstanceData> : PassiveSkillDefBase where TInstanceData : SkillDef.BaseSkillInstanceData
//    {
//        public static PassiveSkillDef<TInstanceData> Create( 
//            CreateInstanceDataDelegate createInstanceData, 
//            OnAssignedDelegate onAssigned, 
//            OnUnassignedDelegate onUnassigned, 
//            GetCurrentIconDelegate getCurrentIcon,
//            CanExecuteDelegate canExecute,
//            IsReadyDelegate isReady,
//            FixedUpdateDelegate fixedUpdate
//        )
//        {
//            var skill = ScriptableObject.CreateInstance<PassiveSkillDef<TInstanceData>>();
//            skill.createInstanceData = createInstanceData;
//            skill.onAssigned = onAssigned;
//            skill.onUnassigned = onUnassigned;
//            skill.getCurrentIcon = getCurrentIcon;
//            skill.canExecute = canExecute;
//            skill.isReady = isReady;
//            skill.fixedUpdate = fixedUpdate;

//            return skill;
//        }

//        public TInstanceData instanceData
//        {
//            get => this.currentSkillSlot.skillInstanceData as TInstanceData;
//        }

//        public delegate TInstanceData CreateInstanceDataDelegate( PassiveSkillDef<TInstanceData> self, GenericSkill skillSlot );
//        public delegate void OnAssignedDelegate( PassiveSkillDef<TInstanceData> self, GenericSkill skillSlot, TInstanceData instanceData );
//        public delegate void OnUnassignedDelegate( PassiveSkillDef<TInstanceData> self, GenericSkill skillSlot, TInstanceData instanceData );
//        public delegate Sprite GetCurrentIconDelegate( PassiveSkillDef<TInstanceData> self, GenericSkill skillSlot, TInstanceData instanceData );
//        public delegate Boolean CanExecuteDelegate( PassiveSkillDef<TInstanceData> self, GenericSkill skilLSlot, TInstanceData instanceData );
//        public delegate Boolean IsReadyDelegate( PassiveSkillDef<TInstanceData> self, GenericSkill skillSlot, TInstanceData instanceData );
//        public delegate void FixedUpdateDelegate( PassiveSkillDef<TInstanceData> self, GenericSkill skillSlot, TInstanceData instanceData );


//        private CreateInstanceDataDelegate createInstanceData { get; set; }    
//        private OnAssignedDelegate onAssigned { get; set; }
//        private OnUnassignedDelegate onUnassigned { get; set; }
//        private GetCurrentIconDelegate getCurrentIcon { get; set; }
//        private CanExecuteDelegate canExecute { get; set; }
//        private IsReadyDelegate isReady { get; set; }
//        private FixedUpdateDelegate fixedUpdate { get; set; }

//        private GenericSkill currentSkillSlot { get; set; }



//        public sealed override SkillDef.BaseSkillInstanceData OnAssigned( GenericSkill skillSlot )
//        {
//            this.currentSkillSlot = skillSlot;
//            var data = this.createInstanceData( this, skillSlot );



//            return data;
//        }

//        //public sealed override OnUnassigned( GenericSkill skillSlot )
//        //{

//        //}

//    }
//}
