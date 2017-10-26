using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {

	//Set up the Grid
	public static int w = 10;
	public static int h = 20;

	//variable for score
	public static int scoreOneLine = 40, scoreTwoLines = 100, scoreThreeLines = 300, 
	scoreFourLines = 1200; 

	public static int currentScore = 0, linesScore = 0, numFullRowsThisTurn = 0;

	public Text hud_Score, lines_Score;

	public static Transform[,] grid = new Transform[w, h];

	void Update(){

		UpdateScore ();
		UpdateUI ();
	}

	public void UpdateUI(){

		hud_Score.text = "Score: " + currentScore.ToString ();
		lines_Score.text = "Lines: " + linesScore.ToString ();
	}
	public void UpdateScore(){

		if (numFullRowsThisTurn > 0){

			if(numFullRowsThisTurn == 1){

				ClearedOneLine ();
			}else if(numFullRowsThisTurn == 2){

				ClearedTwoLines ();
			}else if(numFullRowsThisTurn == 3){

				ClearedThreeLines ();
			}else if(numFullRowsThisTurn == 4){

				ClearedFourLines ();
			}
			linesScore += numFullRowsThisTurn;
			numFullRowsThisTurn = 0;
		}
	}

	public void ClearedOneLine(){

		currentScore += scoreOneLine;
		//linesScore += 1;
	}

	public void ClearedTwoLines(){

		currentScore += scoreTwoLines;
		//linesScore += 2;
	}

	public void ClearedThreeLines(){

		currentScore += scoreThreeLines;
		//linesScore += 3;
	}

	public void ClearedFourLines(){

		currentScore += scoreFourLines;
		//linesScore += 4;
	}


	public static Vector2 RoundVec2 (Vector2 v) {
		return new Vector2 (Mathf.Round (v.x),
							Mathf.Round (v.y));
	}

	public static bool InsideBorder (Vector2 pos) {
		
		return ((int)pos.x >= 0 &&
		(int)pos.x < w &&
		(int)pos.y >= 0);
	}

	public static void DeleteRow(int y) {

		for (int x = 0; x < w; x++) {

			Destroy (grid[x , y].gameObject);
			grid [x, y] = null;
		}
	}

	public static void DecreaseRow (int y) {

		for(int x = 0; x < w; ++x){

			if(grid[x , y] != null){
				//move one towards the bottom
				grid [x, y - 1] = grid [x, y];
				grid [x, y] = null;

				//update block positions
				grid [x, y - 1].position += new Vector3 (0, -1, 0);
			}
		}
	}

	public static void DecreaseRowsAbove(int y){

		for (int i = y; i < h; ++i){

			DecreaseRow (i);
		}
			
	}

	public static bool IsRowFull(int y){

		for (int x = 0; x < w; ++x)
			if (grid [x, y] == null)
				return false;
		//Since We found a full row increment the number of full rows
		numFullRowsThisTurn++;
		return true;

	}

	public static void DeleteFullRows(){

		for(int y = 0; y < h; ++y){

			if(IsRowFull (y)){

				DeleteRow (y);
				DecreaseRowsAbove (y + 1);
				--y;

			}
		}
	}
}
