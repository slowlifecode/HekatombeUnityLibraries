using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

namespace Hekatombe.Base
{
	public class ImageTransform : MonoBehaviour {

		private bool _isInit = false;
		private Image _image;
		private RectTransform _trans;
		private Color _originalColor;

		// Use this for initialization
		public void Init()
		{
			if (!_isInit) {
				_image = GetComponent<Image> ().BreakIfNull ();
				_trans = GetComponent<RectTransform> ().BreakIfNull ();
				_originalColor = _image.color;
				_isInit = true;
			}
		}

		public ImageTransform InitReturn()
		{
			Init ();
			return this;
		}

		public void Hide()
		{
			KillTweenImage ();
			_image.color = _originalColor.Transparent ();
		}

		public void Show()
		{
			KillTweenImage ();
			_image.color = _originalColor;
		}

		public void HideSmooth()
		{
			KillTweenImage ();
			_image.DOFade (0, 0.4f).SetEase(Ease.Linear);
		}

		public void ShowSmooth()
		{
			KillTweenImage ();
			Hide ();
			_image.DOFade (1, 0.4f).SetEase(Ease.Linear);
		}

		public Vector2 Size
		{
			get{
				return _trans.sizeDelta;
			}
			set{
				_trans.sizeDelta = value;
			}
		}

		public Vector3 LocalPosition
		{
			get{
				return _trans.localPosition;
			}
			set{
				_trans.localPosition = value;
			}
		}

		public void KillTweens()
		{
			KillTweenImage ();
			KillTweenTransform ();
		}

		public void KillTweenImage()
		{
			_image.DOKill ();
		}

		public void KillTweenTransform()
		{
			_trans.DOKill ();
		}

		public Image Image
		{
			get{
				return _image;
			}
		}

		public Sprite Sprite
		{
			get {
				return _image.sprite;
			}
			set {
				_image.sprite = value;
			}
		}

		public Vector2 Center
		{
			get {
				return _trans.offsetMin+(Size*0.5f);
			}
		}

		public RectTransform RectTransform
		{
			get{
				return _trans;
			}
		}
	}
}
