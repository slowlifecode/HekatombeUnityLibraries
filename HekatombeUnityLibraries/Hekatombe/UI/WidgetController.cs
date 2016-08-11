using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace Hekatombe.UI
{
	public class WidgetController : MonoBehaviour 
	{
		private bool _isInit = false;
	    private CanvasGroup _canvasGroup;
		protected RectTransform _rectTransform;
		[HideInInspector]
		public bool IsShown = false;

		public void InitBase()
		{
			if (!_isInit) {
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

		private void FindReferencesBase()
		{
			_canvasGroup = GetComponent<CanvasGroup> ();
			_rectTransform = GetComponent<RectTransform> ();
		}

		protected virtual void FindReferences()
		{
		}

		protected virtual void OnInit()
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
			if (!IsShown) {
				ToggleBasic (true);
				OnShow ();
			}
		}

		public void Hide()
		{
			if (IsShown) {
				ToggleBasic (false);
				OnHide ();
			}
		}

		private void ToggleBasic(bool flag)
		{
			IsShown = flag;
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