using UnityEngine;
using System.Collections;
using System;
using Hekatombe.UI.Windows;
using UnityEngine.UI;
using Hekatombe.Base;

namespace Hekatombe.UI
{
	public class CanvasController : MonoBehaviour
	{
		private bool _isInit = false;
		private Canvas _canvas;
		protected PanelController _panelEnabled;
		public Camera CamUI{get; set;}
		private Camera _camWorld;
	    public RectTransform RectTransformRef{get;set;}
		private GraphicRaycaster _graphicRaycasterRef;

		public void Init()
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
			_canvas = GetComponent<Canvas> ().BreakIfNull();
			//Only find the camera if it corresponds with the type
			if (_canvas.renderMode == RenderMode.ScreenSpaceCamera) {
				CamUI = gameObject.GetComponentInChildren<Camera> ().BreakIfNull ();
			}
			RectTransformRef = GetComponent<RectTransform> ().BreakIfNull ();
			_graphicRaycasterRef = GetComponent<GraphicRaycaster> ().BreakIfNull ();
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

		public void SetCamWorld(Camera camWorld)
		{
			_camWorld = camWorld;
		}

		protected void PanelEnabled(PanelController panel)
		{
			if (panel != _panelEnabled)
			{
				if (_panelEnabled != null)
				{
					_panelEnabled.Hide ();
				}
				_panelEnabled = panel;
				_panelEnabled.Show ();
			}
		}

		public void HidePanelEnabled()
		{
			if (_panelEnabled != null)
			{
				_panelEnabled.Hide ();
				_panelEnabled = null;
			}
		}

		public GameObject InstantiateUI(string path)
		{
			GameObject prefab = Resources.Load<GameObject>(path);
			GameObject go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			go.transform.SetParent(this.transform);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			return go;
		}

		public Vector2 PositionWorldToScreen(Vector3 posWorld)
		{
			return _camWorld.WorldToScreenPoint (posWorld);
		}

		public Vector2 PositionWorldToUI(Vector3 posWorld)
		{ 	
            Vector2 pos;
            //Get Object 3D position respect the screen
			Vector2 posScreen = PositionWorldToScreen(posWorld);
			//posScreen = Input.mousePosition;//For test purposes
			//Transform Screen position to the UI position
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransformRef, posScreen, CamUI, out pos);
			return pos;
		}

		//Meaning in one RectTransform of a Canvas respect another RectTransform
		public Vector2 PositionUIToUI(Vector3 positionNOTLocal)
		{ 	
			Vector2 pos;
			pos = RectTransformUtility.WorldToScreenPoint(CamUI, positionNOTLocal);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransformRef, pos, CamUI, out pos);
			return pos;
		}

		//Has to be the transform.position!!
		public Vector2 PositionUIToScreen(Vector3 posUI)
		{
			return CamUI.WorldToScreenPoint (posUI);
		}

		public PanelController GetPanelEnabled()
		{
			return _panelEnabled;
		}

		public void EnableInteraction(bool enable)
		{
			_graphicRaycasterRef.enabled = enable;
		}
	}
}
