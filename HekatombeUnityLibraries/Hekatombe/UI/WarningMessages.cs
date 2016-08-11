using UnityEngine;
using System.Collections;
using System;
using Hekatombe.Base;
using UnityEngine.UI;
using DG.Tweening;
using Hekatombe.Services;

namespace Hekatombe.UI
{
	public class ShowWarningMessageEvent
	{
		public string Message;
		public EWarningMessageType EType;
		public bool AlreadyLocalizated; //Used to avoid searching the localization.Used in cases you need to concat data with text

		public ShowWarningMessageEvent(string message, EWarningMessageType type = EWarningMessageType.Info, bool alreadyLocalizated = false)
		{
			Message = message;
			EType = type;
			AlreadyLocalizated = alreadyLocalizated;
		}
	}

	public enum EWarningMessageType
	{
		Info,
		Good,
		Bad
	}

	public class WarningMessages : MonoBehaviour
	{
		private RectTransform _parentRectTransform;
		private int _indexMessage = kMaxMessages-1;
		private const int kMaxMessages = 6;
		private Text[] _messages = new Text [kMaxMessages];
		private Tween[] _messagesTweens = new Tween [kMaxMessages];
		private float _timeLastMessage;
		private int _countTest = 0;

		private static Vector3 kPosInit = new Vector3(0, 500, 0);
		private const float kDeltaY = 180;
		private const float kTimeMessage = 3;

		public void Init(RectTransform parentRectTransform)
		{
			_parentRectTransform = parentRectTransform;
			AddListeners ();
		}

		void OnDestroy()
		{
			RemoveListeners ();
		}

		private void AddListeners()
		{
			EventDispatcher.StAddListener<ShowWarningMessageEvent>(HandleShowWarningMessageEvent);
		}
		
		private void RemoveListeners()
		{
			EventDispatcher.StRemoveListener<ShowWarningMessageEvent>(HandleShowWarningMessageEvent);
		}

		private void HandleShowWarningMessageEvent(ShowWarningMessageEvent e)
		{
			AddMessage (e.Message, e.EType, e.AlreadyLocalizated);
		}

		public static void ShowWarningMessage(string message, EWarningMessageType type = EWarningMessageType.Info, bool alreadyLocalizated = false)
		{
			//Uses this DispatchEvent system to avoid keeping a Singleton of the WarningMessages
            //So if nothing is shown, it's becauseyou haven't initialized the class anywhere (preferably in ServiceLocator)
			EventDispatcher.StRaise<ShowWarningMessageEvent> (new ShowWarningMessageEvent (message, type, alreadyLocalizated));
		}

		private void AddMessage(string message, EWarningMessageType type, bool alreadyLocalizated)
		{
			//Check index
			if (Time.timeSinceLevelLoad > _timeLastMessage + kTimeMessage) {
				_indexMessage = 0;
			} else {
				_indexMessage = (_indexMessage+1) % kMaxMessages;
			}
			
			//Instantiate
			_timeLastMessage = Time.timeSinceLevelLoad;
			Text t = GameObjectExtension.LoadAndInstantiate<Text> ("UI/WarningMessage", Vector3.zero).BreakIfNull();
			t.transform.SetParent (_parentRectTransform);
			t.transform.localScale = Vector3.one;
			Vector3 pos = kPosInit;	
			pos.y += -kDeltaY * _indexMessage;
			t.transform.localPosition = pos;
			
			//Text & Color
			if (alreadyLocalizated) {
				t.text = message;
			} else {
                t.text = Localization.Localization.Get (message);
			}
			switch (type) {
			case EWarningMessageType.Good:
				t.color = Color.green;
				break;
			case EWarningMessageType.Bad:
				t.color = Color.red;
				break;
			}
			//Destroy the possible GameObjects/Tweens
			if (_messagesTweens [_indexMessage] != null) {
				_messagesTweens [_indexMessage].Kill ();
				_messagesTweens [_indexMessage] = null;
			}
			if (_messages [_indexMessage] != null) {
				Destroy(_messages[_indexMessage].gameObject);
				_messages [_indexMessage] = null;
			}
			//Save new message elements
			_messagesTweens[_indexMessage] = t.DOFade (0, 1).SetEase(Ease.Linear).SetDelay(2).OnComplete(()=>Destroy (t.gameObject)).OnStart(()=>Destroy (t.gameObject.GetComponent<Outline>()));//Destrueix l'Outline quan comenci a esvaïr-se
			_messages [_indexMessage] = t;
		}

		#if UNITY_EDITOR
		void Update()
		{
			if (Input.GetKeyDown (KeyCode.M)) {
				WarningMessages.ShowWarningMessage(_countTest + "warning-message_test_to_raise", EWarningMessageType.Good);
				_countTest++;
			}
		}
		#endif
	}
}
