using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellController : MonoBehaviour {

	public static GridCellController GC;

	public int row;
	public int col;
	public int num;
	public int solution;
	public bool hidden;
	public bool playerOn;
	public bool selected;
	public bool selectedBoth;
	public bool player1;
	public bool player2;
	public bool player1Owned;
	public bool player2Owned;
	public bool locked;

	Renderer rend;

	public Material clearNum;
	public Material num1;
	public Material num2;
	public Material num3;
	public Material num4;
	public Material num5;
	public Material num6;
	public Material num7;
	public Material num8;
	public Material num9;


	// Use this for initialization
	void Start () {
		GC = this;
		//print ("Row = " + row + " Col = " + col);

		rend = GetComponent<Renderer>();

		if (num == 0) {
			rend.material = clearNum;
		} else if (num == 1) {
			rend.material = num1;
		} else if (num == 2) {
			rend.material = num2;
		} else if (num == 3) {
			rend.material = num3;
		} else if (num == 4) {
			rend.material = num4;
		} else if (num == 5) {
			rend.material = num5;
		} else if (num == 6) {
			rend.material = num6;
		} else if (num == 7) {
			rend.material = num7;
		} else if (num == 8) {
			rend.material = num8;
		} else if (num == 9) {
			rend.material = num9;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (player1 && player2) {
			if (selected) {
				rend.material.color = new Color32 (0xff, 0xcc, 0x31, 0xff);
			} else rend.material.color = new Color32 (0xff, 0xdb, 0x6e, 0xff);
		} else if (player1) {
			if (selected) {
				rend.material.color = new Color32 (0x05, 0x25, 0x4c, 0xff);
			} else rend.material.color = new Color32 (0x52, 0xa7, 0xfa, 0xff);
		} else if (player2) {
			if (selected) {
				rend.material.color = new Color32 (0x83, 0x0c, 0x0c, 0xff);
			} else rend.material.color = new Color32 (0xfd, 0x60, 0x60, 0xff);
		} 
		if (selectedBoth) {
			rend.material.color = new Color32 (0xff, 0xcc, 0x31, 0xff);
		}

		if (!playerOn) {
			if (num == 0) {
				rend.material = clearNum;
			} else if (num == 1) {
				rend.material = num1;
			} else if (num == 2) {
				rend.material = num2;
			} else if (num == 3) {
				rend.material = num3;
			} else if (num == 4) {
				rend.material = num4;
			} else if (num == 5) {
				rend.material = num5;
			} else if (num == 6) {
				rend.material = num6;
			} else if (num == 7) {
				rend.material = num7;
			} else if (num == 8) {
				rend.material = num8;
			} else if (num == 9) {
				rend.material = num9;
			}
		}

		if (player2Owned) {
			rend.material.color = new Color32 (0x00, 0x4b, 0xa7, 0xff);
		} else if (player1Owned) {
			rend.material.color = new Color32 (0xba, 0x08, 0x09, 0xff);
		}

		if (locked) {
			rend.material.color = new Color32 (0xc0, 0xc0, 0xc0, 0xc0);
		}

		playerOn = false;
		player1 = false;
		player2 = false;

	}


	public bool ChangeCellNumber(int Num) {

		if (Num == solution) {
			if (Num == 1) {
				rend.material = num1;
			} else if (Num == 2) {
				rend.material = num2;
			} else if (Num == 3) {
				rend.material = num3;
			} else if (Num == 4) {
				rend.material = num4;
			} else if (Num == 5) {
				rend.material = num5;
			} else if (Num == 6) {
				rend.material = num6;
			} else if (Num == 7) {
				rend.material = num7;
			} else if (Num == 8) {
				rend.material = num8;
			} else if (Num == 9) {
				rend.material = num9;
			}
		} 

		num = Num;

		return (Num == solution);
	}
}
