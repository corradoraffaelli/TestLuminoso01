using UnityEngine;
using System.Collections;

public class stunnedCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D c) {

		//Debug.Log ("schiacciato");

		if (c.gameObject.tag=="Player" ) {
			//Debug.Log ("schiacciato2");
			transform.parent.SendMessage ("setStunned", true);

		} else {
			//Debug.Log ("nome oggetto " + c.gameObject.name);
		}

	}
}
