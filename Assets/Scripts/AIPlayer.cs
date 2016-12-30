using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

public class AIPlayer : MonoBehaviour {

	private GameController gameCont;
	private string aiPlayerColor = "blue";
	private int[] basicPiecePreference = new int[] {18,11,12,17,19,24,25,5,6,7,10,13,16,20,23,26,29,30,31,1,2,4,8,9,14,22,27,28,32,34,35,0,3,15,21,33,36};
	private IEnumerator aiCoroutine;
	//private int[] piecePreference;
	List<int> piecePreference = new List<int>();

	public void takeTurn() {
		gameCont.RollDice ();
		addPreferableVerticesToPiecePreferenceList ();
		//System.Threading.Thread.Sleep(1000); 
		aiCoroutine = waitForAWhile (3.5f);
		StartCoroutine (aiCoroutine);
	}

	private IEnumerator waitForAWhile(float seconds) {
		//also attempt to claim a piece
		yield return new WaitForSeconds(seconds);
		for (int i = 0; i < piecePreference.Count; i++) {
			if (gameCont.attemptToClaimPiece (piecePreference [i])) {
				break;
			}
		}
	}

	public void GameControllerSetter(GameController gc) {
		gameCont = gc;
	}

	public string getAIPlayerColor() {
		string color = aiPlayerColor;
		return color;
	}

	private void addPreferableVerticesToPiecePreferenceList() {
		piecePreference.Clear ();
		Dictionary<int, List<int>> gamePieceUpVoter = new Dictionary<int, List<int>>();
		initializeGamePieceUpVoter (gamePieceUpVoter);
		int[] arrayOfScores = new int[37];
		for (int h = 0; h < 37; h++) {
			arrayOfScores [h] = 0;
		}

		//Debug.log ("At top of method addPreferableVerticesToPiecePreferenceList");
		for (int i = 0; i < gameCont.Triangles.Length; i++) {
			int[] currentTriVerts = gameCont.Triangles [i].GetComponent<Triangle> ().GetVertices ();
			int blueVerts = 0;
			int redVerts = 0;
			//int blueVertScore = 0;
			for (int j = 0; j < currentTriVerts.Length; j++) {
				if (gameCont.buttonTexts [currentTriVerts [j]].text.Equals ("b")) {
					blueVerts++;
				} else if (gameCont.buttonTexts [currentTriVerts [j]].text.Equals ("r")) {
					redVerts++;
				}
			}
			//Console.Write ("This triangle's vert counts are red: " + redVerts.ToString () + " + blue: " + blueVerts.ToString ());
			if (blueVerts == 2 && redVerts == 0) {
				for (int j = 0; j < currentTriVerts.Length; j++) {
					if (!gameCont.buttonTexts [currentTriVerts [j]].text.Equals ("b") && !gameCont.buttonTexts [currentTriVerts [j]].text.Equals ("r")) {
						//piecePreference.Insert (0, currentTriVerts [j]);
						arrayOfScores[currentTriVerts [j]]+=4;
					}
				}
			} else if (blueVerts == 1 && redVerts < 2) {
				for (int j = 0; j < currentTriVerts.Length; j++) {
					if (!gameCont.buttonTexts [currentTriVerts [j]].text.Equals ("b") && !gameCont.buttonTexts [currentTriVerts [j]].text.Equals ("r")) {
						//piecePreference.Add (currentTriVerts [j]);
						arrayOfScores[currentTriVerts [j]]+=1;
					}
				}
			}
		}

		for (int l = 0; l < arrayOfScores.Length; l++) {
			int thisPiecesScore = arrayOfScores [basicPiecePreference[l]];
			gamePieceUpVoter [thisPiecesScore].Add (basicPiecePreference[l]);
		}

		for (int m = 24; m > 0; m--) {
			List<int> piecesThatGotMScore = gamePieceUpVoter [m];
			for (int n = 0; n < piecesThatGotMScore.Count; n++) {
				piecePreference.Add (piecesThatGotMScore [n]);
			}
		}

		for (int k = 0; k < basicPiecePreference.Length; k++) {
			piecePreference.Add (basicPiecePreference [k]);
		}
	}	

	private Dictionary<int,List<int>> initializeGamePieceUpVoter(Dictionary<int,List<int>> dict) {
		for (int i = 0; i < 25; i++) {
			dict.Add (i, new List<int>());
		}
		return dict;
	}

}
