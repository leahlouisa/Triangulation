using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

	public GameObject[] Triangles;
	public Text[] buttonTexts;
	public Text diceRollText;
	public GameObject diceRollPanel;
	public Button diceRollButton;
	public GameObject gameOverPanel;
	public Button playAgainButton;
	public Button quitGameButton;
	public GameObject redHighlighter;
	public GameObject blueHighlighter;
	public GameObject noMovesPanel;
	public Button nextTurnButton;
	public int redScore;
	public int blueScore;
	public Text redScoreText;
	public Text blueScoreText;
	public GameObject instructionsPanel;
	public GameObject playerIDPanel;
	//public GameObject[] Triangles;

	private int numberOfTurnsWithNoLegalMoves = 0;
	private int totalNumberOfPlayers = 2;
	private string playerColor;
	private string startingPlayerColor = "red";
	private int dieOne;
	private int dieTwo;
	private int redConsolationScore = 0;
	private int blueConsolationScore = 0;
	private AIPlayer aiplayer;


	// Use this for initialization
	void Awake() {
		CreateGameBoard ();
		RegisterSelfWithGamePieces ();	
		//SetStartingPlayer ();
	}

	void CreateGameBoard() {
		System.Random rando = new System.Random();
		List<int> starterNumbers = new List<int>(37);
		starterNumbers.Add (2);
		starterNumbers.Add (3);
		starterNumbers.Add (3);
		starterNumbers.Add (4);
		starterNumbers.Add (4);
		starterNumbers.Add (4);
		starterNumbers.Add (5);
		starterNumbers.Add (5);
		starterNumbers.Add (5);
		starterNumbers.Add (5);
		starterNumbers.Add (6);
		starterNumbers.Add (6);
		starterNumbers.Add (6);
		starterNumbers.Add (6);
		starterNumbers.Add (6);
		starterNumbers.Add (7);
		starterNumbers.Add (7);
		starterNumbers.Add (7);
		starterNumbers.Add (7);
		starterNumbers.Add (7);
		starterNumbers.Add (7);
		starterNumbers.Add (7);
		starterNumbers.Add (8);
		starterNumbers.Add (8);
		starterNumbers.Add (8);
		starterNumbers.Add (8);
		starterNumbers.Add (8);
		starterNumbers.Add (9);
		starterNumbers.Add (9);
		starterNumbers.Add (9);
		starterNumbers.Add (9);
		starterNumbers.Add (10);
		starterNumbers.Add (10);
		starterNumbers.Add (10);
		starterNumbers.Add (11);
		starterNumbers.Add (11);
		starterNumbers.Add (12);
		for (int i = starterNumbers.Count; i > 0; i--) {
			int itemPicker = rando.Next (starterNumbers.Count);
			buttonTexts [i - 1].text = starterNumbers [itemPicker].ToString ();
			buttonTexts [i - 1].GetComponentInParent<GamePiece> ().ResetPiece ();
			starterNumbers.RemoveAt (itemPicker);
		}
		for (int i = 0; i < Triangles.Length; i++) {
			Triangles [i].GetComponent<Image>().color = new Color32 (00, 00, 00, 0);
			SetTriangleVertices ();
		}
	}

	void RegisterSelfWithGamePieces() {
		//and also prevent them from being clicked
		for (int i = 0; i < buttonTexts.Length; i++) {
			buttonTexts [i].GetComponentInParent<GamePiece> ().GameControllerSetter (this);
			buttonTexts [i].GetComponentInParent<Button> ().interactable = false;
		}
	}

	public void CreateAIPlayer() {
		//and register the game controller with it
		aiplayer = gameObject.AddComponent<AIPlayer>() as AIPlayer;
		aiplayer.GameControllerSetter (this);
	}

	public void DestroyAIPlayer() {
		if (aiplayer) {
			Destroy (aiplayer);
		}
	}

	public string GetPlayerSide() {
		return playerColor;
	}

	public bool IsThereAnAIPlayer() {
		if (aiplayer) {
			return true;
		} else {
			return false;
		}
	}

	public void EndTurn() {
		UpdateScores ();
		noMovesPanel.SetActive (false);
		diceRollPanel.SetActive(false);
		for (int i = 0; i < buttonTexts.Length; i++) {
			buttonTexts [i].GetComponentInParent<Button> ().interactable = false;
		}
		playerColor = (playerColor.Equals ("red")) ? "blue" : "red";
		if (playerColor.Equals("red")) {
			redHighlighter.SetActive(true);
			blueHighlighter.SetActive(false);
		} else {
			blueHighlighter.SetActive(true);
			redHighlighter.SetActive(false);
		}
		if (aiplayer) {
			if (!playerColor.Equals (aiplayer.getAIPlayerColor ())) {
				diceRollButton.interactable = true;
			} else {
				aiplayer.takeTurn ();
			}
		} else {
			diceRollButton.interactable = true;
		}
	}

	int CheckForLegalMoves() {
		//and make those buttons interactable
		//Debug.Log("CheckForLegalMoves gets called");
		string sumDice = (dieOne + dieTwo).ToString();
		string diffDice = (Math.Abs (dieOne - dieTwo)).ToString();
		int legalMoveCount = 0;
		for (int i = 0; i < buttonTexts.Length; i++) {
			//Debug.Log("button[i] is " + buttonTexts[i] + " and sumDice is " + sumDice);
			if (buttonTexts [i].text.Equals (sumDice) || buttonTexts [i].text.Equals (diffDice)) {
				//Debug.Log("Your if statement works");
				buttonTexts [i].GetComponentInParent<Button>().interactable = true;
				legalMoveCount++;
			}
		}
		//Debug.Log ("legalMoveCount= " + legalMoveCount);
		return legalMoveCount;
	}

	public bool attemptToClaimPiece(int pieceNum) {
		//if CheckForLegalMoves made this button interactable, claim it
		if (buttonTexts [pieceNum].GetComponentInParent<Button> ().interactable) {
			buttonTexts [pieceNum].GetComponentInParent<GamePiece> ().ClaimSpace ();
			return true;
		} else {
			return false;
		}
	}

	bool CheckForEndOfGame(int numTurnNoMoves) {
		if (numberOfTurnsWithNoLegalMoves >= totalNumberOfPlayers) {
			return true;
		}
		else return false;
	}
		
	public void RollDice() {
		diceRollButton.interactable = false;
		System.Random rando = new System.Random();
		dieOne = rando.Next (1, 7);
		dieTwo = rando.Next (1, 7);
		diceRollText.text = "You rolled \n a " + dieOne + " and a " + dieTwo;
		diceRollPanel.SetActive(true);
		int legalMoveCount = CheckForLegalMoves ();
		if (legalMoveCount == 0) {
			if (playerColor.Equals ("red"))
				redConsolationScore++;
			else
				blueConsolationScore++;
			UpdateScores ();
			numberOfTurnsWithNoLegalMoves++;
			bool gameOver = CheckForEndOfGame (numberOfTurnsWithNoLegalMoves);
			if (gameOver) {
				EndGame ();
			} else {
				noMovesPanel.GetComponentInChildren<Text> ().text = char.ToUpper(playerColor[0]) + playerColor.Substring(1) + ", you have no legal moves!";
				noMovesPanel.SetActive (true);
			}
		} else if (numberOfTurnsWithNoLegalMoves > 0) {
			numberOfTurnsWithNoLegalMoves--;
		}
	}

	void UpdateScores() {
		// and paint triangles!
		//Debug.Log("UpdateScores got called");
		int redNonConsolationScore = 0;
		int blueNonConsolationScore = 0;
		for (int i = 0; i < Triangles.Length; i++) {
			int[] currentTriVerts = Triangles [i].GetComponent<Triangle> ().GetVertices ();
			//Debug.Log("currentTriVerts[0,1,2]: " + currentTriVerts[0] + " " + currentTriVerts[1] + " " + currentTriVerts[2]);
			int redVerts = 0;
			int blueVerts = 0;
			for (int j = 0; j < currentTriVerts.Length; j++) {
				if (buttonTexts [currentTriVerts [j]].text.Equals ("r"))
					redVerts++;
				else if (buttonTexts [currentTriVerts [j]].text.Equals ("b"))
					blueVerts++;
			}
			if (redVerts == 3) {
				Triangles [i].GetComponent<Image> ().color = new Color32 (150, 0, 0, 255);
				redNonConsolationScore +=3;
			} else if (blueVerts == 3) {
				Triangles [i].GetComponent<Image> ().color = new Color32 (0, 0, 150, 255);
				blueNonConsolationScore +=3;
			}
		}
		redScore = redNonConsolationScore + redConsolationScore;
		blueScore = blueNonConsolationScore + blueConsolationScore;
		redScoreText.text = redScore.ToString ();
		blueScoreText.text = blueScore.ToString ();
	}

	public void EndGame() {
		for (int i = 0; i < buttonTexts.Length; i++) {
			buttonTexts [i].GetComponentInParent<Button> ().interactable = false;
		}
		diceRollButton.interactable = false;
		if (redScore > blueScore) {
			gameOverPanel.GetComponentInChildren<Text> ().text = "Red wins!!!";
			gameOverPanel.GetComponent<Image> ().color = new Color32 (255, 0, 0, 200);
		} else if (blueScore > redScore) {
			gameOverPanel.GetComponentInChildren<Text>().text = "Blue wins!!!";
			gameOverPanel.GetComponent<Image> ().color = new Color32 (0, 0, 255, 200);
		} else {
			gameOverPanel.GetComponentInChildren<Text> ().text = "You tied!";
			gameOverPanel.GetComponent<Image> ().color = new Color32 (255, 255, 255, 200);
		}
		gameOverPanel.SetActive(true);
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void StartOver() {
		CreateGameBoard ();
		SetStartingPlayer ();
		UpdateScores ();
		gameOverPanel.SetActive (false);
		noMovesPanel.SetActive (false);
		diceRollPanel.SetActive(false);
		if (aiplayer) {
			if (!playerColor.Equals (aiplayer.getAIPlayerColor ())) {
				diceRollButton.interactable = true;
			} else {
				aiplayer.takeTurn ();
			}
		} else {
			diceRollButton.interactable = true;
		}
	}


	public void SetStartingPlayer() {
		// and zeroize the scores
		playerColor = startingPlayerColor;
		if (playerColor.Equals("red")) {
			redHighlighter.SetActive(true);
			blueHighlighter.SetActive (false);
		} else {
			blueHighlighter.SetActive(true);
			redHighlighter.SetActive (false);
		}
		redScore = 0;
		blueScore = 0;
		redConsolationScore = 0;
		blueConsolationScore = 0;
		if (startingPlayerColor.Equals ("red")) {
			startingPlayerColor = "blue";
		} else {
			startingPlayerColor = "red";
		}
		//playerIDPanel is only active at game launch
		playerIDPanel.SetActive (false);
	}

	public void HideInstructions() {
		instructionsPanel.SetActive (false);
	}

	int[] FindNeighbors(int indexInQuestion) {
		int[] neighbors = new int[1];
		switch(indexInQuestion) {
		case 0:
			neighbors = new int[3] {1,4,5};
			return neighbors;
		case 1:
			neighbors = new int[4] {0,2,5,6};
			return neighbors;
		case 2:
			neighbors = new int[4] {1,3,6,7};
			return neighbors;
		case 3:
			neighbors = new int[3] {2,7,8};
			return neighbors;
		case 4:
			neighbors = new int[4] {0,5,9,10};
			return neighbors;
		case 5:
			neighbors = new int[6] {0,1,4,6,10,11};
			return neighbors;
		case 6:
			neighbors = new int[6] {1,2,5,7,11,12};
			return neighbors;
		case 7:
			neighbors = new int[6] {2,3,6,8,12,13};
			return neighbors;
		case 8:
			neighbors = new int[4] {3,7,13,14};
			return neighbors;
		case 9:
			neighbors = new int[4] {4,10,15,16};
			return neighbors;
		case 10:
			neighbors = new int[6] {4,5,9,11,16,17};
			return neighbors;
		case 11:
			neighbors = new int[6] {5,6,10,12,17,18};
			return neighbors;
		case 12:
			neighbors = new int[6] {6,7,11,13,18,19};
			return neighbors;
		case 13:
			neighbors = new int[6] {7,8,12,14,19,20};
			return neighbors;
		case 14:
			neighbors = new int[4] {8,13,20,21};
			return neighbors;
		case 15:
			neighbors = new int[3] {9,16,22};
			return neighbors;
		case 16:
			neighbors = new int[6] {9,10,15,17,22,23};
			return neighbors;
		case 17:
			neighbors = new int[6] {10,11,16,18,23,24};
			return neighbors;
		case 18:
			neighbors = new int[6] {11,12,17,19,24,25};
			return neighbors;
		case 19:
			neighbors = new int[6] {12,13,18,20,25,26};
			return neighbors;
		case 20:
			neighbors = new int[6] {13,14,19,21,26,27};
			return neighbors;
		case 21:
			neighbors = new int[3] {14,20,27};
			return neighbors;
		case 22:
			neighbors = new int[4] {15,16,23,28};
			return neighbors;
		case 23:
			neighbors = new int[6] {16,17,22,24,28,29};
			return neighbors;
		case 24:
			neighbors = new int[6] {17,18,23,25,29,30};
			return neighbors;
		case 25:
			neighbors = new int[6] {18,19,24,26,30,31};
			return neighbors;
		case 26:
			neighbors = new int[6] {19,20,25,27,31,32};
			return neighbors;
		case 27:
			neighbors = new int[4] {20,21,26,32};
			return neighbors;
		case 28:
			neighbors = new int[4] {22,23,29,33};
			return neighbors;
		case 29:
			neighbors = new int[6] {23,24,28,30,33,34};
			return neighbors;
		case 30:
			neighbors = new int[6] {24,25,29,31,34,35};
			return neighbors;
		case 31:
			neighbors = new int[6] {25,26,30,32,35,36};
			return neighbors;
		case 32:
			neighbors = new int[4] {26,27,31,36};
			return neighbors;
		case 33:
			neighbors = new int[3] {28,29,34};
			return neighbors;
		case 34:
			neighbors = new int[4] {29,30,33,35};
			return neighbors;
		case 35:
			neighbors = new int[4] {30,31,34,36};
			return neighbors;
		case 36:
			neighbors = new int[3] {31,32,35};
			return neighbors;
		}
		return neighbors;
	}

	private void SetTriangleVertices() {
		int[] verts = new int[3];
		for (int i = 0; i < Triangles.Length; i++) {
			switch (i) {
			case 0:
				verts = new int[3] {0,1,5};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 1:
				verts = new int[3] {1,2,6};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 2:
				verts = new int[3] {2,3,7};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 3:
				verts = new int[3] {0,4,5 };
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 4:
				verts = new int[3] {1,5,6 };
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 5:
				verts = new int[3] {2,6,7 };
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 6:
				verts = new int[3] { 3,7,8};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 7:
				verts = new int[3] { 4,5,10};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 8:
				verts = new int[3] { 5,6,11};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 9:
				verts = new int[3] { 6,7,12};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 10:
				verts = new int[3] { 7,8,13};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 11:
				verts = new int[3] { 4,9,10};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 12:
				verts = new int[3] { 5,10,11};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 13:
				verts = new int[3] { 6,11,12};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 14:
				verts = new int[3] { 7,12,13};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 15:
				verts = new int[3] { 8,13,14};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 16:
				verts = new int[3] { 9,10,16};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 17:
				verts = new int[3] { 10,11,17};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 18:
				verts = new int[3] { 11,12,18};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 19:
				verts = new int[3] { 12,13,19};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 20:
				verts = new int[3] { 13,14,20};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 21:
				verts = new int[3] { 9,15,16};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 22:
				verts = new int[3] { 10,16,17};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 23:
				verts = new int[3] { 11,17,18};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 24:
				verts = new int[3] { 12,18,19};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 25:
				verts = new int[3] { 13,19,20};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 26:
				verts = new int[3] { 14,20,21};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 27:
				verts = new int[3] { 15,16,22};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 28:
				verts = new int[3] { 16,17,23};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 29:
				verts = new int[3] { 17,18,24};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 30:
				verts = new int[3] { 18,19,25};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 31:
				verts = new int[3] { 19,20,26};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 32:
				verts = new int[3] { 20,21,27};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 33:
				verts = new int[3] { 16,22,23};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 34:
				verts = new int[3] { 17,23,24};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 35:
				verts = new int[3] { 18,24,25};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 36:
				verts = new int[3] { 19,25,26};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 37:
				verts = new int[3] { 20,26,27};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 38:
				verts = new int[3] { 22,23,28};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 39:
				verts = new int[3] { 23,24,29};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 40:
				verts = new int[3] { 24,25,30};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 41:
				verts = new int[3] { 25,26,31};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 42:
				verts = new int[3] { 26,27,32};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 43:
				verts = new int[3] { 23,28,29};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 44:
				verts = new int[3] { 24,29,30};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 45:
				verts = new int[3] { 25,30,31};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 46:
				verts = new int[3] { 26,31,32};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 47:
				verts = new int[3] { 28,29,33};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 48:
				verts = new int[3] { 29,30,34};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 49:
				verts = new int[3] { 30,31,35};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 50:
				verts = new int[3] { 31,32,36};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 51:
				verts = new int[3] { 29,33,34};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 52:
				verts = new int[3] { 30,34,35};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;
			case 53:
				verts = new int[3] { 31,35,36};
				Triangles[i].GetComponent<Triangle>().SetVertices(verts);
				break;

			}
		}
	}

}

