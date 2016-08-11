using UnityEngine;

namespace Hekatombe.Services
{
	/******
	 * It's used like an interface to force Scene controllers have common methods:
	 *****/

	public class SceneController : MonoBehaviour 
	{
		[HideInInspector]
		public bool IsInit = false;

		/*
		//In the Main Scene Controller you have to copy this
		void Start () 
		{
			new ServiceLocator().Init(this.gameObject, OnDataLoadedCallback);
		}
		*/

		// Use this for initialization
		protected virtual void FindReferences()
        {
            Debug.LogError ("This method should be override without calling base");
		}

		protected virtual void Init()
        {
            Debug.LogError ("This method should be override without calling base");
		}

		protected virtual void OnDataLoadedCallback()
		{
			IsInit = true;
			FindReferences ();
			Init ();
		}
	}
}