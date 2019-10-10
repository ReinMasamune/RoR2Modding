using BepInEx;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RoR2;
using UnityEngine;

namespace SurvivorUtils
{
    [BepInPlugin("com.ReinThings.SurvivorUtils", "ReinSurvivorUtils", "1.0.0")]
    public class SurvivorUtils : BaseUnityPlugin
    {
        private static bool eventRegistered = false;
        private static bool hasAlreadyAddedSurvivors = false;

        private static List<SurvivorDef> newSurvivors = new List<SurvivorDef>();
        private static List<string> modInfoList = new List<string>();
        private static List<string> survNameList = new List<string>();

        public static bool AddNewSurvivor( SurvivorDef survivor , string survivorName = "", [CallerFilePath] string file = null, [CallerMemberName] string name = null, [CallerLineNumber] int lineNumber = 0)
        {
            //Get a mod name from filepath
            string modName = "[Mod really messed up?]";
            int start = file.LastIndexOf('/');
            int end = file.LastIndexOf('.');
            end -= start;
            if (end > 0)
            {
                modName = file.Substring(start + 1, end);
            }
            else
            {
                modName = file.Substring(start + 1);
            }
            
            //Pre-generate the string that will be logged referring to this specific survivor
            string modInfo = "Mod: " + modName + " Method: " +  name + " Line: " + lineNumber.ToString();

            //Block this from running if the survivor list was already generated, return false so a mod can be aware of this in script
            if ( hasAlreadyAddedSurvivors )
            { 
                Debug.Log("Tried to add survivor after list was created at: " + modInfo);
                return false;
            }

            //Add the survivor to the temp list along with the name and modinfo
            newSurvivors.Add(survivor);
            modInfoList.Add(modInfo);
            survNameList.Add(survivorName);
            RegisterEvent();

            //Return true so a mod can be aware that their survivor was added successfully in script
            return true;
        }

        private static void RegisterEvent()
        {
            //Make sure we aren't registering the event to add survivors more than once
            if( eventRegistered )
            {
                return;
            }

            //Actually register the event
            RoR2.SurvivorCatalog.getAdditionalSurvivorDefs += AddSurvivorAction;
            eventRegistered = true;
        }

        private static void UnRegisterEvent()
        {
            //Unregister the event, it will not be called again anyway but good habits
            RoR2.SurvivorCatalog.getAdditionalSurvivorDefs -= AddSurvivorAction;
        }

        private static void AddSurvivorAction(List<SurvivorDef> obj)
        {
            //Set this to true so no more survivors can be added to the list while this is happening, or afterwards
            hasAlreadyAddedSurvivors = true;

            //Get the count of the new survivors added, and the number of vanilla survivors
            int count = newSurvivors.Count;
            int baseCount = SurvivorCatalog.idealSurvivorOrder.Length;

            //Increase the size of the order array to accomodate the added survivors
            Array.Resize<SurvivorIndex>(ref SurvivorCatalog.idealSurvivorOrder, baseCount + count);

            //Increase the max survivor count to ensure there is enough space on the char select bar
            SurvivorCatalog.survivorMaxCount += count;

            //Loop through the new survivors
            for ( int i = 0; i < count; i++ )
            {
                SurvivorDef curSurvivor = newSurvivors[i];

                //Check if the current survivor has been registered in bodycatalog. Log if it has not, but still add the survivor
                if( BodyCatalog.FindBodyIndex(curSurvivor.bodyPrefab) == -1 || BodyCatalog.GetBodyPrefab( BodyCatalog.FindBodyIndex(curSurvivor.bodyPrefab ) ) != curSurvivor.bodyPrefab )
                {
                    Debug.Log("Survivor: " + survNameList[i] + " is not properly registered in bodycatalog by: " + modInfoList[i]);
                }

                //Log that a survivor is being added, and add that survivor
                Debug.Log("Survivor: " + survNameList[i] + " added by: " + modInfoList[i]);
                obj.Add(newSurvivors[i]);

                //Add that new survivor to the order array so the game knows where to put it in character select
                SurvivorCatalog.idealSurvivorOrder[baseCount + i] = (SurvivorIndex)(i + baseCount + 1);
            }

            //Unregister the event because it won't be called again anyway
            UnRegisterEvent();
        }
    }
}
