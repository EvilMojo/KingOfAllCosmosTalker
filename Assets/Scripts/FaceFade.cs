using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceFade : MonoBehaviour {

	public float transparency;
	public float fade;
	public float scale;
	public float scaleup;
	public const float SCALEMAX = 1.184004f;
	public bool activate;

	// Use this for initialization
	void Start () {
		refresh ();
	}

	// Update is called once per frame
	void Update () {
		if (activate) {
			if (transparency <= 1.0f) {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
				transparency = transparency + fade;
				fade = fade + 0.1f;
			}

			if (scale < SCALEMAX) {
				scale = scale + scaleup;
				scaleup = scaleup / 2 + .05f;
				this.gameObject.transform.localScale = new Vector2 (scale, scale);
			} else if (scale > SCALEMAX) {
				scale = SCALEMAX;
				this.gameObject.transform.localScale = new Vector2 (SCALEMAX, SCALEMAX);
			}
		}
	}

	public void refresh() {
		activate = false;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		transparency = 0.0f;
		fade = 0.0f;
		scale = 0.0f;
		scaleup = 0.0f;
		this.gameObject.transform.localScale = new Vector2 (0, 0);
	}
}
