using UnityEngine;
using UnityEngine.Networking;
using System;

namespace WispSurvivor.Components
{
    public class WispUIController : NetworkBehaviour
    {
        public WispPassiveController passive;

        private float boxesStartH = 1100;
        private float boxesSpacing = 14f;
        private float boxesStartV = 480f;
        private float boxesW = 24f;
        private float boxesH = 12f;

        private static int texW = 12;
        private static int texH = 6;

        private Rect[] boxes = new Rect[10];

        private bool paused = false;

        private Texture2D[] colors = new Texture2D[8]
        {
            CreateSolidTex( new Color( 0f, 0f, 0f, 1f ) ),
            CreateSolidTex( new Color( 1f, 1f, 1f, 1f ) ),
            CreateSolidTex( new Color( 1f, 0f, 0f, 1f ) ),
            CreateSolidTex( new Color( 0f, 1f, 0f, 1f ) ),
            CreateSolidTex( new Color( 0f, 0f, 1f, 1f ) ),
            CreateSolidTex( new Color( 1f, 1f, 0f, 1f ) ),
            CreateSolidTex( new Color( 0f, 1f, 1f, 1f ) ),
            CreateSolidTex( new Color( 1f, 0f, 1f, 1f ) ),
        };

        private GUIStyle style = new GUIStyle
        {
            border = new RectOffset(6, 6, 1, 1)
        };

        private uint[] colorStates = new uint[10]
        {
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };

        public void Awake()
        {
            for( int i = 0; i < 10; i++ )
            {
                boxes[i] = new Rect(boxesStartH, boxesStartV + i * boxesSpacing, boxesW, boxesH);
            }

            RoR2.RoR2Application.onPauseStartGlobal += () => paused = true;
            RoR2.RoR2Application.onPauseEndGlobal += () => paused = false;
        }

        public void Update()
        {
            if (!base.hasAuthority) return;
            UpdateBarColors(passive.ReadCharge());
        }

        public void OnGUI()
        {
            if (!base.hasAuthority || paused) return;
            for( int i = 0; i < 10; i++ )
            {
                GUI.Box(boxes[i], colors[colorStates[9 - i]], style );
            }
        }

        private static Texture2D CreateSolidTex( Color c )
        {
            Texture2D tex = new Texture2D(texW, texH);

            Color[] cols = new Color[texW * texH];
            for( int i = 0; i < cols.Length; i++ )
            {
                cols[i] = c;
            }

            tex.SetPixels(0, 0, texW, texH, cols);
            tex.Apply();
            return tex;
        }

        private void UpdateBarColors( double charge )
        {
            uint c = (uint) Math.Round(charge / 10.0) * 10;

            uint[] newColors = new uint[10];

            int val = 0;
            int cStep = 10;
            while( val < c )
            {
                cStep = val <= 200 ? 10 : (cStep * 2);
                for( int i = 0; i < 10; i++ )
                {
                    newColors[i] += (c-val) >= cStep ? 1u : 0u;
                    val += cStep;
                }
            }

            for( int i = 0; i < 10; i++ )
            {
                newColors[i] %= 8u;
            }

            colorStates = newColors;
        }
    }
}
