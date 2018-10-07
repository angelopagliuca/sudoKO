using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlScreenButtons : MonoBehaviour {

	private string currentBG = "directions";

	public Image imageBG;
	public Sprite directionSprite;
	public Sprite sudoSprite;
	public Sprite koSprite;
	public Sprite nextSprite;
	public Sprite nextHighlightSprite;
	public Sprite nextPressedSprite;
	public Sprite menuSprite;
	public Sprite menuHighlightSprite;
	public Sprite menuPressedSprite;

	public GameObject next;

	//public Image nextImage;
	//public Sprite nextSprite;

	public void BackClick() {
		if (currentBG == "directions") {
			SceneManager.LoadScene ("MainMenu");
		} else if (currentBG == "sudo") {
			imageBG.sprite = directionSprite;
			currentBG = "directions";
		} else if (currentBG == "ko") {
			imageBG.sprite = sudoSprite;
			currentBG = "sudo";
			next.GetComponent<Image> ().sprite = nextSprite;
			SpriteState spriteState = next.GetComponent<Button> ().spriteState;
			spriteState.highlightedSprite = nextHighlightSprite;
			spriteState.pressedSprite = nextPressedSprite;
			next.GetComponent<Button> ().spriteState = spriteState;
		}
	}

	public void NextClick() {
		if (currentBG == "directions") {
			imageBG.sprite = sudoSprite;
			currentBG = "sudo";
		} else if (currentBG == "sudo") {
			imageBG.sprite = koSprite;
			currentBG = "ko";
			next.GetComponent<Image> ().sprite = menuSprite;
			SpriteState spriteState = next.GetComponent<Button> ().spriteState;
			spriteState.highlightedSprite = menuHighlightSprite;
			spriteState.pressedSprite = menuPressedSprite;
			next.GetComponent<Button> ().spriteState = spriteState;
		} else if (currentBG == "ko") {
			SceneManager.LoadScene ("MainMenu");
		}
	}

}