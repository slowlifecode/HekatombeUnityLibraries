using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Hekatombe.Base;

namespace Hekatombe.Base
{
	public class InputFieldExtended : InputField {

		Text _placeholder;

		public void Init()
		{
			_placeholder = placeholder.GetComponentBreakIfNull<Text> ();
		}

		public string PlaceholderText
		{
			get{
				return _placeholder.text;
			}
			set{
				_placeholder.text = value;
			}
		}
	}
}
