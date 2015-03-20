using UnityEngine;
using System.Collections;

public class MagicLantern : Tool {

	//public GameObject lantern;
	
	public GameObject glasses;

	float distance_to_screen;
	Vector3 pos_move;

	public GameObject raggio_cerchio;
	public GameObject raggio;
	public GameObject cameraPoint;
	public GameObject camera;

	Vector3 cameraPointPos;
	
	public float RotationSpeed = 100;
	private Quaternion _lookRotation;
	private Vector3 _direction;

	//grandezza della sprite con il raggio
	float xSize;
	float ySize;
	private Bounds boundsRay;
	
	//INITIALIZATION PART------------------------------------------------------------------------------------------------------------------

	protected override void initializeTool() {

		boundsRay = raggio.GetComponent<SpriteRenderer>().bounds;
		xSize = boundsRay.size.x;
		ySize = boundsRay.size.y;
		//inventory = GameObject.Find ("Inventory");
		//getUsableGlasses ();
		
	}



	/*
	void getUsableGlasses(){
		
		foreach (Transform child in inventory.transform) {
			if(child.name=="Glasses") {
				glasses = child.gameObject;
				break;
			}
		}
		
		foreach (Transform child in glasses.transform) {
			if(child.tag=="basicGlass" || child.tag=="animatedGlass") {
				//glasses = child.gameObject;
				Glass newG = new Glass(child.GetComponent<SpriteRenderer>().sprite, child.tag);
				subTools.Add(newG);
			}
		}
		
	}
	*/

	//UPDATING PART-------------------------------------------------------------------------------------------------------------------------

	//qui va inserita la logica del tool, usata nell'update...
	protected override void useTool() {
		//posiziono la lenterna di fronte al personaggio
		toolGameObject.transform.position = new Vector3(player.transform.position.x+0.4f,player.transform.position.y+0.8f,player.transform.position.z);

		cameraPointPos = cameraPoint.transform.position;
		raggio.transform.position = cameraPointPos;

		_direction = (raggio_cerchio.transform.position - cameraPointPos).normalized;

		//setto la direzione del raggio
		raggio.transform.right = _direction;
		
		//setto la direzione della camera
		if (raggio.transform.localEulerAngles.z > 180)
			camera.transform.localEulerAngles = new Vector3 (raggio.transform.localEulerAngles.x, raggio.transform.localEulerAngles.x, (raggio.transform.localEulerAngles.z-360) / 2);
		else
			camera.transform.localEulerAngles = new Vector3 (raggio.transform.localEulerAngles.x, raggio.transform.localEulerAngles.x, raggio.transform.localEulerAngles.z / 2);
		
		
		float distance = Vector3.Distance (raggio_cerchio.transform.position, cameraPointPos);
		
		raggio.transform.localScale = new Vector3(distance / xSize,1,1);







		distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		pos_move = actualMousePosition;
		//pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen ));
		transform.position = new Vector3( pos_move.x, pos_move.y, pos_move.z );
	}
	
	
}
