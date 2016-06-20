using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTracker: MonoBehaviour {

	public int score = 0;
	private Text scoreText;
	
	void Start(){
		scoreText = GetComponent<Text>();
	}
	
	public void Score(int points){
		Debug.Log("Scored Points");
		score += points;
		scoreText.text = score.ToString();
	}
	
	public void scoreReset(){
		score = 0;
		scoreText.text = score.ToString();
	}
}
