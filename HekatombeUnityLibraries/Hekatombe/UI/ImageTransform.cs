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
		private CanvasGroup _canvasGroup;

		// Use this for initialization
		public void Init()
		{
			if (!_isInit) {
				_image = GetComponent<Image> ().BreakIfNull ();
				_trans = GetComponent<RectTransform> ().BreakIfNull ();
				_originalColor = _image.color;
				_isInit = true;
				//Don't BreakIfNull _canvasGroup, because can be unused in many cases
				_canvasGroup = GetComponent<CanvasGroup> ();
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
			if (_canvasGroup == null) {
				_image.color = _originalColor.Transparent ();
			} else {
				_canvasGroup.alpha = 0;
			}
		}

		public void Show()
		{
			KillTweenImage ();
			if (_canvasGroup == null) {
				_image.color = _originalColor;
			} else {
				_canvasGroup.alpha = 1;
			}
		}

		public void HideSmooth()
		{
			KillTweenImage ();
			if (_canvasGroup == null) {
				_image.DOFade (0, 0.4f).SetEase(Ease.Linear);
			} else {
				_canvasGroup.DOFade (0, 0.4f).SetEase (Ease.Linear);
			}
		}

		public void ShowSmooth()
		{
			KillTweenImage ();
			Hide ();
			if (_canvasGroup == null) {
				_image.DOFade (1, 0.4f).SetEase (Ease.Linear);
			} else {
				_canvasGroup.DOFade (1, 0.4f).SetEase (Ease.Linear);
			}
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
			if (_canvasGroup != null) {
				_canvasGroup.DOKill ();
			}
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

		public CanvasGroup CanvasGroup
		{
			get{
				return _canvasGroup;
			}
		}

		public void ResizeProportionalButKeepHeight()
		{
			float perc = Sprite.textureRect.width / Sprite.textureRect.height;
			Size = Size.CopyVectorButModifyX (Size.y * perc);
		}

		public void ResizeProportionalButKeepWidth()
		{
			float perc = Sprite.textureRect.height / Sprite.textureRect.width;
			Size = Size.CopyVectorButModifyY (Size.x * perc);
		}
	}
}
