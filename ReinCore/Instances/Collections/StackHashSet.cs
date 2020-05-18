namespace ReinCore
{
    using System;

    public ref struct StackSet<TObject>
    {
        public unsafe StackSet( Byte* ptr, Int32 maxLength )
        {
            for( Int32 i = 0; i < maxLength; ++i )
            {
                ptr[i] = 0;
            }

            this.ptr = ptr;
            this.count = 0;
            this.indexCount = 0;
            this.maxLength = maxLength * 8;
        }

        public Int32 count { get; private set; }

        public Boolean Contains( TObject item )
        {
            if( this.count == 0 ) return false;

            Int32 code = item.GetHashCode();
            Int32 ind = code % this.maxLength;
            Int32 index = ind / 8;
            Byte insideInd = (Byte)(ind % 8);
            Byte mask = (Byte)(0b0000_0001 << insideInd);
            unsafe
            {
                Byte cVal = this.ptr[index];
                if( ( cVal & mask ) != 0 ) return true;
            }
            return false;
        }

        public Boolean Add( TObject item )
        {
            Int32 code = item.GetHashCode();
            Int32 ind = code % this.maxLength;
            Int32 index = ind / 8;
            Byte insideInd = (Byte)(ind % 8);
            Byte mask = (Byte)(0b0000_0001 << insideInd);
            unsafe
            {
                Byte cVal = this.ptr[index];
                if( ( cVal & mask ) != 0 ) return false;

                cVal |= mask;
                this.ptr[index] = cVal;
                this.count++;
            }

            return true;
        }

        public void Clear()
        {
            if( this.count == 0 ) return;
            for( Int32 i = 0; i < this.maxLength; i += 8 )
            {
                unsafe
                {
                    this.ptr[i / 8] = 0;
                }
            }
        }

        public Boolean Remove( TObject item )
        {
            if( this.count == 0 ) return false;
            Int32 code = item.GetHashCode();
            Int32 ind = code % this.maxLength;
            Int32 index = ind / 8;
            Byte insideInd = (Byte)(ind % 8);
            Byte mask = (Byte)(0b0000_0001 << insideInd);
            unsafe
            {
                Byte cVal = this.ptr[index];
                if( ( cVal & mask ) == 0 ) return false;

                cVal &= (Byte)~mask;
                this.ptr[index] = cVal;
                this.count--;
            }

            return true;
        }

        private readonly unsafe Byte* ptr;
        private readonly Int32 indexCount;
        private readonly Int32 maxLength;
    }
}
