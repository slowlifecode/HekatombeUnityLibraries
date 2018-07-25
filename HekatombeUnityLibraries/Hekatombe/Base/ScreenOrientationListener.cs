using UnityEngine;
using Hekatombe.Services;

namespace Hekatombe.Base
{
    public class ScreenOrientationListener 
    {
		private EventDispatcher _eventDispatcher;
        private ScreenOrientation _screenOrientation = ScreenOrientation.AutoRotation;
		private EventChangeScreenOrientationYield _eventYield;
		//Sometimes with one frame the UI is not fully reestructured, so wait for several frames to send the EventYield
		private int _numFramesEventYield;

		public ScreenOrientationListener(EventDispatcher eventDispatcher)
    	{
			_eventDispatcher = eventDispatcher;
		}
  
		//Call this every Update on the Main Controller of the scene
		public void UpdateScreenOrientation()
		{
			//Yield one frame the Orientation Events to give time the UI to restructurate
			if (_eventYield != null && _numFramesEventYield <= 0) {
				_eventDispatcher.Raise<EventChangeScreenOrientationYield> (_eventYield);
				_eventYield = null;
			}
			_numFramesEventYield = Mathf.Max(0, _numFramesEventYield-1);
			ScreenOrientation so = Screen.orientation;
			if (so != _screenOrientation)
			{
				_screenOrientation = so;
				//Debug.Log ("Unity Screen Orientation: " + _screenOrientation);
				_eventDispatcher.Raise<EventChangeScreenOrientation> (new EventChangeScreenOrientation(_screenOrientation));
				_eventYield = new EventChangeScreenOrientationYield (_screenOrientation);
				_numFramesEventYield = 2;
			}
		}
    }

	public class EventChangeScreenOrientation
	{
		public ScreenOrientation ScreenOrientationMy;
		public bool IsPortrait = false;

		public EventChangeScreenOrientation()
		{
		}

		public EventChangeScreenOrientation(ScreenOrientation so)
		{
			Init (so);
		}

		protected void Init(ScreenOrientation so)
		{
			ScreenOrientationMy = so;
			IsPortrait = false;

			switch (ScreenOrientationMy) {
			case ScreenOrientation.Portrait:
			case ScreenOrientation.PortraitUpsideDown:
				IsPortrait = true;
				break;
			}
		}
	}

	public class EventChangeScreenOrientationYield : EventChangeScreenOrientation
	{
		public EventChangeScreenOrientationYield(ScreenOrientation so)
		{
			Init (so);
		}
	}
}
