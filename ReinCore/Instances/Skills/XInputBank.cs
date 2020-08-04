namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;

    using RoR2;

    public class XInputBank : InputBankTest
    {
        internal void SetCap( Int32 newCount )
        {
            for( Int32 i = newCount; i > this.customInputs.Count; --i ) this.customInputs.RemoveAt( this.customInputs.Count - 1 );
            for( Int32 i = this.customInputs.Count; i < newCount; ++i ) this.customInputs.Add( default );
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<InputBankTest.ButtonState> __backingInputs;
        internal List<InputBankTest.ButtonState> customInputs { get => this.__backingInputs ??= new List<ButtonState>( KeybindsCore.GetInputsCount( this.SetCap ) ); }
        public InputBankTest.ButtonState this[Int32 index]
        {
            get => this.customInputs[index];
            internal set => this.customInputs[index] = value;
        }
    }
}
