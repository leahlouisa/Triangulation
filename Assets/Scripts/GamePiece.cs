using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePiece : MonoBehaviour {

	public Button button;
	public SpriteRenderer renderer;
	public Text buttonText;
	private GameController gameCont;

	public void ClaimSpace() {
		if (gameCont.GetPlayerSide().Equals("red")) {
			renderer.color = new Color32 (255, 0, 0, 255);
			buttonText.color = new Color32 (255, 0, 0, 255);
			buttonText.text = "r";
		} else {
			renderer.color = new Color32 (0, 0, 255, 255);
			buttonText.color = new Color32 (0, 0, 255, 255);
			buttonText.text = "b";
		}

		button.interactable = false;
		gameCont.EndTurn ();
	}

	public void ResetPiece() {
		renderer.color = new Color32 (255, 255, 255, 255);
		buttonText.color = new Color32 (0, 0, 0, 255);
	}

	public void GameControllerSetter(GameController gc) {
		gameCont = gc;
	}

}