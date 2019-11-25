using RoR2.ConVar;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace WispSurvivor.Components
{
    public class WispUIController : NetworkBehaviour
    {
        public WispPassiveController passive;

        private Single barPosVFrac = 0.5f;
        private Single barPosHFrac = 0.55f;
        private Single barHeightFrac = 0.35f;
        private Single barWidthFrac = 0.02f;
        private Single spaceFrac = 0.5f;
        private Single boxHeightFrac = 0.75f;

        private Single boxesStartH = 1100;
        private Single boxesSpacing = 14f;
        private Single boxesStartV = 480f;
        private Single boxesW = 24f;
        private Single boxesH = 12f;

        private int width;
        private int height;

        private Single scale;

        private Int32 texW = 12;
        private Int32 texH = 6;

        private Rect[] boxes = new Rect[10];

        private Boolean paused = false;

        BaseConVar scaleVar;

        private Color[] color = new Color[8]
        {
            new Color( 0f, 0f, 0f, 1f ),
            new Color( 1f, 1f, 1f, 1f ),
            new Color( 1f, 0f, 0f, 1f ),
            new Color( 0f, 1f, 0f, 1f ),
            new Color( 0f, 0f, 1f, 1f ),
            new Color( 1f, 1f, 0f, 1f ),
            new Color( 0f, 1f, 1f, 1f ),
            new Color( 1f, 0f, 1f, 1f ),
        };

        private Texture2D[] colors = new Texture2D[8];

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
            scaleVar = RoR2.Console.instance.FindConVar( "hud_scale" );
            RecalcBarRect();
            RoR2.RoR2Application.onPauseStartGlobal += () => this.paused = true;
            RoR2.RoR2Application.onPauseEndGlobal += () => this.paused = false;
        }

        public void Update()
        {
            if( !base.hasAuthority ) return;
            this.UpdateBarColors( this.passive.ReadCharge() );
        }

        public void FixedUpdate()
        {
            float temp = 0f;
            
            if( Screen.width != width || Screen.height != height || (TextSerialization.TryParseInvariant( scaleVar.GetString() , out temp ) && temp != scale ) )
            {
                RecalcBarRect();
            }
        }

        public void OnGUI()
        {
            if( !base.hasAuthority || this.paused ) return;
            for( Int32 i = 0; i < 10; i++ )
            {
                GUI.Box( this.boxes[i], this.colors[this.colorStates[9 - i]], this.style );
            }
        }

        private void RecalcBarRect()
        {
            width = Screen.width;
            height = Screen.height;
            TextSerialization.TryParseInvariant( scaleVar.GetString(), out scale );

            float barWidth = width * barWidthFrac * scale / 100 ;
            float barHeight = height * barHeightFrac * scale / 100;

            float spaceSize = barHeight * spaceFrac / 10;
            float barSize = spaceSize * boxHeightFrac;

            boxesSpacing = spaceSize;
            boxesH = barSize;
            boxesW = barWidth;

            boxesStartH = width * barPosHFrac;

            float barV = height * barPosVFrac;
            boxesStartV = barV - (barHeight / 4f);

            texW = Mathf.CeilToInt( boxesW / 2f );
            texH = Mathf.CeilToInt( boxesH / 2f );

            for( Int32 i = 0; i < 10; i++ )
            {
                this.boxes[i] = new Rect( this.boxesStartH, this.boxesStartV + ( i * ( spaceSize ) ), this.boxesW, this.boxesH );
            }

            CreateTextures();
        }

        private void CreateTextures()
        {
            for( int i = 0; i < 8; i++ )
            {
                colors[i] = CreateSolidTex( color[i] );
            }
        }

        private Texture2D CreateSolidTex( Color c )
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
