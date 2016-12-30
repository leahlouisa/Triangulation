using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour {

	private GameController gameCont;
	private string aiPlayerColor = "blue";
	private int[] piecePreference = new int[] {18,11,12,17,19,24,25,5,6,7,10,13,16,20,23,26,29,30,31,0,1,2,3,4,8,9,14,15,21,22,27,28,32,33,34,35,36};
	private IEnumerator aiCoroutine;

	public void takeTurn() {
		gameCont.RollDice ();
		//System.Threading.Thread.Sleep(1000); 
		aiCoroutine = waitForAWhile (3.5f);
		StartCoroutine (aiCoroutine);
	}

	private IEnumerator waitForAWhile(float seconds) {
		yield return new WaitForSeconds(seconds);
		for (int i = 0; i < piecePreference.Length; i++) {
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
}
