using System;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Components
{
    public class WispUIController : NetworkBehaviour
    {
        public WispPassiveController passive;

        private Single boxesStartH = 1100;
        private Single boxesSpacing = 14f;
        private Single boxesStartV = 480f;
        private Single boxesW = 24f;
        private Single boxesH = 12f;

        private static Int32 texW = 12;
        private static Int32 texH = 6;

        private Rect[] boxes = new Rect[10];

        private Boolean paused = false;

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

        private UInt32[] colorStates = new UInt32[10]
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
            for( Int32 i = 0; i < 10; i++ )
            {
                this.boxes[i] = new Rect( this.boxesStartH, this.boxesStartV + i * this.boxesSpacing, this.boxesW, this.boxesH );
            }

            RoR2.RoR2Application.onPauseStartGlobal += () => this.paused = true;
            RoR2.RoR2Application.onPauseEndGlobal += () => this.paused = false;
        }

        public void Update()
        {
            if( !base.hasAuthority ) return;
            this.UpdateBarColors( this.passive.ReadCharge() );
        }

        public void OnGUI()
        {
            if( !base.hasAuthority || this.paused ) return;
            for( Int32 i = 0; i < 10; i++ )
            {
                GUI.Box( this.boxes[i], this.colors[this.colorStates[9 - i]], this.style );
            }
        }

        private static Texture2D CreateSolidTex( Color c )
        {
            Texture2D tex = new Texture2D(texW, texH);

            Color[] cols = new Color[texW * texH];
            for( Int32 i = 0; i < cols.Length; i++ )
            {
                cols[i] = c;
            }

            tex.SetPixels( 0, 0, texW, texH, cols );
            tex.Apply();
            return tex;
        }

        private void UpdateBarColors( Double charge )
        {
            UInt32 c = (UInt32) Math.Round(charge / 10.0) * 10;

            UInt32[] newColors = new UInt32[10];

            Int32 val = 0;
            Int32 cStep = 10;
            while( val < c )
            {
                cStep = val <= 200 ? 10 : (cStep * 2);
                for( Int32 i = 0; i < 10; i++ )
                {
                    newColors[i] += (c - val) >= cStep ? 1u : 0u;
                    val += cStep;
                }
            }

            for( Int32 i = 0; i < 10; i++ )
            {
                newColors[i] %= 8u;
            }

            this.colorStates = newColors;
        }
    }
}
