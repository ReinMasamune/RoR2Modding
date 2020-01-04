namespace ModSync
{
    using BepInEx;
    //using R2API.Utils;
    using System;

    //[R2APISubmoduleDependency()]
    //[BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.ModlistMessages", "Rein-ModlistMessages", "1.0.0" )]
    public partial class Main : BaseUnityPlugin
    {
        const Int16 serverToClient = 200;
        const Int16 clientToServer = 201;
        const Single messageTimeout = 5f;

        private event Action Load;
        private event Action FirstFrame;
        private event Action Enable;
        private event Action Disable;
        private event Action Frame;
        private event Action PostFrame;
        private event Action Tick;
        private event Action GUI;

        partial void AddAttributes();
        partial void CreateConfig();
        partial void GetMods();
        partial void Hook();


        public Main()
        {
            this.AddAttributes();
            this.CreateConfig();
            this.GetMods();
            this.Hook();
        }

        private void Awake() => this.Load?.Invoke();
        private void Start() => this.FirstFrame?.Invoke();
        private void OnEnable() => this.Enable?.Invoke();
        private void OnDisable() => this.Disable?.Invoke();
        private void Update() => this.Frame?.Invoke();
        private void LateUpdate() => this.PostFrame?.Invoke();
        private void FixedUpdate() => this.Tick?.Invoke();
        private void OnGUI() => this.GUI?.Invoke();
    }
}
