using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour {


	public EventSystem E;
	private GameObject selection;

	void Start() {
		selection = E.firstSelectedGameObject;
	}

	void Update() {
		if (E.currentSelectedGameObject != selection) {
			if (E.currentSelectedGameObject == null) {
				E.SetSelectedGameObject(selection);
			} else {
				selection = E.currentSelectedGameObject;
			}
		}
	}


	public void Play() {
		SceneManager.LoadScene("MainBoard");
	}

	public void Controls() {
		SceneManager.LoadScene("ControlsScreen");
	}

	public void QuitGame() {
		Application.Quit();
	}
}
