#include "pch.h"
#include "Array.h"

template<typename TElement, System::UInt32 length>
inline TElement& Util::Array<TElement, length>::operator[]( System::UInt32 index )
{
	return data[index];
}