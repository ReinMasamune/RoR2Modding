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
    public class ProjectileGhostScaler : MonoBehaviour
    {
        public Vector3 scaleMult;

        public void Start()
        {
            Debug.Log("I Exist");
            var controller = gameObject.GetComponent<ProjectileController>();
            if( controller )
            {
                Debug.Log("Controller Found");
                var ghost = controller.ghost;
                if( ghost )
                {
                    Debug.Log("Ghost found");
                    var ghostObj = ghost.gameObject;
                    if( ghostObj )
                    {
                        Debug.Log("ghostObj found");
                        var scale = new Vector3();
                        ParticleSystem ps;
                        ParticleSystem.MainModule main;
                        foreach( Transform t in ghostObj.GetComponentsInChildren<Transform>() )
                        {
                            Debug.Log("Scaling transform");
                            scale = t.lossyScale;
                            ghostObj.transform.localScale = Vector3.Scale(scale, scaleMult);
                            ps = t.gameObject.GetComponent<ParticleSystem>();
                            if( ps )
                            {
                                main = ps.main;
                                //main.simulationSpace = ParticleSystemSimulationSpace.World;
                                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
                            }
                        }

                    }
                }
            }
        }
    }
}