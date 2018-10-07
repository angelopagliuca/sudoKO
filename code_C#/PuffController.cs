using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffController : MonoBehaviour {

	public Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(velocity * Time.deltaTime);
	}
}
