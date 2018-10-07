using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject cell;
	public GridCellController[,] board;

	public static GameManager GM = null;

	public bool musicTurnOn = true;

	public int maxLevel;

	public bool isSelTimerOn;
	public float selectionTimer = 10f;

	public int firstPlayer;

	public bool isTimerOn;
	public float timer = 30f;

	public bool isNumTimerOn;
	public float numTimer = 10f;

	public bool fightingDone = false;
	public bool isFighting = false;

	public GameObject player1;
	public GameObject player2;

	public GameObject RedSprite;
	public GameObject BlueSprite;
	public GameObject BoxBt;
	public GameObject Grid;
	public GameObject BoardCanvas;
	public GameObject Fighter1;
	public GameObject Fighter2;
	public GameObject Background;
	public GameObject RedHealth;
	public GameObject BlueHealth;
	public GameObject HealthBar;
	public GameObject Diamond;
	public GameObject BlueBolt;
	public GameObject RedBolt;
	public GameObject BackgroundBoard;
	public GameObject RedControl;
	public GameObject BlueControl;
	public GameObject NumberControl;

	public int player1Counter = 0;
	public int player2Counter = 0;

	public struct Puzzle {
		public int[,] solved;
		public int[,] unsolved;

		public Puzzle(int[,] s, int[,] u)
		{
			solved = s;
			unsolved = u;
		}
	}

	void Awake() {
		if (GM == null) {
			GM = this;
			GM.maxLevel = 1;
		} else if (GM != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
		board = new GridCellController[9, 9];
	}

	// Use this for initialization
	void Start () {
		SoundManager.S.StopFightMusic ();
		SoundManager.S.StopBoardMusic ();
	}
	
	// Update is called once per frame
	void Update () {

		if (isNumTimerOn && !isFighting) {
			numTimer -= Time.deltaTime;
			//print ("Timer = " + (int) numTimer);
			if (Mathf.Floor(numTimer) == 4) {
				SoundManager.S.PlayClock ();
			}

			if (numTimer <= 0) {
				SelectNumberController.SN.LostTime ();
				SoundManager.S.StopClock ();
			}
		}

		if (isTimerOn) {
			timer -= Time.deltaTime;
			//print ("Timer = " + (int) timer);
			if (timer <= 0) {
				if (firstPlayer == 0) {
					EndTimer (3);
				} else if (firstPlayer == 1) {
					EndTimer (2);
				} else if (firstPlayer == 2) {
					EndTimer (1);
				}
			}
		}

		if (isSelTimerOn) {
			selectionTimer -= Time.deltaTime;

			if (selectionTimer <= 0) {
				SoundManager.S.StopClock ();
				isSelTimerOn = false;
				if (firstPlayer == 1) {
					player2.GetComponent<PlayerController> ().TimeOverSelect(1);
				} else {
					player1.GetComponent<PlayerController> ().TimeOverSelect(2);
				}
			}
		}

		if (musicTurnOn) {
			string currentScene = SceneManager.GetActiveScene ().name;
			if (currentScene == "MainMenu") {
				//SoundManager.S.PlayMainMusic ();
			} else if (currentScene == "Level1" || currentScene == "Level2" || currentScene == "Level3" || currentScene == "Level4") {
				//SoundManager.S.PlayBackgroundMusic ();
			}
			musicTurnOn = false;
		}


	}

	public void ResetTimer () {
		isTimerOn = false;
		timer = 30f;
	}

	private void EndTimer (int player) {
		isTimerOn = false;

		float count1 = 0f;
		float count2 = 0f;
		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				if (GameManager.GM.board [i, j].player1Owned) {
					count1++;
				}
				if (GameManager.GM.board [i, j].player2Owned) {
					count2++;
				}
			}
		}

		if (player == 1 || player == 3) {
			if (count1 != 0) {
				int rando = (int)Mathf.Floor (Random.Range (0f, count1 - .01f));
				for (int i = 0; i < 9; i++) {
					for (int j = 0; j < 9; j++) {
						if (GameManager.GM.board [i, j].player1Owned) {
							if (rando == 0) {
								GameManager.GM.board [i, j].player1Owned = false;
							}
							rando--;
						}
					}
				}
			}
		}

		if (player == 2 || player == 3) {
			if (count2 != 0) {
				int rando = (int)Mathf.Floor (Random.Range (0f, count2 - .01f));
				for (int i = 0; i < 9; i++) {
					for (int j = 0; j < 9; j++) {
						if (GameManager.GM.board [i, j].player2Owned) {
							if (rando == 0) {
								GameManager.GM.board [i, j].player2Owned = false;
							}
							rando--;
						}
					}
				}
			}
		}

		SelectNumberController.SN.CountOwnedTiles ();

		if (player == 3) {
			isTimerOn = true;
			timer = 30f;
		}
	}

	public void TransitionToFighting() {
		//shift camera
		Camera mc = Camera.main;
		mc.transform.position = new Vector3(0, 1, -10);
		mc.transform.rotation = new Quaternion(0, 0, 0, 1);
		mc.orthographic = true;
		mc.orthographicSize = 15;

		RedSprite.SetActive(false);
		BlueSprite.SetActive(false);
		BoxBt.SetActive(false);
		Grid.SetActive(false);
		BoardCanvas.SetActive(false);
		BackgroundBoard.SetActive (false);
		RedControl.SetActive (false);
		BlueControl.SetActive (false);
		NumberControl.SetActive (false);
		isNumTimerOn = false;
		numTimer = 10f;


		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				GameManager.GM.board[i, j].gameObject.SetActive(false);
			}
		}
			
		Fighter1.SetActive(true);
		Fighter2.SetActive(true);
		Fighter1.GetComponent<FighterController>().Reset();
		Fighter2.GetComponent<FighterController>().Reset();

		Background.SetActive(true);
		RedHealth.SetActive(true);
		BlueHealth.SetActive(true);
		HealthBar.SetActive(true);
		Diamond.SetActive(true);
		BlueBolt.SetActive(true);
		RedBolt.SetActive(true);

		fightingDone = false;
		isFighting = true;
	}

	public void TransitionToSolving(int player) {

		SoundManager.S.StopFightMusic ();
		SoundManager.S.PlayBoardMusic ();

		Camera mc = Camera.main;
		mc.transform.position = new Vector3(20, 50, 0);
		Quaternion rotate = Quaternion.identity;
		rotate.eulerAngles = new Vector3(68, 0, 0);
		mc.transform.rotation = rotate;
		mc.orthographic = false;
		mc.fieldOfView = 60;

		RedSprite.SetActive(true);
		BlueSprite.SetActive(true);
		BoxBt.SetActive(true);
		Grid.SetActive(true);
		BoardCanvas.SetActive(true);
		BackgroundBoard.SetActive (true);
		RedControl.SetActive (true);
		BlueControl.SetActive (true);
		NumberControl.SetActive (true);

		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				GameManager.GM.board[i, j].gameObject.SetActive(true);
			}
		}
			
		Fighter1.SetActive(false);
		Fighter2.SetActive(false);
		Background.SetActive(false);
		RedHealth.SetActive(false);
		BlueHealth.SetActive(false);
		HealthBar.SetActive(false);
		Diamond.SetActive(false);
		BlueBolt.SetActive(false);
		RedBolt.SetActive(false);

		firstPlayer = player;
		isFighting = false;
		fightingDone = true;

		PlayerController.PC.state = STATE.SOLVING;
		GameManager.GM.isNumTimerOn = true;
		GameManager.GM.numTimer = 10f;
		Color color = PlayerController.PC.redRend.material.color;
		color.a = .5f;
		PlayerController.PC.redRend.material.color = color;
		PlayerController.PC.blueRend.material.color = color;
	}

	public void CreateBoard() {

		SoundManager.S.PlayBoardMusic ();
		SoundManager.S.StopMainMusic ();

		Puzzle puzzle = PuzzleReader.GeneratePuzzle ();
		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				Vector3 pos = new Vector3 (j * 5, 2.5f, i * 5);
				GameObject gridCell = Instantiate (cell, pos, Quaternion.identity);
				gridCell.GetComponent<GridCellController> ().row = i;
				gridCell.GetComponent<GridCellController> ().col = j;
				gridCell.GetComponent<GridCellController> ().num = puzzle.unsolved[i, j];
				gridCell.GetComponent<GridCellController> ().solution = puzzle.solved[i, j];
				if (puzzle.unsolved [i, j] == 0) {
					gridCell.GetComponent<GridCellController> ().hidden = true;
				}
				else gridCell.GetComponent<GridCellController> ().hidden = false;
				board[i, j] = (GridCellController)gridCell.GetComponent<GridCellController>();
			}
		}
	}


}
