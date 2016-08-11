using UnityEngine;
using UnityEngine.UI;

namespace Hekatombe.Base
{
    public static class UIExtension
	{
		public static void AssignSpriteAndSetSize(this Image image, Sprite spr)
		{
			AssignSpriteAndSetSize (image, spr, 10);
		}

        public static void AssignSpriteAndSetSize(this Image image, Sprite spr, float multiplier)
		{
			image.sprite = spr;
			RectTransform rTrans = image.GetComponent<RectTransform> ().BreakIfNull ();
			rTrans.sizeDelta = new Vector2 (spr.rect.width, spr.rect.height) * multiplier; //Per l'ampliacio de la mida de la UI
        }
    }
}

