using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoardController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.GM.CreateBoard ();
		GameManager.GM.RedSprite = GameObject.Find("RedSprite");
		GameManager.GM.BlueSprite = GameObject.Find("BlueSprite");
		GameManager.GM.BoxBt = GameObject.Find("BoxBt");
		GameManager.GM.Grid = GameObject.Find("Grid");
		GameManager.GM.BoardCanvas = GameObject.Find("BoardCanvas");
		GameManager.GM.Fighter1 = GameObject.Find("Fighter1");
		GameManager.GM.Fighter2 = GameObject.Find("Fighter2");
		GameManager.GM.Background = GameObject.Find("Background");
		GameManager.GM.RedHealth = GameObject.Find("RedHealth");
		GameManager.GM.BlueHealth = GameObject.Find("BlueHealth");
		GameManager.GM.HealthBar = GameObject.Find("HealthBar");
		GameManager.GM.Diamond = GameObject.Find("Diamond");
		GameManager.GM.BlueBolt = GameObject.Find("BlueBolt");
		GameManager.GM.RedBolt = GameObject.Find("RedBolt");
		GameManager.GM.BackgroundBoard = GameObject.Find ("final background");
		GameManager.GM.RedControl = GameObject.Find ("RedControlsOverlay");
		GameManager.GM.BlueControl = GameObject.Find ("BlueControlsOverlay");
		GameManager.GM.NumberControl = GameObject.Find ("NumbersOverlay");


		GameManager.GM.Fighter1.SetActive(false);
		GameManager.GM.Fighter2.SetActive(false);
		GameManager.GM.Background.SetActive(false);
		GameManager.GM.RedHealth.SetActive(false);
		GameManager.GM.BlueHealth.SetActive(false);
		GameManager.GM.HealthBar.SetActive(false);
		GameManager.GM.Diamond.SetActive(false);
		GameManager.GM.BlueBolt.SetActive(false);
		GameManager.GM.RedBolt.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
