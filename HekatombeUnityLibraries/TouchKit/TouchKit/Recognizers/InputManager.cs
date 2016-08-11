using UnityEngine;
using System.Collections;
using Hekatombe.Services;

public class EventSwipe
{
	public TKSwipeDirection4Dirs Direction;

	public EventSwipe(TKSwipeDirection4Dirs dir)
	{
		Direction = dir;
	}
}

public class InputManager : MonoBehaviour{

	public void Init()
	{
		//Swipe Touch
		TouchKit.removeAllGestureRecognizers();
		//(8 directions)
		//var recognizer = new TKSwipeRecognizer();
		//(4 directions)
		var recognizer = new TKSwipeRecognizer4Dirs(0.15f);
		recognizer.gestureRecognizedEvent += ( r) => {
			CreateSwipeEvent (r.completedSwipeDirection);
		};
		TouchKit.addGestureRecognizer( recognizer );
	}

	private void CreateSwipeEvent(TKSwipeDirection4Dirs dir)
	{
		EventDispatcher.StRaise<EventSwipe> (new EventSwipe(dir));
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W)) {
			CreateSwipeEvent (TKSwipeDirection4Dirs.Up);
		} else if (Input.GetKeyDown(KeyCode.A)) {
			CreateSwipeEvent (TKSwipeDirection4Dirs.Left);
		} else if (Input.GetKeyDown(KeyCode.S)) {
			CreateSwipeEvent (TKSwipeDirection4Dirs.Down);
		} else if (Input.GetKeyDown(KeyCode.D)) {
			CreateSwipeEvent (TKSwipeDirection4Dirs.Right);
		}
	}
}
