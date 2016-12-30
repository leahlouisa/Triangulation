using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePiece : MonoBehaviour {

	public Sprite blueWoman;
	public Sprite redWoman;
	public Sprite robot;
	public Sprite unclaimedButton;
	public Button button;
	public SpriteRenderer renderer;
	public Text buttonText;
	private GameController gameCont;
	public Sprite[] gamePieceSprites;


	public void ClaimSpace() {
		if (gameCont.GetPlayerSide().Equals("red")) {
			//renderer.color = new Color32 (255, 0, 0, 255);
			buttonText.color = new Color32 (255, 0, 0, 0);
			buttonText.text = "r";
			renderer.sprite = redWoman;

		} else {
			//renderer.color = new Color32 (0, 0, 255, 255);
			buttonText.color = new Color32 (0, 0, 255, 0);
			buttonText.text = "b";
			if (gameCont.IsThereAnAIPlayer()) {
				renderer.sprite = robot;
			} else {
				renderer.sprite = blueWoman;
			}
		}

		button.interactable = false;
		gameCont.EndTurn ();
	}

	public void ResetPiece(int pieceValue) {
		//renderer.color = new Color32 (255, 255, 255, 255);
		buttonText.color = new Color32 (0, 0, 0, 0);
		renderer.sprite = gamePieceSprites[pieceValue - 1];
	}

	public void GameControllerSetter(GameController gc) {
		gameCont = gc;
	}

	public string GetOwner() {
		string owner = buttonText.text;
		return owner;
	}
}