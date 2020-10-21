#pragma once

namespace Util
{
	template<typename TElement, System::UInt32 length>
	ref class Array
	{
		T arr[size];
	public:
		T& operator[](size_t i)
		{
			return arr[i];
		}
	};
}