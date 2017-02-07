using System;
using UnityEngine;

namespace Hekatombe.Base
{
	public class LayersBase
	{	
		//Remember that has to keep the same order than layers
		//The 3 first Unknown layers are mandatory, if not order is broken
		//TODO: Improve this to use EnumClass to let add more Layers on the project without modify this class
		public const int LayDefault = 0;
		public const int LayTransparentFX = 1;
		public const int LayIgnoreRaycast = 2;
		public const int LayUnknown0 = 3;
		public const int LayWater = 4;
		public const int LayUI = 5;
		public const int LayUnknown1 = 6;
		public const int LayUnknown2 = 7;
	}
}