using System.Collections;
using UnityEngine;

// Analysis disable CheckNamespace
public class GameController : MonoBehaviour {
// Analysis restore CheckNamespace
	//groups
	public GameObject[] tetriminos;

	private GameObject previewTetrimino;
	private GameObject nextTetrimino;

	public float fallSpeed = 1.0f;
	private bool isValidPos;

	private bool gameStarted = false;

	private Vector2 previewTetriminoPosition = new Vector2(-7f, 6.5f);

	void Start(){


		//spawn initial Group
		DisplayPreview();
	}

	void Update(){

		UpdateLevel ();
		UpdateSpeed ();
	}

	void UpdateLevel() {


	}

	void UpdateSpeed(){


	}


	public void SpawnNext(int i) {
		
//		int i = Random.Range(0, tetriminos.Length);

		// Spawn Group at current Position
		Instantiate(tetriminos[i],
			transform.position,
			Quaternion.identity); 

		//Call the IsValidGridPosition method from the tetrimino script and set variable reference

//		for(int j = 0; j < tetriminos.Length; j++){
//
//			isValidPos = tetriminos [i].GetComponent<Tetrimino> ().IsValidGridPos ();
//
//			//Default position not valid then Game over
//			if(!isValidPos){
//				Debug.Log ("GAME OVER");
//				Destroy (tetriminos[i]);
//				//Debug.Log ("IsValidGridPos = " + isValidGridPos ());
//			}
//		}

	}

	public void DisplayPreview(){


		// Random Index
		int i = Random.Range(0, tetriminos.Length);
		int j = Random.Range(0, tetriminos.Length);
		if (!gameStarted){

			gameStarted = true;
		
			// Spawn Group at current Position
			nextTetrimino = Instantiate(tetriminos[j],
		    transform.position,
			Quaternion.identity); 
		
			previewTetrimino = Instantiate(tetriminos[i],
				previewTetriminoPosition,
				Quaternion.identity);
			previewTetrimino.GetComponent<Tetrimino> ().enabled = false;
		
		
		} else{
		
			previewTetrimino.transform.localPosition = transform.position;
			nextTetrimino = previewTetrimino;
			nextTetrimino.GetComponent<Tetrimino> ().enabled = true;
		
			previewTetrimino = Instantiate(tetriminos[i],
				previewTetriminoPosition,
				Quaternion.identity);
			previewTetrimino.GetComponent<Tetrimino> ().enabled = false;
		
		}
	}


}




