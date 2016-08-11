using System;
using System.Collections.Generic;

namespace Hekatombe.DataHelpers
{
    public struct BytesHelper
    {
        public byte[] Bytes;

        public string String
        {
            get
            {
                return Bytes != null ? System.Text.Encoding.ASCII.GetString(Bytes) : string.Empty;
            }

            set
            {
                Bytes = System.Text.Encoding.ASCII.GetBytes(value);
            }
        }

        public int Length
        {
            get
            {
                return Bytes != null ? Bytes.Length : 0;
            }
        }

		public BytesHelper(string str)
        {
			Bytes = System.Text.Encoding.ASCII.GetBytes(str);
		}
		
		//That Encoding stuff in UTF8, a little dirty, but solves the problem
		//Instructions for the UTF8:
		// * Have to "save as" the doc.json with Sublime Text 
		// * Save with encoding > "UTF8"
		// * Has to read string as UTF8, therefore it should looks like:
		// * new JsonAttrParser().Parse(new Data(localizationJsonStr, System.Text.Encoding.UTF8), true));
		public BytesHelper(string str, System.Text.Encoding enc)
		{
			Bytes = enc.GetBytes(str);
		}

		public BytesHelper(byte[] bytes)
        {
            Bytes = bytes;
        }

        public override string ToString()
        {
            return String;
		}
		
		public string StringUTF8 {
			get {
				return Bytes != null ? System.Text.Encoding.UTF8.GetString (Bytes) : string.Empty;
			}
		}
    }

}

