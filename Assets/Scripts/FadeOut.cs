using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

	public float transparency;
	public float fade;
	public bool active = false, reverseFade = false;
	public CoreControl core;

	// Use this for initialization
	void Start () {
		core = GameObject.Find ("corecontrol").GetComponent<CoreControl> ();
		transparency = 1.0f;
		fade = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}

		if (!active) {
			if (Input.GetKeyDown ("1")) {
				this.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -4.5f);
				core.go (1);
				active = true;
			} else if (Input.GetKeyDown ("2")) {
				this.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -4.5f);
				core.go (2);
				active = true;
			} else if (Input.GetKeyDown ("3")) {
				this.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -4.5f);
				core.go (3);
				active = true;
			} else if (Input.GetKeyDown ("4")) {
				this.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -4.5f);
				core.go (4);
				active = true;
			} else if (Input.GetKeyDown ("5")) {
				this.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -4.5f);
				core.go (5);
				active = true;
			}
		}

		if (active && !reverseFade && transparency > 0.0f) {
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
			transparency = transparency + fade;
			fade = fade - 0.0005f;
		} else if (active && reverseFade && transparency < 1.0f) {
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
			transparency = transparency + fade;
			fade = fade + 0.0005f;
		}

		if (transparency >= 1.0f && reverseFade) {
			active = false;
			reverseFade = false;
			core.refresh ();
			transparency = 1.0f;
			fade = 0;
//			transparency = 1.0f;
//			fade = 0.0f;
		}
	}

	public void reverse() {
		reverseFade = true;
		fade = 0;
		transparency = 0;
		this.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -7.0f);
	}

}
