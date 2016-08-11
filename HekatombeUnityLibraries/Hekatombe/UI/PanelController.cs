using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Hekatombe.Base;

namespace Hekatombe.UI
{
	public class PanelController : MonoBehaviour 
	{
		private bool _isInit = false;
	    private CanvasGroup _canvasGroup;
		protected CanvasController _canvasCtrl;

		public void Init()
		{
			if (!_isInit)
			{
				_isInit = true;
				FindReferencesBase ();
				FindReferences ();
				AddListeners ();
				OnInit ();
			}
		}

		void OnDestroy()
		{
			RemoveListeners ();
		}

		protected virtual void OnInit()
		{
		}

		private void FindReferencesBase()
		{
			_canvasGroup = GetComponent<CanvasGroup>().BreakIfNull("Panel Named: " + name + " don't has a CanvasGroup controller");
			_canvasCtrl = transform.parent.GetComponent<CanvasController> ().BreakIfNull ("A Parent with a CanvasController is missing for this Panel: " + name);
		}

		protected virtual void FindReferences()
		{
		}

		protected virtual void AddListeners()
		{
		}

		protected virtual void RemoveListeners()
		{
		}

		public void Show()
		{
			ToggleBasic(true);
			OnShow();
		}

		public void Hide()
		{
			ToggleBasic(false);
			OnHide();
		}

		protected void ToggleBasic(bool flag)
		{
			_canvasGroup.alpha = flag?1.0f:0.0f;
			_canvasGroup.interactable = flag;
			_canvasGroup.blocksRaycasts = flag;
		}

		protected virtual void OnShow()
		{

	    }
	    
	    protected virtual void OnHide()
	    {
	        
	    }
	}
}