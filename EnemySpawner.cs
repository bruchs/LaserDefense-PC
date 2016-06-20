using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public float speed = 1f;
	public float width = 1f;
	public float height = 1f;
	public int count = 1;
	public int scoreValue = 350;
	public GameObject enemySpaceShip;
	public GameObject enemySecondFormation;
	public GameObject enemyThirdFormation;
	
	private bool movingRight = true;
	private bool isCreated;
	private bool secEnemyForCreated = false;
	private bool thiEnemyForCreated = false;
	private float xmax;
	private float xmin;
	private float spawnDelay = 0.5f;
	
	private GameObject enemyMediumLeft;
	private GameObject player;
	private ScoreTracker track;
	
	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distanceToCamera));
		xmax = rightEdge.x;
		xmin = leftEdge.x;
		SpawnUntilFull();
		track = GameObject.Find("ScoreCount").GetComponent<ScoreTracker>();
		}
	
	public void SpawnUntilFull(){
		Transform freePosition = NextFreePosition();
		if(freePosition){
			GameObject enemy = Instantiate(enemySpaceShip, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()){
			Invoke("SpawnUntilFull", spawnDelay);
		}
		
	}

	// Update is called once per frame
	void Update () {
		if(movingRight){
			this.transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		float rightEdgeOfFormation = transform.position.x + (7.5f * width);
		float leftEdgeOfFormation = transform.position.x - (7.5f * width);
		if(leftEdgeOfFormation < xmin){
			movingRight = true;
		} else if(rightEdgeOfFormation > xmax){
			movingRight = false;
		}
		if(AllMembersDead()){
			SpawnUntilFull();
			count++;
			print(count);
			speed = speed + 1;
			track.Score(scoreValue);
		}
		
		if(count / 3 == 1 && !secEnemyForCreated ){
			GameObject secEnemyFormation = Instantiate(enemySecondFormation, new Vector3(0f,-0.5f,0f), Quaternion.identity) as GameObject;
			secEnemyForCreated = true;
		}
		if(count / 6 == 1 && !thiEnemyForCreated){
			GameObject thiEnemyFormation = Instantiate(enemyThirdFormation, new Vector3(0f, -1f,-0f), Quaternion.identity) as GameObject;
			thiEnemyForCreated = true;
		}
	}	
	
	Transform NextFreePosition(){
		foreach(Transform childPositionGameObject in transform){
			if(childPositionGameObject.childCount == 0){
				return childPositionGameObject;
			}
		}return null;
	}
	
	public bool AllMembersDead(){
		foreach(Transform childPositionGameObject in transform){
			if(childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
}
