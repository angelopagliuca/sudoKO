using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterController : MonoBehaviour {

	public Material Sprite00;
	public Material Sprite09;
	public Material Sprite10;
	public Material Sprite15;
	public Material Sprite20;
	public Material Sprite25;
	public Material Sprite30;
	public Material Sprite35;
	public GameObject opponent;
	public GameObject puff;
	public Image background;
	public Sprite standardBackground;
	public Sprite fightBackground;
	public Sprite koBackground;

	public int player = 0;
	public float health = 100.0f;
	public float crouch_damage = 4.0f;
	public float stand_damage = 10.0f;
	public float jump_damage = 20.0f;

	private float max_ver_speed = 75.0f;
	private float max_hor_speed = 15.0f;
	private float max_flinch_speed = 7.0f;
	private float max_attack_time = 0.50f;
	private float max_flinch_time = 0.40f;
	private float grav = -200.0f;

	private float curr_ver_speed = 0.0f;
	private float curr_hor_speed = 0.0f;
	private float curr_attack_time = 0.0f;
	private float curr_flinch_time = 0.0f;

	private float x_size;
	private float y_size;
	private int game_mode = 0;
	private float mode_timer = 0.0f;

	private KeyCode key_jump;
	private KeyCode key_crouch;
	private KeyCode key_right;
	private KeyCode key_left;
	private KeyCode key_attack;

	// 00 - DEAD
	// 09 - Flinch
	// 10 - Crouching
	// 15 - Crouching Attacking
	// 20 - Standing
	// 25 - Standing Attacking
	// 30 - Jumping
	// 35 - Jumping Attacking 
	public int state = 20;

	// Use this for initialization
	void Start () {
		state = 20;
		gameObject.GetComponent<Renderer>().material = getSprite();

		x_size = Mathf.Abs(transform.localScale.x);
		y_size = transform.localScale.y;

		if (player == 1) {
			key_jump = KeyCode.W;
			key_crouch = KeyCode.S;
			key_right = KeyCode.D;
			key_left = KeyCode.A;
			key_attack = KeyCode.LeftShift;
		} else if (player == 2) {
			key_jump = KeyCode.UpArrow;
			key_crouch = KeyCode.DownArrow;
			key_right = KeyCode.RightArrow;
			key_left = KeyCode.LeftArrow;
			key_attack = KeyCode.RightShift;
		}
	}

	public void Reset() {
		if (player == 1) {
			health = 100;
			state = 20;
			gameObject.GetComponent<Renderer>().material = getSprite();
			curr_flinch_time = 0.0f;
			curr_hor_speed = 0.0f;
			curr_ver_speed = 0.0f;
			curr_attack_time = 0.0f;
			transform.position = new Vector3(-12, -6, 0);
			game_mode = 1;
			mode_timer = 2.0f;
			background.sprite = fightBackground;
			SoundManager.S.PlayBoxingBell();
		} else if (player == 2) {
			health = 100;
			state = 20;
			gameObject.GetComponent<Renderer>().material = getSprite();
			curr_flinch_time = 0.0f;
			curr_hor_speed = 0.0f;
			curr_ver_speed = 0.0f;
			curr_attack_time = 0.0f;
			transform.position = new Vector3(12, -6, 0);
			game_mode = 1;
			mode_timer = 2.0f;
			background.sprite = fightBackground;
		}
	}

	// Update is called once per frame
	void Update () {
		if (game_mode == 0) {
			return;
		} else if (game_mode == 1) {
			mode_timer -= Time.deltaTime;
			if (mode_timer <= 0.0f) {
				background.sprite = standardBackground;
				game_mode = 2;
				mode_timer = 0.0f;
				SoundManager.S.PlayFightSound();
			}
			return;
		} else if (game_mode == 3) {
			mode_timer -= Time.deltaTime;
			if (mode_timer <= 0.0f) {
				background.sprite = standardBackground;
				game_mode = 0;
				mode_timer = 0.0f;
				if (state != 00) {
					GameManager.GM.TransitionToSolving(player);
				}
			}
			return;
		}

		if (state == 00 && game_mode != 3 && game_mode != 0) {
			game_mode = 3;
			mode_timer = 2.0f;
			opponent.GetComponent<FighterController>().game_mode = 3;
			opponent.GetComponent<FighterController>().mode_timer = 2.0f;
			background.sprite = koBackground;
			SoundManager.S.PlayKOSound();
		}

		//process input and motion
		moveUsingSpeeds();
		processInputAndStates();

		//update sprite
		gameObject.GetComponent<Renderer>().material = getSprite();
	}

	private void processInputAndStates() {
		switch (state) {
		case 00:
			break;
		case 09:
			curr_ver_speed += grav * Time.deltaTime;
			if (transform.position.y <= -6.0f) {
				curr_ver_speed = 0.0f;
			}
			curr_flinch_time -= Time.deltaTime;
			if (curr_flinch_time < 0.0f) {
				curr_flinch_time = 0.0f;
				if (transform.position.y > -6.0f) {
					state = 30;
				} else {
					if (Input.GetKey(key_crouch)) {
						state = 10;
					} else {
						state = 20;
					}
				}
				curr_hor_speed = 0.0f;
				if (player == 1) {
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
				} else if (player == 2) {
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
				}
			}
			break;
		case 10:
			if (Input.GetKeyUp(key_crouch)) {
				state = 20;
			}
			if (Input.GetKeyUp(key_left)) {
				if (curr_hor_speed < 0.0f) {
					curr_hor_speed = 0.0f;
				}
			}
			if (Input.GetKeyUp(key_right)) {
				if (curr_hor_speed > 0.0f) {
					curr_hor_speed = 0.0f;
				}
			}
			if (Input.GetKeyDown(key_right)) {
				curr_hor_speed = max_hor_speed;
			}
			if (Input.GetKeyDown(key_left)) {
				curr_hor_speed = -max_hor_speed;
			}
			if (Input.GetKeyDown(key_jump)) {
				state = 30;
				curr_ver_speed = max_ver_speed;
				SoundManager.S.PlayJumpSound();
			}
			if (Input.GetKeyDown(key_attack)) {
				curr_attack_time = max_attack_time;
				curr_hor_speed = 0.0f;
				state = 15;
				if (player == 1) {
					Vector3 hitPoint = new Vector3(transform.position.x + (78.0f/156.0f)*x_size, transform.position.y + (28.0f/221.0f)*y_size, transform.position.z);
					opponent.GetComponent<FighterController>().takeDamage(hitPoint, crouch_damage);
					SoundManager.S.PlayPunchSound();
				} else if (player == 2) {
					Vector3 hitPoint = new Vector3(transform.position.x - (78.0f/156.0f)*x_size, transform.position.y + (28.0f/221.0f)*y_size, transform.position.z);
					opponent.GetComponent<FighterController>().takeDamage(hitPoint, crouch_damage);
					SoundManager.S.PlayPunchSound();
				}
			}
			break;
		case 15:
			curr_attack_time -= Time.deltaTime;
			if (curr_attack_time < 0.0f) {
				curr_attack_time = 0.0f;
				if (Input.GetKey(key_crouch)) {
					state = 10;
				} else {
					state = 20;
				}
				if (player == 1) {
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
				} else if (player == 2) {
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
				}
			}
			break;
		case 20:
			if (Input.GetKeyDown(key_crouch)) {
				state = 10;
			}
			if (Input.GetKeyUp(key_left)) {
				if (curr_hor_speed < 0.0f) {
					curr_hor_speed = 0.0f;
				}
			}
			if (Input.GetKeyUp(key_right)) {
				if (curr_hor_speed > 0.0f) {
					curr_hor_speed = 0.0f;
				}
			}
			if (Input.GetKeyDown(key_right)) {
				curr_hor_speed = max_hor_speed;
			}
			if (Input.GetKeyDown(key_left)) {
				curr_hor_speed = -max_hor_speed;
			}
			if (Input.GetKeyDown(key_jump)) {
				state = 30;
				curr_ver_speed = max_ver_speed;
				SoundManager.S.PlayJumpSound();
			}
			if (Input.GetKeyDown(key_attack)) {
				curr_attack_time = max_attack_time;
				curr_hor_speed = 0.0f;
				state = 25;
				if (player == 1) {
					Vector3 hitPoint = new Vector3(transform.position.x + (49.0f/156.0f)*x_size, transform.position.y + (54.0f/221.0f)*y_size, transform.position.z);
					opponent.GetComponent<FighterController>().takeDamage(hitPoint, stand_damage);
					SoundManager.S.PlayPunchSound();
				} else if (player == 2) {
					Vector3 hitPoint = new Vector3(transform.position.x - (49.0f/156.0f)*x_size, transform.position.y + (54.0f/221.0f)*y_size, transform.position.z);
					opponent.GetComponent<FighterController>().takeDamage(hitPoint, stand_damage);
					SoundManager.S.PlayPunchSound();
				}
			}
			break;
		case 25:
			curr_attack_time -= Time.deltaTime;
			if (curr_attack_time < 0.0f) {
				curr_attack_time = 0.0f;
				state = 20;
				if (player == 1) {
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
				} else if (player == 2) {
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
				}
			}
			break;
		case 30:
			curr_ver_speed += grav * Time.deltaTime;
			if (transform.position.y == -6.0f) {
				curr_ver_speed = 0.0f;
				state = 20;
			}
			if (Input.GetKeyUp(key_left)) {
				if (curr_hor_speed < 0.0f) {
					curr_hor_speed = 0.0f;
				}
			}
			if (Input.GetKeyUp(key_right)) {
				if (curr_hor_speed > 0.0f) {
					curr_hor_speed = 0.0f;
				}
			}
			if (Input.GetKeyDown(key_right)) {
				curr_hor_speed = max_hor_speed;
			}
			if (Input.GetKeyDown(key_left)) {
				curr_hor_speed = -max_hor_speed;
			}
			if (Input.GetKeyDown(key_attack)) {
				curr_attack_time = max_attack_time;
				curr_hor_speed = 0.0f;
				state = 35;
				if (player == 1) {
					Vector3 hitPoint = new Vector3(transform.position.x + (64.0f/156.0f)*x_size, transform.position.y - (109.0f/221.0f)*y_size, transform.position.z);
					opponent.GetComponent<FighterController>().takeDamage(hitPoint, jump_damage);
					SoundManager.S.PlayKickSound();
				} else if (player == 2) {
					Vector3 hitPoint = new Vector3(transform.position.x - (64.0f/156.0f)*x_size, transform.position.y - (109.0f/221.0f)*y_size, transform.position.z);
					opponent.GetComponent<FighterController>().takeDamage(hitPoint, jump_damage);
					SoundManager.S.PlayKickSound();
				}
			}
			break;
		case 35:
			curr_attack_time -= Time.deltaTime;
			curr_ver_speed += grav * Time.deltaTime;
			if (curr_attack_time < 0.0f) {
				curr_attack_time = 0.0f;
				if (transform.position.y > -6.0f) {
					state = 30;
				} else {
					state = 20;
					curr_ver_speed = 0.0f;
				}
				if (player == 1) {
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
				} else if (player == 2) {
					if (Input.GetKey(key_right)) {
						curr_hor_speed = max_hor_speed;
					}
					if (Input.GetKey(key_left)) {
						curr_hor_speed = -max_hor_speed;
					}
				}
			}
			break;
		default:
			break;
		}
	}

	public void takeDamage(Vector3 damagePos, float damageDealt) {
		switch (state) {
		case 00:
			return;
		case 09:
			return;
		case 10:
			if (player == 1) {
				if (damagePos.x <= transform.position.x + (12.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (35.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = -max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			} else if (player == 2) {
				if (damagePos.x >= transform.position.x - (12.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (35.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			}
			return;
		case 15:
			if (player == 1) {
				if (damagePos.x <= transform.position.x + (12.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (35.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = -max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			} else if (player == 2) {
				if (damagePos.x >= transform.position.x - (12.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (35.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			}
			return;
		case 20:
			if (player == 1) {
				if (damagePos.x <= transform.position.x + (0.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (78.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = -max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			} else if (player == 2) {
				if (damagePos.x >= transform.position.x - (0.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (78.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			}
			return;
		case 25:
			if (player == 1) {
				if (damagePos.x <= transform.position.x + (0.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (78.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = -max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			} else if (player == 2) {
				if (damagePos.x >= transform.position.x - (0.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (78.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-110.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			}
			return;
		case 30:
			if (player == 1) {
				if (damagePos.x <= transform.position.x + (10.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (83.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-80.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = -max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			} else if (player == 2) {
				if (damagePos.x >= transform.position.x - (10.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (83.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-80.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			}
			return;
		case 35:
			if (player == 1) {
				if (damagePos.x <= transform.position.x + (22.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (43.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-94.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = -max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			} else if (player == 2) {
				if (damagePos.x >= transform.position.x - (22.0f/156.0f)*x_size &&
					damagePos.y <= transform.position.y + (43.0f/221.0f)*y_size &&
					damagePos.y >= transform.position.y + (-94.0f/221.0f)*y_size) {
					health -= damageDealt;
					if (health <= 0.0f) {
						health = 0.0f;
						state = 00;
					} else {
						state = 09;
						curr_flinch_time = max_flinch_time;
						curr_hor_speed = max_flinch_speed;
					}
					visualDamageEffects(damagePos);
				}
			}
			return;
		default:
			return;
		}
	}

	private void visualDamageEffects(Vector3 damagePos) {
		if (player == 1) {
			SoundManager.S.PlayHurt1Sound();

			Vector3 pos1 = damagePos + new Vector3(Random.Range(-3.0f, 0.0f), Random.Range(-3.0f, 3.0f), 0.0f);
			Vector3 pos2 = damagePos + new Vector3(Random.Range(-3.0f, 0.0f), Random.Range(-3.0f, 3.0f), 0.0f);
			Vector3 pos3 = damagePos + new Vector3(Random.Range(-3.0f, 0.0f), Random.Range(-3.0f, 3.0f), 0.0f);

			Vector3 vel1 = new Vector3(Random.Range(-7.0f, -3.0f), Random.Range(-2.0f, 2.0f), 0.0f);
			Vector3 vel2 = new Vector3(Random.Range(-7.0f, -3.0f), Random.Range(-2.0f, 2.0f), 0.0f);
			Vector3 vel3 = new Vector3(Random.Range(-7.0f, -3.0f), Random.Range(-2.0f, 2.0f), 0.0f);

			float scale1 = Random.Range(1.0f, 3.0f);
			float scale2 = Random.Range(1.0f, 3.0f);
			float scale3 = Random.Range(1.0f, 3.0f);

			GameObject puff1 = (GameObject)Instantiate(puff, pos1, Quaternion.identity);
			GameObject puff2 = (GameObject)Instantiate(puff, pos2, Quaternion.identity);
			GameObject puff3 = (GameObject)Instantiate(puff, pos3, Quaternion.identity);

			puff1.GetComponent<PuffController>().velocity = vel1;
			puff2.GetComponent<PuffController>().velocity = vel2;
			puff3.GetComponent<PuffController>().velocity = vel3;

			puff1.transform.localScale = new Vector3(scale1, scale1, 1.0f);
			puff2.transform.localScale = new Vector3(scale2, scale2, 1.0f);
			puff3.transform.localScale = new Vector3(scale3, scale3, 1.0f);

		} else if (player == 2) {
			SoundManager.S.PlayHurt2Sound();

			Vector3 pos1 = damagePos + new Vector3(Random.Range(0.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);
			Vector3 pos2 = damagePos + new Vector3(Random.Range(0.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);
			Vector3 pos3 = damagePos + new Vector3(Random.Range(0.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);

			Vector3 vel1 = new Vector3(Random.Range(3.0f, 7.0f), Random.Range(-2.0f, 2.0f), 0.0f);
			Vector3 vel2 = new Vector3(Random.Range(3.0f, 7.0f), Random.Range(-2.0f, 2.0f), 0.0f);
			Vector3 vel3 = new Vector3(Random.Range(3.0f, 7.0f), Random.Range(-2.0f, 2.0f), 0.0f);

			float scale1 = Random.Range(1.0f, 3.0f);
			float scale2 = Random.Range(1.0f, 3.0f);
			float scale3 = Random.Range(1.0f, 3.0f);

			GameObject puff1 = (GameObject)Instantiate(puff, pos1, Quaternion.identity);
			GameObject puff2 = (GameObject)Instantiate(puff, pos2, Quaternion.identity);
			GameObject puff3 = (GameObject)Instantiate(puff, pos3, Quaternion.identity);

			puff1.GetComponent<PuffController>().velocity = vel1;
			puff2.GetComponent<PuffController>().velocity = vel2;
			puff3.GetComponent<PuffController>().velocity = vel3;

			puff1.transform.localScale = new Vector3(scale1, scale1, 1.0f);
			puff2.transform.localScale = new Vector3(scale2, scale2, 1.0f);
			puff3.transform.localScale = new Vector3(scale3, scale3, 1.0f);

		}
	}

	private void moveUsingSpeeds() {
		transform.Translate(new Vector3(curr_hor_speed * Time.deltaTime, curr_ver_speed * Time.deltaTime, 0));

		if (transform.position.y < -6.0f) {
			transform.position = new Vector3(transform.position.x, -6.0f, transform.position.z);
		} else if (transform.position.y > 10.0f) {
			curr_ver_speed = -5.0f;
			transform.position = new Vector3(transform.position.x, 10.0f, transform.position.z);
		}

		if (transform.position.x > 20.0f) {
			transform.position = new Vector3(20.0f, transform.position.y, transform.position.z);
		} else if (transform.position.x < -20.0f) {
			transform.position = new Vector3(-20.0f, transform.position.y, transform.position.z);
		}

		if (player == 1) {
			if (transform.position.x > opponent.transform.position.x - 2.0f) {
				transform.position = new Vector3(opponent.transform.position.x - 2.0f, transform.position.y, transform.position.z);
			}
		} else if (player == 2) {
			if (transform.position.x < opponent.transform.position.x + 2.0f) {
				transform.position = new Vector3(opponent.transform.position.x + 2.0f, transform.position.y, transform.position.z);
			}
		}

	}

	private Material getSprite() {
		switch (state) {
		case 00:
			return Sprite00;
		case 09:
			return Sprite09;
		case 10:
			return Sprite10;
		case 15:
			return Sprite15;
		case 20:
			return Sprite20;
		case 25:
			return Sprite25;
		case 30:
			return Sprite30;
		case 35:
			return Sprite35;
		default:
			return Sprite20;
		}
	}

}
