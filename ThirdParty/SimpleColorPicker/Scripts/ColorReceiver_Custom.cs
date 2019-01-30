using UnityEngine;

public class ColorReceiver_Custom : MonoBehaviour {

	public LightningArtist latk;
	public float minBrightness = 0.25f;
    Color color;

	void OnColorChange(HSBColor color) {
        this.color = color.ToColor();
		Color c = new Color(color.ToColor().r, color.ToColor().g, color.ToColor().b);
		if (c.r < minBrightness && c.g < minBrightness && c.b < minBrightness) {
			c.r += minBrightness;
			c.g += minBrightness;
			c.b += minBrightness;
		}
		latk.mainColor = new Color(color.ToColor().r, color.ToColor().g, color.ToColor().b);
	}

}
