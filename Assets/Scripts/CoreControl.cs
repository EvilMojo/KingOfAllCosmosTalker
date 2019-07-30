using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//[RequireComponent(typeof(AudioSource))]
public class CoreControl : MonoBehaviour {

	public GameObject black;

	public GameObject left;
	public GameObject centre;
	public GameObject right;
	public GameObject textRoot;

	public GameObject face;
	public GameObject mouth;
	public GameObject eyes;

	public AudioSource fugueSource;
	public AudioClip fugueClip;

	string[] textInput;

	float transparency = 0.5f;

	public int letterTimer, letterIndex, textSpeed, lineIndex, pause;

	public bool removeText = false;
	public int deleteCount;
	public int talkingDelay;

	public Shader meshShader;

	public bool exit;
	public int exitDelay = 120;

	// Use this for initialization
	void Start () {

		black = GameObject.Find ("black");

		fugueClip = Resources.Load<AudioClip> ("Sounds/fugue");
		fugueSource = this.gameObject.AddComponent<AudioSource> ();
		this.GetComponent<AudioSource> ().clip=fugueClip;
	
		textInput = new string[10];
		textRoot = GameObject.Find ("Text");

		textSpeed = 3;
		lineIndex = 0;
		deleteCount = 0;
		pause = 0;
		talkingDelay = -1;

		left = GameObject.Find ("TextCloudEndLeft");
		centre = GameObject.Find ("TextCloudBody");
		right = GameObject.Find ("TextCloudEndRight");

		face = GameObject.Find ("face");
		mouth = GameObject.Find ("mouth");
		eyes = GameObject.Find ("eyes");

		left.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
		left.SetActive (false);

		right.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
		right.SetActive (false);

		centre.GetComponent<MeshFilter> ().mesh = createMesh (5.0f, 5.0f);
		centre.GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("blue");

		removeBlue ();

		centre.SetActive (false);

		//1,46,131
		letterTimer = letterIndex = -1;

	}

	// Update is called once per frame
	void Update () {
		if (exit == false) {
			if (talkingDelay == 0) {
				left.SetActive (true);
				right.SetActive (true);
				centre.SetActive (true);
				if (removeText && letterTimer == 0) {
					destroyText (deleteCount);
					//destroyText (textInput [lineIndex].Length - 1);
					positionTextCursor (textInput, lineIndex + 1, true);
					lineIndex++;
					deleteCount = 0;
					removeText = false;
					//pause = 50;
				}
				if (letterTimer == 0 && textInput [lineIndex] != null) {
					//print (textRoot);
					//print (letterIndex + " < " + textInput[lineIndex].Length);
					if (letterIndex < textInput [lineIndex].Length) {
						mouth.GetComponent<Talk> ().setTalking (true);
						//print (letterIndex);
						//print (textRoot.transform.position.x + ", " + textRoot.transform.position.y + ", " + textRoot.transform.position.z);
						if (textInput [lineIndex] [letterIndex] == '`') {
							switch (textInput [lineIndex] [letterIndex + 1]) {
							case 'r':
								textRoot.GetComponent<TextMesh> ().color = Color.red;
								break;
							case 'g':
								textRoot.GetComponent<TextMesh> ().color = Color.green;
								break;
							case 'b':
								textRoot.GetComponent<TextMesh> ().color = Color.blue;
								break;
							case 'y':
								textRoot.GetComponent<TextMesh> ().color = Color.yellow;
								break;
							case 'm':
								textRoot.GetComponent<TextMesh> ().color = Color.magenta;
								break;
							case 'c':
								textRoot.GetComponent<TextMesh> ().color = Color.cyan;
								break;
							case 'w':
								textRoot.GetComponent<TextMesh> ().color = Color.white;
								break;
							}
							letterIndex = letterIndex + 2;
						} else if (textInput [lineIndex] [letterIndex] == '-') {
							int numberIndex = 1;
							string glareDuration = "";
							int num = -1;
							bool cont = true;
							int.TryParse ((textInput [lineIndex] [letterIndex + numberIndex]).ToString (), out num);

							while (num <= 9 && num >= 0 && cont) {
								glareDuration = glareDuration + (textInput [lineIndex] [letterIndex + numberIndex]).ToString ();
								numberIndex++;
								num = -1;
								cont = int.TryParse ((textInput [lineIndex] [letterIndex + numberIndex]).ToString (), out num);
							}

							eyes.GetComponent<EyeControl> ().glare (int.Parse (glareDuration) * textSpeed);
							letterIndex = letterIndex + numberIndex;
							
						} else {
							GameObject text = Instantiate (textRoot, textRoot.transform.position, textRoot.transform.rotation);
							text.name = text.name + deleteCount.ToString ();
							text.GetComponent<TextMesh> ().alignment = TextAlignment.Center;
							text.GetComponent<TextMesh> ().anchor = TextAnchor.MiddleCenter;
							text.GetComponent<TextMesh> ().text = textInput [lineIndex] [letterIndex].ToString ();
							//print (textInput [textLine] [letterIndex]);
							letterTimer = textSpeed;
							letterIndex++;
							deleteCount++;

							textRoot.transform.Translate (new Vector3 (.4f, 0, 0));
							text.AddComponent<Appear> ();
						}
					} else {
						//if (lineIndex == textInput.Length) {
						//	letterTimer = -1;
						//} else if (lineIndex <= textInput.Length) {
						lineIndex++;
						letterIndex = 0;

						//End of file?
						if (textInput [lineIndex] == null) {
							mouth.GetComponent<Talk> ().setTalking (false);
							exit = true;
						}
					//Empty line, destroy all text
					else if (textInput [lineIndex].Length == 0) {
							letterTimer = 30;
							removeText = true;
							mouth.GetComponent<Talk> ().setTalking (false);
						} else { //New line in same text gap
							letterTimer = 10;
							//Move letter spawn down and across
							positionTextCursor (textInput, lineIndex, false);
						}
					}


				} else if (letterTimer > 0) {
					letterTimer--;
				}
			} else {
				talkingDelay--;
			}
		} else if (exit == true) {
			if (exitDelay > 0) {
				exitDelay--;
			} else if (exitDelay==0) {
				exit = false;
				exitTalking ();
			}
		}
	}

	public string[] readTextFile(string file) {
		int i = 0;
		string[] txt = new string[100];
		StreamReader input = new StreamReader(file);

		while(!input.EndOfStream) {
			string line = input.ReadLine();
			txt[i] = line;
			i++;
		}

		return txt;
	}

	public void destroyText(int length) {
		GameObject target;
		for (int i = 0; i <= length; i++) {
			target = GameObject.Find ("Text(Clone)" + i.ToString());
			Destroy (target);
		}
	}

	public void positionTextCursor(string[] input, int currIndex, bool line1) {
		float indent = .2f;
		int linelength = 0;

		for (int i = 0; i < input [currIndex].Length; i++) {
			linelength++;
			if (input [currIndex][i] != null) {
				//print (input [currIndex][i]);
				if (input [currIndex][i].Equals('`')) {
					//print ("Removing colour characters");
					linelength = linelength - 2;
				} else if (input [currIndex][i].Equals('-')) {
					int numberIndex = 1;
					int num = -1;
					bool cont = true;
					int.TryParse ((input [currIndex][i + numberIndex]).ToString (), out num);

					while (num <= 9 && num >= 0 && cont) {
						print ("num");
						numberIndex++;
						num = -1;
						cont = int.TryParse ((input [currIndex][i + numberIndex]).ToString (), out num);
						print (num + " " + cont);
					}
					linelength = linelength - numberIndex;
					//print ("Removing " + numberIndex + " number indexes");
				}
			}
		}
		//print ("Line size " + linelength);
		//This is line 1
		if (line1) {
			if (input [currIndex + 1] != null) {
				//indent = input [currIndex].Length * .02f;
				//If next line is null - this is a single line sentence
				//print (0 - (input [currIndex].Length) * textRoot.GetComponent<MeshRenderer>().bounds.size.x);
				if (input [currIndex].Length != 0 && input [currIndex + 1].Length == 0) {

					print ("Moving line to start for line 1 of 1");
					//textRoot.transform.position = new Vector3 (0 - (input [currIndex].Length) * textRoot.GetComponent<MeshRenderer>().bounds.size.x, .0f, -8.0f);
					textRoot.transform.position = new Vector3 (0 - (linelength) * textRoot.GetComponent<TextMesh> ().characterSize / 5 + indent, .0f, -8.0f);
					left.transform.position = new Vector3 (textRoot.transform.position.x - 0.5f, 0, -4.0f);
					right.transform.position = new Vector3 (-left.transform.position.x, 0, -4.0f);
					centre.GetComponent<MeshFilter> ().mesh = createMesh (right.transform.position.x - (left.GetComponent<SpriteRenderer> ().bounds.size.x) / 2, 5.0f);
					centre.transform.position = new Vector3 (centre.transform.position.x, centre.transform.position.y, -4);
					//-3.4 3.175
					//centre.transform.localScale = new Vector3((right.transform.position.x * 2), centre.transform.localScale.y, centre.transform.localScale.z);
					//if next line has content - this is a multi line sentence and is line 1
				} else if (input [currIndex].Length != 0 && input [currIndex + 1].Length != 0) {
					print ("Moving line to start for line 1 of 2");
					textRoot.transform.position = new Vector3 (0 - (linelength) * textRoot.GetComponent<TextMesh> ().characterSize / 5 + indent, .4f, -8.0f);

					if (input [currIndex].Length >= input [currIndex + 1].Length) {
						left.transform.position = new Vector3 (0 - (linelength) * 0.2f - 0.5f, 0, -4.0f);
						right.transform.position = new Vector3 (-left.transform.position.x, 0, -4.0f);
						centre.GetComponent<MeshFilter> ().mesh = createMesh (right.transform.position.x - (left.GetComponent<SpriteRenderer> ().bounds.size.x) / 2, 5.0f);
						centre.transform.position = new Vector3 (centre.transform.position.x, centre.transform.position.y, -4);
					} else {
						left.transform.position = new Vector3 (0 - (input [currIndex + 1].Length) * 0.2f - 0.5f, 0, -4.0f);
						right.transform.position = new Vector3 (-left.transform.position.x, 0, -4.0f);
						centre.GetComponent<MeshFilter> ().mesh = createMesh (right.transform.position.x - (left.GetComponent<SpriteRenderer> ().bounds.size.x) / 2, 5.0f);
						centre.transform.position = new Vector3 (centre.transform.position.x, centre.transform.position.y, -4);
					}
					//This is line 2 for multiline texts
				} else {
					print ("Moving line to start for line 2 of 2");
					//textRoot.transform.position = new Vector3 (0 - (input [currIndex].Length - 1) * 0.2f + indent * (float)(input [currIndex].Length) / (20), -.2f, -8.0f);
					textRoot.transform.position = new Vector3 (0 - (linelength) * textRoot.GetComponent<TextMesh> ().characterSize / 5 + indent, -.2f, -8.0f);
					//Only if this is longer
				}
			} else if (input [currIndex + 1] == null && input [currIndex].Length != 0 && input [currIndex - 1].Length != 0) {
				print ("Null next, line 2 of 2");
				//textRoot.transform.position = new Vector3 (0 - (input [currIndex].Length - 1) * 0.2f + indent * (float)(input [currIndex].Length) / (20), -.2f, -8.0f);
				textRoot.transform.position = new Vector3 (0 - (linelength) * textRoot.GetComponent<TextMesh> ().characterSize / 5 + indent, -.2f, -8.0f);
				//Only if this is longer

			} else if (input [currIndex + 1] == null && input [currIndex].Length != 0 && input [currIndex - 1].Length == 0) {

				print ("Null next, line 1 of 1");
				//textRoot.transform.position = new Vector3 (0 - (input [currIndex].Length) * textRoot.GetComponent<MeshRenderer>().bounds.size.x, .0f, -8.0f);
				textRoot.transform.position = new Vector3 (0 - (linelength) * textRoot.GetComponent<TextMesh> ().characterSize / 5 + indent, .0f, -8.0f);
				left.transform.position = new Vector3 (textRoot.transform.position.x - 0.5f, 0, -4.0f);
				right.transform.position = new Vector3 (-left.transform.position.x, 0, -4.0f);
				centre.GetComponent<MeshFilter> ().mesh = createMesh (right.transform.position.x - (left.GetComponent<SpriteRenderer> ().bounds.size.x) / 2, 5.0f);
				centre.transform.position = new Vector3 (centre.transform.position.x, centre.transform.position.y, -4);

			}

			//textRoot.transform.Translate(new Vector2(, 0));
		} else {
			print ("Line 2 of 2");
			textRoot.transform.position = new Vector3 (0 - (linelength) * textRoot.GetComponent<TextMesh> ().characterSize / 5 + indent, -.2f, -8.0f);
		}


	}

	public Mesh createMesh(float width, float height) {
		Mesh m = new Mesh ();
		m.vertices = new Vector3[] {
			new Vector3 (-width, -height, 0.01f),
			new Vector3 (-width, height, 0.01f),
			new Vector3 (width, height, 0.01f),
			new Vector3 (width, -height, 0.01f)
		};
		m.uv = new Vector2[] {
			new Vector2 (0,0),
			new Vector2 (0,1),
			new Vector2 (1,1),
			new Vector2 (1,0)
		};
		m.triangles = new int[] {0, 1, 2, 0, 2, 3};
		m.RecalculateNormals ();

		return m;
	}

	public void go(int file) {
		if (file == 1) {
			textInput = readTextFile ("Assets/Resources/Text/1.txt");
			letterTimer = textSpeed;
			letterIndex = 0;
			positionTextCursor (textInput, lineIndex, true);
		} else if (file == 2) {
			textInput = readTextFile ("Assets/Resources/Text/2.txt");
			letterTimer = textSpeed;
			letterIndex = 0;
			positionTextCursor (textInput, lineIndex, true);
		} else if (file == 3) {
			textInput = readTextFile ("Assets/Resources/Text/3.txt");
			letterTimer = textSpeed;
			letterIndex = 0;
			positionTextCursor (textInput, lineIndex, true);
		} else if (file == 4) {
			textInput = readTextFile ("Assets/Resources/Text/4.txt");
			letterTimer = textSpeed;
			letterIndex = 0;
			positionTextCursor (textInput, lineIndex, true);
		} else if (file == 5) {
			textInput = readTextFile ("Assets/Resources/Text/5.txt");
			letterTimer = textSpeed;
			letterIndex = 0;
			positionTextCursor (textInput, lineIndex, true);
		}

		face.GetComponent<FaceFade>().activate = true;
		fugueSource.Play (0);
		talkingDelay = 90;
	}

	public void removeBlue() {
		left.transform.position = new Vector3 (-1.68f, -0.025f, 1.3f);
		right.transform.position = new Vector3 (-1.68f, -0.025f, 1.3f);
		centre.transform.position = new Vector3 (0, -0.01f, 1.3f);
	}

	public void exitTalking() {

		black.GetComponent<FadeOut> ().reverse ();
		destroyText (deleteCount);
		removeBlue ();
	}

	public void refresh() {

		textInput = new string[10];
		textRoot = GameObject.Find ("Text");

		textSpeed = 4;
		lineIndex = 0;
		deleteCount = 0;
		pause = 0;
		talkingDelay = -1;

		left.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
		left.SetActive (false);

		right.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, transparency);
		right.SetActive (false);

		centre.GetComponent<MeshFilter> ().mesh = createMesh (5.0f, 5.0f);
		centre.GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("blue");

		removeBlue ();

		centre.SetActive (false);

		//1,46,131
		letterTimer = letterIndex = -1;

		fugueSource.Stop ();
		face.GetComponent<FaceFade> ().refresh ();
		mouth.GetComponent<Talk> ().refresh ();
		eyes.GetComponent<EyeControl> ().refresh ();
	}
}