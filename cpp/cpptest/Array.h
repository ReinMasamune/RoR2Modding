#pragma once

namespace Util
{
	template<typename TElement, System::UInt32 length>
	ref class Array
	{
		private:
		array<TElement> ^ data = gcnew array<TElement>( length );

		public:
		property TElement default[ UInt32 ]
		{
			TElement get( UInt32 index ) { return data[index]; }
			//void set( UInt32 index, TElement value ) { data[index] = value; };
		}
	};
}