using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour {

	//GameObject grid;

	private int individualScore = 100;

	private float individualScoreTime;
	private float fallSpeed;

	private const float continuousVerticalSpeed = 0.05f;  //Speed at which the tetrimino can move when button is held
	private const float continuousHorizontalSpeed = 0.1f; // //Speed at which the tetrimino can move when button is held
	private const float buttonDownWaitMax = 0.2f;  // How long to wait before the tetrimino recognizes that a button is held down

	private float verticalTimer = 0;
	private float horizontalTimer = 0;
	private float buttonDownWaitTimerHorizontal = 0;
	private float buttonDownWaitTimerVertical = 0;

	private bool moveImmediateHorizontal = false;
	private bool moveImmediateVertical = false;




	// Time since last gravity tick
	float lastFall = 0;

	// Use this for initialization
	void Start () {
		fallSpeed = GameObject.Find ("Game Controller").GetComponent<GameController> ().fallSpeed;

		//Default position not valid then Game over
		if(!IsValidGridPos ()){
			

			Debug.Log ("GAME OVER");
			Destroy (gameObject);
			//Debug.Log ("IsValidGridPos = " + isValidGridPos ());
		}
	}
	
	// Update is called once per frame
	void Update () {

		CheckUserInput ();
		UpdateIndividualScore ();

	}

	void UpdateIndividualScore(){

		if(individualScoreTime < 1){

			individualScoreTime += Time.deltaTime;
		}else {

			individualScoreTime = 0;
			individualScore = Mathf.Max (individualScore - 10, 0);
		}
	}

	public bool IsValidGridPos(){

		foreach (Transform child in transform){
			Vector2 v = GridManager.RoundVec2 (child.position);

		//Not inside border
		if (!GridManager.InsideBorder (v))
			return false;

		//Block in Grid cell (not part of same group)?
		if (GridManager.grid [(int)v.x, (int)v.y] != null &&
		    GridManager.grid [(int)v.x, (int)v.y].parent != transform)
			return false;
		}
	return true;
	}

	void UpdateGrid(){

		//Remove Old Children from grid
		for (int y = 0; y < GridManager.h; ++y)
			for (int x = 0; x < GridManager.w; ++x)
				if (GridManager.grid [x, y] != null)
				if (GridManager.grid [x, y].parent == transform)
					GridManager.grid [x, y] = null;

		//Add new children to grid
		foreach (Transform child in transform){

			Vector2 v = GridManager.RoundVec2 (child.position);
			GridManager.grid [(int)v.x, (int)v.y] = child;
		}
					
	}

	void CheckUserInput(){


		if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow)){


			moveImmediateHorizontal = false;
			horizontalTimer = 0;
			buttonDownWaitTimerHorizontal = 0;


		}

		if(Input.GetKeyUp (KeyCode.DownArrow)){

			moveImmediateVertical = false;
			verticalTimer = 0;
			buttonDownWaitTimerVertical = 0;

		}


		if (Input.GetKey (KeyCode.LeftArrow)){
			MoveLeft ();

		}

		if (Input.GetKey (KeyCode.RightArrow)){

			MoveRight ();
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)){
			
			Rotate ();
		}

		if (Input.GetKey (KeyCode.DownArrow)|| Time.time - lastFall >= 1){

			MoveDown ();
		}
	}
	void MoveLeft(){

		if (moveImmediateHorizontal) {



			if (buttonDownWaitTimerHorizontal < buttonDownWaitMax) {

				buttonDownWaitTimerHorizontal += Time.deltaTime;
				return;
			}

			if (horizontalTimer < continuousHorizontalSpeed) {

				horizontalTimer += Time.deltaTime;
				return;
			}
		}

		if(!moveImmediateHorizontal){
			moveImmediateHorizontal = true;
		}

		horizontalTimer = 0;
		//Modify position
		transform.position += new Vector3 (-1, 0, 0);

		//See if the input is valid
		if (IsValidGridPos ())
			//Its valid update the grid
			UpdateGrid ();
		else
			//Its not valid. revert.
			transform.position += new Vector3(1, 0, 0);

	}

	void MoveRight(){


		if (moveImmediateHorizontal) {


			if (buttonDownWaitTimerHorizontal < buttonDownWaitMax) {

				buttonDownWaitTimerHorizontal += Time.deltaTime;
				return;
			}

			if (horizontalTimer < continuousHorizontalSpeed) {

				horizontalTimer += Time.deltaTime;
				return;
			}
		}
		if(!moveImmediateHorizontal){

			moveImmediateHorizontal = true;
		}

		horizontalTimer = 0;

		//Move Right
		//Modify Position
		transform.position += new Vector3 (1, 0, 0);

		//See if the input is valid
		if (IsValidGridPos ())
			//Its valid update the grid
			UpdateGrid ();
		else 
			//Its not valid. Revert.
			transform.position += new Vector3 (-1, 0, 0);
	}

	void MoveDown(){

		if (moveImmediateVertical) {


			if (buttonDownWaitTimerHorizontal < buttonDownWaitMax) {

				buttonDownWaitTimerHorizontal += Time.deltaTime;
				return;
			}

			if (verticalTimer < continuousVerticalSpeed) {

				verticalTimer += Time.deltaTime;
				return;
			}
		}
		if(!moveImmediateVertical){
			moveImmediateVertical = true;
		}

		verticalTimer = 0;
		//Move Down
		//Modify Position
		transform.position += new Vector3 (0, -1, 0);

		//See if it is valid
		if (IsValidGridPos ()) {
			Debug.Log ("IsValidGridPos = " + IsValidGridPos ());
			//Its valid update the grid
			UpdateGrid ();
		}else {
			//Its not valid. Revert
			transform.position += new Vector3 (0, 1, 0);

			//Clear filled horizontal lines
			GridManager.numFullRowsThisTurn = 0;
			GridManager.DeleteFullRows ();


			//Spawn the next GGroup
			FindObjectOfType<GameController> ().DisplayPreview ();

			GridManager.currentScore += individualScore;

			//Disable Script
			enabled = false;
		}
		lastFall = Time.time;

	}

	void Rotate(){

		//Rotate
		transform.Rotate (0, 0, -90);

		//see if it is valid
		if (IsValidGridPos ())
			//Its valid. Update the grid
			UpdateGrid ();
		else 
			//Its not valid. Revert
			transform.Rotate (0, 0, 90);

	}
}
