using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : MonoBehaviour {

	public float maxSize;
	public float minSize;
	public bool goBig;
	public float scale, scalechange;

	// Use this for initialization
	void Start () {
		scale = this.gameObject.transform.lossyScale.x;
		scalechange = 0.04f;

		minSize = 0.25f;
		maxSize = 0.5f;

		this.gameObject.transform.localScale = new Vector2 (maxSize-0.01f, maxSize-0.01f);

		goBig = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (scale < maxSize) {
				if (goBig == false) {
				scale = scale - scalechange;
				this.gameObject.transform.localScale = new Vector2 (scale, scale);
			} else if (goBig == true) {
				scale = scale + scalechange;
				this.gameObject.transform.localScale = new Vector2 (scale, scale);
			}
		}

		if (scale <= minSize) {
			goBig = true;
		} else if (scale > maxSize) {
			scale = maxSize;
			this.gameObject.transform.localScale = new Vector2 (maxSize, maxSize);
		}
	}
}
/*if (scale < SCALEMAX) {
	scale = scale + scaleup;
	scaleup = scaleup/2 + .05f;
	this.gameObject.transform.localScale = new Vector2 (scale, scale);
} else if (scale > SCALEMAX) {
	scale = SCALEMAX;
	this.gameObject.transform.localScale = new Vector2 (SCALEMAX, SCALEMAX);
}*/