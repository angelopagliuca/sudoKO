using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinController : MonoBehaviour {

	public static WinController WC;

	public Sprite redWinImage;
	public Sprite blueWinImage;
	public Sprite tieImage;
	public GameObject winPanel;
	public GameObject redPanel;
	public GameObject bluePanel;
	public Text timeText;
	public Text redScoreText;
	public Text blueScoreText;

	void Awake () {
		if (WC == null) {
			WC = this;
		} else if (WC != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		WC = this;
		winPanel.SetActive (false);
	}

	// Update is called once per frame
	/*void Update () {
		if (Input.GetKey (KeyCode.X)) {
			for (int i = 0; i < 9; i++) {
				for (int j = 0; j < 9; j++) {
					GameManager.GM.board[i, j].hidden = false;
				}
			}
			PlayerWinCheck ();
		}
	}*/

	public void PlayerWinCheck () {
		bool solved = true;
		int blueScore = 0;
		int redScore = 0;
		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				if (GameManager.GM.board[i, j].hidden) {
					solved = false;
				} else {
					if (GameManager.GM.board[i, j].player1Owned) {
						redScore++;
					} else if (GameManager.GM.board[i, j].player2Owned) {
						blueScore++;
					}
				}
			}
		}

		redScoreText.text = redScore + "   Red";
		blueScoreText.text = blueScore + "   Blue";

		if (solved) {
			winPanel.SetActive(true);
			if (redScore > blueScore) {
				winPanel.GetComponent<Image>().sprite = redWinImage;
			} else if (redScore == blueScore) {
				winPanel.GetComponent<Image>().sprite = tieImage;
			} else if (redScore < blueScore) {
				winPanel.GetComponent<Image>().sprite = blueWinImage;
			}
			redPanel.SetActive(false);
			bluePanel.SetActive(false);
			timeText.enabled = false;
			redScoreText.text = "";
			blueScoreText.text = "";
			SoundManager.S.PlayBoxingBell();
			Time.timeScale = 0;
		}
	}

	public void exit2MainMenu() {
		Application.Quit ();
	}
}
