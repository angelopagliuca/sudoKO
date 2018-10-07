using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNumberController : MonoBehaviour {

	public static SelectNumberController SN;

	public GameObject RedSolve;
	public GameObject BlueSolve;

	public bool solve = false;
	private bool secondPlayerGo;

	private int selectedNumber = 0;
	private bool placeNum = false;

	public int row1;
	public int col1;
	public int row2;
	public int col2;

	private GameObject player1;
	private GameObject player2;

	public bool noSecondSelection = false;


	// Use this for initialization
	void Start () {
		SN = this;

		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");

		GameManager.GM.player1 = player1;
		GameManager.GM.player2 = player2;
	}
	
	// Update is called once per frame
	void Update () {
		if (solve) {
			if (!secondPlayerGo) {
				NumberSelect ();
				StatePanelController.SP.solvePlayer = GameManager.GM.firstPlayer;
				if (placeNum) {
					FirstPlayerSelect ();
					//CountOwnedTiles ();
					placeNum = false;
					secondPlayerGo = true;
					GameManager.GM.numTimer = 10f;
				}
			} else if (!noSecondSelection) {
				GameManager.GM.isNumTimerOn = true;
				if (GameManager.GM.board [row2, col2].hidden) {
					NumberSelect ();
					if (GameManager.GM.firstPlayer == 1) {
						StatePanelController.SP.solvePlayer = 2;
					} else {
						StatePanelController.SP.solvePlayer = 1;
					}
					if (placeNum) {
						SecondPlayerSelect ();
						//CountOwnedTiles ();
						placeNum = false;
						secondPlayerGo = false;
						solve = false;
					}
				} else {
					NoSelection ();
				}
			} else if (noSecondSelection) {
				NoSelection ();
			}
		}
	}

	public void LostTime () {
		if (!secondPlayerGo) {
			placeNum = false;
			secondPlayerGo = true;
			GameManager.GM.numTimer = 10f;
			GameManager.GM.board [row1, col1].selected = false;
		} else {
			NoSelection ();
			GameManager.GM.board [row2, col2].selected = false;
		}
	}

	private void CreateRedSolve (Vector3 pos) {
		GameObject particles = (GameObject)Instantiate (RedSolve, pos, Quaternion.identity);
		Destroy (particles, 1.0f);
	}

	private void CreateBlueSolve (Vector3 pos) {
		GameObject particles = (GameObject)Instantiate (BlueSolve, pos, Quaternion.identity);
		Destroy (particles, 1.0f);
	}

	public void CountOwnedTiles () {
		int player1Count = 0;
		int player2Count = 0;
		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				if (GameManager.GM.board [i, j].player1Owned == true) {
					player1Count++;
				}
				if (GameManager.GM.board [i, j].player2Owned == true) {
					player2Count++;
				}
			}
		}

		GameManager.GM.player1Counter = player1Count;
		GameManager.GM.player2Counter = player2Count;
	}

	void NoSelection () {
		placeNum = false;
		secondPlayerGo = false;
		solve = false;
		GameManager.GM.board [row2, col2].locked = false;
		noSecondSelection = false;
		player1.GetComponent<PlayerController> ().state = STATE.SELECTION;
		player1.GetComponent<PlayerController> ().canMove = true;
		player2.GetComponent<PlayerController> ().state = STATE.SELECTION;
		player2.GetComponent<PlayerController> ().canMove = true;
		StatePanelController.SP.ChangeStatePanel("selection");
		GameManager.GM.isTimerOn = true;
		GameManager.GM.timer = 30f;
		GameManager.GM.firstPlayer = 0;
	}

	void FirstPlayerSelect() {
		bool success = GameManager.GM.board [row1, col1].ChangeCellNumber (selectedNumber);
		SoundManager.S.PlayPlacingNumberSound ();
		if (success) {
			GameManager.GM.board [row1, col1].hidden = false;
			GameManager.GM.board [row1, col1].selectedBoth = false;
			if (GameManager.GM.firstPlayer == 1) {
				GameManager.GM.board [row1, col1].player1Owned = true;
				GameManager.GM.board [row1, col1].player2Owned = false;
				CreateRedSolve (new Vector3 (col1 * 5, 2.5f, row1 * 5));
				SoundManager.S.PlayCompleteSquare();
			} else {
				GameManager.GM.board [row1, col1].player2Owned = true;
				GameManager.GM.board [row1, col1].player1Owned = false;
				CreateBlueSolve (new Vector3 (col1 * 5, 2.5f, row1 * 5));
				SoundManager.S.PlayCompleteSquare();
			}
			MasterCheck (GameManager.GM.firstPlayer, row1, col1);
			WinController.WC.PlayerWinCheck ();			// CHANGED HERE
		} else {
			//print ("wrong1");
			SoundManager.S.PlayWrongTileSound();
			GameManager.GM.board [row1, col1].num = 0;
		} 
		GameManager.GM.board [row1, col1].selected = false;
	}

	void SecondPlayerSelect() {
		bool success = GameManager.GM.board [row2, col2].ChangeCellNumber (selectedNumber);
		SoundManager.S.PlayPlacingNumberSound ();
		if (success) {
			GameManager.GM.board [row2, col2].hidden = false;
			if (GameManager.GM.firstPlayer == 1) {
				GameManager.GM.board [row2, col2].player2Owned = true;
				GameManager.GM.board [row2, col2].player1Owned = false;
			} else {
				GameManager.GM.board [row2, col2].player1Owned = true;
				GameManager.GM.board [row2, col2].player2Owned = false;
			}
			if (GameManager.GM.firstPlayer == 1) {
				MasterCheck (2, row2, col2);
				CreateRedSolve (new Vector3 (col2 * 5, 2.5f, row2 * 5));
				SoundManager.S.PlayCompleteSquare();
				WinController.WC.PlayerWinCheck ();			// CHANGED HERE
			} else {
				MasterCheck (1, row2, col2);
				WinController.WC.PlayerWinCheck ();			// CHANGED HERE
				CreateBlueSolve (new Vector3 (col2 * 5, 2.5f, row2 * 5));
				SoundManager.S.PlayCompleteSquare();
			}
		} else {
			//print ("wrong2");
			SoundManager.S.PlayWrongTileSound();
			GameManager.GM.board [row2, col2].num = 0;
		} 
		GameManager.GM.board [row2, col2].selected = false;
		GameManager.GM.board [row2, col2].selectedBoth = false;
		player1.GetComponent<PlayerController> ().state = STATE.SELECTION;
		player1.GetComponent<PlayerController> ().canMove = true;
		player2.GetComponent<PlayerController> ().state = STATE.SELECTION;
		player2.GetComponent<PlayerController> ().canMove = true;
		StatePanelController.SP.ChangeStatePanel("selection");
		GameManager.GM.isTimerOn = true;
		GameManager.GM.timer = 30f;
		GameManager.GM.firstPlayer = 0;
	}

	void NumberSelect () {
		if (Input.GetKeyDown("1")) {
			selectedNumber = 1;
			placeNum = true;
		} else if (Input.GetKeyDown("2")) {
			selectedNumber = 2;
			placeNum = true;
		} else if (Input.GetKeyDown("3")) {
			selectedNumber = 3;
			placeNum = true;
		} else if (Input.GetKeyDown("4")) {
			selectedNumber = 4;
			placeNum = true;
		} else if (Input.GetKeyDown("5")) {
			selectedNumber = 5;
			placeNum = true;
		} else if (Input.GetKeyDown("6")) {
			selectedNumber = 6;
			placeNum = true;
		} else if (Input.GetKeyDown("7")) {
			selectedNumber = 7;
			placeNum = true;
		} else if (Input.GetKeyDown("8")) {
			selectedNumber = 8;
			placeNum = true;
		} else if (Input.GetKeyDown("9")) {
			selectedNumber = 9;
			placeNum = true;
		}
	}

	void MasterCheck (int player, int row, int col) {

		bool Block = BlockCheck (row, col);
		bool Row = RowCheck (row, col);
		bool Col = ColumnCheck (row, col);

		if (Block) {
			//TakeBlock (player, row, col);
			TakeCross (player, row, col);
		}

		if (Row) {
			//TakeRow (player, row);
			TakeCross (player, row, col);
		}

		if (Col) {
			//TakeCol (player, col);
			TakeCross (player, row, col);
		}
	}

	void TakeCross (int player, int row, int col) {
		if (player == 1) {
			if (row != 0) {
				if (GameManager.GM.board [row - 1, col].hidden == true) {
					GameManager.GM.board [row - 1, col].num = GameManager.GM.board [row - 1, col].solution;
					GameManager.GM.board [row - 1, col].hidden = false; 
				}
				GameManager.GM.board [row - 1, col].player1Owned = true;
				GameManager.GM.board [row - 1, col].player2Owned = false; 
				CreateRedSolve (new Vector3 (col * 5, 2.5f, (row - 1)* 5));
			}
			if (row != 8) {
				if (GameManager.GM.board [row + 1, col].hidden == true) {
					GameManager.GM.board [row + 1, col].num = GameManager.GM.board [row + 1, col].solution;
					GameManager.GM.board [row + 1, col].hidden = false; 
				}
				GameManager.GM.board [row + 1, col].player1Owned = true; 
				GameManager.GM.board [row + 1, col].player2Owned = false; 
				CreateRedSolve (new Vector3 (col * 5, 2.5f, (row + 1) * 5));
			}
			if (col != 0) {
				if (GameManager.GM.board [row, col - 1].hidden == true) {
					GameManager.GM.board [row, col - 1].num = GameManager.GM.board [row, col - 1].solution;
					GameManager.GM.board [row, col - 1].hidden = false; 
				}
				GameManager.GM.board [row, col - 1].player1Owned = true; 
				GameManager.GM.board [row, col - 1].player2Owned = false; 
				CreateRedSolve (new Vector3 ((col - 1) * 5, 2.5f, row * 5));
			}
			if (col != 8) {
				if (GameManager.GM.board [row, col + 1].hidden == true) {
					GameManager.GM.board [row, col + 1].num = GameManager.GM.board [row, col + 1].solution;
					GameManager.GM.board [row, col + 1].hidden = false; 
				}
				GameManager.GM.board [row, col + 1].player1Owned = true; 
				GameManager.GM.board [row, col + 1].player2Owned = false; 
				CreateRedSolve (new Vector3 ((col + 1) * 5, 2.5f, row * 5));
			}
		} else {
			if (row != 0) {
				if (GameManager.GM.board [row - 1, col].hidden == true) {
					GameManager.GM.board [row - 1, col].num = GameManager.GM.board [row - 1, col].solution;
					GameManager.GM.board [row - 1, col].hidden = false; 
				}
				GameManager.GM.board [row - 1, col].player2Owned = true; 
				GameManager.GM.board [row - 1, col].player1Owned = false; 
				CreateBlueSolve (new Vector3 (col * 5, 2.5f, (row - 1) * 5));
			}
			if (row != 8) {
				if (GameManager.GM.board [row + 1, col].hidden == true) {
					GameManager.GM.board [row + 1, col].num = GameManager.GM.board [row + 1, col].solution;
					GameManager.GM.board [row + 1, col].hidden = false; 
				}
				GameManager.GM.board [row + 1, col].player2Owned = true; 
				GameManager.GM.board [row + 1, col].player1Owned = false; 
				CreateBlueSolve (new Vector3 (col * 5, 2.5f, (row + 1) * 5));
			}
			if (col != 0) {
				if (GameManager.GM.board [row, col - 1].hidden == true) {
					GameManager.GM.board [row, col - 1].num = GameManager.GM.board [row, col - 1].solution;
					GameManager.GM.board [row, col - 1].hidden = false; 
				}
				GameManager.GM.board [row, col - 1].player2Owned = true; 
				GameManager.GM.board [row, col - 1].player1Owned = false;
				CreateBlueSolve (new Vector3 ((col - 1) * 5, 2.5f, row * 5));
			}
			if (col != 8) {
				if (GameManager.GM.board [row, col + 1].hidden == true) {
					GameManager.GM.board [row, col + 1].num = GameManager.GM.board [row, col + 1].solution;
					GameManager.GM.board [row, col + 1].hidden = false; 
				}
				GameManager.GM.board [row, col + 1].player2Owned = true; 
				GameManager.GM.board [row, col + 1].player1Owned = false; 
				CreateBlueSolve (new Vector3 ((col + 1) * 5, 2.5f, row * 5));
			}
		}
	}

	bool BlockCheck (int row, int col) {
		//int block;
		if (row < 3 && col < 3) {
			//block = 0;
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (row < 3 && 3 <= col && col < 6) {
			//block = 1;
			for (int i = 0; i < 3; i++) {
				for (int j = 3; j < 6; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (row < 3 && 6 <= col && col < 9) {
			//block = 2;
			for (int i = 0; i < 3; i++) {
				for (int j = 6; j < 9; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (3 <= row && row < 6 && col < 3) {
			//block = 3;
			for (int i = 3; i < 6; i++) {
				for (int j = 0; j < 3; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (3 <= row && row < 6 && 3 <= col && col < 6) {
			//block = 4;
			for (int i = 3; i < 6; i++) {
				for (int j = 3; j < 6; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (3 <= row && row < 6 && 6 <= col && col < 9) {
			//block = 5;
			for (int i = 3; i < 6; i++) {
				for (int j = 6; j < 9; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (6 <= row && row < 9 && col < 3) {
			//block = 6;
			for (int i = 6; i < 9; i++) {
				for (int j = 0; j < 3; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (6 <= row && row < 9 && 3 <= col && col < 6) {
			//block = 7;
			for (int i = 6; i < 9; i++) {
				for (int j = 3; j < 6; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		} else if (6 <= row && row < 9 && 6 <= col && col < 9) {
			//block = 8;
			for (int i = 6; i < 9; i++) {
				for (int j = 6; j < 9; j++) {
					if (GameManager.GM.board [i, j].hidden == true)
						return false;
				} 
			}
		}
		return true;
	}

	bool RowCheck (int row, int col) {
		for (int i = 0; i < 9; i++) {
			if (GameManager.GM.board [row, i].hidden == true)
				return false;
		}
		return true;
	}

	bool ColumnCheck (int row, int col) {
		for (int i = 0; i < 9; i++) {
			if (GameManager.GM.board [i, col].hidden == true)
				return false;
		} 
		return true;
	}

	void TakeBlock (int player, int row, int col) {
		if (player == 1) {
			if (row < 3 && col < 3) {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (row < 3 && 3 <= col && col < 6) {
				for (int i = 0; i < 3; i++) {
					for (int j = 3; j < 6; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (row < 3 && 6 <= col && col < 9) {
				for (int i = 0; i < 3; i++) {
					for (int j = 6; j < 9; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (3 <= row && row < 6 && col < 3) {
				for (int i = 3; i < 6; i++) {
					for (int j = 0; j < 3; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (3 <= row && row < 6 && 3 <= col && col < 6) {
				for (int i = 3; i < 6; i++) {
					for (int j = 3; j < 6; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (3 <= row && row < 6 && 6 <= col && col < 9) {
				for (int i = 3; i < 6; i++) {
					for (int j = 6; j < 9; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (6 <= row && row < 9 && col < 3) {
				for (int i = 6; i < 9; i++) {
					for (int j = 0; j < 3; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (6 <= row && row < 9 && 3 <= col && col < 6) {
				for (int i = 6; i < 9; i++) {
					for (int j = 3; j < 6; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			} else if (6 <= row && row < 9 && 6 <= col && col < 9) {
				for (int i = 6; i < 9; i++) {
					for (int j = 6; j < 9; j++) {
						GameManager.GM.board [i, j].player1Owned = true;
						GameManager.GM.board [i, j].player2Owned = false;
					} 
				}
			}
		} else {
			if (row < 3 && col < 3) {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (row < 3 && 3 <= col && col < 6) {
				for (int i = 0; i < 3; i++) {
					for (int j = 3; j < 6; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (row < 3 && 6 <= col && col < 9) {
				for (int i = 0; i < 3; i++) {
					for (int j = 6; j < 9; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (3 <= row && row < 6 && col < 3) {
				for (int i = 3; i < 6; i++) {
					for (int j = 0; j < 3; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (3 <= row && row < 6 && 3 <= col && col < 6) {
				for (int i = 3; i < 6; i++) {
					for (int j = 3; j < 6; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (3 <= row && row < 6 && 6 <= col && col < 9) {
				for (int i = 3; i < 6; i++) {
					for (int j = 6; j < 9; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (6 <= row && row < 9 && col < 3) {
				for (int i = 6; i < 9; i++) {
					for (int j = 0; j < 3; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (6 <= row && row < 9 && 3 <= col && col < 6) {
				for (int i = 6; i < 9; i++) {
					for (int j = 3; j < 6; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			} else if (6 <= row && row < 9 && 6 <= col && col < 9) {
				for (int i = 6; i < 9; i++) {
					for (int j = 6; j < 9; j++) {
						GameManager.GM.board [i, j].player2Owned = true;
						GameManager.GM.board [i, j].player1Owned = false;
					} 
				}
			}
		}
	}

	void TakeRow (int player, int row) {
		if (player == 1) {
			for (int i = 0; i < 9; i++) {
				GameManager.GM.board [row, i].player1Owned = true;
				GameManager.GM.board [row, i].player2Owned = false;
			}
		} else {
			for (int i = 0; i < 9; i++) {
				GameManager.GM.board [row, i].player2Owned = true;
				GameManager.GM.board [row, i].player1Owned = false;
			}
		}
	}

	void TakeCol (int player, int col) {
		if (player == 1) {
			for (int i = 0; i < 9; i++) {
				GameManager.GM.board [i, col].player1Owned = true;
				GameManager.GM.board [i, col].player2Owned = false;
			}
		} else {
			for (int i = 0; i < 9; i++) {
				GameManager.GM.board [i, col].player2Owned = true;
				GameManager.GM.board [i, col].player1Owned = false;
			}
		}
	}
}
