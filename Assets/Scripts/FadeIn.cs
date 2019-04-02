	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

	public float transparency;
	public float fade;

	// Use this for initialization
	void Start () {
		transparency = 0.0f;
		fade = 0.0f;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
	}

	// Update is called once per frame
	void Update () {
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
		if (transparency < 1.0f)
			transparency = transparency + fade;
		else if (transparency > 1.0f) {
			transparency = 1.0f;
			fade = fade + 0.0005f;
		}
	}
}
