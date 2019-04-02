using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour {

	public Sprite[] mouth;
	public bool talking;
	public int mouthIndex;
	public int wait, waitbase;
	public bool reverse;

	public AudioSource talkSource;
	public AudioClip[] talkClip;

	// Use this for initialization
	void Start () {
		reverse = false;
		waitbase = 4;
		wait = waitbase;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		talking = false;
		mouthIndex = 0;
		mouth = new Sprite[4];
		for (int i = 0; i <= 3; i++) {
			mouth [i] = Resources.Load<Sprite> ("mouth" + i.ToString ());
		}

		talkClip = new AudioClip[5];
		for (int i = 0; i < 5; i++) {
			talkClip[i] = Resources.Load<AudioClip> ("Sounds/voice"+i.ToString());
		}
		talkSource = this.gameObject.AddComponent<AudioSource> ();
		//talkSource.clip=fugueClip;
		//fugueSource.Play (0);

	}
	
	// Update is called once per frame
	void Update () {
		
		if (wait > 0) wait--;
		if (wait == 0) {
			wait = waitbase;
			//print (mouthIndex);
			if (talking) {
				if (!talkSource.isPlaying) {
					talkSource.clip = talkClip[Random.Range(0, 4)];
					talkSource.Play (0);
				}
				if (reverse) {
					mouthIndex--;
				} else {
					mouthIndex++;
				}
				if (mouthIndex == 4) {
					reverse = true;
					mouthIndex--;
				} else if (mouthIndex <= 0) {
					reverse = false;
					mouthIndex = 0;
				}
				this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = mouth [mouthIndex];
			}
		} 

		if (!talking) {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = mouth [0];
			mouthIndex = 0;
			wait = -1;
			talkSource.Stop ();
		}
	}

	public void setTalking(bool talking) {
		this.talking = talking;
		wait = waitbase;
	}

	public void refresh() {
		reverse = false;
		waitbase = 4;
		wait = waitbase;
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		talking = false;
		mouthIndex = 0;
	}
}
