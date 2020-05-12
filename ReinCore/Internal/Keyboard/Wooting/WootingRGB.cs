namespace ReinCore.Wooting
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Rein.Properties;

    using UnityEngine;

    using static global::ReinCore.Wooting.WootingRGBHelpers;

    internal class WootingRGB : IKeyboardRGB, IDisposable
    {
        #region Constants
        internal const String wootingDllName = "wooting-rgb-sdk64.dll";
        internal const Int32 maxRows = 6;
        internal const Int32 maxCols = 21;
        #endregion
        #region Constructor
        static WootingRGB()
        {

        }

        internal WootingRGB( KeyboardType type )
        {
            if( instance != null )
            {
                throw new Exception( String.Format( "Cannot create multiple instances of {0}", nameof( WootingRGB ) ) );
            }

            if( !this.CheckIsValid( type ) )
            {
                throw new ArgumentException( "Wrong keyboard type" );
            }

            instance = this;
        }

        ~WootingRGB()
        {
            this.InstanceCleanup();
            StaticCleanup();
        }
        #endregion
        #region Static External



        #endregion
        #region Static Internal
        private static WootingRGB instance;
        private static Boolean assemblyLoaded = false;
        private static readonly Boolean updateRegistered = false;
        private static Boolean polling = true;
        private static Boolean pendingChanges = false;
        private static String dllPath;
        private static IList<GlobalKeys> allKeys;

        private static event Action onDisconnect;
        private static event Action onReconnect;

        private static void StaticCleanup()
        {
            try
            {
                _ = AppExit();
            } catch { }
            try
            {
                Directory.Delete( dllPath, true );
            } catch { }
        }

        private static void LoadAssembly()
        {
            dllPath = Tools.LoadUnmanagedAssembly( wootingDllName, Rein.Properties.Resources.wooting_rgb_sdk64 );
            SetupDisconnectCallback();
            disconnectCallback += OnDisconnect;
            assemblyLoaded = true;
        }

        private static void OnDisconnect()
        {
            if( !polling )
            {
                ReinCore.fixedUpdate += Poll;
                polling = true;

                onDisconnect?.Invoke();
            }
        }
        private static void Poll()
        {
            if( IsConnected() )
            {
                onReconnect?.Invoke();
                ReinCore.fixedUpdate -= Poll;
                polling = false;
            }
        }

        private static void LateUpdate()
        {
            if( pendingChanges )
            {
                pendingChanges &= !UpdateKeys();
            }
        }
        #endregion
        #region Instance External
        public Boolean CheckIsValid( KeyboardType type ) => type == KeyboardType.Wooting;
        public void LogDeviceInfo()
        {

        }
        public Boolean SetKeyRGB( GlobalKeys key, Color rgb )
        {
            Boolean temp = SetKey( key, rgb );
            pendingChanges |= temp;
            return temp;
        }
        public IList<GlobalKeys> GetAllKeys()
        {
            if( allKeys == null )
            {
                allKeys = GlobalKeyExtensionsWooting.GetAllWootingKeys();
            }

            return allKeys;
        }
        public void Activate()
        {
            if( !assemblyLoaded )
            {
                LoadAssembly();
            }

            ReinCore.lateUpdate += LateUpdate;
        }
        public void Deactivate() => ReinCore.lateUpdate -= LateUpdate;
        public Boolean CheckConnected()
        {
            if( !assemblyLoaded )
            {
                LoadAssembly();
            }

            return IsConnected();
        }
        public void Dispose()
        {
            this.InstanceCleanup();
            StaticCleanup();
        }
        #endregion
        #region Instance Internal
        private void InstanceCleanup()
        {

        }
        #endregion


    }
}
