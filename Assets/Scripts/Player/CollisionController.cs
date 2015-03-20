using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour {

	public GameObject playerLowerPosition;
	private BoxCollider2D coll;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("SoftGround"))
		{
			//Physics2D.
			//coll = fooObj.GetComponent<BoxCollider2D>();
			//coll.bounds.
			//Transform childUpper = fooObj.transform.FindChild("Upper");
			//if (playerLowerPosition.transform.position.y > childUpper.transform.position.y)
			//	coll.enabled = true;
			//else
			//	coll.enabled = false;
		}
	}
}
