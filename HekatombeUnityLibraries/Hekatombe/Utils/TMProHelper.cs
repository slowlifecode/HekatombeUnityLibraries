using System.Collections;
using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

namespace Hekatombe.Utils
{
	public static class TMProExtension {

		//Per intentar que posi el text en blanc abans de fer la transició, perquè el molt fill de puta no ho fa
		public static void DOText(TextMeshProUGUI lbl, string text, float timeMultiplier, string id, float delay)
		{
			//tweenToKill ();
			DOTween.Kill(id);
			lbl.text = "";
			lbl.StartCoroutine (DOTextSkip (id, lbl, text, timeMultiplier, delay));
		}

		public static void DOText(TextMeshProUGUI lbl, string text, float timeMultiplier, string id)
		{
			DOText(lbl, text, timeMultiplier, id, 0);
		}

		public static void DOTextEmpty(TextMeshProUGUI lbl, string id)
		{
			DOTween.Kill(id);
			lbl.text = "";
		}

		private static IEnumerator DOTextSkip(string id, TextMeshProUGUI lbl, string text, float timeMultiplier, float delay)
		{
			yield return null;
			DOTween.To (() => lbl.text, x => lbl.text = x, text, text.Length * timeMultiplier).SetDelay(delay).SetEase (Ease.Linear).SetId (id);
		}

		public static string GetSpriteString(string idSprite)
		{
			return string.Format ("<sprite name=\"{0}\">", idSprite);
		}

		public static void SetFont(this TextMeshProUGUI lbl, string font)
		{
			if (font.IsEmpty ()) {
				Debug.LogWarning ("Not found specified");
				return;
			}
			string path = TMP_Settings.defaultFontAssetPath + font;
			TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset> (path);
			if (fontAsset != null) {
				lbl.font = fontAsset;
			} else {
				Debug.LogError ("Font not found in: " + path);
			}
		}
	}
}