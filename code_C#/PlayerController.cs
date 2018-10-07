using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DIRECTION { UP, DOWN, LEFT, RIGHT }
public enum STATE { SELECTION, SOLVING, FIGHTING }

public class PlayerController : MonoBehaviour {

	public static PlayerController PC;

	public int playerNumber = 0;
	public float speed = 10.0f;

	private int cell_i;
	private int cell_j;

	private float buttonCooldown = 0f;
	private DIRECTION dir = DIRECTION.DOWN;
	private Vector3 pos;

	public bool canMove = true;
	private bool moving = false;

	public STATE state = STATE.SELECTION;

	public GameObject RedSprite;
	public GameObject BlueSprite;
	public Renderer redRend;
	public Renderer blueRend;

	// Use this for initialization
	void Start () {
		PC = this;
		redRend = RedSprite.GetComponent<Renderer> ();
		blueRend = BlueSprite.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		cell_i = (int)Mathf.Floor((transform.position.z + 2.5f) / 5.0f);
		cell_j = (int)Mathf.Floor((transform.position.x + 2.5f) / 5.0f);

		if (GameManager.GM.firstPlayer == 2) {
			if (playerNumber == 2) {
				SelectNumberController.SN.row1 = cell_i;
				SelectNumberController.SN.col1 = cell_j;
			} else if (playerNumber == 1) { 
				SelectNumberController.SN.row2 = cell_i;
				SelectNumberController.SN.col2 = cell_j;
			}
		} else if (GameManager.GM.firstPlayer == 1) {
			if (playerNumber == 2) {
				SelectNumberController.SN.row2 = cell_i;
				SelectNumberController.SN.col2 = cell_j;
			} else if (playerNumber == 1) { 
				SelectNumberController.SN.row1 = cell_i;
				SelectNumberController.SN.col1 = cell_j;
			}
		}

		GameManager.GM.board [cell_i, cell_j].playerOn = true;


		if (state == STATE.SELECTION) {
			buttonCooldown -= Time.deltaTime;
			PlayerMovement ();

		} else if (state == STATE.SOLVING) { 
			SolvingControl ();
		}
	}

	void SolvingControl () {
		SelectNumberController.SN.solve = true;
	}

	void PlayerMovement () {

		Vector3 playerPosition = transform.position;

		if (playerNumber == 2) {

			if (canMove) {
				pos = transform.position;
				MoveP2 ();
			}

			if (moving) {
				if (transform.position == pos) {
					canMove = true;
					moving = false;

					MoveP2 ();
				}
				transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
			}
		} else if (playerNumber == 1) {

			if (canMove) {
				pos = transform.position;
				MoveP1 ();
			}

			if (moving) {
				if (transform.position == pos) {
					canMove = true;
					moving = false;

					MoveP1 ();
				}
				transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
			}
		}

		//SELECTED CELL

		if (playerNumber == 2) {
			GameManager.GM.board [cell_i, cell_j].player1 = true;

			if (Input.GetKeyDown (KeyCode.RightShift) && GameManager.GM.firstPlayer != 2 && !GameManager.GM.isFighting && canMove) {
				SoundManager.S.PlayClick ();
				if (GameManager.GM.board [cell_i, cell_j].hidden) {
					if (GameManager.GM.board [cell_i, cell_j].selected == true) {
						GameManager.GM.board [cell_i, cell_j].selectedBoth = true;
						GameManager.GM.TransitionToFighting();
						SoundManager.S.PlayBoxingBell ();
						SoundManager.S.StopBoardMusic ();
						SoundManager.S.PlayFightMusic ();
						GameManager.GM.selectionTimer = 0f;
						state = STATE.SOLVING;
						SoundManager.S.StopClock ();
					} else {
						GameManager.GM.board [cell_i, cell_j].selected = true;
						StatePanelController.SP.BlueText.text = "Waiting";
						GameManager.GM.ResetTimer ();
					}
					canMove = false;
					//state = STATE.SOLVING;
					if (!GameManager.GM.isSelTimerOn) {
						GameManager.GM.isSelTimerOn = true;
						GameManager.GM.selectionTimer = 10f;
						SoundManager.S.PlayClock ();
						GameManager.GM.firstPlayer = playerNumber;
					} else if (GameManager.GM.fightingDone) {
						GameManager.GM.fightingDone = false;
						GameManager.GM.isSelTimerOn = false;
						SoundManager.S.StopClock ();
						state = STATE.SOLVING;
						StatePanelController.SP.ChangeStatePanel("solve");
						StatePanelController.SP.solvePlayer = 2;
					} else if (!GameManager.GM.isFighting) {
						GameManager.GM.isSelTimerOn = false;
						SoundManager.S.StopClock ();
						state = STATE.SOLVING;
						StatePanelController.SP.ChangeStatePanel("solve");
						StatePanelController.SP.solvePlayer = 2;
					}
				}
			}


		} else if (playerNumber == 1) {
			GameManager.GM.board [cell_i, cell_j].player2 = true;

			if (Input.GetKeyDown (KeyCode.LeftShift) && GameManager.GM.firstPlayer != 1 && !GameManager.GM.isFighting && canMove) {
				SoundManager.S.PlayClick ();
				if (GameManager.GM.board [cell_i, cell_j].hidden) {
					if (GameManager.GM.board [cell_i, cell_j].selected == true) {
						GameManager.GM.board [cell_i, cell_j].selectedBoth = true;
						GameManager.GM.TransitionToFighting();
						state = STATE.SOLVING;
						SoundManager.S.PlayBoxingBell ();
						SoundManager.S.StopBoardMusic ();
						SoundManager.S.PlayFightMusic ();
						SoundManager.S.StopClock ();
						GameManager.GM.selectionTimer = 0f;
					} else {
						GameManager.GM.board [cell_i, cell_j].selected = true;
						StatePanelController.SP.RedText.text = "Waiting";
						GameManager.GM.ResetTimer ();
					}
					//state = STATE.SOLVING;
					canMove = false;
					if (!GameManager.GM.isSelTimerOn) {
						GameManager.GM.isSelTimerOn = true;
						GameManager.GM.selectionTimer = 10f;
						SoundManager.S.PlayClock ();
						GameManager.GM.firstPlayer = playerNumber;
					} else if (GameManager.GM.fightingDone) {
						GameManager.GM.fightingDone = false;
						GameManager.GM.isSelTimerOn = false;
						state = STATE.SOLVING;
						SoundManager.S.StopClock ();
						StatePanelController.SP.ChangeStatePanel("solve");
						StatePanelController.SP.solvePlayer = 1;
					} else if (!GameManager.GM.isFighting) {
						GameManager.GM.isSelTimerOn = false;
						state = STATE.SOLVING;
						SoundManager.S.StopClock ();
						StatePanelController.SP.ChangeStatePanel("solve");
						StatePanelController.SP.solvePlayer = 1;
					}
				}
			}
		}


		if (transform.position.z > 40.1f) {
			transform.position = new Vector3(transform.position.x, transform.position.y, 40f);
			canMove = true;
			moving = false;
		} else if (transform.position.z < -0.1f) {
			transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
			canMove = true;
			moving = false;
		}

		if (transform.position.x > 40.1f) {
			transform.position = new Vector3(40f, transform.position.y, transform.position.z);
			canMove = true;
			moving = false;
		} else if (transform.position.x < -0.1f) {
			transform.position = new Vector3(0f, transform.position.y, transform.position.z);
			canMove = true;
			moving = false;
		}
	}

	private void MoveP1 () {
		if (Input.GetKey ("w")) {
			if (dir != DIRECTION.UP) {
				buttonCooldown = 1f;
				dir = DIRECTION.UP;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.forward * 5f;
			}
		} else if (Input.GetKey ("s")) {
			if (dir != DIRECTION.DOWN) {
				buttonCooldown = 1f;
				dir = DIRECTION.DOWN;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.back * 5f;
			}
		} else if (Input.GetKey ("a")) {
			if (dir != DIRECTION.LEFT) {
				buttonCooldown = 1f;
				dir = DIRECTION.LEFT;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.left * 5f;
			}
		} else if (Input.GetKey ("d")) {
			if (dir != DIRECTION.RIGHT) {
				buttonCooldown = 1f;
				dir = DIRECTION.RIGHT;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.right * 5f;
			}
		}
	}

	private void MoveP2 () {
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (dir != DIRECTION.UP) {
				buttonCooldown = 1f;
				dir = DIRECTION.UP;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.forward * 5f;
			}
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			if (dir != DIRECTION.DOWN) {
				buttonCooldown = 1f;
				dir = DIRECTION.DOWN;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.back * 5f;
			}
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			if (dir != DIRECTION.LEFT) {
				buttonCooldown = 1f;
				dir = DIRECTION.LEFT;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.left * 5f;
			}
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			if (dir != DIRECTION.RIGHT) {
				buttonCooldown = 1f;
				dir = DIRECTION.RIGHT;
			} else {
				canMove = false;
				moving = true;
				pos += Vector3.right * 5f;
			}
		}
	}

	public void TimeOverSelect(int player) {
		transform.position = pos;

		cell_i = (int)Mathf.Floor((transform.position.z + 2.5f) / 5.0f);
		cell_j = (int)Mathf.Floor((transform.position.x + 2.5f) / 5.0f);

		if (player == 2) {
			if (GameManager.GM.board [cell_i, cell_j].hidden) {
				GameManager.GM.board [cell_i, cell_j].selected = true;
			} else {
				SelectNumberController.SN.noSecondSelection = true;
				GameManager.GM.board [cell_i, cell_j].locked = true;
			}
			GameManager.GM.board [cell_i, cell_j].player2 = true;
			StatePanelController.SP.solvePlayer = 1;
		} else {
			if (GameManager.GM.board [cell_i, cell_j].hidden) {
				GameManager.GM.board [cell_i, cell_j].selected = true;
			} else {
				SelectNumberController.SN.noSecondSelection = true;
				GameManager.GM.board [cell_i, cell_j].locked = true;
			}
			GameManager.GM.board [cell_i, cell_j].player1 = true;
			StatePanelController.SP.solvePlayer = 2;
		}
		canMove = false;
		GameManager.GM.isSelTimerOn = false;
		state = STATE.SOLVING;
		StatePanelController.SP.ChangeStatePanel ("solve");
	}
}
