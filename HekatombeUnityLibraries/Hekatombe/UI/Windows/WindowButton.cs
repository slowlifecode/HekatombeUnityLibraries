using UnityEngine;
using UnityEngine.UI;
using Hekatombe.Base;
using UnityEngine.Events;

namespace Hekatombe.UI.Windows
{
	public class WindowButton : MonoBehaviour {
		[HideInInspector]
		public Button ButtonRef;
		[HideInInspector]
		public Text LblRef;

		public void Init()
		{
			ButtonRef = GetComponent<Button> ().BreakIfNull ();
			LblRef = transform.GetComponentInChildren<Text> ();
			if (LblRef == null) {
				Debug.LogWarning ("Window Button Label not found");
			}
		}

		public void Init(string strButton)
		{
			Init ();
			SetText (strButton);
		}

		public void Init(UnityAction action)
		{
			Init ();
			SetAction (action);
		}

		public void SetText(string text)
		{
			if (LblRef == null) {
				return;
			}
			LblRef.text = text;
		}

		public void CleanActions()
		{
			ButtonRef.onClick.RemoveAllListeners ();
		}

		public void SetAction(UnityAction action)
		{
			CleanActions ();
			ButtonRef.onClick.AddListener(action);
		}

		public void SetActive(bool flag)
		{
			gameObject.SetActive (flag);
		}

		public bool Interactable
		{
			get{
				return ButtonRef.interactable;
			}
			set{
				ButtonRef.interactable = value;
			}
		}
	}
}