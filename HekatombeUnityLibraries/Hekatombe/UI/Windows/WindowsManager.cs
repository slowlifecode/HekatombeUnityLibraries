using UnityEngine;
using System.Collections;
using System;
using Hekatombe.Base;
using System.Collections.Generic;
using Hekatombe.Services;

namespace Hekatombe.UI.Windows
{
	public class OnShowingWindowEvent
	{
		public bool IsShowing;

		public OnShowingWindowEvent(bool isShowing)
		{
			IsShowing = isShowing;
		}
	}

	public class OnHidingWindowEvent
	{
	}

	public class WindowsManager 
	{
		//PWindow pile
		private bool _isInit = false;
		private List<Action> _windows;
		private bool _isShowingWindow = false;
		private static bool _loadFromResources = false;

		public WindowsManager(bool loadFromResources)
	    {
			if (!_isInit) {
				_isInit = true;
				_loadFromResources = loadFromResources;
				_windows = new List<Action> ();
				AddListeners ();
			}
	    }

		public static bool LoadFromResources
		{
			get
			{
				return _loadFromResources;
			}
		}

		public void Kill()
		{
			RemoveListeners ();
		}

		private void AddListeners()
		{
			EventDispatcher.StAddListener<OnHidingWindowEvent> (HandleOnHidingWindowEvent);
		}

		private void RemoveListeners()
		{
			EventDispatcher.StRemoveListener<OnHidingWindowEvent> (HandleOnHidingWindowEvent);
		}

		private void HandleOnHidingWindowEvent(OnHidingWindowEvent e)
		{
			OnHideWindow ();
		}

		//When close a Window check if another has to be shown
		public void OnHideWindow()
		{
			IsShowingWindow = false;
			ShowIfPossible ();
		}

		public void AddEnd(Action action)
		{
			_windows.Add (action);
			ShowIfPossible ();
		}

		//To show a Window that has to be shown immediately after the one it's being shown right now. Ex:
		//-You are in the Payment Window & don't have enough resources, so it's mandatory to show that other window immediately after independently of the windows that are stored in the pile
		public void AddFront(Action action)
		{
			_windows.Insert (0, action);
			ShowIfPossible ();
		}
		
		private void ShowIfPossible()
		{
			if (!IsShowingWindow && _windows.Count > 0) {
				//Executes the action
				if (_windows[0] != null)
				{
					_windows[0]();
					IsShowingWindow = true;
				}
				//Remove that action
				_windows.RemoveAt(0);
			}
		}

		//Between Scenes, there shouldn't be pendant windows
		public void Clear()
		{
			_windows.Clear ();
			IsShowingWindow = false;
		}

		public int Count()
		{
			return _windows.Count;
		}

		private bool IsShowingWindow
		{
			get{
				return _isShowingWindow;
			}

			set{
				_isShowingWindow = value;
				EventDispatcher.StRaise<OnShowingWindowEvent>(new OnShowingWindowEvent(_isShowingWindow));
			}
		}

		public bool GetIsShowingWindow()
		{
			return IsShowingWindow;
		}
	}
}