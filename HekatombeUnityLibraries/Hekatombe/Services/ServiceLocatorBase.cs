using UnityEngine;
using System;

namespace Hekatombe.Services
{
    public class ServiceLocatorBase
    {
		private static ServiceLocatorBase _instance;

    	public void Init(GameObject mainGameObject, Action onDataLoadedCallback)
    	{
			OnInitEveryScene ();
    		if (_instance == null)
			{
				_instance = this;
				//EventDispatcher
				new EventDispatcher();
				OnInitUnique (onDataLoadedCallback);
    		} else {
				if (onDataLoadedCallback != null)
				{
					onDataLoadedCallback();
				}
			}
			OnInitEverySceneAfterUnique ();
        }

		/***
		 * Create a ServiceLocator that inherits ServiceLocatorBase that implements these 2 next methods
		 */

		//Things like GameData that are only initilized once
		protected virtual void OnInitUnique(Action onDataLoadedCallback)
		{
			Debug.LogError ("This method should be override without calling base");
		}

		//Things like the references to the UI, that are initilized everytime a Scene is loaded
		protected virtual void OnInitEveryScene()
        {
            Debug.LogError ("This method should be override without calling base");
		}

		//Things like the references to the UI, that are initilized everytime a Scene is loaded, and need to be loaded after OnInitUnique
		protected virtual void OnInitEverySceneAfterUnique()
		{
		}
    }
}
