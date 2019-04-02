using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDressFade : MonoBehaviour {


	public float transparency;
	public float fade;
	public int updown, wait;

	// Use this for initialization
	void Start () {
		transparency = 1.0f;
		fade = 0.0f;
		wait = 0;
	}

	// Update is called once per frame
	void Update () {

		if (wait == 0) {
			if (this.gameObject.name == "Headdress1") {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
				//print ("HD1: " + transparency);
			} else {
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.00f - transparency);
				//print ("HD2: " + (1.00f - transparency));
			}

			transparency = transparency - fade;
			fade = 0.1f * updown;

			if (transparency <= 0.0f) {
				updown = -1;
				wait = 2;

			} else if (transparency >= 1.0f) {
				updown = 1;
				wait = 2;
			}
		} else
			wait--;
	}
}
