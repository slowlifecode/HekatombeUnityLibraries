using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

namespace Hekatombe.Base
{
	public class ImageTransform : MonoBehaviour {

		protected bool _isInit = false;
		protected Image _image;
		protected RectTransform _trans;
		protected Color _originalColor;
		protected CanvasGroup _canvasGroup;

		// Use this for initialization
		public void Init()
		{
			if (_isInit) {
				return;
			}
			//No força que trobi la Image perquè pot estar utilitzant RawImageTransform que hereda d'aquesta
			_image = GetComponent<Image> ();
			if (_image != null)
			{
				_originalColor = _image.color;
			}
			_trans = GetComponent<RectTransform> ().BreakIfNull ();
			_isInit = true;
			//Don't BreakIfNull _canvasGroup, because can be unused in many cases
			_canvasGroup = GetComponent<CanvasGroup> ();
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
				SetTransparent();
			} else {
				_canvasGroup.alpha = 0;
			}
		}

		protected virtual void SetTransparent()
		{
			_image.color = _originalColor.Transparent ();
		}

		public void Show()
		{
			KillTweenImage ();
			if (_canvasGroup == null) {
				SetOriginalColor();
			} else {
				_canvasGroup.alpha = 1;
			}
		}

		protected virtual void SetOriginalColor()
		{
			_image.color = _originalColor;
		}

		public void HideSmooth()
		{
			KillTweenImage ();
			if (_canvasGroup == null) {
				SetImageDOFade(0, 0.4f);
			} else {
				_canvasGroup.DOFade (0, 0.4f).SetEase (Ease.Linear);
			}
		}

		protected virtual void SetImageDOFade(float value, float time)
		{
			_image.DOFade (value, time).SetEase(Ease.Linear);
		}

		public void ShowSmooth()
		{
			KillTweenImage ();
			Hide ();
			if (_canvasGroup == null) {
				SetImageDOFade(1, 0.4f);
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

		public virtual void KillTweenImage()
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

		public virtual float TextureRectWidth
		{
			get{
				return Sprite.textureRect.width;
			}
		}

		public virtual float TextureRectHeight
		{
			get{
				return Sprite.textureRect.height;
			}
		}

		public void ResizeProportionalButKeepHeight()
		{
			float perc = TextureRectWidth / TextureRectHeight;
			Size = Size.CopyVectorButModifyX (Size.y * perc);
		}

		public void ResizeProportionalButKeepWidth()
		{
			float perc = TextureRectHeight / TextureRectWidth;
			Size = Size.CopyVectorButModifyY (Size.x * perc);
		}
	}
}
