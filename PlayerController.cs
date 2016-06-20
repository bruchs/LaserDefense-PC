using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	public float speed;
	public float laserSpeed;
	public float heroeHealth = 300;
	public GameObject Laser;
	public GameObject heroeFogParticles;
	public GameObject heroeFireParticles;
	public GameObject destroyParticles;
	public AudioClip laserSound;
	public AudioClip shipDamaged;
	public AudioClip heroeDestroyed;
	public AudioSource playerAudioSource;
	public GameObject particleFog ;
	public GameObject particleFire ;
	public Text youDeadText;
	
	private float maxHorizontalPosition = 12.25f;
	private float minHorizontalPosition = -12.25f;
	private float maxVerticalPosition = -1f;
	public float fireRate =0.25F;
	private float nextFire = 0.0F;
	private float minVerticalPosition = -7.25f;

	// Use this for initialization
	void Start ()
	{
	}

	public void ShipMovement ()
	{
		Vector3 moveDir = Vector3.zero;
		moveDir.x = Input.GetAxis ("Horizontal"); // get result of AD keys in X
		moveDir.y = Input.GetAxis ("Vertical"); // get result of WS keys in Z
		// move this object at frame rate independent speed:
		transform.position += moveDir * speed * Time.deltaTime;
		
		// Restrict the player to the game space
		float newX = Mathf.Clamp (transform.position.x, minHorizontalPosition, maxHorizontalPosition);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
		
		float newY = Mathf.Clamp (transform.position.y, minVerticalPosition, maxVerticalPosition);
		transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
		
		
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
		{
			Vector3 touchPosition = Input.GetTouch(0).position;
			
			//Check if it is left or right?
			if(touchPosition.x <  Screen.width/2){
				this.transform.Translate(Vector3.left * speed * Time.deltaTime);
			} else if (touchPosition.x >Screen.width/2) {
				this.transform.Translate(Vector3.right * speed * Time.deltaTime);
			}
			
		}
	}
	
	public void laserShot ()
	{
		Vector3 laserPos = new Vector3 (this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
		Vector3 laserVelocity = new Vector3 (0f, laserSpeed, 0f);
		
		if (Input.GetKeyDown (KeyCode.Space)) {	
			GameObject laser = Instantiate (Laser, laserPos, Quaternion.identity) as GameObject;
			laser.rigidbody2D.velocity = laserVelocity;
			playerAudioSource.audio.clip = laserSound;
			playerAudioSource.audio.Play();	
			
		}
		if ( Time.time > nextFire) 
		{
			nextFire =( Time.time  )+ fireRate;
			GameObject laser = Instantiate (Laser, laserPos, Quaternion.identity) as GameObject;
			laser.rigidbody2D.velocity = laserVelocity;
			playerAudioSource.audio.clip = laserSound;
			playerAudioSource.audio.Play();	
		}
	
	
}

void OnTriggerEnter2D (Collider2D collider)
	{
		EnemyLaserDamage heroeDamage = collider.gameObject.GetComponent<EnemyLaserDamage> ();
		HealItem heroeHeal = collider.gameObject.GetComponent<HealItem> ();

		Debug.Log (heroeHeal);
		Vector3 heroeDamageParticlesPosition = new Vector3 (this.transform.position.x - 0.15f, this.transform.position.y - 0.15f, this.transform.position.z);
		if ( heroeHeal ) {
			heroeHealth += heroeHeal.GetHeal ();
			Debug.Log ("Realiza curacion");
			Debug.Log (heroeHealth);
			test(heroeDamageParticlesPosition);
			
		}
		if (heroeDamage) {
			heroeHealth -= heroeDamage.GetDamage ();
			heroeDamage.HitHeroe ();
			playerAudioSource.audio.clip = shipDamaged;
			playerAudioSource.audio.Play();	
	
			test (heroeDamageParticlesPosition);		
		}
	}	
	
	void test(Vector3 heroeDamageParticlesPosition )
	{
		GameObject particleFogAsset = Resources.Load("Fire Fog") as GameObject;
		GameObject particleFireAsset = Resources.Load("Fire") as GameObject;
		if(heroeHealth == 300){
			Destroy(particleFog);
		}
		
		if (heroeHealth == 200) {
		
			particleFog = Instantiate (particleFogAsset, heroeDamageParticlesPosition, Quaternion.identity)  as GameObject;
			particleFog.transform.parent = this.transform;	
		}
		 
		if (heroeHealth == 100) {
			
			particleFire = Instantiate (particleFireAsset, heroeDamageParticlesPosition, Quaternion.identity) as GameObject;
			particleFire.transform.parent = this.transform;		
		}
		
		if (heroeHealth <= 0) {
			playerAudioSource.audio.clip = heroeDestroyed;
			playerAudioSource.audio.Play();
			Destroy (gameObject);
			GameObject destroyParticleSystem = Instantiate(destroyParticles, this.transform.position, Quaternion.identity) as GameObject;
			youDeadText.text = "You Lose, Press Escape To Play Again";
		}
	}
	// Update is called once per frame
	void Update ()
	{	

			ShipMovement ();
			laserShot ();

		
	}
}
