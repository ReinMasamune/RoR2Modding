#pragma once
#include "Array.h"

using namespace System;
using namespace BepInEx;
using namespace Util;

namespace cpptest 
{
	[BepInPlugin("com.Rein.cpptest", "Cpptest", "1.0.0")]
	public ref class Class1 : BaseUnityPlugin
	{
		public:
		void Awake();
	};
}
