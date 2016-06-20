using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		Application.LoadLevel (name);
	}
	
	public static void LoadLevelStatic(string name){
		Application.LoadLevel(name);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}
	
	void Update(){
		
			if(Input.GetKeyDown(KeyCode.Escape)){
				Application.LoadLevel("Start Menu");
			}
	}
}
