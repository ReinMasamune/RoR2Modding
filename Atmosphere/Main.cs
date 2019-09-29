using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using System;

namespace Atmosphere
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinAtmosphere", "Atmosphere", "1.0.0")]
    public class MainAtmosphere : BaseUnityPlugin
    {
        public void Start()
        {
            On.RoR2.Stage.Start += (orig, self) =>
            {
                SceneWeatherController control = SceneWeatherController.instance;
                if( control )
                {
                    Debug.Log("it is here");
                    Debug.Log(self.gameObject.name);
                    DebugWeatherParams(control.initialWeatherParams);
                    DebugWeatherParams(control.finalWeatherParams);
                }
                else
                {
                    Debug.Log("it is not here");
                }
                orig(self);
            };
        }

        public void DebugWeatherParams( SceneWeatherController.WeatherParams input )
        {
            //Debug.Log(input.ToString());
            Debug.Log("Sun Color:" + input.sunColor.ToString());
            Debug.Log("Sun Intensity:" + input.sunIntensity.ToString());
            Debug.Log("Fog Start:" + input.fogStart.ToString());
            Debug.Log("Fog Scale:" + input.fogScale.ToString());
            Debug.Log("Fog Intensity:" + input.fogIntensity.ToString());
        }
    }
}
