using System;

namespace Hekatombe.Base
{
	public class EnumClass : IEquatable<EnumClass>
	{
		public string Name { private set; get; }
		public int Value { private set; get; }

		//Constructor, just copy it on every inherited class
		public EnumClass( int value, string name )
		{
			Value = value;
			Name = name;
		}

		public override string ToString ()
		{
			return Name + " - " + Value;
		}

		public bool Equals(EnumClass other)
		{
			return other.Name == this.Name && other.Value == this.Value;
		}
	}
}