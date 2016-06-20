using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour {

	public float Health = 150;
	public float shotsPerSeconds = 1f;
	public float enemyLaserSpeed;
	public int scoreEnemyValue = 150;
	public int count;
	public GameObject enemyLaser;
	public GameObject destroyParticles;
	public GameObject damageParticles;
	public GameObject healItem;
	public AudioClip enemyShotSound;
	public AudioClip enemyDamaged;
	public AudioClip enemyDestroyed;
	public AudioSource enemyAudioS;
	
	private ScoreTracker track;
	private bool isDead;
	
	void Start()
	{
		track = GameObject.Find("ScoreCount").GetComponent<ScoreTracker>();
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		LaserDamage missile = collider.gameObject.GetComponent<LaserDamage>();
		
		if(missile){
			Health -= missile.GetDamage();
			enemyAudioS.audio.clip = enemyDamaged;
			enemyAudioS.audio.Play();
			missile.Hit();
			
			if(Health <= 75){
				foreach(Transform child in transform){	
					GameObject enemy = Instantiate(damageParticles, child.transform.position, Quaternion.identity) as GameObject;
					enemy.transform.parent = child;
				}	
			}
			
			if(Health <= 0 ){
				AudioSource.PlayClipAtPoint(enemyDestroyed, transform.position);
				GameObject destroyParticleSystem = Instantiate(destroyParticles, this.transform.position, Quaternion.identity) as GameObject;
				track.Score(scoreEnemyValue);
				Destroy(gameObject);
				
				int x = (Random.Range(1, 20));
				
			}
		}
	}
		
	public void Shoot(){
		Vector3 enemyLaserPos = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);
		Vector3 laserVelocity = new Vector3(0f, enemyLaserSpeed, 0f);

		GameObject laser = Instantiate(enemyLaser, enemyLaserPos, Quaternion.identity) as GameObject;
		laser.rigidbody2D.velocity = laserVelocity;
		enemyAudioS.audio.clip = enemyShotSound;
		enemyAudioS.audio.Play();	
	}
	
	void Update(){
		float probability =  shotsPerSeconds * Time.deltaTime;
		if(Random.value < probability){
			Shoot();
		}	
	}
}