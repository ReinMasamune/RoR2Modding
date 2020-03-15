using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BepInEx;

namespace ReinCore
{
    public abstract class CorePlugin : BaseUnityPlugin
    {
        private protected BepInEx.Logging.ManualLogSource logger
        {
            get => base.Logger;
        }

        private protected BepInEx.Configuration.ConfigFile config
        {
            get => base.Config;
        }

        private protected State state
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
        private State _state = State.Constructor;
        private protected event Action<State> onStateChanged;


        private protected event Action awake;
        private protected event Action enable;
        private protected event Action disable;
        private protected event Action start;
        private protected event Action fixedUpdate;
        private protected event Action update;
        private protected event Action lateUpdate;
        private protected event Action destroy;
        private protected event Action gui;

        private protected virtual void Awake()
        {
            if( this.state == State.Failed ) return;
            this.state = State.Awake;
            this.awake?.Invoke();
        }

        private protected virtual void OnEnable()
        {
            if( this.state == State.Failed ) return;
            this.state = State.Enable;
            this.enable?.Invoke();
        }

        private protected virtual void OnDisable()
        {
            if( this.state == State.Failed ) return;
            this.state = State.Disabled;
            this.disable?.Invoke();
        }

        private protected virtual void Start()
        {
            if( this.state == State.Failed ) return;
            this.state = State.Start;
            this.start?.Invoke();
        }

        private protected virtual void FixedUpdate()
        {
            if( this.state == State.Failed ) return;
            this.fixedUpdate?.Invoke();
        }

        private protected virtual void Update()
        {
            RoR2.RoR2Application.isModded = true;
            if( this.state == State.Failed ) return;
            this.state = State.Running;
            this.update?.Invoke();
        }

        private protected virtual void LateUpdate()
        {
            if( this.state == State.Failed ) return;
            this.lateUpdate?.Invoke();
        }

        private protected virtual void OnDestroy()
        {
            if( this.state == State.Failed ) return;
            this.state = State.Destroying;
            this.destroy?.Invoke();
        }

        private protected virtual void OnGUI()
        {
            if( this.state == State.Failed ) return;
            this.gui?.Invoke();
        }

        private protected virtual void Fail()
        {
            this.state = State.Failed;
        }

        private CorePlugin()
        {
            RoR2.RoR2Application.isModded = true;
        }

        static CorePlugin()
        {
            RoR2.RoR2Application.isModded = true;
        }
    }
}
