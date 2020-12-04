namespace RoR2ShaderWrappers
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using Object = System.Object;
    using UnityObject = UnityEngine.Object;

    public struct RWMaterialProp<T>
    {
        private readonly BaseMaterial mat;
        private readonly String propName;
        private readonly Boolean useCache;

        internal RWMaterialProp(String name, BaseMaterial mat, Boolean useCache)
        {
            this.useCache = useCache;
            this.mat = mat;
            this.propName = name;
            this.cached = this.useCache ? this.mat.GetProperty<T>(name) : default;
        }

        private T cached;

        public T value
        {
            get => this.useCache ? this.cached : this.mat.GetProperty<T>(this.propName);
            set
            {
                if(this.useCache)
                {
                    if((this.cached is null && value is null) || (this.cached?.Equals(value) ?? false)) return;
                    this.cached = value;
                }
                this.mat.SetProperty<T>(this.propName, value);
            }
        }

        public static implicit operator T(RWMaterialProp<T> prop) => prop.value;
    }


    public struct ReadMaterialProp<T>
    {
        private readonly BaseMaterial mat;
        private readonly String propName;
        private readonly Boolean useCache;

        internal ReadMaterialProp(String name, BaseMaterial mat, Boolean useCache)
        {
            this.useCache = useCache;
            this.mat = mat;
            this.propName = name;
            this.cached = this.useCache ? this.mat.GetProperty<T>(name) : default;
        }

        private T cached;

        public T value
        {
            get => this.useCache ? this.cached : this.mat.GetProperty<T>(this.propName);
        }

        public static implicit operator T(ReadMaterialProp<T> prop) => prop.value;
    }

    public readonly struct WriteMaterialProp<T>
    {
        private readonly BaseMaterial mat;
        private readonly String propName;

        internal WriteMaterialProp(String name, BaseMaterial mat)
        {
            this.mat = mat;
            this.propName = name;
        }

        public T value { set => this.mat.SetProperty<T>(this.propName, value); }
    }

    public abstract class BaseMaterial
    {
        private readonly Shader shader;
        private readonly Material _material;
        private readonly Boolean useCache;

        public Material material => this._material;

        internal T GetProperty<T>(String name) => GetSetCache<T>.Get(this.material, name);

        internal void SetProperty<T>(String name, T value) => GetSetCache<T>.Set(this.material, name, value);
 
        protected RWMaterialProp<T> CreateRWProp<T>(String name) => new(name, this, this.useCache);
        protected ReadMaterialProp<T> CreateReadProp<T>(String name) => new(name, this, this.useCache);
        protected WriteMaterialProp<T> CreateWriteProp<T>(String name) => new(name, this);


        public BaseMaterial(Boolean useCache, Material material, Boolean copy = false)
        {
            this.shader = material.shader;
            this._material = copy ? new Material(material) : material;
            this.useCache = useCache;
        }

        private static class GetSetCache<T>
        {
            internal static Action<Material, String, T> Set { get; set; }
            internal static Func<Material, String, T> Get { get; set; }
        }

        static BaseMaterial()
        {
            //GetSetCache<ComputeBuffer>.Get = static (m, s) => m.GetBuffer(s);
            GetSetCache<ComputeBuffer>.Set = static (m, s, v) => m.SetBuffer(s, v);

            GetSetCache<Color>.Get = static (m, s) => m.GetColor(s);
            GetSetCache<Color>.Set = static (m, s, v) => m.SetColor(s, v);

            GetSetCache<Color[]>.Get = static (m, s) => m.GetColorArray(s);
            GetSetCache<Color[]>.Set = static (m, s, v) => m.SetColorArray(s, v);

            GetSetCache<Single>.Get = static (m, s) => m.GetFloat(s);
            GetSetCache<Single>.Set = static (m, s, v) => m.SetFloat(s, v);

            GetSetCache<Single[]>.Get = static (m, s) => m.GetFloatArray(s);
            GetSetCache<Single[]>.Set = static (m, s, v) => m.SetFloatArray(s, v);

            GetSetCache<Int32>.Get = static (m, s) => m.GetInt(s);
            GetSetCache<Int32>.Set = static (m, s, v) => m.SetInt(s, v);

            GetSetCache<Matrix4x4>.Get = static (m, s) => m.GetMatrix(s);
            GetSetCache<Matrix4x4>.Set = static (m, s, v) => m.SetMatrix(s, v);

            GetSetCache<Matrix4x4[]>.Get = static (m, s) => m.GetMatrixArray(s);
            GetSetCache<Matrix4x4[]>.Set = static (m, s, v) => m.SetMatrixArray(s, v);

            GetSetCache<Texture>.Get = static (m, s) => m.GetTexture(s);
            GetSetCache<Texture>.Set = static (m, s, v) => m.SetTexture(s, v);

            GetSetCache<Vector4>.Get = static (m, s) => m.GetVector(s);
            GetSetCache<Vector4>.Set = static (m, s, v) => m.SetVector(s, v);

            GetSetCache<Vector4[]>.Get = static (m, s) => m.GetVectorArray(s);
            GetSetCache<Vector4[]>.Set = static (m, s, v) => m.SetVectorArray(s, v);


            //this._material.SetShaderPassEnabled(name, default);
            //this._material.SetTextureOffset(name, default);
            //this._material.SetTextureScale(name, default);
            //this._material.GetShaderPassEnabled(name);
            //this._material.GetTextureOffset(name);
            //this._material.GetTextureScale(name);

            GetSetCache<Boolean>.Get = static (m, s) => m.GetFloat(s) != 0.0f;
            GetSetCache<Boolean>.Set = static (m, s, v) => m.SetFloat(s, v ? 1f : 0f);

            GetSetCache<Byte>.Get = static (m, s) => (Byte)m.GetFloat(s);
            GetSetCache<Byte>.Set = static (m, s, v) => m.SetFloat(s, (Single)v);

            GetSetCache<Texture2D>.Get = static (m, s) => (Texture2D)m.GetTexture(s);
            GetSetCache<Texture2D>.Set = static (m, s, v) => m.SetTexture(s, v);

        }
    }

    #region UI
    public class UIBarRemap : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/UI/HGUIBarRemap");

        public UIBarRemap(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
            this.remapTex = this.CreateRWProp<Texture2D>("_RemapTex");
            this.gradientScale = this.CreateRWProp<Single>("_GradientScale");
            this.pingPong = this.CreateRWProp<Boolean>("_PingPong");
        }

        public RWMaterialProp<Texture2D> mainTex { get; }
        public RWMaterialProp<Texture2D> remapTex { get; }
        public RWMaterialProp<Single> gradientScale { get; }
        public RWMaterialProp<Boolean> pingPong { get; }
    }

    public class UIBlur : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/UI/HGUIBlur");

        public UIBlur(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.radius = this.CreateRWProp<Byte>("_Radius");
        }

        public RWMaterialProp<Byte> radius { get; }
    }

    public class IgnoreZ : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/UI/HGUIIgnoreZ");

        public IgnoreZ(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
        }

        public RWMaterialProp<Texture2D> mainTex { get; }
    }

    public class OverBrighten : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/UI/HGUIOverBrighten");

        public OverBrighten(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
            this.tint = this.CreateRWProp<Color>("_Tint");
            this.overbrightenScale = this.CreateRWProp<Single>("_OverbrightenScale");
            this.stencilComp = this.CreateRWProp<Single>("_StencilComp");
            this.stencil = this.CreateRWProp<Single>("_Stencil");
            this.stencilOp = this.CreateRWProp<Single>("_StencilOp");
            this.stencilWriteMask = this.CreateRWProp<Single>("_StencilWriteMask");
            this.stencilReadMask = this.CreateRWProp<Single>("_StencilReadMask");
            this.colorMask = this.CreateRWProp<Single>("_ColorMask");
            this.useUIAlphaClip = this.CreateRWProp<Boolean>("_UseUIAlphaClip");
        }

        public RWMaterialProp<Texture2D> mainTex { get; }
        public RWMaterialProp<Color> tint { get; }
        public RWMaterialProp<Single> overbrightenScale { get; }
        public RWMaterialProp<Single> stencilComp { get; }
        public RWMaterialProp<Single> stencil { get; }
        public RWMaterialProp<Single> stencilOp { get; }
        public RWMaterialProp<Single> stencilWriteMask { get; }
        public RWMaterialProp<Single> stencilReadMask { get; }
        public RWMaterialProp<Single> colorMask { get; }
        public RWMaterialProp<Boolean> useUIAlphaClip { get; }
    }


    #endregion




















    #region FX
    public class VertexOnly : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/FX/HGVertexOnly");

        public VertexOnly(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.tintColor = this.CreateRWProp<Color>("_TintColor");
        }

        public RWMaterialProp<Color> tintColor { get; }
    }

    public class DamageNumber : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/FX/HGDamageNumber");

        public DamageNumber(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.tintColor = this.CreateRWProp<Color>("_TintColor");
            this.critColor = this.CreateRWProp<Color>("_CritColor");
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
            this.characterLimit = this.CreateRWProp<Single>("_CharacterLimit");
        }

        public RWMaterialProp<Color> tintColor { get; }
        public RWMaterialProp<Color> critColor { get; }
        public RWMaterialProp<Texture2D> mainTex { get; }
        public RWMaterialProp<Single> characterLimit { get; }
    }

    public class Distortion : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/FX/HGVertexOnly");

        public Distortion(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.colour = this.CreateRWProp<Color>("_Colour");
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
            this.bumpMap = this.CreateRWProp<Texture2D>("_BumpMap");
            this.magnitude = this.CreateRWProp<Single>("_Magnitude");
        }

        public RWMaterialProp<Color> colour { get; }
        public RWMaterialProp<Texture2D> mainTex { get; }
        public RWMaterialProp<Texture2D> bumpMap { get; }
        public RWMaterialProp<Single> magnitude { get; }
    }
    #endregion






























    #region PostProcess
    public class OutlineHighlight : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/PostProcess/HGOutlineHighlight");

        public OutlineHighlight(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.color = this.CreateRWProp<Color>("_Color");
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
            this.outlineMap = this.CreateRWProp<Texture2D>("_OutlineMap");
        }

        public RWMaterialProp<Color> color { get; }
        public RWMaterialProp<Texture2D> mainTex { get; }
        public RWMaterialProp<Texture2D> outlineMap { get; }
    }

    public class SobelBuffer : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/PostProcess/HGSobelBuffer");

        public SobelBuffer(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.color = this.CreateRWProp<Color>("_Color");
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
        }

        public RWMaterialProp<Color> color { get; }
        public RWMaterialProp<Texture2D> mainTex { get; }
    }

    public class ScreenDamage : BaseMaterial
    {
        private static readonly Shader _shader = Resources.Load<Shader>("Shaders/PostProcess/HGScreenDamage");

        public ScreenDamage(Boolean useCache, Material material, Boolean copy = false) : base(useCache, material, copy)
        {
            this.tint = this.CreateRWProp<Color>("_Tint");
            this.normalMap = this.CreateRWProp<Texture2D>("_NormalMap");
            this.mainTex = this.CreateRWProp<Texture2D>("_MainTex");
            this.tintStrength = this.CreateRWProp<Single>("_TintStrength");
            this.desaturationStrength = this.CreateRWProp<Single>("_DesaturationStrength");
            this.distortionStrength = this.CreateRWProp<Single>("_DistortionStrength");
        }

        public RWMaterialProp<Color> tint { get; }
        public RWMaterialProp<Texture2D> normalMap { get; }
        public RWMaterialProp<Texture2D> mainTex { get; }
        public RWMaterialProp<Single> tintStrength { get; }
        public RWMaterialProp<Single> desaturationStrength { get; }
        public RWMaterialProp<Single> distortionStrength { get; }
    }


    #endregion
}