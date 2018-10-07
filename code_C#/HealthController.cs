using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	public GameObject fighter;
	private FighterController fc;
	private RectTransform rt;
	private float originalWidth;

	// Use this for initialization
	void Start () {
		fc = fighter.GetComponent<FighterController>();
		rt = gameObject.GetComponent<RectTransform>();
		originalWidth = rt.sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update () {
		rt.sizeDelta = new Vector2((fc.health / 100.0f) * originalWidth, rt.sizeDelta.y);
	}
}
