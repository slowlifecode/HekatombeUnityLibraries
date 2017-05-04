using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

namespace Hekatombe.Base
{
	/***********
	 * Class to make easier to change from RawImage or Image or viceversa
	 * Simply pass the boolean if it has to deal with a RawImage or not
	 */ 
	public class RawImageTransform : ImageTransform {

		private bool _isRaw = true;
		private RawImage _rawImage;
		private bool _isInitRaw = false;

		// Use this for initialization
		public void InitRaw(bool isRaw)
		{
			base.Init();
			if (_isInitRaw) {
				return;
			}
			_isInitRaw = true;
			_isRaw = isRaw;
			_rawImage = GetComponent<RawImage> ();
			if (_isRaw)
			{
				_originalColor = _rawImage.color;
			}
		}
			
		protected override void SetTransparent()
		{
			if (_isRaw)
			{
				_rawImage.color = _originalColor.Transparent ();
			} else {
				base.SetTransparent();
			}
		}

		protected override void SetOriginalColor()
		{
			if (_isRaw)
			{
				_rawImage.color = _originalColor;
			} else {
				base.SetOriginalColor();
			}
		}
			
		protected override void SetImageDOFade(float value, float time)
		{
			if (_isRaw)
			{
				_rawImage.DOFade(value, time).SetEase(Ease.Linear);
			} else {
				base.SetImageDOFade(value, time);
			}
		}

		public override float TextureRectWidth
		{
			get{
				if (_isRaw)
				{
					return _rawImage.texture.width;
				} else {
					return base.TextureRectWidth;
				}
			}
		}

		public override float TextureRectHeight
		{
			get{
				if (_isRaw)
				{
					return _rawImage.texture.height;
				} else {
					return base.TextureRectWidth;
				}
			}
		}

		/*
		public Texture Texture
		{
			get {
				if (_isRaw)
				{
					return _rawImage.texture;
				} else {
					return _image.mainTexture;
				}
			}
			set {
				if (_isRaw)
				{
					_rawImage.texture = value;
				} else {
					//_image.mainTexture = value;
				}
			}
		}
		*/

		public RawImage GetRawImage()
		{
			return _rawImage;
		}

		public override void KillTweenImage()
		{
			base.KillTweenImage();
			if (_isRaw)
			{
				_rawImage.DOKill();
			}
		}
	}
}
