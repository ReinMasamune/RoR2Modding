using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WispSurvivor.Modules
{
    public static class WispMaterialModule
    {
        public static Gradient[] fireGradients = new Gradient[8];
        public static Gradient[][] armorGradients = new Gradient[8][];
        public static Gradient[] electricGradients = new Gradient[8];
        public static Color[] fireColors = new Color[8];
        public static Texture2D[] fireTextures = new Texture2D[8];
        public static Texture2D[][] armorTextures = new Texture2D[8][];
        public static Texture2D[] electricTextures = new Texture2D[8];
        public static Material[][] fireMaterials = new Material[8][];
        public static Material[][] otherMaterials = new Material[8][];
        public static Material[] flareMats = new Material[8];
        public static Material[] armorMaterials = new Material[8];
        public static Material[] burnOverlayMaterials = new Material[8];
        public static Material[][] eliteFlameMaterials = new Material[8][];
        public static BurnEffectController.EffectParams[] burnOverlayParams = new BurnEffectController.EffectParams[8];
        public static Shader effectShader;

        public static void DoModule( GameObject body, Dictionary<Type, Component> dic, WispSurvivorMain m )
        {
            GenerateGradients();
            GenerateTextures();
            GenerateMaterials( dic );
        }

        private static void GenerateGradients()
        {
            GradientAlphaKey[][] aKeys = new GradientAlphaKey[8][];
            GradientColorKey[][] cKeys = new GradientColorKey[8][];
            GradientAlphaKey[][] aKeyArm = new GradientAlphaKey[8][];
            GradientColorKey[][] cKeyArm = new GradientColorKey[8][];
            GradientAlphaKey[][] aKeyArm2 = new GradientAlphaKey[8][];
            GradientColorKey[][] cKeyArm2 = new GradientColorKey[8][];
            GradientAlphaKey[][] aKeyEl = new GradientAlphaKey[8][];
            GradientColorKey[][] cKeyEl = new GradientColorKey[8][];

            GradientAlphaKey[] solidAKey = new GradientAlphaKey[1];
            solidAKey[0] = new GradientAlphaKey( 1f, 0f );

            GradientAlphaKey[] arm2A = new GradientAlphaKey[3];
            arm2A[0] = new GradientAlphaKey( 0f, 1f );
            arm2A[1] = new GradientAlphaKey( 0f, 0.7f );
            arm2A[2] = new GradientAlphaKey( 1f, 0.4f );

            //Ancient wisp generic
            aKeys[0] = new GradientAlphaKey[3];
            aKeys[0][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[0][1] = new GradientAlphaKey( 0f, 0.5f );
            aKeys[0][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[0] = new GradientColorKey[2];
            cKeys[0][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeys[0][1] = new GradientColorKey( new Color( 0.8f, 0.300f, 0.9f ), 0f );

            fireColors[0] = new Color( 0.8f, 0.3f, 0.9f );

            aKeyArm[0] = solidAKey;
            cKeyArm[0] = new GradientColorKey[5];
            cKeyArm[0][0] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 1f );
            cKeyArm[0][1] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 0.6f );
            cKeyArm[0][2] = new GradientColorKey( new Color( 0.8f, 0.3f, 0.9f ), 0.4f );
            cKeyArm[0][3] = new GradientColorKey( new Color( 0.8f, 0.3f, 0.9f ), 0.3f );
            cKeyArm[0][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[0] = arm2A;
            cKeyArm2[0] = new GradientColorKey[5];
            cKeyArm2[0][0] = new GradientColorKey( new Color( 0.4f, 0.15f, 0.45f ), 1f );
            cKeyArm2[0][1] = new GradientColorKey( new Color( 0.8f, 0.3f, 0.9f ), 0.5f );
            cKeyArm2[0][2] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[0][3] = new GradientColorKey( new Color( 0.8f, 0.3f, 0.9f ), 0.2f );
            cKeyArm2[0][4] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[0] = solidAKey;
            cKeyEl[0] = new GradientColorKey[4];
            cKeyEl[0][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[0][1] = new GradientColorKey( fireColors[0], 0.6f );
            cKeyEl[0][2] = new GradientColorKey( fireColors[0], 0.4f );
            cKeyEl[0][3] = new GradientColorKey( Color.black, 0.35f );

            //Lesser wisp
            aKeys[1] = new GradientAlphaKey[3];
            aKeys[1][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[1][1] = new GradientAlphaKey( 0f, 0.5f );
            aKeys[1][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[1] = new GradientColorKey[2];
            cKeys[1][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeys[1][1] = new GradientColorKey( new Color( 0.906f, 0.420f, 0.235f ), 0f );

            fireColors[1] = new Color( 0.906f, 0.420f, 0.235f, 1.0f );

            aKeyArm[1] = solidAKey;
            cKeyArm[1] = new GradientColorKey[5];
            cKeyArm[1][0] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 1f );
            cKeyArm[1][1] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 0.6f );
            cKeyArm[1][2] = new GradientColorKey( new Color( 0.906f, 0.42f, 0.235f ), 0.4f );
            cKeyArm[1][3] = new GradientColorKey( new Color( 0.906f, 0.42f, 0.235f ), 0.3f );
            cKeyArm[1][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[1] = arm2A;
            cKeyArm2[1] = new GradientColorKey[5];
            cKeyArm2[1][0] = new GradientColorKey( new Color( 0.45f, 0.21f, 0.11f ), 1f );
            cKeyArm2[1][1] = new GradientColorKey( new Color( 0.906f, 0.420f, 0.235f ), 0.5f );
            cKeyArm2[1][2] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[1][3] = new GradientColorKey( new Color( 0.906f, 0.420f, 0.235f ), 0.2f );
            cKeyArm2[1][4] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[1] = solidAKey;
            cKeyEl[1] = new GradientColorKey[4];
            cKeyEl[1][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[1][1] = new GradientColorKey( fireColors[1], 0.6f );
            cKeyEl[1][2] = new GradientColorKey( fireColors[1], 0.4f );
            cKeyEl[1][3] = new GradientColorKey( Color.black, 0.35f );

            //Greater wisp
            aKeys[2] = new GradientAlphaKey[3];
            aKeys[2][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[2][1] = new GradientAlphaKey( 0f, 0.5f );
            aKeys[2][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[2] = new GradientColorKey[2];
            cKeys[2][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeys[2][1] = new GradientColorKey( new Color( 0.400f, 0.769f, 0.192f ), 0f );

            fireColors[2] = new Color( 0.400f, 0.769f, 0.192f, 1.0f );

            aKeyArm[2] = solidAKey;
            cKeyArm[2] = new GradientColorKey[5];
            cKeyArm[2][0] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 1f );
            cKeyArm[2][1] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 0.6f );
            cKeyArm[2][2] = new GradientColorKey( new Color( 0.4f, 0.769f, 0.192f ), 0.4f );
            cKeyArm[2][3] = new GradientColorKey( new Color( 0.4f, 0.769f, 0.192f ), 0.3f );
            cKeyArm[2][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[2] = arm2A;
            cKeyArm2[2] = new GradientColorKey[5];
            cKeyArm2[2][0] = new GradientColorKey( new Color( 0.2f, 0.35f, 0.1f ), 1f );
            cKeyArm2[2][1] = new GradientColorKey( new Color( 0.4f, 0.769f, 0.192f ), 0.5f );
            cKeyArm2[2][2] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[2][3] = new GradientColorKey( new Color( 0.400f, 0.769f, 0.192f ), 0.2f );
            cKeyArm2[2][4] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[2] = solidAKey;
            cKeyEl[2] = new GradientColorKey[4];
            cKeyEl[2][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[2][1] = new GradientColorKey( fireColors[2], 0.6f );
            cKeyEl[2][2] = new GradientColorKey( fireColors[2], 0.4f );
            cKeyEl[2][3] = new GradientColorKey( Color.black, 0.35f );

            //Archaic wisp
            aKeys[3] = new GradientAlphaKey[3];
            aKeys[3][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[3][1] = new GradientAlphaKey( 0f, 0.75f );
            aKeys[3][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[3] = new GradientColorKey[2];
            cKeys[3][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeys[3][1] = new GradientColorKey( new Color( 1f, 0.590f, 0.806f ), 0f );

            fireColors[3] = new Color( 1f, 0.590f, 0.806f, 1.0f );

            aKeyArm[3] = solidAKey;
            cKeyArm[3] = new GradientColorKey[5];
            cKeyArm[3][0] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 1f );
            cKeyArm[3][1] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 0.6f );
            cKeyArm[3][2] = new GradientColorKey( new Color( 1f, 0.59f, 0.806f ), 0.4f );
            cKeyArm[3][3] = new GradientColorKey( new Color( 1f, 0.59f, 0.806f ), 0.3f );
            cKeyArm[3][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[3] = arm2A;
            cKeyArm2[3] = new GradientColorKey[5];
            cKeyArm2[3][0] = new GradientColorKey( new Color( 0.5f, 0.3f, 0.4f ), 1f );
            cKeyArm2[3][1] = new GradientColorKey( new Color( 1f, 0.590f, 0.806f ), 0.5f );
            cKeyArm2[3][2] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[3][3] = new GradientColorKey( new Color( 1f, 0.590f, 0.806f ), 0.2f );
            cKeyArm2[3][4] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[3] = solidAKey;
            cKeyEl[3] = new GradientColorKey[4];
            cKeyEl[3][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[3][1] = new GradientColorKey( fireColors[3], 0.6f );
            cKeyEl[3][2] = new GradientColorKey( fireColors[3], 0.4f );
            cKeyEl[3][3] = new GradientColorKey( Color.black, 0.35f );

            //Lunar wisp
            aKeys[4] = new GradientAlphaKey[3];
            aKeys[4][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[4][1] = new GradientAlphaKey( 0f, 0.5f );
            aKeys[4][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[4] = new GradientColorKey[7];
            cKeys[4][0] = new GradientColorKey( new Color( 0f, 0f, 0.05f ), 1f );
            cKeys[4][1] = new GradientColorKey( new Color( 0f, 0f, 0.1f ), 0.75f );
            cKeys[4][2] = new GradientColorKey( new Color( 0.1f, 0.11f, 0.6f ), 0.7f );
            cKeys[4][3] = new GradientColorKey( new Color( 0.19f, 0.19f, 0.85f ), 0.63f );
            cKeys[4][4] = new GradientColorKey( new Color( 0.4f, 0.55f, 1.0f ), 0.32f );
            cKeys[4][5] = new GradientColorKey( new Color( 0.48f, 0.69f, 1.0f ), 0.25f );
            cKeys[4][6] = new GradientColorKey( new Color( 0.5f, 0.75f, 1.0f ), 0f );

            fireColors[4] = new Color( 0.5f, 0.75f, 1f, 1.0f );

            aKeyArm[4] = solidAKey;
            cKeyArm[4] = new GradientColorKey[5];
            cKeyArm[4][0] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 1f );
            cKeyArm[4][1] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 0.6f );
            cKeyArm[4][2] = new GradientColorKey( new Color( 0.5f, 0.75f, 1f ), 0.4f );
            cKeyArm[4][3] = new GradientColorKey( new Color( 0.5f, 0.75f, 1f ), 0.3f );
            cKeyArm[4][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[4] = arm2A;
            cKeyArm2[4] = new GradientColorKey[5];
            cKeyArm2[4][0] = new GradientColorKey( new Color( 0.25f, 0.4f, 0.5f ), 1f );
            cKeyArm2[4][1] = new GradientColorKey( new Color( 0.5f, 0.75f, 1f ), 0.5f );
            cKeyArm2[4][2] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[4][3] = new GradientColorKey( new Color( 0.5f, 0.75f, 1f ), 0.2f );
            cKeyArm2[4][4] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[4] = solidAKey;
            cKeyEl[4] = new GradientColorKey[4];
            cKeyEl[4][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[4][1] = new GradientColorKey( fireColors[4], 0.6f );
            cKeyEl[4][2] = new GradientColorKey( fireColors[4], 0.4f );
            cKeyEl[4][3] = new GradientColorKey( Color.black, 0.35f );

            //Solar wisp
            aKeys[5] = new GradientAlphaKey[3];
            aKeys[5][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[5][1] = new GradientAlphaKey( 0f, 0.5f );
            aKeys[5][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[5] = new GradientColorKey[7];
            cKeys[5][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeys[5][1] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.75f );
            cKeys[5][2] = new GradientColorKey( new Color( 0.3f, 0.1f, 0.05f ), 0.7f );
            cKeys[5][3] = new GradientColorKey( new Color( 0.5f, 0.3f, 0.05f ), 0.63f );
            cKeys[5][4] = new GradientColorKey( new Color( 0.9f, 0.7f, 0.1f ), 0.32f );
            cKeys[5][5] = new GradientColorKey( new Color( 0.9f, 0.8f, 0.4f ), 0.25f );
            cKeys[5][6] = new GradientColorKey( new Color( 0.9f, 0.95f, 0.8f ), 0f );

            fireColors[5] = new Color( 0.95f, 0.95f, 0.05f, 1f );

            aKeyArm[5] = solidAKey;
            cKeyArm[5] = new GradientColorKey[5];
            cKeyArm[5][0] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 1f );
            cKeyArm[5][1] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 0.6f );
            cKeyArm[5][2] = new GradientColorKey( new Color( 0.95f, 0.95f, 0.05f ), 0.4f );
            cKeyArm[5][3] = new GradientColorKey( new Color( 0.95f, 0.95f, 0.05f ), 0.3f );
            cKeyArm[5][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[5] = arm2A;
            cKeyArm2[5] = new GradientColorKey[5];
            cKeyArm2[5][0] = new GradientColorKey( new Color( 0.5f, 0.5f, 0f ), 1f );
            cKeyArm2[5][1] = new GradientColorKey( new Color( 0.95f, 0.95f, 0.05f ), 0.5f );
            cKeyArm2[5][2] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[5][3] = new GradientColorKey( new Color( 0.95f, 0.95f, 0.05f ), 0.2f );
            cKeyArm2[5][4] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[5] = solidAKey;
            cKeyEl[5] = new GradientColorKey[4];
            cKeyEl[5][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[5][1] = new GradientColorKey( fireColors[5], 0.6f );
            cKeyEl[5][2] = new GradientColorKey( fireColors[5], 0.4f );
            cKeyEl[5][3] = new GradientColorKey( Color.black, 0.35f );

            //Abyssal wisp
            aKeys[6] = new GradientAlphaKey[3];
            aKeys[6][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[6][1] = new GradientAlphaKey( 0f, 0.5f );
            aKeys[6][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[6] = new GradientColorKey[7];
            cKeys[6][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeys[6][1] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.75f );
            cKeys[6][2] = new GradientColorKey( new Color( 0.5f, 0.0f, 0.0f ), 0.7f );
            cKeys[6][3] = new GradientColorKey( new Color( 0.7f, 0.0f, 0.0f ), 0.63f );
            cKeys[6][4] = new GradientColorKey( new Color( 1f, 0.1f, 0.1f ), 0.32f );
            cKeys[6][5] = new GradientColorKey( new Color( 1f, 0.3f, 0.3f ), 0.15f );
            cKeys[6][6] = new GradientColorKey( new Color( 1f, 0.5f, 0.5f ), 0f );

            fireColors[6] = new Color( 0.95f, 0.05f, 0.05f, 1f );

            aKeyArm[6] = solidAKey;
            cKeyArm[6] = new GradientColorKey[5];
            cKeyArm[6][0] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 1f );
            cKeyArm[6][1] = new GradientColorKey( new Color( 0.05f, 0.05f, 0.05f ), 0.6f );
            cKeyArm[6][2] = new GradientColorKey( new Color( 0.95f, 0.05f, 0.05f ), 0.4f );
            cKeyArm[6][3] = new GradientColorKey( new Color( 0.95f, 0.05f, 0.05f ), 0.3f );
            cKeyArm[6][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[6] = arm2A;
            cKeyArm2[6] = new GradientColorKey[5];
            cKeyArm2[6][0] = new GradientColorKey( new Color( 0.5f, 0f, 0f ), 1f );
            cKeyArm2[6][1] = new GradientColorKey( new Color( 0.95f, 0.05f, 0.05f ), 0.5f );
            cKeyArm2[6][2] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[6][3] = new GradientColorKey( new Color( 0.95f, 0.05f, 0.05f ), 0.2f );
            cKeyArm2[6][4] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[6] = solidAKey;
            cKeyEl[6] = new GradientColorKey[4];
            cKeyEl[6][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[6][1] = new GradientColorKey( fireColors[6], 0.6f );
            cKeyEl[6][2] = new GradientColorKey( fireColors[6], 0.4f );
            cKeyEl[6][3] = new GradientColorKey( Color.black, 0.35f );

            //Iridescent wisp
            aKeys[7] = new GradientAlphaKey[3];
            aKeys[7][0] = new GradientAlphaKey( 0f, 1f );
            aKeys[7][1] = new GradientAlphaKey( 0f, 0.5f );
            aKeys[7][2] = new GradientAlphaKey( 0.7f, 0f );

            cKeys[7] = new GradientColorKey[8];
            cKeys[7][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeys[7][1] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.9f );
            cKeys[7][2] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.7f );
            cKeys[7][3] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.5f );
            cKeys[7][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.3f );
            cKeys[7][5] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.1f );
            cKeys[7][6] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.05f );
            cKeys[7][7] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0f );

            fireColors[7] = new Color( 1f, 1f, 1f, 1f );

            aKeyArm[7] = solidAKey;
            cKeyArm[7] = new GradientColorKey[8];
            cKeyArm[7][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeyArm[7][1] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.51f );
            cKeyArm[7][2] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.5f );
            cKeyArm[7][3] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.475f );
            cKeyArm[7][4] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.45f );
            cKeyArm[7][5] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.375f );
            cKeyArm[7][6] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.25f );
            cKeyArm[7][7] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0f );

            aKeyArm2[7] = arm2A;
            cKeyArm2[7] = new GradientColorKey[8];
            cKeyArm2[7][0] = new GradientColorKey( new Color( 0f, 0f, 0f ), 1f );
            cKeyArm2[7][1] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.6f );
            cKeyArm2[7][2] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.5f );
            cKeyArm2[7][3] = new GradientColorKey( Color.white, 0.4f );
            cKeyArm2[7][4] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.4f );
            cKeyArm2[7][5] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.3f );
            cKeyArm2[7][6] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.2f );
            cKeyArm2[7][7] = new GradientColorKey( Color.black, 0.1f );

            aKeyEl[7] = solidAKey;
            cKeyEl[7] = new GradientColorKey[8];
            cKeyEl[7][0] = new GradientColorKey( Color.black, 0.65f );
            cKeyEl[7][1] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.625f );
            cKeyEl[7][2] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.575f );
            cKeyEl[7][3] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.525f );
            cKeyEl[7][4] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.475f );
            cKeyEl[7][5] = new GradientColorKey( new Color( 0f, 0f, 0f ), 0.425f );
            cKeyEl[7][6] = new GradientColorKey( new Color( 1f, 1f, 1f ), 0.375f );
            cKeyEl[7][7] = new GradientColorKey( Color.black, 0.35f );

            for( Int32 i = 0; i < 8; i++ )
            {
                fireGradients[i] = new Gradient
                {
                    alphaKeys = aKeys[i],
                    colorKeys = cKeys[i],
                    mode = GradientMode.Blend
                };

                armorGradients[i] = new Gradient[2];
                armorGradients[i][0] = new Gradient
                {
                    mode = GradientMode.Blend,
                    alphaKeys = aKeyArm[i],
                    colorKeys = cKeyArm[i]
                };
                armorGradients[i][1] = new Gradient
                {
                    mode = GradientMode.Blend,
                    alphaKeys = aKeyArm2[i],
                    colorKeys = cKeyArm2[i]
                };
                electricGradients[i] = new Gradient
                {
                    mode = GradientMode.Blend,
                    alphaKeys = aKeyEl[i],
                    colorKeys = cKeyEl[i]
                };
            }
        }

        private static void GenerateTextures()
        {
            for( Int32 i = 0; i < 8; i++ )
            {
                fireTextures[i] = CreateNewRampTex( fireGradients[i] );
                armorTextures[i] = new Texture2D[armorGradients[0].Length];
                for( Int32 j = 0; j < armorGradients[0].Length; j++ )
                {
                    armorTextures[i][j] = CreateNewRampTex( armorGradients[i][j] );
                }
                electricTextures[i] = CreateNewRampTex( electricGradients[i] );
            }
        }

        private static void GenerateMaterials( Dictionary<Type, Component> dic )
        {
            Material[] baseMats = GetBaseMaterials(dic);

            for( Int32 i = 0; i < 8; i++ )
            {
                fireMaterials[i] = new Material[baseMats.Length];
            }

            Material tempMat;
            for( Int32 i = 0; i < baseMats.Length; i++ )
            {
                tempMat = baseMats[i];

                for( Int32 j = 0; j < 8; j++ )
                {
                    fireMaterials[j][i] = MonoBehaviour.Instantiate<Material>( tempMat );
                }
            }

            for( Int32 i = 0; i < 8; i++ )
            {
                fireMaterials[i][0].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][0].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][0].SetFloat( "_Boost", 3.7f );
                fireMaterials[i][0].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][0].SetFloat( "_InvFade", 2f );
                fireMaterials[i][0].SetFloat( "_SrcBlend", 5f );

                fireMaterials[i][1].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][1].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][1].SetFloat( "_Boost", 7f );
                fireMaterials[i][1].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][1].SetFloat( "_InvFade", 2f );
                fireMaterials[i][1].SetFloat( "_SrcBlend", 5f );

                fireMaterials[i][5].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][5].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][5].SetFloat( "_Boost", 2.7f );
                fireMaterials[i][5].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][5].SetFloat( "_InvFade", 2f );
                fireMaterials[i][5].SetFloat( "_SrcBlend", 5f );

                fireMaterials[i][6].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][6].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][6].SetFloat( "_Boost", 4.5f );
                fireMaterials[i][6].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][6].SetFloat( "_InvFade", 2f );
                fireMaterials[i][6].SetFloat( "_SrcBlend", 5f );

                fireMaterials[i][7].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][7].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][7].SetFloat( "_Boost", 4.5f );
                fireMaterials[i][7].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][7].SetFloat( "_InvFade", 2f );
                fireMaterials[i][7].SetFloat( "_SrcBlend", 5f );

                fireMaterials[i][8].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][8].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][8].SetFloat( "_Boost", 4.5f );
                fireMaterials[i][8].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][8].SetFloat( "_InvFade", 2f );
                fireMaterials[i][8].SetFloat( "_SrcBlend", 5f );

                fireMaterials[i][9].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][9].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][9].SetFloat( "_Boost", 4.5f );
                fireMaterials[i][9].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][9].SetFloat( "_InvFade", 2f );
                fireMaterials[i][9].SetFloat( "_SrcBlend", 5f );

                fireMaterials[i][10].SetTexture( "_RemapTex", fireTextures[i] );
                fireMaterials[i][10].SetFloat( "_AlphaBias", 0f );
                fireMaterials[i][10].SetFloat( "_Boost", 4.5f );
                fireMaterials[i][10].SetFloat( "_DstBlend", 10f );
                fireMaterials[i][10].SetFloat( "_InvFade", 2f );
                fireMaterials[i][10].SetFloat( "_SrcBlend", 5f );
            }

            Material baseArmorMaterial = MonoBehaviour.Instantiate<Material>(Resources.Load<GameObject>("Prefabs/CharacterBodies/AncientWispBody").GetComponent<ModelLocator>().modelTransform.Find("AncientWispMesh").GetComponent<SkinnedMeshRenderer>().material);
            baseArmorMaterial.DisableKeyword( "DITHER" );
            baseArmorMaterial.DisableKeyword( "_EMISSION" );
            baseArmorMaterial.EnableKeyword( "FLOWMAP" );
            baseArmorMaterial.EnableKeyword( "FRESNEL_EMISSION" );
            baseArmorMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            baseArmorMaterial.SetTexture( "_MainTex", null );
            baseArmorMaterial.SetTextureScale( "_FlowHeightmap", new Vector2( 4f, 4f ) );
            baseArmorMaterial.SetFloat( "_FEON", 1f );
            baseArmorMaterial.SetFloat( "_FlowDiffuseStrength", 1f );
            baseArmorMaterial.SetFloat( "_FlowmapOn", 1 );
            baseArmorMaterial.SetFloat( "_FresnelBoost", 17.46f );
            baseArmorMaterial.SetFloat( "_FresnelPower", 0.48f );
            baseArmorMaterial.SetFloat( "_NormalStrength", 0.84f );
            baseArmorMaterial.SetFloat( "_DiffuseBias", 0f );
            baseArmorMaterial.SetFloat( "_DiffuseExponent", 0f );
            baseArmorMaterial.SetFloat( "_DiffuseHardness", 0f );
            baseArmorMaterial.SetFloat( "_DiffuseScale", 0f );
            baseArmorMaterial.SetFloat( "_RimBoost", 0f );
            baseArmorMaterial.SetFloat( "_RimPower", 0f );
            baseArmorMaterial.SetFloat( "_RimStrength", 0f );
            baseArmorMaterial.SetColor( "_Color", new Color( 0.05f, 0.05f, 0.05f, 1f ) );
            baseArmorMaterial.SetColor( "_RimTint", new Color( 0f, 0f, 0f, 1f ) );
            baseArmorMaterial.SetColor( "_SpecularTint", new Color( 0f, 0f, 0f, 1f ) );

            Texture tempTex = Resources.Load<GameObject>("Prefabs/ArchWispFireTrail").GetComponent<DamageTrail>().segmentPrefab.GetComponent<ParticleSystemRenderer>().material.GetTexture("_Cloud2Tex");

            baseArmorMaterial.SetTexture( "_FlowHeightmap", tempTex );
            baseArmorMaterial.SetTexture( "_FlowTex", tempTex );

            baseArmorMaterial.SetInt( "_FlowmapOn", 1 );
            baseArmorMaterial.SetInt( "_FEON", 1 );


            for( Int32 i = 0; i < 8; i++ )
            {
                armorMaterials[i] = MonoBehaviour.Instantiate<Material>( baseArmorMaterial );
                armorMaterials[i].SetTexture( "_FlowHeightRamp", armorTextures[i][0] );
                armorMaterials[i].SetTexture( "_FresnelRamp", armorTextures[i][1] );
            }

            Material baseBurnMaterial = Resources.Load<Material>("Materials/MatOnHelfire");

            for( Int32 i = 0; i < 8; i++ )
            {
                burnOverlayMaterials[i] = MonoBehaviour.Instantiate<Material>( baseBurnMaterial );
                burnOverlayMaterials[i].SetTexture( "_RemapTex", fireTextures[i] );

                burnOverlayParams[i] = new BurnEffectController.EffectParams
                {
                    startSound = "Play_item_proc_igniteOnKill_Loop",
                    stopSound = "Stop_item_proc_igniteOnKill_Loop",
                    overlayMaterial = burnOverlayMaterials[i],
                    fireEffectPrefab = Resources.Load<GameObject>( "Prefabs/HelfireEffect" )
                };
            }

            Material base1 = Resources.Load<Material>("Materials/matElitePoisonParticleReplacement");
            Material base2 = Resources.Load<Material>("Materials/matEliteHauntedParticleReplacement");

            for( Int32 i = 0; i < 8; i++ )
            {
                eliteFlameMaterials[i] = new Material[2];
                eliteFlameMaterials[i][0] = MonoBehaviour.Instantiate<Material>( base1 );
                eliteFlameMaterials[i][0].SetTexture( "_RemapTex", fireTextures[i] );
                eliteFlameMaterials[i][1] = MonoBehaviour.Instantiate<Material>( base2 );
                eliteFlameMaterials[i][1].SetTexture( "_RemapTex", fireTextures[i] );
            }

        }

        private static Material[] GetBaseMaterials( Dictionary<Type, Component> dic )
        {
            Material[] mats = new Material[11];

            //matAncientWispFire
            mats[0] = dic.C<ModelLocator>().modelTransform.GetComponent<CharacterModel>().baseParticleSystemInfos[0].renderer.material;

            Transform sourceObj1 = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/AncientWispExplosion").transform.Find("Particles");
            Transform sourceObj2 = Resources.Load<GameObject>("Prefabs/Effects/HelfireIgniteEffect").transform;
            Transform sourceObj3 = Resources.Load<GameObject>("Prefabs/Effects/BeamSphereExplosion").transform.Find("InitialBurst");
            //matGenericFire
            mats[1] = sourceObj1.Find( "Sparks" ).GetComponent<ParticleSystemRenderer>().material;
            //matCutWispLarge
            mats[2] = sourceObj1.Find( "Flames" ).GetComponent<ParticleSystemRenderer>().material;
            //matCutShockwave
            mats[3] = sourceObj1.Find( "Ring" ).GetComponent<ParticleSystemRenderer>().material;
            //matWispEnrage
            mats[4] = Resources.Load<GameObject>( "Prefabs/Effects/AncientWispEnrage" ).transform.Find( "SwingTrail" ).GetComponent<ParticleSystemRenderer>().material;
            //matAncientWilloWispPillar?
            mats[5] = Resources.Load<GameObject>( "Prefabs/Effects/ImpactEffects/AncientWispPillar" ).transform.Find( "Particles" ).Find( "Flames, Tube, CenterHuge" ).GetComponent<ParticleSystemRenderer>().material;
            //matHelfirePuff
            mats[6] = sourceObj2.Find( "Puff" ).GetComponent<ParticleSystemRenderer>().material;
            //matHelfireIgniteEffectFlare
            mats[7] = sourceObj2.Find( "Flare" ).GetComponent<ParticleSystemRenderer>().material;
            //matBeamSphereBeam
            mats[8] = sourceObj3.Find( "Ring" ).GetComponent<ParticleSystemRenderer>().material;
            //matBeamSphereCenter
            mats[9] = sourceObj3.Find( "Flames" ).GetComponent<ParticleSystemRenderer>().material;
            //matBeamSphereLightning
            mats[10] = sourceObj3.Find( "Lightning" ).GetComponent<ParticleSystemRenderer>().material;

            effectShader = mats[1].shader;

            return mats;
        }

        private static T C<T>( this Dictionary<Type, Component> dic ) where T : Component => dic[typeof( T )] as T;

        private static Texture2D CreateNewRampTex( Gradient grad )
        {
            Texture2D tex = new Texture2D(256, 16, TextureFormat.RGBA32, false);

            Color tempC;
            Color[] tempCs = new Color[16];

            for( Int32 i = 0; i < 256; i++ )
            {
                tempC = grad.Evaluate( i / 255f );
                for( Int32 j = 0; j < 16; j++ )
                {
                    tempCs[j] = tempC;
                }

                tex.SetPixels( 255 - i, 0, 1, 16, tempCs );
            }
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Apply();
            return tex;
        }

        private static void DebugMaterialInfo( Material m )
        {
            Debug.Log( "Material name: " + m.name );
            String[] s = m.shaderKeywords;
            Debug.Log( "Shader keywords" );
            for( Int32 i = 0; i < s.Length; i++ )
            {
                Debug.Log( s[i] );
            }

            Debug.Log( "Shader name: " + m.shader.name );

            Debug.Log( "Texture Properties" );
            String[] s2 = m.GetTexturePropertyNames();
            for( Int32 i = 0; i < s2.Length; i++ )
            {
                Debug.Log( s2[i] + " : " + m.GetTexture( s2[i] ) );
            }
        }
    }
}