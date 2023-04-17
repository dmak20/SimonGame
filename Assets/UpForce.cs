using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpForce : MonoBehaviour {

	[SerializeField]
	float forceMultiplier = 1000;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire1")) {
			GetComponent<Rigidbody> ().AddForce (Vector3.up * forceMultiplier);

		}


	}
}
