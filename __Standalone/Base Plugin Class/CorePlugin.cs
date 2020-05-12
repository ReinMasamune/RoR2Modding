namespace ReinCore
{
    using System;

    using BepInEx;

    public abstract class CorePlugin : BaseUnityPlugin
    {
        protected abstract void Init();

        protected BepInEx.Logging.ManualLogSource logger
        {
            get => base.Logger;
        }

        protected BepInEx.Configuration.ConfigFile config
        {
            get => base.Config;
        }

        protected State state
        {
            get => this._state;
            private set
            {
                if( value != this._state )
                {
                    this.onStateChanged?.Invoke( value );
                }
                this._state = value;
            }
        }
#pragma warning disable IDE1006 // Naming Styles
        private State _state;
#pragma warning restore IDE1006 // Naming Styles

        protected event Action<State> onStateChanged;

        protected event Action awake;
        protected event Action enable;
        protected event Action disable;
        protected event Action start;
        protected event Action fixedUpdate;
        protected event Action update;
        protected event Action lateUpdate;
        protected event Action destroy;
        protected event Action gui;

        private protected virtual void Awake()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.state = State.Awake;
            this.awake?.Invoke();
        }

        private protected virtual void OnEnable()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.state = State.Enable;
            this.enable?.Invoke();
        }

        private protected virtual void OnDisable()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.state = State.Disabled;
            this.disable?.Invoke();
        }

        private protected virtual void Start()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.state = State.Start;
            this.start?.Invoke();
        }

        private protected virtual void FixedUpdate()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.fixedUpdate?.Invoke();
        }

        private protected virtual void Update()
        {
            RoR2.RoR2Application.isModded = true;
            if( this.state == State.Failed )
            {
                return;
            }

            this.state = State.Running;
            this.update?.Invoke();
        }

        private protected virtual void LateUpdate()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.lateUpdate?.Invoke();
        }

        private protected virtual void OnDestroy()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.state = State.Destroying;
            this.destroy?.Invoke();
        }

        private protected virtual void OnGUI()
        {
            if( this.state == State.Failed )
            {
                return;
            }

            this.gui?.Invoke();
        }

        protected virtual void Fail() => this.state = State.Failed;

        protected CorePlugin()
        {
            RoR2.RoR2Application.isModded = true;
            this._state = State.Init;
            this.Init();
        }

        static CorePlugin()
        {
            RoR2.RoR2Application.isModded = true;
        }
    }
}
