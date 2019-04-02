using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControl : MonoBehaviour {

	public Sprite blinkSprite;
	public Sprite glareSprite;
	public int nextBlink;
	public int holdBlink;
	public bool blinking;
	public int glareFor;

	// Use this for initialization
	void Start () {
		blinking = false;
		blinkSprite = Resources.Load<Sprite> ("blink");
		glareSprite = Resources.Load<Sprite> ("glare");
		nextBlink = Random.Range (80, 120);
		holdBlink = 2;
		glareFor = 0;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (glareFor == 0) {
			if (blinking) {
				if (holdBlink == 0) {
					this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
					nextBlink = Random.Range (3, 30);
					blinking = false;
				}
				holdBlink--;
			} else if (!blinking) {
				if (nextBlink == 0) {
					blink ();
					blinking = true;
					holdBlink = 2;
				}
				nextBlink--;
			}
		} else {
			glareFor--;
		}

	}

	public void blink() {
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = blinkSprite;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	}

	public void glare(int glareFor) {
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = glareSprite;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		this.glareFor = glareFor;
	}

	public void refresh() {
		blinking = false;
		nextBlink = Random.Range (80, 120);
		holdBlink = 2;
		glareFor = 0;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}
}
