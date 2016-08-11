using Hekatombe.Base;

namespace Hekatombe.UI.Windows
{
	//Make a inherit class of this in your common scripts
	public class EWindowNames : EnumClass
	{
		public static readonly EWindowNames Default = new EWindowNames(0, "Default");

		//Constructor, just copy it on every inherited class
		public EWindowNames( int value, string name ):base( value, name){}
	}
}