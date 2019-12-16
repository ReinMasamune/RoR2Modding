using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA.Components
{
    public class InputMaster : MonoBehaviour
    {
        public InputBankTest dummyInputs;

        private Boolean enabled = false;

        private CharacterBody body;
        private InputBankTest bodyInputs;
        private InputBankTest masterInputs;

        private CharacterBody origEngiBody;
        private CharacterMaster origEngiMaster;


        FieldInfo characterBody_InputBankTest;
        FieldInfo _aimDirection_InputBankTest;

        PropertyInfo bodyInstanceObject_CharacterMaster;


        public void SetMaster( GameObject obj )
        {
            //enabled = true;
            masterInputs = obj.GetComponent<InputBankTest>();

            //characterBody_InputBankTest.SetValue( bodyInputs, obj.GetComponent<CharacterBody>() );
            
            body.masterObject.GetComponent<BaseAI>().enabled = false;

            origEngiBody = obj.GetComponent<CharacterBody>();
            origEngiMaster = origEngiBody.master;

            //origEngiMaster.playerCharacterMasterController.InvokeMethod( "SetBody", gameObject );
            bodyInstanceObject_CharacterMaster.SetValue( origEngiMaster, gameObject );

            SetButtonsFalse( masterInputs );

        }

        public void UnsetMaster()
        {
            enabled = false;
            //masterInputs = null;

            //characterBody_InputBankTest.SetValue( bodyInputs, body );
            body.masterObject.GetComponent<BaseAI>().enabled = true;
            //origEngiMaster.playerCharacterMasterController.InvokeMethod( "SetBody", origEngiBody.gameObject );
            bodyInstanceObject_CharacterMaster.SetValue( origEngiMaster, origEngiBody.gameObject );
        }

        private void Awake()
        {
            bodyInputs = GetComponent<InputBankTest>();
            body = GetComponent<CharacterBody>();

            characterBody_InputBankTest = typeof( InputBankTest ).GetField( "characterBody", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static );
            _aimDirection_InputBankTest = typeof( InputBankTest ).GetField( "_aimDirection", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static );
            bodyInstanceObject_CharacterMaster = typeof( CharacterMaster ).GetProperty( "bodyInstanceObject", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static );
        }

        private void FixedUpdate()
        {
            if( enabled )
            {
                //bodyInputs.lookPitch = masterInputs.lookPitch;
                //bodyInputs.lookYaw = masterInputs.lookYaw;
                bodyInputs.moveVector = masterInputs.moveVector;
                bodyInputs.skill1 = masterInputs.skill1;
                bodyInputs.skill2 = masterInputs.skill2;
                bodyInputs.skill3 = masterInputs.skill3;
                bodyInputs.skill4 = masterInputs.skill4;

                bodyInputs.jump = masterInputs.jump;
                bodyInputs.sprint = masterInputs.sprint;
                bodyInputs.activateEquipment = masterInputs.activateEquipment;

                _aimDirection_InputBankTest.SetValue( bodyInputs, _aimDirection_InputBankTest.GetValue( masterInputs ) );
            }
        }

        private void Update()
        {
            if( enabled )
            {
                //bodyInputs.lookPitch = masterInputs.lookPitch;
                //bodyInputs.lookYaw = masterInputs.lookYaw;
                bodyInputs.moveVector = masterInputs.moveVector;
                bodyInputs.skill1 = masterInputs.skill1;
                bodyInputs.skill2 = masterInputs.skill2;
                bodyInputs.skill3 = masterInputs.skill3;
                bodyInputs.skill4 = masterInputs.skill4;

                bodyInputs.jump = masterInputs.jump;
                bodyInputs.sprint = masterInputs.sprint;
                bodyInputs.activateEquipment = masterInputs.activateEquipment;

                _aimDirection_InputBankTest.SetValue( bodyInputs, _aimDirection_InputBankTest.GetValue( masterInputs ) );
            }
        }

        private void SetButtonsFalse( InputBankTest input )
        {
            input.emoteRequest = -1;
            input.activateEquipment = FalseButton();
            input.interact = FalseButton();
            input.jump = FalseButton();
            input.ping = FalseButton();
            input.sprint = FalseButton();
            input.skill1 = FalseButton();
            input.skill2 = FalseButton();
            input.skill3 = FalseButton();
            input.skill4 = FalseButton();
        }

        private InputBankTest.ButtonState FalseButton()
        {
            return new InputBankTest.ButtonState
            {
                down = false,
                wasDown = false
            };
        }
    }
}
