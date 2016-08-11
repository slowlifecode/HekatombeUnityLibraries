using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Hekatombe.Utils;
using Hekatombe.Base;
using Hekatombe.Localization;

namespace Hekatombe.UI.Windows
{
	public class WindowDefault : WindowController 
	{
		public Text LblTitle;
		public Text LblQuestion;
	    public WindowButton ButtonOk;
	    public WindowButton ButtonNo;
		public WindowButton ButtonExit;

		private Action OkCallback;
		private Action NoCallback;

		//Just show one Ok Button
		public static WindowDefault Create(string title, string question)
		{
			WindowDefault window = Create (title, question, null, null);
			window.HideButtonNo ();
			return window;
		}
		
		public static WindowDefault Create(string title, string question, Action okCallback)
		{
			return Create (title, question, okCallback, null);
		}
		
		public static WindowDefault Create(string title, string question, Action okCallback, bool showNoButton)
		{
			WindowDefault window = Create (title, question, okCallback, null);
			if (!showNoButton) 
			{
				window.HideButtonNo();
			}
			return window;
		}
		
		public static WindowDefault Create(string title, string question, Action okCallback, Action noCallback)
		{
			WindowDefault window = (WindowDefault)Create (EWindowNames.Default);
			window.Init (title, question, okCallback, noCallback);
			window.Show ();
			return window;
		}

		public void Init(string title, string question, Action okCallback, Action noCallback)
		{
			LblTitle.text = title;
			LblQuestion.text = question;
            //TODO: Improve default texts by passing them on the creation method
			//ButtonOk.Init (Localization.Get("ui-button_ok"));
			ButtonOk.Init("OK");
			//ButtonNo.Init (Localization.Get("ui-button_no"));
			ButtonNo.Init("NO");
			if (ButtonExit != null) {
				ButtonExit.Init ();
			}
			OkCallback = okCallback;
			NoCallback = noCallback;
		}

		override protected void ShowSpec()
		{
			FadeIn ();
		}

		override protected void HideSpec()
		{
			FadeOut ();
		}

	    protected override void OnStart()
	    {
			ButtonOk.SetAction(() => OnButtonOkClick());
			ButtonNo.SetAction(() => OnButtonNoClick());
			if (ButtonExit != null) {
				ButtonExit.SetAction (() => OnButtonExit ());
			}
	    }

	    private void OnButtonOkClick()
	    {
			if (OkCallback != null) 
			{
				OkCallback();
			}
	        Hide();
	    }
	    
	    private void OnButtonNoClick()
		{
			if (NoCallback != null) 
			{
				NoCallback();
			}
	        Hide();
	    }
	    
	    private void OnButtonExit()
		{
			OnButtonNoClick ();
	    }

		private void HideButtonNo()
		{
			ButtonNo.gameObject.SetActive (false);
			Vector3 posButton = ButtonOk.transform.localPosition;
			posButton.x = 0;
			ButtonOk.transform.localPosition = posButton;
		}
	}
}
