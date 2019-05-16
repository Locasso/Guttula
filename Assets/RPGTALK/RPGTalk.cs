using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;



[AddComponentMenu("Seize Studios/RPG Talk")]
public class RPGTalk : MonoBehaviour {

	[Tooltip("Should the talk be initiated whe the script awakes?")]
	public bool startOnAwake = true;

	[Tooltip("Any object that should be shown or hidden with the text")]
	public GameObject[] showWithDialog;
	
	[Tooltip("UI with the text")]
	public Text textUI;

	[Tooltip("Sould have the name of the dialoger?")]
	public bool dialoger;

	[Tooltip("UI with the talker text")]
	public Text dialogerUI;

	[Tooltip("Should the element follow someone?")]
	public bool shouldFollow;

	[Tooltip("Who to follow?")]
	public GameObject follow;

	[Tooltip("If he is following someone, should be an offset?")]
	public Vector3 followOffset;

	[Tooltip("The show with objects should be Billboard?")]
	public bool billboard = true;

	[Tooltip("Billboard based on main camera?")]
	public bool mainCamera = true;

	[Tooltip("Billboard based on other camera")]
	public Camera otherCamera;

	[Tooltip("Text file to be the talk")]
	public TextAsset txtToParse;
	

	[Tooltip("If the player click on it, should it be skipped?")]
	public bool enableQuickSkip = true;

	[Tooltip("Some script to look for a feedback when the talk is finished. Leave blank if no feedback is needed")]
	public MonoBehaviour callbackScript;

	[Tooltip("Function to be called when the talk finish")]
	public string callbackFunction;

	[Tooltip("An animator boolean can be set when the character is talking")]
	public Animator animatorWhenTalking;

	[Tooltip("Name of the boolean property in animator")]
	public string animatorBooleanName;

	[Tooltip("Name of the int property in animator meaning the talker")]
	public string animatorIntName;

	/*[Tooltip("Name fo the trigger to be set in line. Leave null if none should be used")]
	public string triggerInLineName;

	[Tooltip("What line should the previous trigger be playerd?")]
	public int triggerInLine;*/


	
	// Wich position of the talk are we?
	private int cutscenePosition = 0;
	
	[Tooltip("Speed of the text, in characters per second")]
	public float textSpeed = 50.0f;
	//wich character are we?
	private float currentChar = 0.0f;

	//a list with every element of the cutscene
	private List<RpgtalkElement> rpgtalkElements;

	[Tooltip("A GameObject to blink when the text is finished")]
	public GameObject blinkWhenReady;

	[Tooltip("Is there any variable in the text?")]
	public RPGTalkVariable[] variables;

	[Tooltip("Should there be photos?")]
	public bool shouldUsePhotos;

	[Tooltip("The photo of the person who is talking")]
	public RPGTalkPhoto[] photos;

	[Tooltip("The UI that the photo should be applied to")]
	public Image UIPhoto;

	[Tooltip("If the dialog should stay on screen even if the text has ended")]
	public bool shouldStayOnScreen;

	bool lookForClick = true;

	[Tooltip("Audio to be played while the character is talking")]
	public AudioClip textAudio;
	[Tooltip("Audio to be played when player passes the Talk")]
	public AudioClip passAudio;
	AudioSource rpgAudioSorce;


	[Tooltip("Pass with mouse Click")]
	public bool passWithMouse = true;

	[Tooltip("Pass with some button set on Project Settings > Input")]
	public string passWithInputButton;

	[Tooltip("Line to Start reading the text")]
	public int lineToStart = 1;
	[Tooltip("Line to Stop reading the text (Leave -1 if should read until the end)")]
	public int lineToBreak = -1;

	private int actualLineToStart;
	private int actualLineToBreak;

	[Tooltip("Tries to break long lines into several talks")]
	public bool wordWrap = true;
	public int maxCharInWidth = 50;
	public int maxCharInHeight = 4;


	void Awake(){
		if (startOnAwake) {
			NewTalk ();
		}
	}

	/// <summary>
	/// Starts a new Talk.
	/// </summary>
	public void NewTalk(){

		//reduce one for the line to Start and break
		if(lineToBreak == -1){
			actualLineToBreak = lineToBreak;
		}else{
			actualLineToBreak = lineToBreak-1;
		}
		actualLineToStart = lineToStart-1;

		if (textAudio != null) {
			if (rpgAudioSorce == null) {
				rpgAudioSorce = gameObject.AddComponent<AudioSource> ();
			}
		}

		lookForClick = true;

		//Stop any blinking arrows that shouldn't appear
		CancelInvoke ("blink");
		if (blinkWhenReady) {
			blinkWhenReady.SetActive (false);
		}


		//reset positions
		cutscenePosition = 1;
		currentChar = 0;


		//create a new CutsCeneElement
		rpgtalkElements = new List<RpgtalkElement>();
		
		
		if(txtToParse != null) {
			// read the TXT file into the elements list
			StringReader reader = new StringReader (txtToParse.text);
			
			string line = reader.ReadLine(); 
			int currentLine = 0;

			while (line != null) {
				
				if (currentLine >= actualLineToStart) {
					if (actualLineToBreak == -1 || currentLine <= actualLineToBreak) {

						if (wordWrap) {
							CheckIfTheTextFits (line);
						} else {
							rpgtalkElements.Add (readSceneElement (line));
						}
					}
				}
				
				line = reader.ReadLine();
				currentLine++;
			}


			if(rpgtalkElements.Count == 0){
				Debug.LogError ("The Line To Start and the Line To Break are not fit for the given TXT");
				return;
			}
		}





		//Set the speaker name and photo
		if (dialoger) {
			dialogerUI.text = rpgtalkElements [0].speakerName;
			if (shouldUsePhotos) {
				for (int i = 0; i < photos.Length; i++) {
					if (photos [i].name == rpgtalkElements [0].originalSpeakerName) {
						UIPhoto.sprite = photos [i].photo;
						if(animatorWhenTalking && animatorIntName != ""){
							animatorWhenTalking.SetInteger (animatorIntName, i);
						}
					}
				}
			}
		}

		//show what need to be shown
		textUI.enabled = true;
		if (dialoger) {
			dialogerUI.enabled = true;
		}
		for (int i = 0; i < showWithDialog.Length; i++) {
			showWithDialog[i].SetActive(true);
		}


		//if we have an animator.. play it
		if (animatorWhenTalking != null) {
			animatorWhenTalking.SetBool (animatorBooleanName, true);
		}
	}

	private RpgtalkElement readSceneElement(string line) {
		
		RpgtalkElement newElement = new RpgtalkElement();

		newElement.originalSpeakerName = line;

		//replace any variable that may exist on the text
		for (int i = 0; i < variables.Length; i++) {
			if (line.Contains (variables[i].variableName)) {
				line = line.Replace (variables[i].variableName, variables[i].variableValue);
			}
		}

		//If we want to show the dialoger's name, slipt the line at the ':'
		if (dialoger) {

			if (line.IndexOf (':') != -1) {

				string[] splitLine = line.Split (new char[] { ':' }, 2);

				newElement.speakerName = splitLine [0].Trim ();

				newElement.dialogText = splitLine [1].Trim ();

				string[] originalSplitLine = newElement.originalSpeakerName.Split (new char[] { ':' },2);

				newElement.originalSpeakerName = originalSplitLine [0].Trim ();

			} else {
				newElement.dialogText = line;
			}
		} else {
			newElement.dialogText = line;
		}

		newElement.hasDialog = true;


		
		return newElement;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!textUI.gameObject.activeInHierarchy) {
			return;
		}


		if(follow != null){
			Vector3 newPos = follow.transform.position - followOffset;
			Quaternion newRotation = follow.transform.rotation;
			if (billboard) {
				if (mainCamera) {
					newRotation = Camera.main.transform.rotation;
				} else {
					newRotation = otherCamera.transform.rotation;
				}
			}

			for (int i = 0; i < showWithDialog.Length; i++) {
				showWithDialog[i].transform.position = newPos;
				showWithDialog [i].transform.rotation = newRotation;
			}




		}


		if (textUI.enabled &&
			currentChar >= rpgtalkElements [cutscenePosition - 1].dialogText.Length) {

			//if we hit the end of the talk, but we should stay on screen, return.
			//but if we have a callback, he can click on it once more.
			if (cutscenePosition >= rpgtalkElements.Count && shouldStayOnScreen) {
				if(lookForClick && (
					(passWithMouse && Input.GetMouseButtonDown (0)) ||
					(passWithInputButton != "" && Input.GetButtonDown(passWithInputButton))
				)){
					//if have an audio... playit
					if (passAudio != null && !rpgAudioSorce.isPlaying) {
						rpgAudioSorce.clip = passAudio;
						rpgAudioSorce.Play ();
					}
					if(callbackScript != null){
						callbackScript.Invoke(callbackFunction,0f);
						//Stop any blinking arrows that shouldn't appear
						CancelInvoke ("blink");
						if (blinkWhenReady) {
							blinkWhenReady.SetActive (false);
						}
					}
					lookForClick = false;
				}

				CancelInvoke ("blink");
				if (blinkWhenReady) {
					blinkWhenReady.SetActive (false);
				}
				return;
			}

			//if we reached the end of the line and click on the screen...
			if (
				(passWithMouse && Input.GetMouseButtonDown (0)) ||
				(passWithInputButton != "" && Input.GetButtonDown(passWithInputButton))
			) {//if have an audio... playit
				if (passAudio != null) {
					rpgAudioSorce.clip = passAudio;
					rpgAudioSorce.Play ();
				}
				textUI.enabled = false;
				PlayNext ();

			}
			return;
		}




		//if we're currently showing dialog, then start scrolling it
		if(textUI.enabled) {
			// if there's still text left to show
			if(currentChar < rpgtalkElements[cutscenePosition - 1].dialogText.Length) {
				
				//ensure that we don't accidentally blow past the end of the string
				currentChar = Mathf.Min(
					currentChar + textSpeed * Time.deltaTime,
					rpgtalkElements[cutscenePosition - 1].dialogText.Length);
				
				textUI.text =
					rpgtalkElements[cutscenePosition - 1].dialogText.Substring(0, (int)currentChar)
					;


				//if have an audio... playit
				if (textAudio != null && !rpgAudioSorce.isPlaying) {
					rpgAudioSorce.clip = textAudio;
					rpgAudioSorce.Play ();
				}

			} 
			
			if(enableQuickSkip == true &&
				(
					(passWithMouse && Input.GetMouseButtonDown (0)) ||
					(passWithInputButton != "" && Input.GetButtonDown(passWithInputButton))
				)
				&& currentChar > 3) {
				textUI.text = rpgtalkElements[cutscenePosition - 1].dialogText;
				currentChar = rpgtalkElements[cutscenePosition - 1].dialogText.Length;
			}

			if(currentChar >= rpgtalkElements[cutscenePosition - 1].dialogText.Length){
				blink();

				//if we have an animator.. stop it
				if (animatorWhenTalking != null) {
					animatorWhenTalking.SetBool (animatorBooleanName, false);
				}

				if(cutscenePosition >= rpgtalkElements.Count && callbackScript == null){
					//Stop any blinking arrows that shouldn't appear
					/*CancelInvoke ("blink");
					if (blinkWhenReady) {
						blinkWhenReady.SetActive (false);
					}*/
				}
			}




			
		}


	}

	void blink(){
		if (blinkWhenReady) {
			blinkWhenReady.SetActive (!blinkWhenReady.activeInHierarchy);
			Invoke ("blink", .5f);
		}
	}

	void CheckIfTheTextFits(string line){

		//make base calculations for the size of the font and the textUI.
		/*int widthBase = Mathf.FloorToInt(4f * textUI.fontSize);
		int heightBase = Mathf.FloorToInt(textUI.fontSize/3);
		int maxCharInWidth = Mathf.FloorToInt ((widthBase * textUI.rectTransform.rect.width) / 438);
		int maxCharInHeight = Mathf.FloorToInt ((heightBase * textUI.rectTransform.rect.height) / 71);*/


		int maxCharsOnUI = maxCharInWidth * maxCharInHeight;
		if (line.Length > maxCharsOnUI) {

			//how many talks would be necessary to fit this text?
			int howMuchMore = Mathf.CeilToInt((float)line.Length / (float)maxCharsOnUI);
			string newLine = "";
			int cuttedInSpace = -1;

			for (int i = 0; i < howMuchMore; i++) {
				//get the characeter that we should start saying
				int startChar = i * maxCharsOnUI;
				if(cuttedInSpace != -1){
					startChar = cuttedInSpace;
					cuttedInSpace = -1;
				}


				//if the new line fits the talk...
				if (line.Substring (startChar, 
					line.Length - (startChar)).Length < maxCharsOnUI) {
					newLine = line.Substring (startChar, 
						line.Length - (startChar));
				} else {
					//if it not, search for spaces near to the last word and cut it
					cuttedInSpace = line.IndexOf (" ", startChar+ (maxCharsOnUI - 20));
					if(cuttedInSpace != -1){
						newLine = line.Substring (startChar, cuttedInSpace-startChar);
					}else{
						newLine = line.Substring (startChar, maxCharsOnUI);
					}
				}

				rpgtalkElements.Add (readSceneElement (newLine));
			}
		} else {

			rpgtalkElements.Add (readSceneElement (line));
		}
	}


	/// <summary>
	/// Finish the talk, skipping every dialog. The callback function still going to be called
	/// </summary>
	public void EndTalk() {
		if (textUI.gameObject.activeInHierarchy) {
			if (shouldStayOnScreen) {
				cutscenePosition = rpgtalkElements.Count-1;
			} else {
				cutscenePosition = rpgtalkElements.Count;
			}
			PlayNext ();
		}
	}



	/// <summary>
	/// Plays the next dialog in the current Talk.
	/// </summary>
	public void PlayNext() {
		// increment the cutscene counter
		cutscenePosition++;
		currentChar = 0;


		/*if (triggerInLineName != "" && cutscenePosition == triggerInLine) {
			animatorWhenTalking.SetTrigger (triggerInLineName);
		}*/

		CancelInvoke ("blink");
		if (blinkWhenReady) {
			blinkWhenReady.SetActive (false);
		}
		
		if(cutscenePosition <= rpgtalkElements.Count) {

			textUI.enabled = true;
			
			RpgtalkElement currentRpgtalkElement = rpgtalkElements[cutscenePosition - 1];

			if (dialoger) {
				dialogerUI.enabled = true;

				dialogerUI.text = currentRpgtalkElement.speakerName;
				if (shouldUsePhotos) {
					for (int i = 0; i < photos.Length; i++) {
						if (photos [i].name == currentRpgtalkElement.originalSpeakerName) {
							UIPhoto.sprite = photos [i].photo;
							if(animatorWhenTalking && animatorIntName != ""){
								animatorWhenTalking.SetInteger (animatorIntName, i);

							}
						}
					}
				}
			}



			//if we have an animator.. play it
			if (animatorWhenTalking != null) {
				animatorWhenTalking.SetBool (animatorBooleanName, true);
			}




		} else {
			if (!shouldStayOnScreen) {
				textUI.enabled = false;
				if (dialoger) {
					dialogerUI.enabled = false;
				}
				for (int i = 0; i < showWithDialog.Length; i++) {
					showWithDialog [i].SetActive (false);
				}
			}

			if(callbackScript != null){
				callbackScript.Invoke(callbackFunction,0f);
			}
		}

		
	}
		


	private class RpgtalkElement {
		public bool hasDialog = false;
		public bool allowPlayerAdvance = true;
		public string speakerName;
		public string originalSpeakerName;
		public string dialogText;
		
		public override string ToString () {
			return  "(" + this.hasDialog + ")" + this.speakerName + "::" + this.dialogText + "\n";
		}
	}

}

//A class to be the variables a text could have
[System.Serializable]
public class RPGTalkVariable{
	public string variableName;
	public string variableValue;
}

//A class to be the photo of the person who is talking
[System.Serializable]
public class RPGTalkPhoto{
	public string name;
	public Sprite photo;
}
