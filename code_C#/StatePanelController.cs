using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatePanelController : MonoBehaviour {

	public static StatePanelController SP;

	//public GameObject selectionPanel;
	//public GameObject solvePanel;
	public Text timerTxt;
	//public Text playerTxt;
	public int solvePlayer;

	public GameObject RedInfo;
	public GameObject BlueInfo;
	public Text RedText;
	public Text BlueText;

	public float timer = 0f;

	public string state = "selection";

	// Use this for initialization
	void Start () {
		SP = this;	

		//selectionPanel.SetActive (true);
		//solvePanel.SetActive (false);

		RedText.text = "Select";
		BlueText.text = "Select";

	}
	
	// Update is called once per frame
	void Update () {
		if (state == "selection") {
			if (GameManager.GM.isSelTimerOn) {
				timer = GameManager.GM.selectionTimer;
				timerTxt.text = "" + (Mathf.Ceil (timer));
			} else if (GameManager.GM.isTimerOn) {
				timer = GameManager.GM.timer;
				timerTxt.text = "" + (Mathf.Ceil (timer));
			}
		} else if (state == "solve") {
			timer = GameManager.GM.numTimer;
			timerTxt.text = "" + (Mathf.Ceil (timer));
			if (solvePlayer == 1) {
				RedText.text = "Solve";
				BlueText.text = "Waiting";
			} else {
				BlueText.text = "Solve";
				RedText.text = "Waiting";
			}
		}
	}

	public void ChangeStatePanel(string State) {
		if (State == "selection") {
			state = "selection";
			RedText.text = "Select";
			BlueText.text = "Select";
			GameManager.GM.isTimerOn = true;
			GameManager.GM.timer = 30f;
			GameManager.GM.isNumTimerOn = false;
			GameManager.GM.numTimer = 10f;
			ChangeColorsSelection ();
			SoundManager.S.StopClock ();
		
		} else if (State == "solve") {
			state = "solve";
			GameManager.GM.isNumTimerOn = true;
			GameManager.GM.numTimer = 10f;
			ChangeColorsSolving ();
			SoundManager.S.StopClock ();
		}
	}

	public void ChangeColorsSelection() {
		Color color = PlayerController.PC.redRend.material.color;
		color.a = 1;
		PlayerController.PC.redRend.material.color = color;
		PlayerController.PC.blueRend.material.color = color;
	}	

	public void ChangeColorsSolving() {
		Color color = PlayerController.PC.redRend.material.color;
		color.a = .5f;
		PlayerController.PC.redRend.material.color = color;
		PlayerController.PC.blueRend.material.color = color;
	}
}
