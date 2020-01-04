namespace ImprovedVisions
{
    using BepInEx;
    //using R2API.Utils;
    using System;

    //[R2APISubmoduleDependency()]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.ImprovedVisions", "Rein-ImprovedVisions", "1.0.0" )]
    public partial class Main : BaseUnityPlugin
    {
        private event Action Load;
        private event Action FirstFrame;
        private event Action Enable;
        private event Action Disable;
        private event Action Frame;
        private event Action PostFrame;
        private event Action Tick;
        private event Action GUI;

        private Boolean canLoad = false;

        partial void Hook();

        public Main()
        {
            this.Hook();
        }

        public void Awake() => this.Load?.Invoke();
        public void Start() => this.FirstFrame?.Invoke();
        public void OnEnable() => this.Enable?.Invoke();
        public void OnDisable() => this.Disable?.Invoke();
        public void Update() => this.Frame?.Invoke();
        public void LateUpdate() => this.PostFrame?.Invoke();
        public void FixedUpdate() => this.Tick?.Invoke();
        public void OnGUI() => this.GUI?.Invoke();
    }
}
