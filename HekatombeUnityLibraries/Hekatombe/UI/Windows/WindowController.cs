using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Hekatombe.Base;
using Hekatombe.Services;
using DG.Tweening;

namespace Hekatombe.UI.Windows
{
	public class WindowController : MonoBehaviour 
	{
		protected CanvasGroup _canvasGroup;
		protected RectTransform _rectTransform;
		public Action OnCloseCallback;
		private bool _isShown = false;
		//Probably the Window can have a container. If there it is default scale and fade methods modify this
		protected RectTransform _container;
		protected Vector3 _posOriginal;

		public static RectTransform CanvasWindows;

		private const string kWindowBasePath = "UI/Windows/";

		//Info: Has to be called every SceneInit
		public static void InitCanvasWindows()
		{
			if (CanvasWindows == null) 
			{
				//Find an Object called CanvasWindows or instantiate a new Object to place the Windows
				GameObject go = GameObject.Find ("CanvasWindows");
				if (go != null) {
					CanvasWindows = go.GetComponent<RectTransform> ();
				} else {
					CanvasWindows = GameObjectExtension.LoadAndInstantiate<RectTransform> (kWindowBasePath + "CanvasWindows", Vector3.zero);
				}
			}
		}

		//After InitCanvasWindows, but EventDispatcher has to be initialized, so has to init ServiceLocator first
		public static void AddWarningMessages()
		{
			//Message Warning Component
			WarningMessages wm = CanvasWindows.gameObject.AddComponent<WarningMessages> ();
			wm.Init (CanvasWindows);
		}

		public static WindowController Create(EWindowNames winName)
	    {
			Transform trans = DynamicAssetLoader.Spawn(WindowsManager.LoadFromResources, "UI/Windows/", "Window" + winName.Name, Vector3.zero, Quaternion.identity).transform;
			trans.gameObject.SetLayerRecursively (LayersBase.LayUI);
			trans.SetParent(CanvasWindows.transform);
			trans.localScale = Vector3.one;
			trans.SetSiblingIndex (0);
			RectTransform rectTransform = trans.GetComponent<RectTransform> ();
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			WindowController window =  trans.GetComponent<WindowController> ().BreakIfNull("No Component WindowController attached to " + trans.name);
			window.FindInternalReferences ();
			return window;
	    }

		public void SetInFront()
		{
			transform.SetSiblingIndex (transform.parent.childCount-1);
		}

		public void FindInternalReferences()
		{
			if (_canvasGroup == null) {
				_canvasGroup = GetComponent<CanvasGroup> ().BreakIfNull ("Window Named: " + name + " don't has a CanvasGroup controller");
				_rectTransform = (RectTransform)_canvasGroup.transform;
				//If doesn't have a child called Container, select the only child. If not, select itself.
				if (transform.Find ("Container")) {
					_container = (RectTransform)transform.Find ("Container");
				} else if (_rectTransform.childCount == 1) {
					_container = (RectTransform)_rectTransform.GetChild (0);
				} else {
					_container = _rectTransform;
				}
				_posOriginal = _container.localPosition;
			}
			FindReferences ();
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

		void Start()
		{
			OnStart();
		}

		protected virtual void OnStart()
		{
		}

		public void Show()
		{
			_isShown = true;
			AddListeners ();
			ShowSpec ();
		}

		public void Hide()
		{
			_isShown = false;
			RemoveListeners ();
			if (OnCloseCallback != null) 
			{
				OnCloseCallback();
			}
	        HideSpec();
		}

		//Just Hide if it's not already hidden and notify it
		public bool HideSafe()
		{
			if (IsShown) {
				Hide ();
				return true;
			}
			return false;
		}

		//Destroy immediately the Window because a change on Scene
		//This way it is removed from WindowManager
		public void HideForcedOnLoadScene()
		{
			_isShown = false;
			if (OnCloseCallback != null) 
			{
				OnCloseCallback();
			}
			HideEnd ();
		}

		protected virtual void ShowSpec()
		{
	    }
	    
        //All HideSpecs that overwrite had to finally call HideEnd() at the end of the FadeOut
	    protected virtual void HideSpec()
	    {
			HideEnd ();
		}

		protected void HideEnd()
		{
			//Previous: WindowControllers, opposite from Panels, has to be destroyed after hide
			_canvasGroup.alpha = 0;
			//Hide Window
			DestroyWindow();
            //Notice Manager that loads next Window (If there is a manager)
			EventDispatcher.StRaise<OnHidingWindowEvent>(new OnHidingWindowEvent());
		}

		protected void FadeIn()
		{
			FadeIn (0.25f);
		}

		protected void FadeIn(float time)
		{
			_canvasGroup.alpha = 0;
			_canvasGroup.DOFade (1, time).SetEase (Ease.Linear);
			_container.localScale = Vector2.one * 0.5f;
			_container.DOScale (Vector3.one * 1, time).SetEase (Ease.OutBack);
		}

		protected void FadeOut()
		{
			float time = 0.15f;
			_canvasGroup.DOFade (0, time).SetEase (Ease.Linear);
			_container.DOScale (Vector3.one * 0.5f, time).SetEase (Ease.InQuad).OnComplete(HideEnd);
		}

		protected void FadeInFromSide(float time, float deltaX)
		{
			_canvasGroup.alpha = 0;
			_canvasGroup.DOFade (1, time).SetEase (Ease.Linear);
			_container.localPosition = _posOriginal.CopyVectorButModifyX (deltaX);
			_container.DOLocalMoveX (_posOriginal.x, time).SetEase (Ease.OutQuad);
		}

		protected void FadeOutToSide(float time, float deltaX)
		{
			_canvasGroup.DOFade (0, time).SetEase (Ease.Linear);
			_container.DOLocalMoveX (_container.localPosition.x+deltaX, time).SetEase (Ease.OutQuad).OnComplete(HideEnd);
		}

		private void DestroyWindow()
		{
			DynamicAssetLoader.Despawn(WindowsManager.LoadFromResources, gameObject);
		}

		public bool IsShown
		{
			get{
				return _isShown;
			}
			set{
				_isShown = value;
			}
		}
	}
}