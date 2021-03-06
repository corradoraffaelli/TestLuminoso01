using UnityEngine;
using System.Collections;

public class MagicLantern : Tool {

	//public GameObject lantern;
	//public GameObject cube;

	//public GameObject glasses;
	public GameObject raggio_cerchio;
	public GameObject raggio;
	public GameObject cameraPoint;
	public GameObject camera;
	
	public float RotationSpeed = 100;
	public float zPositionEnvironment = 0.0f;
	public float resizeFactor = 4.0f;

	public Sprite normalRay;
	public Sprite normalCircle;
	public Sprite pressedRay;
	public Sprite pressedCircle;

	public Sprite goodGlass;
	public Sprite badGlass;
	public Sprite projectionSprite;

	public GameObject projectionPrefab;

	Vector3 cameraPointPos;
	Vector3 _direction;
	float distance_to_screen;
	Vector3 pos_move;

	//grandezza della sprite con il raggio
	float xSize;
	float ySize;
	Bounds boundsRay;

	SpriteRenderer spRendRay;
	SpriteRenderer spRendCircle;

	ProjectionCollision PC;
	bool wasGoodProjection = false;

	GameObject gameObjectProjection;
	
	//INITIALIZATION PART------------------------------------------------------------------------------------------------------------------

	protected override void initializeTool() {

		boundsRay = raggio.GetComponent<SpriteRenderer>().bounds;
		xSize = boundsRay.size.x;
		ySize = boundsRay.size.y;

		spRendRay = raggio.GetComponent<SpriteRenderer> ();
		spRendCircle = raggio_cerchio.GetComponent<SpriteRenderer> ();

		PC = transform.GetComponent<ProjectionCollision>();
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

	protected override void activationToolFunc()
	{
		//posiziono la lenterna di fronte al personaggio
		toolGameObject.transform.position = new Vector3(player.transform.position.x+0.4f,player.transform.position.y+0.8f,player.transform.position.z);
		Debug.Log ("attivata");
	}

	//qui va inserita la logica del tool, usata nell'update...
	protected override void useTool() {

		normalMovementsUnderMouse ();

		if (usingDrag)
			changeRaySprite (true);

		if (Input.GetMouseButtonUp (0)) {
			changeRaySprite (false);
			if (PC.isColliding())
				placeImage();
		}
			
		if (PC.isColliding() && !wasGoodProjection) {
			switchProjection (true);
			wasGoodProjection = true;
		} else if (!PC.isColliding() && wasGoodProjection) {
			switchProjection(false);
			wasGoodProjection = false;
		}


	}
	
	void normalMovementsUnderMouse(){

		//posiziono la lenterna di fronte al personaggio (ora non serve, la lanterna è "child" del player, da verificare la correttezza della cosa
		//toolGameObject.transform.position = new Vector3(player.transform.position.x+0.4f,player.transform.position.y+0.8f,player.transform.position.z);
		
		//posiziono l'origine del cerchio sotto il mouse
		pos_move = new Vector3 (actualMousePosition.x, actualMousePosition.y, zPositionEnvironment);
		raggio_cerchio.transform.position = new Vector3( pos_move.x, pos_move.y, pos_move.z );
		
		//prendo la posizione del punto frontale della sprite della camera, e ci piazzo l'origine del raggio
		cameraPointPos = cameraPoint.transform.position;
		raggio.transform.position = cameraPointPos;
		
		//prendo la direzione tra l'inizio del raggio e la posizione del cerchio
		_direction = (raggio_cerchio.transform.position - cameraPointPos).normalized;
		
		//cambio la scala del raggio in base alla distanza tra camera e cerchio
		float distance = Vector3.Distance (raggio_cerchio.transform.position, cameraPointPos);
		raggio.transform.localScale = new Vector3(distance / xSize,distance / (ySize*resizeFactor),1);
		raggio_cerchio.transform.localScale = new Vector3(raggio.transform.localScale.y,raggio.transform.localScale.y,1);
		
		//setto la direzione del raggio
		//se il personaggio non sta guardando verso destra la sua scala è -1, devo perciò correggere quella della direzione, di conseguenza
		PlayerMovements PM = player.GetComponent<PlayerMovements> ();
		if (!PM.FacingRight) {
			_direction = new Vector3 (-_direction.x, _direction.y,_direction.z);
		}
		raggio.transform.right = _direction;
		
		//setto la direzione della camera
		if (raggio.transform.localEulerAngles.z > 180)
			camera.transform.localEulerAngles = new Vector3 (raggio.transform.localEulerAngles.x, raggio.transform.localEulerAngles.x, (raggio.transform.localEulerAngles.z-360) / 2);
		else
			camera.transform.localEulerAngles = new Vector3 (raggio.transform.localEulerAngles.x, raggio.transform.localEulerAngles.x, raggio.transform.localEulerAngles.z / 2);

		//test flipping personaggio
		//flippa se il raggio punta dietro al personaggio
		if ((PM.FacingRight && raggio_cerchio.transform.position.x < player.transform.position.x)
		    || (!PM.FacingRight && raggio_cerchio.transform.position.x > player.transform.position.x))
			PM.c_flip ();
	}

	void changeRaySprite(bool needToChangeSprite)
	{
		if (needToChangeSprite) {
			spRendRay.sprite = pressedRay;
			spRendCircle.sprite = pressedCircle;
		} else {
			spRendRay.sprite = normalRay;
			spRendCircle.sprite = normalCircle;
		}
	}

	void placeImage()
	{
		deleteOldProjection ();

		GameObject actualGO;

		Bounds objBounds = PC.getSpriteBounds ();
		gameObjectProjection = Instantiate <GameObject> (projectionPrefab);

		gameObjectProjection.transform.position = new Vector3(actualMousePosition.x, actualMousePosition.y, zPositionEnvironment);

		SpriteRenderer actualSprite = gameObjectProjection.transform.GetComponent<SpriteRenderer> ();
		Bounds newObjBounds = actualSprite.bounds;

		float spriteScale = objBounds.size.x / newObjBounds.size.x;
		gameObjectProjection.transform.localScale = new Vector3 (spriteScale, spriteScale, spriteScale);

	}

	void deleteOldProjection()
	{
		if (gameObjectProjection)
			Destroy (gameObjectProjection);
	}

	void switchProjection(bool good)
	{
		if (good)
			PC.changeSprite (goodGlass);
		else
			PC.changeSprite (badGlass);

		Debug.Log ("cambiata sprite");
	}
}
